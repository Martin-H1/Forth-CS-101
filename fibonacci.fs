\ Computes the n number in the Fibonacci sequence.
: fibonacci ( n -- n )
  \ First make sure we have a positive integer.
  dup
  0<= if
    cr ." Fibonacci requires a positive integer. "
    drop
  else
    \ if 1 then return 1
    dup
    1 = if
      drop 1
    else
      \ if 2 then return 1
      dup
      2 = if
        drop 1
      else
        \ seed the stack with 1 1 n
        1 tuck swap
        \ loop using the stack to generate Fibonacci pairs.
        2 do
          tuck +
        loop
        \ add the last two pairs.
        +
      then
    then
  then ;