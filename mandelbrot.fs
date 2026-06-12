\ Setup constants to remove magic numbers to allow
\ for greater zoom with different scale factors.
20  constant MAXITER
-39 constant MINVAL
40  constant MAXVAL
20 5 lshift constant RESCALE
RESCALE 4 * constant S_ESCAPE

\ These variables hold values during the escape calculation.
variable c-real
variable c-imag
variable z-real
variable z-imag
variable iters

\ Compute squares, but rescale to remove extra scaling factor.
: zr_sq z-real @ dup RESCALE */ ;
: zi_sq z-imag @ dup RESCALE */ ;

\ Translate escape count to ascii greyscale.
: .char
  s" ..,'~!^:;[/<&?oxOX#   "
  drop + 1
  type ;

\ Numbers above 4 will always escape, so compare to a scaled value.
: escapes?
  S_ESCAPE > ;

\ Increment count and compare to max iterations.
: count_and_test?
  iters @ 1+ dup iters !
  MAXITER > ;

\ stores the row column values from the stack for the escape calculation.
: init_vars
  5 lshift dup c-real ! z-real !
  5 lshift dup c-imag ! z-imag !
  1 iters ! ;

\ Performs a single iteration of the escape calculation.
: doescape
    zr_sq zi_sq 2dup +
    escapes? if
      2drop
      true
    else
      - c-real @ +   \ leave result on stack
      z-real @ z-imag @ RESCALE */ 1 lshift
      c-imag @ + z-imag !
      z-real !                   \ Store stack item into z-real
      count_and_test?
    then ;

\ Iterates on a single cell to compute its escape factor.
: docell
  init_vars
  begin
    doescape
  until
  iters @
  .char ;

\ For each cell in a row.
: dorow
  MAXVAL MINVAL do
    dup I
    docell
  loop
  drop ;

\ For each row in the set.
: mandelbrot
  cr
  MAXVAL MINVAL do
    i dorow cr
  loop ;

\ Run the computation.
mandelbrot
