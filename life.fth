\ Implementation of Conway's game of life in Forth.
\ See http://en.wikipedia.org/wiki/Conway's_Game_of_Life

\ constants for board height and width
16 constant height
16 constant width
height width * constant size

\ allocate two arrays to hold current and next generations
create gen_curr size allot
create gen_next size allot

\ iterators and their associated operators
variable row
variable col

: rowFirst 0 row ! ;

: rowNext
    1 row +! ;

: rowAtEnd?
    row @ height >= ;

: rowForEach ( token -- )
    >r
    rowFirst
    begin
	r@ execute rowNext rowAtEnd?
    until
    rdrop ;

\ Returns index of the row after current using wrap around.
: row+ ( -- index )
    row @ 1 + height mod ;

\ Returns index of the column before current using wrap around.
: row- ( -- index )
    row @ 1 - height mod ;

: colFirst 0 col ! ;

: colNext
    1 col +! ;

: colAtEnd?
    col @ width >= ;

: colForEach ( token -- )
    >r
    colFirst
    begin
	r@ execute colNext colAtEnd?
    until
    rdrop ;

\ Returns index of the column after current using wrap around.
: col+ ( -- index )
    col @ 1 + width mod ;

\ Returns index of the column before current using wrap around.
: col- ( -- index )
    col @ 1 - width mod ;

\ moves bytes from next gen to current.
: moveCurr ( -- )
    gen_next gen_curr size move ;

\ retrieve a cell value from the current generation
: curr@ ( col row -- n )
    width * + gen_curr + c@ ;

\ stores a value into a cell from the current generation
: curr! ( n col row -- )
    width * + gen_curr + c! ;

\ Parses a pattern string into current board.
\ This function is unsafe and will over write memory.
: >curr ( addr count -- )
    rowFirst colFirst
    1-
    for
        dup c@
        dup '|' <> if
            32 <> 1 and
            col @ row @ curr!
	    colNext
	else
	    drop
	    rowNext
	    colFirst
	then
	1+
    next
    drop ;

: .cell ( row -- )
    col @ over curr@ . ;

\ prints the row from the current generation to output
: .currRow ( -- )
    cr row @
    ['] .cell colForEach
    drop ;

\ Prints the current board generation to standard output
: .curr
    ['] .currRow
    rowForEach
    cr ;

\ clears next array for the subsequent generation computation
: nextErase ( -- )
    gen_next size erase ;

\ retrieve a cell value from the current generation
: next@ ( col row -- n )
    width * + gen_next + c@ ;

\ stores a cell into the next generation
: next! ( n col row -- )
    width * + gen_next + c! ;

\ computes the sum of the neigbors of the current cell.
: calcSum ( -- n )
   col-  row-  curr@
   col @ row-  curr@ +
   col+  row-  curr@ +
   col-  row @ curr@ +
   col+  row @ curr@ +
   col-  row+  curr@ +
   col @ row+  curr@ +
   col+  row+  curr@ + ;

: calcCell ( -- )
    calcSum

    \ The board was initialized to zeros. So unless explicitly marked live,
    \ all cells die in the next generation. There are two rules we'll apply
    \ to mark a cell live.

    \ Is the current cell dead?
    col @ row @ curr@ 0=
    if
        \ Any dead cell with three live neighbours becomes a live cell.
	3 =
    else
	\ Any live cell with two or three live neighbours survives.
        dup 2 >= swap 3 <= and
    then
    if
        1 col @ row @ next!
    then ;

: calcRow ( row -- )
    ['] calcCell colForEach ;

: calcGen ( -- )
    nextErase
    ['] calcRow rowForEach
    moveCurr ;

: life ( generations -- )
    ;

: glider s"  *|  *|***" >curr ;
