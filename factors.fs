\ predicate used to determine if a number is a divisor
: factor? ( n1 n2 - f )
   mod 0= ;

\ prints n2 if it is a factor of n1
: .factor ( n1 n2 - )
    tuck    \ n2 n1 n2
    factor? \ n2 f
    if
       .
    else
       drop
    then ;

\ prints all the numbers less than the number that are factors.
: factors ( n - )
  dup 2     \ from 2 to n
  do
    dup i   \ n n i
    .factor \ n
  loop
  drop ;
  
