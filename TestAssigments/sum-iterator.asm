//Iteration
//R1++ til R0
//R0 udfyldes før kørsel

// Setup
@R1
M=0

(LOOP)
    @R1
    D=M
    @R0
    D=D-M
    @END
    D;JGE

    @R1
    M=M+1
    
    @LOOP
    0;JMP

(END)
   @END
   0;JMP
