use std::{env, fs::File};

mod file_handler;
mod asm_binary_parser;

use crate::file_handler::FileHandler;
use crate::asm_binary_parser::AsmBinaryParser;

fn main() {
    let file_manager = FileHandler;
    let asm_parser = AsmBinaryParser::new();

    let args: Vec<String> = env::args().collect();
    dbg!(&args);

    let file_path = &args[1];
    
    let file_content: String = file_manager.read_per_line(&file_path).unwrap().to_string();
    dbg!(&file_content);
    let hack_content = asm_parser.parse_to_binary(file_content);

    file_manager.save_as_binary_hack(&file_path, hack_content);
}