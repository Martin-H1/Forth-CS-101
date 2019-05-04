; After watching this video: https://youtu.be/5mFpVDpKX70 on the Collatz conjecture,
; I decided to Forth CS-101 it!

: odd? dup 1 and ;
: 3n+1 dup 2* + 1 + ;
: collatz(n) odd? if 3n+1 else 2/ then ;
: .n dup . ;
: collatz begin .n dup 1 = invert while collatz(n) repeat cr drop ;

15 collatz
; Produces this output: 15 46 23 70 35 106 53 160 80 40 20 10 5 16 8 4 2 1 

1024 collatz

; Produces this output: 1024 512 256 128 64 32 16 8 4 2 1
