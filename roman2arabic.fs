\ String parsing is another important part of a language to understand. So I started looking into a
\ FORTH program to parse a Roman numeral into an Arabic one. I found some pretty freaky FORTH to do this,
\ so I began reverse engineering it, then completely re-wrote it.

\ Helpers used to create cell values, and access portion of the cell.
: makePair 7 lshift + ;  ( char value -- array[i] )
: >char  127 and ;       ( array[i] -- char )
: >value 7 rshift ;      ( array[i] -- value )

\ Create an associative array by left shifting Roman values with the assoicated character
create (arabic)
   char M 1000 makePair ,  \ 128077
   char D  500 makePair ,  \  64068
   char C  100 makePair ,  \  12867
   char L   50 makePair ,  \   6476
   char X   10 makePair ,  \   1368
   char V    5 makePair ,  \    726
   char I    1 makePair ,  \    201
does> ( char addr -- value value )
   7 cells      \ char addr 7*cell_size
   bounds       \ char addr+7*cell_size addr
   do
      i @       \ char array[i]
      over over \ char array[i] char array[i]
      >char =   \ char array[i] f
      if        \ chars match, then return value
         nip >value leave
      else
         drop   \ char
      then
      1 cells  \ increment index one cell.
   +loop
   dup
;

\ adds the value of char to sum and keeps a copy of its value in digit
: do_digit (  sum digit char -- new_sum new_digit )
   (arabic) >r   \ sum digit new_digit
   over over     \ sum digit new_digit digit new_digit
   < if
      \ old digit is less than new, so subtract it from sum
      -rot 2* negate +
      swap       \ adjusted_sum new_digit
   else
      nip        \ sum new_digit
   then
   + r>          \ new_sum new_digit
;

\ parses a counted string in roman numeral form.
: >arabic ( addr count - )
   0 -rot 0 -rot \ sum digit addr count
   begin
      over over  \ sum digit addr count addr count
      while      \ count is non-zero
      c@         \ load the character at addr
      -rot >r >r \ sum digit char
      do_digit   \ sum digit
      r> r>      \ sum digit addr count
      1 /string  \ advance to next character
   repeat
   \ ( sum digit addr count addr )
   2drop         \ sum digit addr
   2drop         \ sum
;

s" MCMLXXXIV" >arabic .
