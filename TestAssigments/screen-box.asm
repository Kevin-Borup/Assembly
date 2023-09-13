// R0 box height

// Setup
@SCREEN
D=A
@addr
M=D  

@0
D=M
@n
M=D  // n = RAM[0]
@i
M=0  // i = 0

 
(LOOP)
	@i
	D=M
	@n
	D=D-M
	@END
	D;JGT

	@addr
	A=M
	M=-1

	@i
	M=M+1

	@32 // Word length of this specific screen
	D=A
	@addr
	M=D+M // Progress the word length into the screen address, to flip the next word values.
	@LOOP
	0;JMP

	
(END)
	@END
	0;JMP