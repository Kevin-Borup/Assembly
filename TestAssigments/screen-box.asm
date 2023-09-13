// Fill R0 with starting row,
// Fill R1 with word length

// Setup
@R1
D=0
 
(LOOP)
	@R0
	D = M
	@R1
	D = D * M
  SCREEN
	@END
	D;JEQ
	
(END)
	@END
		0;JMP