\ Implementation of Conway's game of life in Forth.
\ See http://en.wikipedia.org/wiki/Conway's_Game_of_Life

\ constants for board height and width
24 constant height
32 constant width
height width * constant size

\ allocate two arrays to hold current and next generations
create gen_curr size allot
create gen_next size allot

\ iterators and their associated operators
variable row
variable col

\ Sets the row offset to zero
: rowFirst ( -- ) 0 row ! ;

\ Advances the offset by the width.
: rowNext ( -- )
    width row +! ;

\ At end if current offset exceeds array size.
: rowAtEnd?
    row @ size >= ;

\ Iterator used to apply a function to the rows.
: rowForEach ( xt -- )
    rowFirst
    begin
	dup execute rowNext rowAtEnd?
    until
    drop ;

\ Returns index of the row after current using wrap around.
: row+ ( -- index )
    row @ width + size mod ;

\ Returns index of the column before current using wrap around.
: row- ( -- index )
    row @ width - size mod ;

: colFirst 0 col ! ;

: colNext
    1 col +! ;

: colAtEnd?
    col @ width >= ;

: colForEach ( xt -- )
    colFirst
    begin
	dup execute colNext colAtEnd?
    until
    drop ;

\ Returns index of the column after current using wrap around.
: col+ ( -- index )
    col @ 1 + width mod ;

\ Returns index of the column before current using wrap around.
: col- ( -- index )
    col @ 1 - width mod ;

\ moves bytes from next gen to current.
: moveCurr ( -- )
    gen_next gen_curr size move ;

\ clears curr array to clear out junk in ram
: currErase ( -- )
    gen_curr size erase ;

\ retrieve a cell value from the current generation
: curr@ ( col row -- n )
    + gen_curr + c@ ;

\ stores a value into a cell from the current generation
: curr! ( n col row -- )
    + gen_curr + c! ;

\ Parses a pattern string into current board.
\ This function is unsafe and will over write memory.
: >curr ( addr count -- )
    currErase
    rowFirst colFirst
    1-
    for
        dup c@
        dup '|' <> if
            bl <> 1 and
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

: .cell ( -- )
    col @ row @ curr@
    if '*' else '.' then
    emit ;

\ prints the row from the current generation to output
: .currRow ( -- )
    cr ['] .cell colForEach ;

\ Prints the current board generation to standard output
: .curr
    ['] .currRow rowForEach
    cr ;

\ retrieve a cell value from the next generation
: next@ ( col row -- n )
    + gen_next + c@ ;

\ stores a cell into the next generation
: next! ( n col row -- )
    + gen_next + c! ;

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

    \ Unless explicitly marked live, all cells die in the next generation.
    \ There are two rules we'll apply to mark a cell live.

    \ Is the current cell dead?
    col @ row @ curr@ 0=
    if
        \ Any dead cell with three live neighbours becomes a live cell.
	3 =
    else
	\ Any live cell with two or three live neighbours survives.
        dup 2 >= swap 3 <= and
    then
    1 and
    col @ row @ next! ;

: calcRow ( row -- )
    ['] calcCell colForEach ;

: calcGen ( -- )
    ['] calcRow rowForEach
    moveCurr ;

: life ( -- )
    page
    begin calcGen 0 0 at-xy .curr key? until ;

\ Test cases taken from Rosetta code's implementation
: blinker s" |***" >curr ;
: toad s" ***| ***" >curr ;
: pentomino s" **| **| *" >curr ;
: pi s" **| **|**" >curr ;
: glider s"  *|  *|***" >curr ;
: pulsar s" *****|*   *" >curr ;
: ship s"  ****|*   *|    *|   *" >curr ;
: pentadecathalon s" **********" >curr ;
: clock s"  *|  **|**|  *" >curr ;
