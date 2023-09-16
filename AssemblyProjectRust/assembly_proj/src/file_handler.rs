use std::fs;
use std::fs::File;
use std::io::{self, Write};
use std::path::PathBuf;

pub struct FileHandler;

impl FileHandler {
    pub fn read_per_line(&self, file_path: &String) -> io::Result<String> {
        fs::read_to_string(file_path)
    }

    pub fn save_as_binary_hack(&self, file_path: &String, file_content: Vec<u16>) -> () {
        let mut path = PathBuf::from(file_path);
        path.set_extension("hack");
        let mut f: File = File::create(&path).expect("Unable to create .hack file");

        for instruction in &file_content {
            let string_bytes = format!("{:016b}\n", instruction);
            f.write_all(string_bytes.as_bytes()).expect("Failed to write to .hack file");
        }
    }
}
