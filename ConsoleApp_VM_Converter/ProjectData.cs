using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp_VM_Converter
{
    internal class ProjectData
    {
        public static string[] GetProjectData(bool proj7part1files, bool proj7part2files)
        {
            int pathsLength = proj7part1files ? 5 : 0;
            pathsLength += proj7part2files ? 6 : 0;

            string[] filePaths = new string[pathsLength];

            if (proj7part1files && proj7part2files)
            {
                //BasicTest
                filePaths[0] = "C:\\ZBC Data-Kommunikation\\H3\\Assembly\\nand2tetris\\projects\\07\\MemoryAccess\\BasicTest\\BasicTest.vm";
                //PointerTest
                filePaths[1] = "C:\\ZBC Data-Kommunikation\\H3\\Assembly\\nand2tetris\\projects\\07\\MemoryAccess\\PointerTest\\PointerTest.vm";
                //StaticTest
                filePaths[2] = "C:\\ZBC Data-Kommunikation\\H3\\Assembly\\nand2tetris\\projects\\07\\MemoryAccess\\StaticTest\\StaticTest.vm";
                //SimpleAdd
                filePaths[3] = "C:\\ZBC Data-Kommunikation\\H3\\Assembly\\nand2tetris\\projects\\07\\StackArithmetic\\SimpleAdd\\SimpleAdd.vm";
                //StackTest
                filePaths[4] = "C:\\ZBC Data-Kommunikation\\H3\\Assembly\\nand2tetris\\projects\\07\\StackArithmetic\\StackTest\\StackTest.vm";
                //FibonacciElement
                filePaths[5] = "C:\\ZBC Data-Kommunikation\\H3\\Assembly\\nand2tetris\\projects\\08\\FunctionCalls\\FibonacciElement";
                //NestedCall
                filePaths[6] = "C:\\ZBC Data-Kommunikation\\H3\\Assembly\\nand2tetris\\projects\\08\\FunctionCalls\\NestedCall\\Sys.vm";
                //SimpleFunction
                filePaths[7] = "C:\\ZBC Data-Kommunikation\\H3\\Assembly\\nand2tetris\\projects\\08\\FunctionCalls\\SimpleFunction\\SimpleFunction.vm";
                //StaticsTest
                filePaths[8] = "C:\\ZBC Data-Kommunikation\\H3\\Assembly\\nand2tetris\\projects\\08\\FunctionCalls\\StaticsTest";
                //BasicLoop
                filePaths[9] = "C:\\ZBC Data-Kommunikation\\H3\\Assembly\\nand2tetris\\projects\\08\\ProgramFlow\\BasicLoop\\BasicLoop.vm";
                //FibonacciSeries
                filePaths[10] = "C:\\ZBC Data-Kommunikation\\H3\\Assembly\\nand2tetris\\projects\\08\\ProgramFlow\\FibonacciSeries\\FibonacciSeries.vm";
            }
            else if (proj7part1files)
            {
                //BasicTest
                filePaths[0] = "C:\\ZBC Data-Kommunikation\\H3\\Assembly\\nand2tetris\\projects\\07\\MemoryAccess\\BasicTest\\BasicTest.vm";
                //PointerTest
                filePaths[1] = "C:\\ZBC Data-Kommunikation\\H3\\Assembly\\nand2tetris\\projects\\07\\MemoryAccess\\PointerTest\\PointerTest.vm";
                //StaticTest
                filePaths[2] = "C:\\ZBC Data-Kommunikation\\H3\\Assembly\\nand2tetris\\projects\\07\\MemoryAccess\\StaticTest\\StaticTest.vm";
                //SimpleAdd
                filePaths[3] = "C:\\ZBC Data-Kommunikation\\H3\\Assembly\\nand2tetris\\projects\\07\\StackArithmetic\\SimpleAdd\\SimpleAdd.vm";
                //StackTest
                filePaths[4] = "C:\\ZBC Data-Kommunikation\\H3\\Assembly\\nand2tetris\\projects\\07\\StackArithmetic\\StackTest\\StackTest.vm";
            }
            else
            {
                //FibonacciElement
                filePaths[0] = "C:\\ZBC Data-Kommunikation\\H3\\Assembly\\nand2tetris\\projects\\08\\FunctionCalls\\FibonacciElement";
                //NestedCall
                filePaths[1] = "C:\\ZBC Data-Kommunikation\\H3\\Assembly\\nand2tetris\\projects\\08\\FunctionCalls\\NestedCall";
                //SimpleFunction
                filePaths[2] = "C:\\ZBC Data-Kommunikation\\H3\\Assembly\\nand2tetris\\projects\\08\\FunctionCalls\\SimpleFunction";
                //StaticsTest
                filePaths[3] = "C:\\ZBC Data-Kommunikation\\H3\\Assembly\\nand2tetris\\projects\\08\\FunctionCalls\\StaticsTest";
                //BasicLoop
                filePaths[4] = "C:\\ZBC Data-Kommunikation\\H3\\Assembly\\nand2tetris\\projects\\08\\ProgramFlow\\BasicLoop";
                //FibonacciSeries
                filePaths[5] = "C:\\ZBC Data-Kommunikation\\H3\\Assembly\\nand2tetris\\projects\\08\\ProgramFlow\\FibonacciSeries";
            }

            return filePaths;
        }
    }
}
