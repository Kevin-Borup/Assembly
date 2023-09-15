use std::fs;
use std::fs::File;
use std::io::{self, Write};
use std::mem::size_of;
use std::path::PathBuf;

pub struct FileHandler;

impl FileHandler {
    pub fn read_per_line(&self, file_path: &String) -> io::Result<String> {
        fs::read_to_string(file_path)
    }

    pub fn save_as_binary_hack(&self, file_path: &String, file_content: Vec<u16>) -> () {
        let mut path = PathBuf::from(file_path);
        path.set_extension("hack");
        let mut f: File = File::options()
            .write(true)
            .open(path.to_str().unwrap().to_string())
            .unwrap()
            .into();

        let len = size_of::<u16>() * file_content.len();
        for instruction in file_content {
            f.write_all(unsafe {
                std::slice::from_raw_parts(instruction.to_owned() as *const u8, len)
            })
            .expect("TODO: panic message");
        }
    }
}
