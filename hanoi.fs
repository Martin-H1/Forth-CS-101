\ Move disk simply prints the move.
\ arguments unused dest_post source_post disk_number
: hanoi_move_disk ( n1 n2 n3 n4 -- )
   ." Move disk " . ." from " . ." to " . cr 
   drop ;

\ Recursively moves sub-tower, a disk, and sub-tower
\ arguments spare_post dest_post source_post disk_number
: hanoi_move_tower ( n1 n2 n3 n4 -- )
   dup 0=
   if
      hanoi_move_disk ( needs all arguments except spare_post )
   else
      \ Push arguments dest_post, spare_post, source_post, disk_number - 1
      2over swap 2over 1-
      recurse

      \ Duplicate the stack and move the disk.
      2over 2over
      hanoi_move_disk

      \ Last call, consume arugments rather than duplicate them.
      \ Reformat the stack to contain source, dest, spare, disk - 1
      >r rot rot swap r> 1-
      recurse
   then ;

\ seeds the stack and calls moves tower
: hanoi_move_4 ( -- spare dest source disk )
   2 3 1 4
   cr
   hanoi_move_tower  ;