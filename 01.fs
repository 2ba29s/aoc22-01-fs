256 constant max-line
create line-buffer max-line 2 + allot
variable fd-in
variable sum
create top3 3 cells allot 0 top3 !

: handle-line 
    line-buffer swap s>number? if
        ( add to sum )
        drop dup ( . ) sum +!
    else 2drop then ;

: print-results ( -- )
    ." 1: " top3 @ . cr
    ." 2: " top3 1 cells + @ . cr
    ." 3: " top3 2 cells + @ . cr
    0 3 0 ?do top3 i cells + @ + loop
    ." Sum: " . cr ;

: sum-complete
    ( sum complete )
    ( ." Total: " sum @ . cr )
    ( slot the sum into our top three )
    3 0 ?do
        top3 i cells + @ sum @ < if
            ( slot the new sum in here )
            0 2 i - -do top3 i 1- cells + @ top3 i cells + ! 1 -loop
            sum @ top3 i cells + !
            leave
        then
    loop
    0 sum ! ;

: open-file ( filename -- ) r/o open-file throw fd-in ! ;
: close-file ( -- ) fd-in @ close-file throw ;
: fetch-line ( -- n ) line-buffer max-line fd-in @ read-line throw ;

: solve ( filename -- )
    open-file 0 sum !

    begin fetch-line
    while dup if handle-line else drop sum-complete then
    repeat sum-complete
    
    close-file
    print-results ;
