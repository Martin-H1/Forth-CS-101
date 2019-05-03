\ compute the number with the highest non sign bit set.
1 cell 8 * 2 - lshift
create max_bit 1 cells allot
max_bit !

\ shift "bit" to the highest power of four <= n.
: starting_bit ( n - n start_bit )
   max_bit @
   begin 2dup <= while \ one > n
      2 rshift
   repeat ;

\ predicate who's name says it all 
: n>=r+b? ( n r b - n r b f )
      \ copy n to return stack
      rot dup >r -rot

      \ compute result + bit
      2dup +

      \ n >= result + bit
      r> swap >= ;

\ round up n2 by one if n1 is greater
: round_up ( n1 n2 - n1 n2 )
   2dup > if
       1+
   then ;

\ Fast integer square root algorithm, with rounding the result to
\ the next greater integer if the fractional part is 0.5 or greater.
\ For example 2->1, 3->2, 4->2, 6->2, 7->3, 9->3
: isqrtr \ ( n - n^1/2 )
   \ Throughout the function we'll juggle three numbers on the stack:
   \ n (input), bit (computed), and result (output, starts at 0).
   starting_bit 0

   begin over while       \ bit is not zero

      n>=r+b? if
         2dup + >r        \ push result + bit to return stack
         rot r> - -rot    \ n = n - (result + one)
         over 2 * +       \ result += 2 * bit;
      then

      1 rshift swap       \ divide result by 2
      2 rshift swap       \ divide bit by 4.
   repeat

   \ bit has outlived its usefulness
   swap drop

   \ Do arithmetic rounding to nearest integer
   round_up

   \ clean off n to return only result.
   swap drop ;