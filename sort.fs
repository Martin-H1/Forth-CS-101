\ Swaps the values at two different cell addresses
: cell_swap ( addr1 addr2 -- )
   2dup              \ duplicate both addresses
   L@	             \ load from addr2 (addr2 consumed)
   >r                \ push value onto return stack
   L@                 \ load from addr1 (addr1 consumed)
   swap L!            \ put data under addr2 and store (addr2 consumed)
   r>                \ load addr2's origina value from return stack
   swap L! ;          \ save to addr1

\ Compares values at two different cell addresses and puts flag on stack
: cell_greater? ( addr1 addr2 -- flag )
   L@	             \ load from addr2 (addr2 consumed)
   swap L@            \ load from addr1 (addr1 consumed)
   swap > ;          \ correct order of values and compare.

\ Applies the execution token n1 times to arguments further
\ down the stack which are used by the token function.
\  token - execution token of comparison function
\  n1 - the number of times to apply the function.
: apply \ ( token n1 -- )
   >r \ copy n1 to return stack
   begin
      \ preserving a copy of the token, but execute it also.
      >r r@ execute r>

      \ move n1 to data stack, decrement, preserve and test.
      r> 1- dup >r 0=
   until
   r> drop drop ;

\ Applies the execution token n1 times to arguments further
\ down the stack which are used by the token function. Loops
\ if the execution function returned true and the count is
\ not zero.
\  token - execution token of comparison function
\  n1 - the number of times to apply the function.
: apply? \ ( token n1 -- )
   >r \ copy n1 to return stack
   begin
      \ preserving a copy of the token, but execute it also.
      >r r@ execute

      \ Test the return function's flag
      if
         \ move token back
         r>
         \ decrement and test the count, but preserve it.
         r> 1- dup >r 0=
      else
         \ move token back and push exit flag
         r> true
      then
   until
   r> drop drop ;

\ Used to allocate an array of cells of the desired size passed on stack
: array	( n -- addr )
   create
     cells	     \ converts count into size in bytes
     here over erase \ clear the buffer and
     allot           \ allocate space
   Does>
     swap cells  + ; \ run time gives address of cell at index

10 array foo

\ load an array named foo with values for test.
: foo_init
   4 0 foo L!
   3 1 foo L!
   9 2 foo L!
   7 3 foo L!
   14 4 foo L!
   0 5 foo L!
   13 6 foo L!
   27 7 foo L!
   101 8 foo L!
   1 9 foo L! ;

: foo_dump
   0 foo ? 1 foo ? 2 foo ? 3 foo ? 4 foo ? 5 foo ? 6 foo ? 7 foo ? 8 foo ? 9 foo ? ;

foo_init

\ Used to set up loop limits from current addresses on
\ the stack.
\  addr1 - the stop address of the cells to sort
\  addr2 - the start address of the cells to sort
\  token - execution token of comparison function
: selection_push_limits ( addr1 addr2 token --- addr1 addr2 token limit start )
   >r                \ save the comparison function
   over cell +       \ set up loop limits
   over
   r>                \ get the token
   -rot ;            \ push it under loop limits

\ Variation on the classic selection sort, always o(n^2),
\ but it is simple and generic because it uses an execution
\ token to compare cells.
\
\  addr1 - the stop address of the cells to sort
\  addr2 - the start address of the cells to sort
\  token - execution token of comparison function
: selection_sort ( addr1 addr2 token --- )
   selection_push_limits
   do		     \ from addr1 to addr2 step cell
      selection_push_limits
      drop i cell +
      ?do	     \ from i+1 to addr2 step cell
         dup j i rot \ compare j and its adjacent cell
         execute if
            j i cell_swap
         then
      cell +loop
   cell +loop
   drop drop drop ;

foo_dump
9 foo 0 foo ' cell_greater? selection_sort
foo_dump

\ inner loop of bubble sort, worst case is o(n^2), it performs
\ a single bubble up oeration.
\  addr - the starting address of the cells to sort
\  count - the number of cells to sort
\  token - execution token of comparison function
\ returns
\  swapped - flag to indicate if a swap was performed.
: bubble_up_loop? ( addr count token -- addr count token swapped )
   \ compute do loop limits
   >r over over cell * + >r over r> swap r> -rot

   \ push the flag under the loop limits
   false -rot

   \ from addr1 to addr2-cell step cell
   do
      \ stack contains addr count token swapped
      swap           \ bring the comparison function to tos
      dup i i cell + rot \ compare i and its adjacent cell
      execute if
         i i cell + cell_swap
         swap        \ bring swapped to tos and or with true
         -1 or
      else
         swap        \ bring swapped to tos only
      then
   cell +loop ;

\ Classic bubble sort, worst case is o(n^2), but often
\ better for nearly sorted data.  Also generic because
\ it uses an execution token to compare cells.
\
\  addr - the starting address of the cells to sort
\  count - the number of cells to sort
\  token - execution token of comparison function
: bubble_sort ( addr1 addr2 token --- )
   over
   ['] bubble_up_loop?
   swap
   \ stack has addr, count, and token for apply?
   apply?
   \ pop off the preserved arguments.
   drop drop drop ;

foo_init
foo_dump
0 foo 10 ' cell_greater? bubble_sort
foo_dump



\ allocate the array for the string pointers
10 array string_ptrs

\ load the array with pointers to the strings.
: string_ptrs_init
   c" hello" 0 string_ptrs L!
   c" mother" 1 string_ptrs L!
   c" father" 2 string_ptrs L!
   c" dog pod" 3 string_ptrs L!
   c" the grid" 4 string_ptrs L!
   c" apple" 5 string_ptrs L!
   c" bananna" 6 string_ptrs L!
   c" club" 7 string_ptrs L!
   c" flub" 8 string_ptrs L!
   c" rubber" 9 string_ptrs L! ;

\ Compares character values at two different byte addresses and puts flag on stack
: char> ( addr1 addr2 -- flag )
   C@	             \ load from addr2 (addr2 consumed)
   swap C@           \ load from addr1 (addr1 consumed)
   swap > ;          \ correct order of values and compare.

\ Returns true flage if the character at addr1 is lexically greater than character at addr2
: c$.char> \ ( addr1 addr2 -- addr1++ addr2++ flag )
   over 1 + -rot
   dup 1 + -rot
   char> ;

\ Compares character values at two different byte addresses and puts flag on stack
: char= ( addr1 addr2 -- flag )
   C@	             \ load from addr2 (addr2 consumed)
   swap C@           \ load from addr1 (addr1 consumed)
   swap = ;          \ correct order of values and compare.

\ Returns true flage if the character at addr1 is lexically greater than character at addr2
: c$.char= \ ( addr1 addr2 -- addr1++ addr2++ flag )
   over 1 + -rot
   dup 1 + -rot
   char= ;

\ Puts the length of the counted string on the stack.
: c$.length \ ( caddr -- n1 )
   c@ ;

\ Advances the caddr pointer past the count byte.
: c$.ptr \ ( caddr -- addr )
   1 + ;

\ Prints the counted string at caddr
: c$.print \ ( caddr -- )
   dup c$.ptr swap c$.length type ;

: string_ptrs_dump
   cr
   0  string_ptrs L@ c$.print cr
   1  string_ptrs L@ c$.print cr
   2  string_ptrs L@ c$.print cr
   3  string_ptrs L@ c$.print cr
   4  string_ptrs L@ c$.print cr
   5  string_ptrs L@ c$.print cr
   6  string_ptrs L@ c$.print cr
   7  string_ptrs L@ c$.print cr
   8  string_ptrs L@ c$.print cr
   9  string_ptrs L@ c$.print cr ;

string_ptrs_init
string_ptrs_dump

\ code debugged to this point.

\ Returns true flage if caddr1 is lexically greater than caddr2
: c$.> \ ( caddr1 caddr2 -- flag )
   \ convert both caddrs to ptrs and length
   >r
   dup
   c$.ptr swap
   c$.length
   r@
   c$.length min
   r>
   c$.ptr swap 0
   \ stack contains ptr1 ptr2 count 0
   do
     i .
     c$.char>
     if 
       true leave
     then
   loop
   \ pop the two pointers, but leave flag.
   swap drop swap drop ;

0 string_ptrs 10 ' c$.> bubble_sort
