\ Calculate the Mandelbot set using complex numbers. Normally this requires
\ floating point arithmetic, but we'll use fixed point and multiply by a
\ scaling factor. Since the Mandelbrot set lies within -2, -2i to +2, 2i,
\ we need at least one sign bit and two integer bits. However, the escape
\ calculation requires more bits to avoid an overflow. So seven bits should
\ be plenty. This allows the remaining bits to be used for fractional bits.

\ Setup constants to remove magic numbers. Interogate the runtime with cell
\ to determine machine precision. This allows greater zoom with different
\ scale factors.

1                        \ Bit to shift to create rescale factor
cell 8 *                 \ Compute number of bits in a cell.
7 -                      \ Calculate the number of fractional bits
lshift       constant RESCALE
RESCALE -2 * constant MINVAL
RESCALE  2 * constant MAXVAL
20           constant MAXITER
RESCALE 4 *  constant S_ESCAPE
MAXVAL MINVAL - 80 / constant STEP

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
  dup c-real ! z-real !
  dup c-imag ! z-imag !
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
    dup i
    docell
    STEP
  +loop
  drop ;

\ For each row in the set.
: mandelbrot
  cr
  MAXVAL MINVAL do
    i dorow cr
    STEP
  +loop ;

\ Run the computation.
mandelbrot
