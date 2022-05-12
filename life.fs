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

: row+
    1 row +! ;

: rowAtEnd?
    row @ height >= ;

: colFirst 0 col ! ;

: col+
    1 col +! ;

: colatEnd?
    col @ width >= ;

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
	    col+
	else
	    drop
	    row+
	    colFirst
	then
	1+
	next
    drop ;

\ prints the i row from the current generation to output
: .currRow ( row -- )
    colFirst
    begin
        col @ over curr@ . col+ colAtEnd?
    until
    drop ;

\ Prints the current board generation to standard output
: .curr
    rowFirst
    begin
        cr row @ .currRow row+ rowAtEnd?
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

: life
    ;

: glider s"  *|  *|***" >curr ;
