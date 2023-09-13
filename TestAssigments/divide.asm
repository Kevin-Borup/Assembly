//Multiplication
//R2 = R0 / R1
//R0 og R1 udfyldes før kørsel
//R2 er sum, R3 er rest

// Setup
@R2
M=0
@R0
D=M
@R3
M=D

(LOOP)
    // If i>R1 goto END
    @R3
    D=M
    @R1
    D=D-M
    @END
    D;JLT

    //Rest
    @R3
    M=D
    // Another succesful division
    @R2
    M=M+1

    // Repeat
    @LOOP
    0;JMP

(END)
    @END
    0;JMP