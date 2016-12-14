\ Setup constants to remove magic numbers to allow
\ for greater zoom with different scale factors.
20  CONSTANT MAXITER
-39 CONSTANT MINVAL
40  CONSTANT MAXVAL
20  CONSTANT RESCALE
80  CONSTANT S_ESCAPE

\ These variables hold values during the escape calculation.
VARIABLE CREAL
VARIABLE CIMAG
VARIABLE ZREAL
VARIABLE ZIMAG
VARIABLE COUNT

\ Compute squares, but rescale to remove extra scaling factor.
: ZR_SQ ZREAL @ DUP RESCALE */ ;
: ZI_SQ ZIMAG @ DUP RESCALE */ ;

\ Translate escape count to ascii greyscale.
: .CHAR
  S" . .,'~!^:;[/<&?oxOX#  "
  DROP
  SWAP + 1
  TYPE ;

\ Numbers above 4 will always escape, so compare to a scaled value.
: ESCAPES?
  S_ESCAPE > ;

\ Increment count and compare to max iterations.
: COUNT_AND_TEST?
  COUNT @ 1+ DUP COUNT !
  MAXITER > ;

\ stores the row column values from the stack for the escape calculation.
: INIT_VARS
  DUP CREAL ! ZREAL !
  DUP CIMAG ! ZIMAG !
  1 COUNT ! ;

\ Performs a single iteration of the escape calculation.
: DOESCAPE
    ZR_SQ ZI_SQ +
    ESCAPES? IF
      TRUE
    ELSE
      ZR_SQ ZI_SQ - CREAL @ +   \ leave result on stack
      ZREAL @ ZIMAG @ RESCALE */ 2*
      CIMAG @ + ZIMAG !
      ZREAL !                   \ Store stack item into ZREAL
      COUNT_AND_TEST?
    THEN ;

\ Iterates on a single cell to compute its escape factor.
: DOCELL
  INIT_VARS
  BEGIN
    DOESCAPE
  UNTIL
  COUNT @
  .CHAR ;

\ For each cell in a row.
: DOROW
  MAXVAL MINVAL DO
    DUP I
    DOCELL
  LOOP
  DROP ;

\ For each row in the set.
: MANDLEBROT
  CR
  MAXVAL MINVAL DO
    I DOROW CR
  LOOP ;

\ Run the computation.
MANDLEBROT
