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

        public void SaveAsAsmFile(string filePath, string[] fileContent)
        {

            var asmFile = Path.ChangeExtension(filePath, ".asm");

            try
            {
                using (var stream = File.Create(asmFile))
                {
                    byte[] buffer = new byte[fileContent.Length];
                    for (int i = 0; i < fileContent.Length; i++)
                    {
                        buffer[i] = Convert.ToByte(fileContent);
                    }

                    stream.Write(buffer);
                }
            }
            catch (Exception e)
            {
                throw new Exception("Failed to write to file", e);
            }
            
        }
    }
}
