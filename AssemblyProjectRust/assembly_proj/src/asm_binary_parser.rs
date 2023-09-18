use std::collections::HashMap;
use std::convert::TryFrom;

pub struct AsmBinaryParser {
    symbol_table: HashMap<String, u16>,
    label_lineshift_inc: usize,
    variable_address_inc: u16,
}

impl AsmBinaryParser {
    pub fn new() -> AsmBinaryParser {
        let mut predefined_table: HashMap<String, u16> = HashMap::new();
        for x in 0..16 {
            predefined_table.insert(format!("R{}", x), x);
        }

        predefined_table.insert("SCREEN".to_string(), 16384);
        predefined_table.insert("KBD".to_string(), 24576);

        predefined_table.insert("SP".to_string(), 0);
        predefined_table.insert("LCL".to_string(), 1);
        predefined_table.insert("ARG".to_string(), 2);
        predefined_table.insert("THIS".to_string(), 3);
        predefined_table.insert("THAT".to_string(), 4);

        AsmBinaryParser {
            symbol_table: predefined_table,
            label_lineshift_inc: 0,
            variable_address_inc: 16,
        }
    }

    pub fn parse_to_binary(&mut self, content: String) -> Vec<u16> {
        let mut asm_content = Self::clean_content_pure_asm(&content);
        self.get_all_labels_instructions(&asm_content);
        // dbg!(&asm_content);
        asm_content = self.remove_all_labels_instructions(&asm_content);
        // dbg!(&asm_content);

        self.translate_all_instructions(&asm_content)
    }

    fn clean_content_pure_asm(content: &str) -> String {
        content
            .lines()
            .filter_map(|line| line.split("//").next())
            .map(|x| x.replace(" ", ""))
            .filter(|s| !s.is_empty())
            .collect::<Vec<_>>()
            .join("\n")
    }

    fn get_all_labels_instructions(&mut self, content: &str) {
        for (i, line) in content.lines().enumerate() {
            if line.starts_with('(') {
                // let converted_to_label = line.replace('(', "@").replace(')', "").to_string();
                // dbg!(&converted_to_label);
                // dbg!(&i);
                // dbg!(&self.label_lineshift_inc);
                // dbg!(&(i-self.label_lineshift_inc));
                self.symbol_table
                    .insert(
                        line.replace('(', "").replace(')', "").to_string(), //Converts (XXX) -> XXX
                        u16::try_from(i-self.label_lineshift_inc).unwrap());
                self.label_lineshift_inc += 1;
            }
        }
    }

    fn remove_all_labels_instructions(&mut self, content: &str) -> String {
        content
            .lines()
            .filter(|line| !line.starts_with('('))
            .collect::<Vec<_>>()
            .join("\n")
    }

    fn translate_all_instructions(&mut self, content: &str) -> Vec<u16> {
        content
            .lines()
            .map(|line| self.translate_instruction(line))
            .collect::<Vec<u16>>()
    }

    fn translate_instruction(&mut self, instruction: &str) -> u16 {
        match instruction.starts_with('@') {
            true => self.translate_a_instruction(instruction),
            false => self.translate_c_instruction(instruction),
        }
    }

    fn translate_a_instruction(&mut self, instruction: &str) -> u16 {
        // dbg!(&instruction);

        let symbol = &instruction[1..];

        if let Ok(value) = symbol.parse::<u16>() {
            return value;
        }

        match self.symbol_table.get(symbol) {
            Some(val) => {
                // dbg!(&val);
                *val
            },
            None => {
                let new_address = self.variable_address_inc;
                self.variable_address_inc += 1;
                self.symbol_table.insert(symbol.to_string(), new_address);
                // dbg!(&new_address);
                new_address
            }
        }
    }

    fn translate_c_instruction(&self, instruction: &str) -> u16 {
        // dbg!(&instruction);
        let prepared_inst = match instruction.contains('=') {
            true => instruction.into(),
            false => format!("null={}", instruction),
        };

        let dest_comp_jump: Vec<&str> = prepared_inst.split(&['=', ';']).collect();

        let dest = Self::dest_bits(dest_comp_jump.get(0).unwrap_or(&""));
        let comp = Self::comp_bits(dest_comp_jump.get(1).unwrap_or(&""));
        let jump = Self::jump_bits(dest_comp_jump.get(2).unwrap_or(&""));
        // dbg!(&dest);
        // dbg!(&comp);
        // dbg!(&jump);


        let a = if dest_comp_jump.get(1).unwrap_or(&"").contains("M") {"1"} else {"0"};
        // dbg!(&a);

        let binary = format!("111{}{}{}{}", a, comp, dest, jump);
        // dbg!(&binary);
        u16::from_str_radix(&binary, 2).unwrap()
    }

    // A catalog collection  ---- Could also be solved by checking if contains A -> d1 = 1, D -> d2 = 1, M -> d3 = 1
    fn dest_bits(dest: &str) -> &str {
        match dest {
            "null" => "000",
            "M" => "001",
            "D" => "010",
            "MD" | "DM" => "011",
            "A" => "100",
            "AM" | "MA" => "101",
            "AD" | "DA" => "110",
            "AMD" | "ADM" | "MDA" | "MAD" | "DMA" | "DAM" => "111",
            _ => "000",
        }
    }

    fn comp_bits(comp: &str) -> &str {
        match comp {
            "0"             => "101010",
            "1"             => "111111",
            "-1"            => "111010",
            "D"             => "001100",
            "A" | "M"       => "110000",
            "!D"            => "001101",
            "!A" | "!M"     => "110001",
            "-D"            => "001111",
            "-A" | "-M"     => "110011",
            "D+1"           => "011111",
            "A+1" | "M+1"   => "110111",
            "D-1"           => "001110",
            "A-1" | "M-1"   => "110010",
            "D+A" | "D+M"   => "000010",
            "D-A" | "D-M"   => "010011",
            "A-D" | "M-D"   => "000111",
            "D&A" | "D&M"   => "000000",
            "D|A" | "D|M"   => "010101",
            _ => "",
        }
    }

    fn jump_bits(jump: &str) -> &str {
        match jump {
            "JGT" => "001",
            "JEQ" => "010",
            "JGE" => "011",
            "JLT" => "100",
            "JNE" => "101",
            "JLE" => "110",
            "JMP" => "111",
            _ => "000",
        }
    }
}
