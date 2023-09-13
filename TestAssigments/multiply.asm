//Multiplication
//R2 = R0 * R1
//R0 og R1 udfyldes før kørsel

// Setup
@R2
M=0
@i
M=0

(LOOP)
    // If i>R1 goto END
    @i
    D=M
    @R1
    D=D-M
    @END
    D;JEQ

    //R2=R2+R0
    @R0
    D=M
    @R2
    M=M+D

    // i++
    @i
    M=M+1

    // Repeat
    @LOOP
    0;JMP

(END)
    @END
    0;JMP