\ Implementation of Conway's game of life in Forth.
\ See http://en.wikipedia.org/wiki/Conway's_Game_of_Life

\ constants for board height and width
16 constant height
16 constant width
height width * constant size

\ allocate two arrays to hold current and next generations
create gen_curr size allot
create gen_next size allot

\ iterators and successor functions
variable row
variable col

: rowFirst 0 row ! ;

: rowNext
    1 row +! ;

: rowAtEnd?
    row @ height >= ;

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

\ prints the row from the current generation to output
: .currRow ( row -- )
    colFirst
    begin
        col @ over curr@ . colNext colAtEnd?
    until
    drop ;

\ Prints the current board generation to standard output
: .curr
    rowFirst
    begin
        cr row @ .currRow rowNext rowAtEnd?
    until
    cr ;

: nextErase ( -- )
    gen_next size erase ;

\ retrieve a cell value from the current generation
: next@ ( col row -- n )
    width * + gen_next + c@ ;

\ stores a cell into the next generation
: next! ( n col row -- )
    width * + gen_next + c! ;

: calcCell ;
: calcRow ;
: calcGen ;

: life
    ;

: glider s"  *|  *|***" >curr ;
