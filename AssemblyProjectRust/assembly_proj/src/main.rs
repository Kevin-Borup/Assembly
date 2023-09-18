use std::env;
use std::error::Error;

mod asm_binary_parser;
mod file_handler;

use crate::asm_binary_parser::AsmBinaryParser;
use crate::file_handler::FileHandler;

fn main() -> Result<(), Box<dyn Error>> {
    let mut asm_parser = AsmBinaryParser::new();

    let args: Vec<String> = env::args().collect();

    let file_path = match args.get(1) {
        Some(path) if path.ends_with(".asm") => path,
        _ => {
            panic!("Include a valid .asm-file as argument.");
        }
    };

    let file_content: String = FileHandler::read_per_line(&file_path)?.into();
    let hack_content = asm_parser.parse_to_binary(file_content);

    FileHandler::save_as_binary_hack(&file_path, hack_content);
    Ok(())
}
