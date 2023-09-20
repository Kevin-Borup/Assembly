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
            VMtoAsmParser parser = new VMtoAsmParser();

            for (int i = 0; i < filePaths.Length; i++)
            {
                string[] fileContent = new string[] { "" };

                if (fileManager.IsPathFolder(filePaths[i]))
                {
                    string[] filesInFolder = fileManager.GetAsmFilesInFolder(filePaths[i]);

                    for (int j = 0; j < filesInFolder.Length; j++)
                    {
                        fileContent.Concat(fileManager.GetContentAsStrings(filesInFolder[j]));
                    }
                }
                else
                {
                    fileContent = fileManager.GetContentAsStrings(filePaths[i]);
                }

                string[] parsedFileContent = parser.ConvertVMtoASM(fileContent);

                fileManager.SaveAsAsmFile(filePaths[i], parsedFileContent);
            }
        }


    }
}