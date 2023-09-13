   @R0
   D=M    // D = RAM[0]

   @8
   D;JGT  // If R0>0 goto 8
   
   @R1
   M=0    // RAM[1]=0
   @10   
   0;JMP  // goto end

   @R1
   M=1    // R1=1

   @10
   0;JMP