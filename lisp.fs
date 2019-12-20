\ node operations. Note first cell is reserved for state bit operations!

\ A single node consists of a status cell and two pointer cells.
\ These status values indicate the state of the node.
0 constant NODE_FREE
1 constant NODE_ACTIVE
3 constant NODE_INT
5 constant NODE_SYMBOL

\ gets the status
: status@ ( addr -- status )
    @ ;

: status! ( value addr -- )
    ! ;

\ gets the car pointer from the node pointer in the stack.
: car ( addr -- addr )
    cell + @ ;

\ sets the car pointer in the node point in the stack with value.
: setcar ( value addr -- )
    cell + ! ;

\ get the cdr pointer from the node point in the stack.
: cdr ( addr -- addr )
    2 cells + @ ;

\ returns the cdr pointer from the node point in the stack.
: setcdr ( value addr -- )
    2 cells + ! ;

\ returns the allocation size of a single node.
: node ( -- size )
    3 cells ;

\ returns the allocation size of n nodes
: nodes ( n -- size )
    node * ;

\ Lisp's heap is an array of these cells
: nodeArray ( n -- addr )
   create
     nodes	     \ converts count into size in bytes
     here over erase \ clear the buffer and
     allot           \ allocate space
   Does>
     swap nodes + ; \ run time gives address of Node at index

500 constant HEAP_SIZE

\ create the heap
HEAP_SIZE nodeArray heap

\ pointer to the first free cell.
variable freeList

\ Init the entire heap as free.
: heapInit
    0 heap freeList !

    \ traverse all nodes linking them to free list.
    HEAP_SIZE 1 - 0 do
        i 1 + heap 
        i heap
       setcdr
    loop

    \ link the last node to null.
    0 HEAP_SIZE 1 - heap setcdr 
;

heapInit

\ pulls the first node off the free list.
: getNode ( -- addr )
    freeList @
    dup
    cdr
    freeList ! ;

\ build a list using two input pointers.
: cons ( car_addr cdr_addr - addr )
    getNode
    tuck
    setcdr
    tuck
    setcar ;

\ An integer cell 
: makeInt ( n -- addr )
    getNode
    dup
    NODE_INT
    swap
    status!
    tuck
    setcar ;

: isnil? ( addr -- flag )
    0= ;


#define mksym(X)              omake(SYM, 1, (obj *)(X))
#define symname(X)            ((char *)((X)->p[0]))
#define mkprimop(X)           omake(PRIMOP, 1, (obj *)(X))
#define primopval(X)          ((primop)(X)->p[0])


\ traverse all nodes accessible from the pointer argument and mark them as active
: mark ( addr -- )
   begin (* PlistCheck*)
      if (p <> nil) then
         if (p^.CLASS = LIST) or (p^.class = dot) then
            begin
               p^.CLASS := ACTIVE;
               PlistCheck(p^.LEFT);
               PlistCheck(p^.RIGHT)
            end
;

\ traverse entire node list and free any inactive nodes.
: sweep
      var
         I: integer;
         Temp: SPTR;
   begin (* Sweep*)
      for I := 0 to ONELESS do
         begin
            Temp := OBLIST[I];
            while Temp <> nil do
               begin
                  Temp^.CLASS := ACTIVE;
                  PlistCheck(Temp^.LEFT^.PLIST);
                  PlistCheck(Temp^.Left^.LConst);
                  PlistCheck(Temp^.Left^.FunVal);
                  {install if names become lists }
                  {PlistCheck(Temp^.Left^.PName);
                  Temp := Temp^.Right
               end
         end;
      PlistCheck(TRLIST);
;

\ Once all of the global nodes are marked active, Collect the rest into a linked list.
: garbageCollection
    sweep
    gather
;

procedure CollectBlock (var B: StorageArray; BlockSize: Integer);
var
   i: integer;
begin (* CollectBlock*)
   for i := 1 to BlockSize do
      begin
         if B[i].class = Active then
            B[i].class := List
         else
            begin
               if LispFreeList = nil then
                  LispFreeList := @B[i]
               else
                  LispLastFree^.Right := @B[i];
               LispLastFree := @B[i];
               Inc(NumberFree)
            end
      end;
   LispLastFree^.Right := nil
end;(*CollectBlock *)

procedure Gather;
   var
      GatherBlock: MemoryPtr;
   begin (* Gather *)
      LispFreeList := nil;
      LispLastFree := nil;
      NumberFree := 0;
      GatherBlock := FirstBlock;
      while GatherBlock <> CurrentBlock do
         begin
            CollectBlock(GatherBlock^.Block, MaxMem);
            GatherBlock := GatherBlock^.Next
         end;
      CollectBlock(CurrentBlock^.Block, (LispFree - 1));
      NumberFree := NumberFree + (MaxMem - LispFree);
      if NumberFree < 2 * TooFewNodes then
         NewBlock
   end;(* Gather *)
