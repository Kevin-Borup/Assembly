// This file is part of www.nand2tetris.org
// and the book "The Elements of Computing Systems"
// by Nisan and Schocken, MIT Press.
// File name: projects/04/Fill.asm

// Runs an infinite loop that listens to the keyboard input.
// When a key is pressed (any key), the program blackens the screen,
// i.e. writes "black" in every pixel;
// the screen should remain fully black as long as the key is pressed. 
// When no key is pressed, the program clears the screen, i.e. writes
// "white" in every pixel;
// the screen should remain fully clear as long as no key is pressed.

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

(LOOP)
    @KBD
    D=M
    @FILLWHITE
    D;JEQ
    @FILLBLACK
    D;JGT

    @LOOP
    0;JMP

(FILLWHITE)
    //Protect against overspill into KBD ram address
    @KBD
    D=A
    @addrStart
    D=D-M
    @RESETSTART
    D;JLE

    @addrStart
    A=M
    M=1

    @addrStart
    M=M+1 

    @LOOP
    0;JMP

(FILLBLACK)
    //Protect against overspill into KBD ram address
    @KBD
    D=A
    @addrStart
    D=D-M
    @RESETSTART
    D;JLE

    @addrStart
    A=M
    M=-1

    @addrStart
    M=M+1

    @LOOP
    0;JMP
