hex

000c constant BRAD_N
000b constant DEGREE_ANGLE
0200 constant ACUTE_ANGLE
0400 constant RIGHT_ANGLE
0800 constant STRAIGHT_ANGLE
0c00 constant REFLEX_ANGLE
1000 constant FULL_ROTATION
000e constant BITSHIFT_FACTOR
4000 constant SCALING_FACTOR

create sineTable
  0000 , 0019 , 0032 , 004b , 0064 , 007d , 0096 , 00af , 00c9 , 00e2 ,
  00fb , 0114 , 012d , 0146 , 015f , 0178 , 0192 , 01ab , 01c4 , 01dd ,
  01f6 , 020f , 0228 , 0241 , 025b , 0274 , 028d , 02a6 , 02bf , 02d8 ,
  02f1 , 030a , 0323 , 033d , 0356 , 036f , 0388 , 03a1 , 03ba , 03d3 ,
  03ec , 0405 , 041e , 0437 , 0451 , 046a , 0483 , 049c , 04b5 , 04ce ,
  04e7 , 0500 , 0519 , 0532 , 054b , 0564 , 057d , 0596 , 05af , 05c8 ,
  05e1 , 05fa , 0613 , 062c , 0645 , 065e , 0677 , 0690 , 06a9 , 06c2 ,
  06db , 06f4 , 070d , 0726 , 073f , 0758 , 0771 , 078a , 07a3 , 07bc ,
  07d5 , 07ee , 0807 , 0820 , 0839 , 0852 , 086b , 0884 , 089c , 08b5 ,
  08ce , 08e7 , 0900 , 0919 , 0932 , 094b , 0964 , 097c , 0995 , 09ae ,
  09c7 , 09e0 , 09f9 , 0a11 , 0a2a , 0a43 , 0a5c , 0a75 , 0a8d , 0aa6 ,
  0abf , 0ad8 , 0af1 , 0b09 , 0b22 , 0b3b , 0b54 , 0b6c , 0b85 , 0b9e ,
  0bb6 , 0bcf , 0be8 , 0c01 , 0c19 , 0c32 , 0c4b , 0c63 , 0c7c , 0c95 ,
  0cad , 0cc6 , 0cde , 0cf7 , 0d10 , 0d28 , 0d41 , 0d59 , 0d72 , 0d8b ,
  0da3 , 0dbc , 0dd4 , 0ded , 0e05 , 0e1e , 0e36 , 0e4f , 0e67 , 0e80 ,
  0e98 , 0eb1 , 0ec9 , 0ee2 , 0efa , 0f12 , 0f2b , 0f43 , 0f5c , 0f74 ,
  0f8c , 0fa5 , 0fbd , 0fd6 , 0fee , 1006 , 101f , 1037 , 104f , 1068 ,
  1080 , 1098 , 10b0 , 10c9 , 10e1 , 10f9 , 1111 , 112a , 1142 , 115a ,
  1172 , 118a , 11a2 , 11bb , 11d3 , 11eb , 1203 , 121b , 1233 , 124b ,
  1263 , 127b , 1294 , 12ac , 12c4 , 12dc , 12f4 , 130c , 1324 , 133c ,
  1354 , 136c , 1383 , 139b , 13b3 , 13cb , 13e3 , 13fb , 1413 , 142b ,
  1443 , 145a , 1472 , 148a , 14a2 , 14ba , 14d1 , 14e9 , 1501 , 1519 ,
  1530 , 1548 , 1560 , 1577 , 158f , 15a7 , 15be , 15d6 , 15ee , 1605 ,
  161d , 1634 , 164c , 1664 , 167b , 1693 , 16aa , 16c2 , 16d9 , 16f1 ,
  1708 , 171f , 1737 , 174e , 1766 , 177d , 1794 , 17ac , 17c3 , 17da ,
  17f2 , 1809 , 1820 , 1838 , 184f , 1866 , 187d , 1895 , 18ac , 18c3 ,
  18da , 18f1 , 1908 , 1920 , 1937 , 194e , 1965 , 197c , 1993 , 19aa ,
  19c1 , 19d8 , 19ef , 1a06 , 1a1d , 1a34 , 1a4b , 1a62 , 1a79 , 1a8f ,
  1aa6 , 1abd , 1ad4 , 1aeb , 1b02 , 1b18 , 1b2f , 1b46 , 1b5d , 1b73 ,
  1b8a , 1ba1 , 1bb7 , 1bce , 1be5 , 1bfb , 1c12 , 1c28 , 1c3f , 1c55 ,
  1c6c , 1c83 , 1c99 , 1caf , 1cc6 , 1cdc , 1cf3 , 1d09 , 1d20 , 1d36 ,
  1d4c , 1d63 , 1d79 , 1d8f , 1da6 , 1dbc , 1dd2 , 1de8 , 1dfe , 1e15 ,
  1e2b , 1e41 , 1e57 , 1e6d , 1e83 , 1e99 , 1eb0 , 1ec6 , 1edc , 1ef2 ,
  1f08 , 1f1e , 1f34 , 1f49 , 1f5f , 1f75 , 1f8b , 1fa1 , 1fb7 , 1fcd ,
  1fe2 , 1ff8 , 200e , 2024 , 2039 , 204f , 2065 , 207b , 2090 , 20a6 ,
  20bb , 20d1 , 20e7 , 20fc , 2112 , 2127 , 213d , 2152 , 2168 , 217d ,
  2192 , 21a8 , 21bd , 21d2 , 21e8 , 21fd , 2212 , 2228 , 223d , 2252 ,
  2267 , 227d , 2292 , 22a7 , 22bc , 22d1 , 22e6 , 22fb , 2310 , 2325 ,
  233a , 234f , 2364 , 2379 , 238e , 23a3 , 23b8 , 23cd , 23e1 , 23f6 ,
  240b , 2420 , 2434 , 2449 , 245e , 2473 , 2487 , 249c , 24b0 , 24c5 ,
  24da , 24ee , 2503 , 2517 , 252c , 2540 , 2554 , 2569 , 257d , 2592 ,
  25a6 , 25ba , 25cf , 25e3 , 25f7 , 260b , 261f , 2634 , 2648 , 265c ,
  2670 , 2684 , 2698 , 26ac , 26c0 , 26d4 , 26e8 , 26fc , 2710 , 2724 ,
  2738 , 274c , 275f , 2773 , 2787 , 279b , 27af , 27c2 , 27d6 , 27ea ,
  27fd , 2811 , 2824 , 2838 , 284b , 285f , 2872 , 2886 , 2899 , 28ad ,
  28c0 , 28d4 , 28e7 , 28fa , 290e , 2921 , 2934 , 2947 , 295a , 296e ,
  2981 , 2994 , 29a7 , 29ba , 29cd , 29e0 , 29f3 , 2a06 , 2a19 , 2a2c ,
  2a3f , 2a52 , 2a65 , 2a77 , 2a8a , 2a9d , 2ab0 , 2ac2 , 2ad5 , 2ae8 ,
  2afa , 2b0d , 2b20 , 2b32 , 2b45 , 2b57 , 2b6a , 2b7c , 2b8e , 2ba1 ,
  2bb3 , 2bc6 , 2bd8 , 2bea , 2bfc , 2c0f , 2c21 , 2c33 , 2c45 , 2c57 ,
  2c6a , 2c7c , 2c8e , 2ca0 , 2cb2 , 2cc4 , 2cd6 , 2ce8 , 2cf9 , 2d0b ,
  2d1d , 2d2f , 2d41 , 2d52 , 2d64 , 2d76 , 2d88 , 2d99 , 2dab , 2dbc ,
  2dce , 2de0 , 2df1 , 2e03 , 2e14 , 2e25 , 2e37 , 2e48 , 2e5a , 2e6b ,
  2e7c , 2e8d , 2e9f , 2eb0 , 2ec1 , 2ed2 , 2ee3 , 2ef4 , 2f05 , 2f16 ,
  2f28 , 2f38 , 2f49 , 2f5a , 2f6b , 2f7c , 2f8d , 2f9e , 2faf , 2fbf ,
  2fd0 , 2fe1 , 2ff1 , 3002 , 3013 , 3023 , 3034 , 3044 , 3055 , 3065 ,
  3076 , 3086 , 3096 , 30a7 , 30b7 , 30c7 , 30d8 , 30e8 , 30f8 , 3108 ,
  3118 , 3128 , 3138 , 3149 , 3159 , 3169 , 3179 , 3188 , 3198 , 31a8 ,
  31b8 , 31c8 , 31d8 , 31e7 , 31f7 , 3207 , 3216 , 3226 , 3236 , 3245 ,
  3255 , 3264 , 3274 , 3283 , 3293 , 32a2 , 32b1 , 32c1 , 32d0 , 32df ,
  32ee , 32fe , 330d , 331c , 332b , 333a , 3349 , 3358 , 3367 , 3376 ,
  3385 , 3394 , 33a3 , 33b2 , 33c1 , 33cf , 33de , 33ed , 33fb , 340a ,
  3419 , 3427 , 3436 , 3444 , 3453 , 3461 , 3470 , 347e , 348c , 349b ,
  34a9 , 34b7 , 34c6 , 34d4 , 34e2 , 34f0 , 34fe , 350c , 351a , 3528 ,
  3536 , 3544 , 3552 , 3560 , 356e , 357c , 3589 , 3597 , 35a5 , 35b3 ,
  35c0 , 35ce , 35dc , 35e9 , 35f7 , 3604 , 3612 , 361f , 362c , 363a ,
  3647 , 3654 , 3662 , 366f , 367c , 3689 , 3696 , 36a4 , 36b1 , 36be ,
  36cb , 36d8 , 36e5 , 36f1 , 36fe , 370b , 3718 , 3725 , 3731 , 373e ,
  374b , 3757 , 3764 , 3771 , 377d , 378a , 3796 , 37a3 , 37af , 37bb ,
  37c8 , 37d4 , 37e0 , 37ed , 37f9 , 3805 , 3811 , 381d , 3829 , 3835 ,
  3841 , 384d , 3859 , 3865 , 3871 , 387d , 3889 , 3894 , 38a0 , 38ac ,
  38b7 , 38c3 , 38cf , 38da , 38e6 , 38f1 , 38fd , 3908 , 3913 , 391f ,
  392a , 3935 , 3941 , 394c , 3957 , 3962 , 396d , 3978 , 3983 , 398e ,
  3999 , 39a4 , 39af , 39ba , 39c5 , 39d0 , 39da , 39e5 , 39f0 , 39fb ,
  3a05 , 3a10 , 3a1a , 3a25 , 3a2f , 3a3a , 3a44 , 3a4f , 3a59 , 3a63 ,
  3a6d , 3a78 , 3a82 , 3a8c , 3a96 , 3aa0 , 3aaa , 3ab4 , 3abe , 3ac8 ,
  3ad2 , 3adc , 3ae6 , 3af0 , 3afa , 3b03 , 3b0d , 3b17 , 3b20 , 3b2a ,
  3b34 , 3b3d , 3b47 , 3b50 , 3b59 , 3b63 , 3b6c , 3b75 , 3b7f , 3b88 ,
  3b91 , 3b9a , 3ba3 , 3bad , 3bb6 , 3bbf , 3bc8 , 3bd1 , 3bda , 3be2 ,
  3beb , 3bf4 , 3bfd , 3c06 , 3c0e , 3c17 , 3c20 , 3c28 , 3c31 , 3c39 ,
  3c42 , 3c4a , 3c53 , 3c5b , 3c63 , 3c6c , 3c74 , 3c7c , 3c84 , 3c8c ,
  3c95 , 3c9d , 3ca5 , 3cad , 3cb5 , 3cbd , 3cc5 , 3ccc , 3cd4 , 3cdc ,
  3ce4 , 3cec , 3cf3 , 3cfb , 3d02 , 3d0a , 3d12 , 3d19 , 3d21 , 3d28 ,
  3d2f , 3d37 , 3d3e , 3d45 , 3d4d , 3d54 , 3d5b , 3d62 , 3d69 , 3d70 ,
  3d77 , 3d7e , 3d85 , 3d8c , 3d93 , 3d9a , 3da1 , 3da7 , 3dae , 3db5 ,
  3dbb , 3dc2 , 3dc9 , 3dcf , 3dd6 , 3ddc , 3de2 , 3de9 , 3def , 3df5 ,
  3dfc , 3e02 , 3e08 , 3e0e , 3e14 , 3e1b , 3e21 , 3e27 , 3e2d , 3e33 ,
  3e38 , 3e3e , 3e44 , 3e4a , 3e50 , 3e55 , 3e5b , 3e61 , 3e66 , 3e6c ,
  3e71 , 3e77 , 3e7c , 3e82 , 3e87 , 3e8c , 3e92 , 3e97 , 3e9c , 3ea1 ,
  3ea7 , 3eac , 3eb1 , 3eb6 , 3ebb , 3ec0 , 3ec5 , 3eca , 3ece , 3ed3 ,
  3ed8 , 3edd , 3ee1 , 3ee6 , 3eeb , 3eef , 3ef4 , 3ef8 , 3efd , 3f01 ,
  3f06 , 3f0a , 3f0e , 3f13 , 3f17 , 3f1b , 3f1f , 3f23 , 3f27 , 3f2b ,
  3f2f , 3f33 , 3f37 , 3f3b , 3f3f , 3f43 , 3f47 , 3f4a , 3f4e , 3f52 ,
  3f55 , 3f59 , 3f5d , 3f60 , 3f64 , 3f67 , 3f6a , 3f6e , 3f71 , 3f74 ,
  3f78 , 3f7b , 3f7e , 3f81 , 3f84 , 3f87 , 3f8a , 3f8d , 3f90 , 3f93 ,
  3f96 , 3f99 , 3f9c , 3f9e , 3fa1 , 3fa4 , 3fa6 , 3fa9 , 3fac , 3fae ,
  3fb1 , 3fb3 , 3fb5 , 3fb8 , 3fba , 3fbc , 3fbf , 3fc1 , 3fc3 , 3fc5 ,
  3fc7 , 3fc9 , 3fcb , 3fcd , 3fcf , 3fd1 , 3fd3 , 3fd5 , 3fd7 , 3fd8 ,
  3fda , 3fdc , 3fde , 3fdf , 3fe1 , 3fe2 , 3fe4 , 3fe5 , 3fe7 , 3fe8 ,
  3fe9 , 3feb , 3fec , 3fed , 3fee , 3fef , 3ff0 , 3ff1 , 3ff2 , 3ff3 ,
  3ff4 , 3ff5 , 3ff6 , 3ff7 , 3ff8 , 3ff9 , 3ff9 , 3ffa , 3ffb , 3ffb ,
  3ffc , 3ffc , 3ffd , 3ffd , 3ffe , 3ffe , 3ffe , 3fff , 3fff , 3fff ,
  3fff , 3fff , 3fff , 3fff , 4000 ,
1025 constant SINE_TABLE_SIZE
decimal

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
  dup BRAD_N 2 - rshift		\ find the quadrent 0, 1, 2, 3
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
