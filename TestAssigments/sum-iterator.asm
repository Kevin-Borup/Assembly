//Iteration
//R1++ til R0
//R0 og R1 udfyldes før kørsel

// Setup
@R0
D=M
@R1
M=0

(LOOP)
    @R0
    D=M
    @R1
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
