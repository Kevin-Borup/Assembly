using ConsoleApp_VM_Converter.FileManagers;
using ConsoleApp_VM_Converter.VM_Parsers;

namespace ConsoleApp_VM_Converter
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] filePaths = new string[] { "" };
            if (args.Length == 0 || !args[0].ToLower().EndsWith(".vm"))
            {
                bool proj7part1files = false;
                bool proj7part2files = true;

                filePaths = ProjectData.GetProjectData(proj7part1files, proj7part2files);
            }
            else
            {
                filePaths = new string[args.Length];
                for (int i = 0; i < args.Length; i++)
                {
                    filePaths[i] = args[i];
                }
            }

            FileHandler fileManager = new FileHandler();
            VMtoAsmParser parser;

            for (int i = 0; i < filePaths.Length; i++)
            {
                parser = new VMtoAsmParser();

                string[] parsedFileContent = new string[] { parser.SysInitializationAsm() };

                if (fileManager.IsPathFolder(filePaths[i]))
                {
                    string[] vmFilesInFolder = fileManager.GetAsmFilesInFolder(filePaths[i]);

                    for (int j = 0; j < vmFilesInFolder.Length; j++)
                    {
                        parsedFileContent = parsedFileContent.Concat(parser.ConvertVMtoASM(fileManager.GetContentAsStrings(vmFilesInFolder[j]), Path.GetFileName(vmFilesInFolder[j]))).ToArray();
                    }
                }
                else
                {
                    parsedFileContent = parser.ConvertVMtoASM(fileManager.GetContentAsStrings(filePaths[i]), Path.GetFileName(filePaths[i]));
                }


                fileManager.SaveAsAsmFile(filePaths[i], parsedFileContent);
            }
        }


    }
}