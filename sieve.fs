\ Begin borrowed code from http://www.forth.org/svfig/Len/bits.htm
\ Note: that code had a bug I fixed as indicated below.

\ Bit_array is a defining word that will create a bit array.
\ l is the number of bytes
create masks 128 c, 64 c, 32 c, 16 c, 8 c, 4 c, 2 c, 1 c,
: bit_array \ ( len -- ) ( i -- m a)
   create allot does>
      swap   \ a i
      8      \ a i 8
      /mod   \ a remainder whole
      swap   \ a whole remainder !MCH bug fix!
      masks  \ a whole remainder a
      +      \ a whole mask_a
      c@     \ a whole m
      -rot   \ m a whole
      + ;    \ m a

\ We also need words to store and fetch bits. The words .@ and .!
\ Will fetch and store, respectively, a Boolean flag on the stack to
\ a bit array. (Do not confuse .! with cset or .@ with creset.) 

: .! ( f m a -- ) rot if cset else creset then ;
: .@ ( m a -- f ) c@ and 0<> ;

\ Examples
\ 3 bit-array thflag  \ Create a bit array for 24 bit-flags
\ 11 thflag ctoggle    \ Toggle the 11th bit
\ 10 thFLAG .@ ( -- f) \ Extract the 10th bit and leave as well-formed Boolean flag

\ End of borrowed code.

\ Create a bit vector to hold the sieve.
\ 128 bytes allows for all primes less than 1024.
128 bit_array sieve

\ locates the index of the first non-zero bit in the sieve
\ with an index greater than the input.
: find_one ( n -- n )
   1+ dup 1024 swap do
      i sieve .@ \ get the bit corresponding to the integer.
      if
         leave
      else
         1+
      then
   loop ;

\ sets bits starting at n.
: set_bits ( n -- )
   1024 swap
   do
      i sieve cset
   loop ;

\ clears bits starting at n in steps of n. So n=2 starts at 2
\ and clears every other bit. While n=3 starts at three and
\ clears every third bit. However, it then resets the first
\ bit as that number is prime.
: clear_bits ( n -- )
   dup 1024 swap
   do
      i sieve creset
      dup
   +loop
   sieve cset ;

\ We'll waste two bits for integers 0 and 1 which can't be prime
: do_sieve
  2 set_bits   \ Assume all numbers 2 or greater are prime.
  2            \ Initially, let p equal 2, the first prime number.

  begin dup 1024 < while
     dup          \ n n
     clear_bits   \ n
     find_one     \ index of first one > n
  repeat ;

\ iterates through the bit vector printing the index of a prime number
: .sieve
   ." List of primes "
   1024 0 do
      i sieve .@ \ get the bit corresponding to the integer.
      if
         i . ." , "
      then
   loop ;