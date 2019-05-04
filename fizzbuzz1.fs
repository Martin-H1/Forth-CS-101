\ I watched this Tom Scott video: https://www.youtube.com/watch?v=QPZ0pIK_wsc
\ and realized it was the perfect example of a CS-101 Forth problem. So I did
\ it two different ways. This is the more straight foreward way:

: FIZZ?  ( n -- flag)
  3 MOD 0= ;

: BUZZ?   ( n -- flag)
  5 MOD 0= ;

: .FIZZ   ( n -- flag)
  FIZZ? DUP
  IF ." Fizz " THEN ;

: .BUZZ   ( n -- flag)
  BUZZ? DUP
  IF ." Buzz " THEN ;

: FIZZBUZZ   ( --)
  101 1 DO
    I .FIZZ I .BUZZ
    OR 0= IF I . THEN
    CR
  LOOP ;

FIZZBUZZ
