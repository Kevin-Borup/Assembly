using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp_VM_Converter.FileManagers
{
    internal class FileHandler
    {
        public string[] GetContentAsStrings(string filePath)
        {
            return File.ReadAllLines(filePath);
        }

        public bool IsPathFolder(string path)
        {
            FileAttributes attr = File.GetAttributes(path);
            return (attr & FileAttributes.Directory) == FileAttributes.Directory;
        }

        public string[] GetAsmFilesInFolder(string folderPath)
        {
            return Directory.GetFiles(folderPath, "*.vm");
        }

        public void SaveAsAsmFile(string filePath, string[] fileContent)
        {

            var asmFile = Path.ChangeExtension(filePath, ".asm");

            try
            {
                using (var stream = File.Create(asmFile))
                {
                    using (var writer =  new StreamWriter(stream))
                    {
                        for (int i = 0; i < fileContent.Length; i++)
                        {
                            writer.Write(fileContent[i]);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Failed to write to file", e);
            }
            
        }
    }
}
