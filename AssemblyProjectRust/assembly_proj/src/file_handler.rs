use std::fs;
use std::fs::File;
use std::io;
use std::os::windows::prelude::{OpenOptionsExt, FileExt};
use std::path::PathBuf;

pub struct FileHandler;

impl FileHandler {
    pub fn read_per_line(&self, file_path: &String) -> io::Result<String> {
        fs::read_to_string(file_path)
    }

    pub fn save_as_binary_hack(&self, file_path: &String, file_content: String) -> () {
        let mut path = PathBuf::from(file_path);
        path.set_extension("hack");
        let mut f: File = File::options().write(true).open(path.to_str().unwrap().to_string()).unwrap().into();
        f.seek_write(file_content.as_bytes(), 0);
    }
}