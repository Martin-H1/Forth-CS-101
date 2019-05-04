\ I watched this Tom Scott video: https://www.youtube.com/watch?v=QPZ0pIK_wsc
\ and realized it was the perfect example of a CS-101 Forth problem. So I did
\ it two different ways. This is approach was inspired by nyef in the 6502
\ forum. He indexed into a list of execution tokens based upon the remainder.
\ The token determines the output, and most of them are the TOS, but the
\ exceptions output text and drop TOS. It's extensible just by changing the
\ execution token table. I tweaked it to use create does>, although as nyef
\ pointed out, my usage isn't ANS standard Forth.

: .FIZZ ( unused --)
  ." Fizz " DROP ;

: .BUZZ ( unused --)
  ." Buzz " DROP ;

: .FIZZBUZZ ( unused --)
  ." Fizz Buzz " DROP ;

CREATE BEES
  ' .FIZZBUZZ , ' . , ' . , ' .FIZZ , ' . ,
  ' .BUZZ , ' .FIZZ , ' . , ' . , ' .FIZZ ,
  ' .BUZZ , ' . , ' .FIZZ , ' . , ' . ,
DOES>  ( i addr --)
  SWAP TUCK
  15 MOD CELLS + @ EXECUTE ;

: FIZZBUZZ
  101 1 DO
    I BEES
    CR
  LOOP ;
