use std::collections::HashMap;

pub struct AsmBinaryParser{
    symbol_table: HashMap<String, u16>,
}

impl AsmBinaryParser {
    pub fn new() -> AsmBinaryParser{
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

    pub fn parse_to_binary(&self, content: String) -> String {
        let hack_content = String::new();
        let asm_content = Self::clean_content_pure_asm(content);



        return hack_content;
    }

    fn clean_content_pure_asm(content: String) -> String {
        let mut cleanedContent = String::new();
        for c in content.lines() {
            // dbg!(&c);
            let d = &c[0..c.find("//").unwrap_or(c.len())];
            // dbg!(&d);
            if !d.is_empty() {
                let e = d;
                cleanedContent.push_str(e);
                dbg!(e);
            }
        }
        return cleanedContent;
    }
}