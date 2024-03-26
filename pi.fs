\ Calculate pi using the Nilakantha infinite series. While more complicated
\ than the Leibniz formula, it is fairly easy to understand, and converges
\ on pi much more quickly.

\ The formula takes three and alternately adds and subtracts fractions with
\ a numerator of 4 and denominator that is the product of three consecutive
\ integers. So each subsequent fraction begins its set of integers with the
\ highest value used in the previous fraction.

\ Described in C syntax n starts at 2 and iterates to the desired precision.
\ pi = 3 + 4/((n)*(++n)*(++n)) - 4/((n)*(++n)*(++n)) + ...

\ Here's a three iteration example with an error slightly more than 0.0007.
\ pi = 3 + 4/(2*3*4) - 4/(4*5*6)
\        + 4/(6*7*8) - 4/(8*9*10)
\        + 4/(10*11*12) - 4/(12*13*14)
\    = 3.14088134088

\ normally this requires floating point arithmetic, but we're using fixed point
\ multiplying by a scaling factor and returned as a ratio.

1 13 lshift constant rescale
rescale 3 * constant three
rescale 4 * constant four

variable n

\ calculates (n)*(++n)*(++n)
: denominator ( -- product )
  n @ dup 1+ dup 1+ dup n ! * * ;

\ calculates a single scaled quotient term 
: quotient ( -- q )
  four denominator U/ ;

\ calculates Qn - Qn+1
: calc_term ( -- q )
  quotient quotient - ;

\ Computes pi as a ratio of integers
: pi ( -- numerator denominator )
  2 n !
  three
  begin
    calc_term dup 0> while +
  repeat
  drop
  rescale ;
