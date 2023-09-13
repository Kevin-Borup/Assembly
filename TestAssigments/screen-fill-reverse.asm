// Setup
@SCREEN
D=A
@addrStart
M=D 

@24575
D=A
@addrEnd
M=D 

(RESETSTART)
    @SCREEN
    D=A
    @addrStart
    M=D
    @LOOP
    0;JMP

(RESETEND)
    @24575
    D=A
    @addrEnd
    M=D 
    @LOOP
    0;JMP

(LOOP)
    @KBD
    D=M[A]
    @LOOPWHITE
    D;JLE
    @LOOPBLACK
    D;JGT

    @LOOP
    0;JMP

(FILLWHITE)
    //Protect against overspill
    @24575
    D=A
    @addrStart
    D=D-M
    @RESETSTART
    D;JLE

    @addrStart
    A=M
    M=1

    @addrStart
    M=M+1 // Progress the word length into the screen address, to flip the next word values.

    @LOOP
    0;JMP

(FILLBLACK)
    //Protect against overspill
    @SCREEN
    D=A
    @addrEnd
    D=D-M
    @RESETEND
    D;JGE

    @addrEnd
    A=M
    M=-1

    @addrEnd
    M=M-1 // Progress the word length into the screen address, to flip the next word values.

    @LOOP
    0;JMP