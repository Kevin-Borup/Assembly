use std::{collections::HashMap, str::Bytes, vec};

pub struct AsmBinaryParser{
    symbol_table: HashMap<String, u16>,
}

impl AsmBinaryParser {
    pub fn new() -> AsmBinaryParser {
        let mut predefined_table: HashMap<String, u16> = HashMap::new();
        for x in 0..15 {
            let num = x.to_string();
            predefined_table.insert( "R".to_string() + &num, x);
        }
        predefined_table.insert("SCREEN".to_string(), 16384);
        predefined_table.insert("KBD".to_string(), 24576);
        
        predefined_table.insert("SP".to_string(), 0);
        predefined_table.insert("LCL".to_string(), 1);
        predefined_table.insert("ARG".to_string(), 2);
        predefined_table.insert("THIS".to_string(), 3);
        predefined_table.insert("THAT".to_string(), 4);
        
        AsmBinaryParser { 
            symbol_table: predefined_table }
    }

    pub fn parse_to_binary(&self, content: String) -> Vec<u16> {
        let hack_content = String::new();
        let asm_content = Self::clean_content_pure_asm(content);

        self.get_all_c_instructions(&asm_content);
        self.translate_all_instructions(&asm_content)
    }

    fn clean_content_pure_asm(content: String) -> String {
        let mut cleanedContent = String::new();
        for c in content.lines() {
            let d = &c[0..c.find("//").unwrap_or(c.len())];
            if !d.is_empty() {
                let e = d;
                cleanedContent.push_str(e);
                dbg!(e);
            }
        }
        return cleanedContent;
    }

    fn get_all_c_instructions(&mut self, content: &String){
        for (i, c) in content.lines().enumerate() {
            if !c.starts_with("@") {
                self.symbol_table.insert(c.to_string(), u16::try_from(i).unwrap() as u16);
            }
        }
    }

    fn translate_all_instructions(&mut self, content: &String) -> Vec<u16> {
        let mut instruction_set: Vec<u16> = Vec::new();


        for (i, c) in content.lines().enumerate() {
            let mut n = 16;
            if c.starts_with("@") {
                if self.symbol_table.contains_key(&c.to_string()) { // A Instruction, predefined variable
                    let a_inst = self.symbol_table.get_key_value(&c.to_string()).unwrap().1.to_be_bytes();
                    instruction_set.push(((a_inst[0] as u16) << 8) | a_inst[1] as u16);
                } else { // A Instruction, user defined variable
                    self.symbol_table.insert(c.to_string(), n);
                    let a_inst = n.to_be_bytes();
                    instruction_set.push(((a_inst[0] as u16) << 8) | a_inst[1] as u16);
                    n += 1;
                }
            } else { // C Instruction
                    let c_inst_parts: Vec<&str> = c.split(&['=', ';']).collect();

                    let dest: &str = &c_inst_parts[0].replace(" ", ""); // Remove all spaces
                    let comp: &str = &c_inst_parts[1].replace(" ", ""); // Remove all spaces
                    let jump  = &c_inst_parts[2].replace(" ", ""); // Remove all spaces

                    let a = if c.contains("M") {"1"} else {"0"};
                    
                    let mut c1: &str = "0";
                    let mut c2: &str = "0";
                    let mut c3: &str = "0";
                    let mut c4: &str = "0";
                    let mut c5: &str = "0";
                    let mut c6: &str = "0";

                    let mut d1: &str = "0";
                    let mut d2: &str = "0";
                    let mut d3: &str = "0";

                    let mut j1: &str = "0";
                    let mut j2: &str = "0";
                    let mut j3: &str = "0";

                    //comp
                    let mut c_letter: &str = if a.eq("0") {"A"} else {"M"};
                    let li = "!".to_owned() + c_letter;     // !M/!A
                    let lm = "-".to_owned() + c_letter;     // !M/!A
                    let lp1 = c_letter.to_owned() + "+1" ;  // M+1/A+1
                    let lm1 = c_letter.to_owned() + "-1" ;  // M-1/A-1
                    let dpl = "D+".to_owned() + c_letter;   // D+M/D+A
                    let dml = "D-".to_owned() + c_letter;   // D-M/D-A
                    let lmd = c_letter.to_owned() + "-D";   // M-D/A-D
                    let dal = "D&".to_owned() + c_letter;   // D&M/D&A
                    let dorl = "D|".to_owned() + c_letter;  // D|M/D|A

                    match comp {
                        "0"             => {c1 = "1"; c2 = "0"; c3 = "1"; c4 = "0"; c5 = "1"; c6 = "0"},
                        "1"             => {c1 = "1"; c2 = "1"; c3 = "1"; c4 = "1"; c5 = "1"; c6 = "1"},
                        "-1"            => {c1 = "1"; c2 = "1"; c3 = "1"; c4 = "0"; c5 = "1"; c6 = "0"},
                        "D"             => {c1 = "0"; c2 = "0"; c3 = "1"; c4 = "1"; c5 = "0"; c6 = "0"},
                        c_letter  => {c1 = "1"; c2 = "1"; c3 = "0"; c4 = "0"; c5 = "0"; c6 = "0"},
                        "!D"            => {c1 = "0"; c2 = "0"; c3 = "1"; c4 = "1"; c5 = "0"; c6 = "1"},
                        li        => {c1 = "1"; c2 = "1"; c3 = "0"; c4 = "0"; c5 = "0"; c6 = "1"},
                        "-D"            => {c1 = "0"; c2 = "0"; c3 = "1"; c4 = "1"; c5 = "1"; c6 = "1"},
                        lm        => {c1 = "1"; c2 = "1"; c3 = "0"; c4 = "0"; c5 = "1"; c6 = "1"},
                        "D+1"           => {c1 = "0"; c2 = "1"; c3 = "1"; c4 = "1"; c5 = "1"; c6 = "1"},
                        lp1       => {c1 = "1"; c2 = "1"; c3 = "0"; c4 = "1"; c5 = "1"; c6 = "1"},
                        "D-1"           => {c1 = "0"; c2 = "0"; c3 = "1"; c4 = "1"; c5 = "1"; c6 = "0"},
                        lm1       => {c1 = "1"; c2 = "1"; c3 = "0"; c4 = "0"; c5 = "1"; c6 = "0"},
                        dpl       => {c1 = "0"; c2 = "0"; c3 = "0"; c4 = "0"; c5 = "1"; c6 = "0"},
                        dml       => {c1 = "0"; c2 = "1"; c3 = "0"; c4 = "0"; c5 = "1"; c6 = "1"},
                        lmd       => {c1 = "0"; c2 = "0"; c3 = "0"; c4 = "1"; c5 = "1"; c6 = "1"},
                        dal       => {c1 = "0"; c2 = "0"; c3 = "0"; c4 = "0"; c5 = "0"; c6 = "0"},
                        dorl      => {c1 = "0"; c2 = "1"; c3 = "0"; c4 = "1"; c5 = "0"; c6 = "1"},
                        _ => {}

                    }

                    //dest
                    if dest.contains("A") {d1 = "1"}
                    if dest.contains("D") {d2 = "1"}
                    if dest.contains("M") {d3 = "1"}

                    //jump
                    match jump.as_str() {
                        "JGT" => {j1 = "0"; j2 = "0"; j3 = "1"},
                        "JEQ" => {j1 = "0"; j2 = "1"; j3 = "0"},
                        "JGE" => {j1 = "0"; j2 = "1"; j3 = "1"},
                        "JLT" => {j1 = "1"; j2 = "0"; j3 = "0"},
                        "JNE" => {j1 = "1"; j2 = "0"; j3 = "1"},
                        "JLE" => {j1 = "1"; j2 = "1"; j3 = "0"},
                        "JMP" => {j1 = "1"; j2 = "1"; j3 = "1"},
                        _ => {}
                    }

                    let mut c_inst_string = String::new();
                    c_inst_string.push_str("111");
                    c_inst_string.push_str(a);
                    c_inst_string.push_str(c1);
                    c_inst_string.push_str(c2);
                    c_inst_string.push_str(c3);
                    c_inst_string.push_str(c4);
                    c_inst_string.push_str(c5);
                    c_inst_string.push_str(c6);
                    c_inst_string.push_str(d1);
                    c_inst_string.push_str(d2);
                    c_inst_string.push_str(d3);
                    c_inst_string.push_str(j1);
                    c_inst_string.push_str(j2);
                    c_inst_string.push_str(j3);

                    let c_inst  = c_inst_string.as_bytes();
                    instruction_set.push(((c_inst[0] as u16) << 8) | c_inst[1] as u16);
            }
        }

        return instruction_set;
    }
}