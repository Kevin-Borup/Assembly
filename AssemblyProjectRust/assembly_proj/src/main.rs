use std::env;

mod asm_binary_parser;
mod file_handler;

use crate::asm_binary_parser::AsmBinaryParser;
use crate::file_handler::FileHandler;

fn main() {
    let file_manager = FileHandler;
    let mut asm_parser = AsmBinaryParser::new();

    let args: Vec<String> = env::args().collect();

    if !args[1].ends_with(".asm") {
        return;
    }

    let file_path = &args[1];

    let file_content: String = file_manager.read_per_line(&file_path).unwrap().into();
    let hack_content = asm_parser.parse_to_binary(file_content);

    file_manager.save_as_binary_hack(&file_path, hack_content);
}
