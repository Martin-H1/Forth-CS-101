hex

\ Values for common angles in brads with ten bits of precision
0003 constant DEGREE_ANGLE	\ approximate, the exact value is 2.84
0080 constant ACUTE_ANGLE	\ 45 degree angle (useful for binary search).
0100 constant RIGHT_ANGLE
0200 constant STRAIGHT_ANGLE
0400 constant FULL_ROTATION

\ Create a first quadrent sine table using scaled integer values.
create sineTable
    0000 , 00C9 , 0192 , 025B , 0324 , 03ED , 04B6 , 057F , 0647 , 0710 ,
    07D9 , 08A2 , 096A , 0A33 , 0AFB , 0BC3 , 0C8B , 0D53 , 0E1B , 0EE3 ,
    0FAB , 1072 , 1139 , 1201 , 12C8 , 138E , 1455 , 151B , 15E2 , 16A8 ,
    176D , 1833 , 18F8 , 19BD , 1A82 , 1B47 , 1C0B , 1CCF , 1D93 , 1E56 ,
    1F19 , 1FDC , 209F , 2161 , 2223 , 22E5 , 23A6 , 2467 , 2528 , 25E8 ,
    26A8 , 2767 , 2826 , 28E5 , 29A3 , 2A61 , 2B1F , 2BDC , 2C98 , 2D55 ,
    2E11 , 2ECC , 2F87 , 3041 , 30FB , 31B5 , 326E , 3326 , 33DE , 3496 ,
    354D , 3604 , 36BA , 376F , 3824 , 38D8 , 398C , 3A40 , 3AF2 , 3BA5 ,
    3C56 , 3D07 , 3DB8 , 3E68 , 3F17 , 3FC5 , 4073 , 4121 , 41CE , 427A ,
    4325 , 43D0 , 447A , 4524 , 45CD , 4675 , 471C , 47C3 , 4869 , 490F ,
    49B4 , 4A58 , 4AFB , 4B9E , 4C3F , 4CE1 , 4D81 , 4E21 , 4EBF , 4F5E ,
    4FFB , 5097 , 5133 , 51CE , 5269 , 5302 , 539B , 5433 , 54CA , 5560 ,
    55F5 , 568A , 571D , 57B0 , 5842 , 58D4 , 5964 , 59F3 , 5A82 , 5B10 ,
    5B9D , 5C29 , 5CB4 , 5D3E , 5DC7 , 5E50 , 5ED7 , 5F5E , 5FE3 , 6068 ,
    60EC , 616F , 61F1 , 6271 , 62F2 , 6371 , 63EF , 646C , 64E8 , 6563 ,
    65DD , 6657 , 66CF , 6746 , 67BD , 6832 , 68A6 , 6919 , 698C , 69FD ,
    6A6D , 6ADC , 6B4A , 6BB8 , 6C24 , 6C8F , 6CF9 , 6D62 , 6DCA , 6E30 ,
    6E96 , 6EFB , 6F5F , 6FC1 , 7023 , 7083 , 70E2 , 7141 , 719E , 71FA ,
    7255 , 72AF , 7307 , 735F , 73B5 , 740B , 745F , 74B2 , 7504 , 7555 ,
    75A5 , 75F4 , 7641 , 768E , 76D9 , 7723 , 776C , 77B4 , 77FA , 7840 ,
    7884 , 78C7 , 7909 , 794A , 798A , 79C8 , 7A05 , 7A42 , 7A7D , 7AB6 ,
    7AEF , 7B26 , 7B5D , 7B92 , 7BC5 , 7BF8 , 7C29 , 7C5A , 7C89 , 7CB7 ,
    7CE3 , 7D0F , 7D39 , 7D62 , 7D8A , 7DB0 , 7DD6 , 7DFA , 7E1D , 7E3F ,
    7E5F , 7E7F , 7E9D , 7EBA , 7ED5 , 7EF0 , 7F09 , 7F21 , 7F38 , 7F4D ,
    7F62 , 7F75 , 7F87 , 7F97 , 7FA7 , 7FB5 , 7FC2 , 7FCE , 7FD8 , 7FE1 ,
    7FE9 , 7FF0 , 7FF6 , 7FFA , 7FFD , 7FFE , 7FFF ,

\ clampAngle takes brads as input and returns an angle that is positive and
\ always less than a FULL_ROTATION. That way we can be sure to use the angle
\ as an index into the trig table.
: clampAngle ( angle - angle between 0 to full rotation )
  FULL_ROTATION
  mod ;

\ Sine takes brads as input and returns a scaled sine value. However, to
\ save memeory the sine table only contains one quadrant, angles in other
\ quadrants are mapped onto the first with sign modification if needed.
: sine ( angle - result )
  clampAngle
  dup 8 rshift			\ find the quadrent 0, 1, 2, 3
  case
    0 of
      cell *			\ convert angle to offset
      sineTable + @		\ index into sine table
    endof
    1 of
      STRAIGHT_ANGLE swap -	\ reflect angle and lookup.
      cell *			\ convert angle to offset
      sineTable + @		\ index into sine table
    endof
    2 of
      STRAIGHT_ANGLE -		\ rotate angle and lookup.
      cell *			\ convert angle to offset
      sineTable + @		\ index into sine table
      negate
    endof
    3 of
      FULL_ROTATION swap -	\ reflect angle and lookup.
      cell *			\ convert angle to offset
      sineTable + @		\ index into sine table
      negate
    endof
   endcase ;

\ cosine is defined in terms of sine by coordinate rotation.
\ cos = sin ( angle + RIGHT_ANGLE )
: cosine ( angle - result )
  RIGHT_ANGLE +
  sine ;

RIGHT_ANGLE sine constant MAX_SINE
RIGHT_ANGLE negate sine constant MIN_SINE

\ clamps n between max and min to prevent inverse trig function overflow.
: clampSine ( n -- clamped_value )
  MIN_SINE
  2dup <			\ Compare without consuming them
  if
    nip				\ If n1 < n2, drop n1 and keep n2
  else
    drop			\ If n1 >= n2, drop n2 and keep n1
  then
  MAX_SINE
  2dup >
  if
    nip
  else
    drop
  then ;

\ arcsine takes an input and maps it to a valid sine value and returns the
\ closest angle that matches it. It is done using binary search of the sine
\ quadrent with sign adjustment.
: arcsine ( sine - brad )
  clampSine
  dup 0 <
  if
    -1 swap abs			\ save the sign
  else
     1 swap
  then

  ACUTE_ANGLE			\ 45° first approximation on data stack
  dup 2/ >r			\ 22.5° correction factor on return stack

  \ data stack has ( sign value approximation )
  begin
    r@ 0 >			\ loop until correction factor reaches zero
  while
    dup >r sine			\ stack has (sign, value, approximation)
    over =
    if				\ exact match, terminate loop
      r> r> drop
      0 >r			\ set correction factor to zero.
    else			\ otherwise adjust with correction factor
      r@ sine			\ stack has (sign, value, approximation )
      over <
      if
        r> r>
	dup 2/ >r		\ halve correction factor
	+			\ approximation too small, add the correction
      else
        r> r>
	dup 2/ >r		\ halve correction factor
	-			\ approximation too large, subtract correction
      then
    then
  repeat
  r> drop			\ clean up return stack
  swap drop			\ drop input argument
  * ;				\ restore the sign

\ arccosine is defined in terms of arcsin with coordinate rotation.
: arccosine
  arcsine
  RIGHT_ANGLE
  -
  negate ;

decimal
