;; This buffer is for notes you don't want to save, and for Lisp evaluation.
;; If you want to create a file, visit that file with C-x C-f,
;; then enter the text in that file's own buffer.

: HEAPM ; ( for FORGETting )

512 CONSTANT NALLOC ( minimum size of new block allocated )

( define a "header" structure )
: >next ; IMMEDIATE ( does nothing, first element in structure )
: >size  CELL+ ;
2 CELLS CONSTANT headerSize

( global variables )
CREATE startBase 2 CELLS ALLOT
VARIABLE freep

: mallocClear ( initialize base, freep )
   startBase DUP >next !
   0 startBase >size !
   startBase freep !
;

: allocateBlock ( prevp p nunits -- address )
   ( if this function is called then p is big enough for nunits )
   OVER >size @ OVER = ( prevp p nunits p.size=nunits? )
   IF ( remove from free list )
      DROP 2DUP >next @ ( prevp p prevp p.next )
      SWAP >next ! ( prevp.next = p.next )
   ELSE ( allocate tail end of p )
      OVER >size @ OVER - >R ( prevp p nunits [ p.size-nunits ] )
      SWAP R@ OVER >size ( prevp nunits p newsize &p.size [ newsize ] ) !     
      R> + ( prevp nunits newp )
      SWAP OVER >size ! ( prevp newp )
   THEN
   SWAP freep ! headerSize +
;

: startOrEnd? ( bp p -- flag )
   DUP >next @ ( bp p p.next )
   OVER > ( bp p p.next>p? )
   IF 2DROP 0 EXIT THEN
   2DUP > -ROT ( bp>p? bp p ) 
   >next @ < ( bp>p? bp<p.next? )
   OR
;

: betweenBlocks? ( bp p -- flag )
  2DUP > -ROT ( bp>p? bp p )
  >next @ < ( bp>p? bp<p.next? )
  AND
;
   
: free ( addr -- )
   headerSize - freep @ ( bp p )
   BEGIN
      2DUP betweenBlocks?
      IF
         -1
      ELSE
         2DUP startOrEnd?
         IF 
            -1
         ELSE
            >next @ 0
         THEN
      THEN
   UNTIL
   OVER DUP >size @ + ( bp p bp+bp.size )
   OVER >next @ = ( bp p bp+bp.size=p.next? )
   IF
      OVER >size ( bp p &bp.size )
      OVER >next @ >size @ ( bp p &bp.size p.next.size )
      SWAP +!
      2DUP >next @ >next @ ( bp p bp p.next.next )
      SWAP >next !
   ELSE
      2DUP >next @ ( bp p bp p.next )
      SWAP >next !
   THEN
   2DUP DUP >size @ + = ( bp p bp=p+p.size? )
   IF
      OVER >size @ ( bp p bp.size )
      OVER >size +!
      SWAP >next @ ( p bp.next )
      OVER >next ! ( p )
   ELSE
      SWAP OVER >next ( p bp &p.next ) !
   THEN
   freep !
;

: morecore ( nunits -- address )
   DUP NALLOC >=
   IF
      HERE OVER ALLOT ( nunits addr )
      SWAP OVER >size ! 
      headerSize + EXIT
   THEN
   HERE NALLOC ALLOT ( nunits addr )
   2DUP >size !
   SWAP 2DUP + ( addr nunits freeptr )
   NALLOC ROT - ( addr freeptr leftover )
   OVER >size ! ( addr freeptr )
   headerSize + free ( addr )
   headerSize +
;
: malloc ( size -- address )
   headerSize + ( nunits )
   freep @ DUP >next @ ROT ( prevp p nunits )
   BEGIN
      OVER >size @ OVER >= ( prevp p nunits p.size>=nunits? )
      IF
        allocateBlock EXIT
      THEN
      OVER freep @ = ( wrapped around? )
      IF 
         NIP NIP ( nunits ) morecore ( addr ) EXIT
      THEN                 
      ROT DROP ( p nunits ) SWAP DUP >next @ ROT
   AGAIN
; 

mallocClear ( starts the ball rolling )
