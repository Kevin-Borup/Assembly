using ConsoleApp_VM_Converter.FileManagers;
using ConsoleApp_VM_Converter.VM_Parsers;

namespace ConsoleApp_VM_Converter
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0 || !args[0].ToLower().EndsWith(".vm"))
            {
                throw new Exception("Include a valid .vm file as argument");
            }

            FileHandler fileManager = new FileHandler();
            VMtoAsmParser parser = new VMtoAsmParser();

            string filePath = args[0];

            string[] fileContent = fileManager.GetContentAsStrings(filePath);

            string[] parsedFileContent = parser.ConvertVMtoASM(fileContent);

            fileManager.SaveAsAsmFile(filePath, parsedFileContent);
        }
    }
}