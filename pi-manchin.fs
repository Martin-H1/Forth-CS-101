\ Calculate Pi using bignums and Machin's formula:
\ Pi = 16*arctan(1/5) - 4*arctan(1/239)
\ Which is computable with the arctangent series (Gregory's series):
\ arctan (x) = (x^1)/1 - (x^3)/3 + (x^5)/5 - (x^7)/7 + .. + -1^n*(x^2n+1)/(2n+1)

\ Use exp to calculate decimal digits in a cell.
: exp ( base exp -- result )
    over swap 1 ?do over * loop nip ;

1 cells 2 *      constant BN-DIGITS \ decimal digits per cell
10 BN-DIGITS exp constant BN-BASE   \ Compute largest base that fits in a cell.
125              constant BN-CELLS  \ 125 cells * 4 digits = 500 digits
                                    \ +5 extra for carry margin
130              constant BN-SIZE   \ total cells allocated per bignum

variable bn-carry            \ scratch for bn*
variable bn-rem              \ scratch for bn/rem
variable bn-a                \ scratch for bn+
variable bn-b

\ ---------------------------------------------------------------------------
\ bignum ( -- )
\ Define a new bignum variable. References push base address.
\ ---------------------------------------------------------------------------
: bignum ( -- )
    create BN-SIZE cells allot
;

\ ---------------------------------------------------------------------------
\ 0>bn ( a -- )
\ Set all cells of bignum a to zero.
\ ---------------------------------------------------------------------------
: 0>bn ( a -- )
    BN-SIZE 0 do
        0 over i cells + !
    loop
    drop
;

\ ---------------------------------------------------------------------------
\ s>bn ( n a -- )
\ Set bignum a to single-cell value n.
\ ---------------------------------------------------------------------------
: s>bn ( n a -- )
    dup 0>bn
    !
;

\ ---------------------------------------------------------------------------
\ bn! ( src dst -- )
\ Copy bignum src to dst.
\ ---------------------------------------------------------------------------
: bn! ( src dst -- )
    BN-SIZE 0 do
        over i cells + @
        over i cells + !
    loop
    2drop
;

\ ---------------------------------------------------------------------------
\ bn+ ( a b -- )
\ b += a  in place, with carry propagation.
\ ---------------------------------------------------------------------------
: bn+ ( a b -- )
    bn-b !
    bn-a !
    0                               \ carry
    BN-SIZE 0 do
        bn-a @ i cells + @          \ a[i]
        bn-b @ i cells + @          \ b[i]
        + +                         \ a[i] + b[i] + carry
        BN-BASE /mod swap           \ new ( carry b[i] )
        bn-b @ i cells + !          \ store new b[i] ( carry )
    loop
    drop
;

\ ---------------------------------------------------------------------------
\ bn- ( a b -- )
\ b -= a  in place. Assumes b >= a (no underflow check).
\ ---------------------------------------------------------------------------
: bn- ( a b -- )
    bn-b !
    bn-a !
    0                               \ borrow
    BN-SIZE 0 do
        bn-b @ i cells + @          \ b[i]
        bn-a @ i cells + @          \ a[i]
        -                           \ b[i] - a[i]
        swap -                      \ - borrow
        dup 0< if
            BN-BASE +
            1
        else
            0
        then
        swap
        bn-b @ i cells + !
    loop
    drop
;

\ ---------------------------------------------------------------------------
\ bn* ( n a -- )
\ a *= n  in place. n must be small enough that n * BN-BASE fits in 32 bits.
\ ---------------------------------------------------------------------------
: bn* ( n a -- )
    0 bn-carry !
    BN-SIZE 0 do
        over                        \ ( n a n )
        over i cells + @            \ ( n a n a[i] )
        um*                         \ ( n a ud_lo ud_hi )
        bn-carry @ 0 d+             \ ( n a result_lo result_hi )
        BN-BASE um/mod              \ ( n a rem quot )
        bn-carry !                  \ save carry, stack: ( n a rem )
        over i cells + !            \ store rem, stack: ( n a )
    loop
    2drop
;

\ ---------------------------------------------------------------------------
\ bn/rem ( n a -- rem )
\ a /= n  in place, high to low. Returns remainder.
\ n must be <= 65535. Quotient per cell must fit in 16 bits (n > BN-BASE/65535).
\ ---------------------------------------------------------------------------
: bn/rem ( n a -- rem )
    0 bn-rem !
    BN-SIZE 0 do
        BN-SIZE 1- i -              \ ( n a index )
        cells over +                \ ( n a addr )
        @                           \ ( n a a[index] )
        bn-rem @                    \ ( n a a[index] rem )
        BN-BASE um*                 \ ( n a a[index] ud_lo ud_high )
        rot 0 d+                    \ ( n a ud_lo+a[index] ud_high  )
        3 pick                      \ ( n a ud_lo+a[index] ud_high n )
        um/mod                      \ ( n a rem quot )
        2 pick                      \ ( n a rem quot a )
        BN-SIZE 1- i -
        cells + !                   \ store quot, stack: ( n a rem )
        bn-rem !                    \ stack: ( n a )
    loop
    2drop                           \ drop both n and a
    bn-rem @
;

\ ---------------------------------------------------------------------------
\ bn0= ( a -- flag )
\ True if all cells of a are zero.
\ ---------------------------------------------------------------------------
: bn0= ( a -- flag )
    true                            \ assume zero
    BN-SIZE 0 do
        over i cells + @
        0<> if
            drop false              \ found non-zero cell
            leave
        then
    loop
    swap drop
;

\ ---------------------------------------------------------------------------
\ bn. ( a -- )
\ Print BN-DIGITS decimal digits. Most significant cell first.
\ First cell printed without leading zeros, rest with leading zeros.
\ ---------------------------------------------------------------------------
variable bn-row-cnt
variable bn-total
variable bn-leading
: bn. ( a -- )
    0 bn-row-cnt !
    0 bn-total !
    false bn-leading !
    BN-SIZE 0 do
        BN-SIZE 1 - i -
        cells over +
        @
        bn-leading @ if
            \ 0-padded BN-DIGITS digits, no trailing space
            0 <# BN-DIGITS 0 do # loop #> type
            BN-DIGITS bn-row-cnt +!
            BN-DIGITS bn-total +!
        else
            dup 0<> if
                1 bn-row-cnt +!
                1 bn-total +!
                true bn-leading !
                space space 0 <# #S #> type ." ." cr
                bn-total @ 4 u.r ." :" space
            else
                drop space
            then
        then
        bn-leading @
        bn-row-cnt @ 63 >
        and
        if
            cr bn-total @ 4 u.r ." :" space
            0 bn-row-cnt !
        then
    loop
    drop
    bn-leading @ 0= if
        ." 0"
    then
;

BN-DIGITS BN-CELLS * constant DIGITS    \ SCALE = 10^DIGITS as a bignum

variable ar-x           \ x in arctan(1/x)
variable ar-x2          \ x squared
variable ar-i           \ current odd term index: 1, 3, 5, 7, ...
variable ar-sign        \ true = next term subtracts, false = next term adds

bignum scale
bignum scratch1
bignum scratch2
bignum sum
bignum term

: make-scale ( -- )
    1 scale s>bn
    DIGITS 0 do
        10 scale bn*
    loop
;

\ Compute arctangent with Gregory's series.
\ arctan (x) = (x^1)/1 - (x^3)/3 + (x^5)/5 - (x^7)/7 + .. + -1^n*(x^2n+1)/(2n+1)

\ Computes the arctanget of the reciprocal of x
: arctan-recip ( x -- bn-addr )
    dup ar-x !
    dup *
    ar-x2 !
    scale term bn!
    ar-x @ term bn/rem drop
    term sum bn!
    1 ar-i !
    false ar-sign !
    begin
        ar-x2 @ term bn/rem drop
        term bn0= 0=
    while
        ar-i @ 2 + dup ar-i !   \ ar-i += 2
        term scratch1 bn!
        scratch1 bn/rem drop
        ar-sign @ if
            scratch1 sum bn+
        else
            scratch1 sum bn-
        then
        ar-sign @ 0= ar-sign !
    repeat
    sum
;

\ Calculate Pi using bignums and Machin's formula:
\ Pi = 16*arctan(1/5) - 4*arctan(1/239)
\ Note: compute using fixed point and a scale factor
: calc-pi ( -- bn-addr )
    5 arctan-recip scratch2 bn!
    16 scratch2 bn*
    239 arctan-recip scratch1 bn!
    4 scratch1 bn*
    scratch1 scratch2 bn-
    scratch2
;

: print-pi ( -- )
    cr ." Pi to " DIGITS . ." digits:" cr
    calc-pi bn. cr
;

make-scale              \ Initialize scale constant
