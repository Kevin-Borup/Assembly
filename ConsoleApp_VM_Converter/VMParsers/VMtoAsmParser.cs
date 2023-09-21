using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp_VM_Converter.VM_Parsers
{
    internal class VMtoAsmParser
    {
        private int labelInc;
        private int functionLabelInc;

        private string currentFileName;

        private Dictionary<string, int> staticLabels;

        public VMtoAsmParser()
        {
            labelInc = 0;
            functionLabelInc = 0;
            staticLabels = new Dictionary<string, int>();
        }

        string[] arithmicCmds = new string[]
        {
            "add",
            "sub",
            "neg",
            "eq",
            "gt",
            "lt",
            "and",
            "or",
            "not",
        };

        string[] memoryCmd = new string[]
        {
            "push",
            "pop",
        };

        string[] branchingCmds = new string[]
        {
            "label",
            "goto",
            "if-goto",
        };

        string[] funcCmds = new string[]
        {
            "function",
            "call",
            "return",
        };

        public string[] ConvertVMtoASM(string[] content, string fileName)
        {
            this.currentFileName = fileName;

            string[] vmcode = CleanedVMcode(content);

            List<string> convertedToAsm = new List<string>();

            for (int i = 0; i < vmcode.Length; i++)
            {
                convertedToAsm.Add(TranslateCmdType(vmcode[i]));
            }

            return convertedToAsm.ToArray();
        }

        private string[] CleanedVMcode(string[] content)
        {
            List<string> cleanedCode = new List<string>();

            for (int i = 0; i < content.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(content[i]))
                {
                    string cleanedLine = content[i];
                    if (cleanedLine.Contains("//"))
                    {
                        cleanedLine = cleanedLine.Substring(0, cleanedLine.IndexOf("//"));
                    }

                    cleanedLine = cleanedLine.TrimEnd();

                    if (!string.IsNullOrWhiteSpace(cleanedLine)) cleanedCode.Add(cleanedLine);
                }
            }

            return cleanedCode.ToArray();
        }

        public string SysInitializationAsm()
        {
            StringBuilder sysInit = new StringBuilder();

            sysInit.AppendLine("@256");
            sysInit.AppendLine("D=A");
            sysInit.AppendLine("@SP");
            sysInit.AppendLine("M=D");
            sysInit.AppendLine("@RETURN_LABEL" + functionLabelInc);
            sysInit.AppendLine("D=A");
            sysInit.AppendLine("@SP");
            sysInit.AppendLine("A=M");
            sysInit.AppendLine("M=D");
            sysInit.AppendLine("@SP");
            sysInit.AppendLine("M=M+1");
            sysInit.AppendLine("@LCL");
            sysInit.AppendLine("D=M");
            sysInit.AppendLine("@SP");
            sysInit.AppendLine("A=M");
            sysInit.AppendLine("M=D");
            sysInit.AppendLine("@SP");
            sysInit.AppendLine("M=M+1");
            sysInit.AppendLine("@ARG");
            sysInit.AppendLine("D=M");
            sysInit.AppendLine("@SP");
            sysInit.AppendLine("A=M");
            sysInit.AppendLine("M=D");
            sysInit.AppendLine("@SP");
            sysInit.AppendLine("M=M+1");
            sysInit.AppendLine("@THIS");
            sysInit.AppendLine("D=M");
            sysInit.AppendLine("@SP");
            sysInit.AppendLine("A=M");
            sysInit.AppendLine("M=D");
            sysInit.AppendLine("@SP");
            sysInit.AppendLine("M=M+1");
            sysInit.AppendLine("@THAT");
            sysInit.AppendLine("D=M");
            sysInit.AppendLine("@SP");
            sysInit.AppendLine("A=M");
            sysInit.AppendLine("M=D");
            sysInit.AppendLine("@SP");
            sysInit.AppendLine("M=M+1");
            sysInit.AppendLine("@SP");
            sysInit.AppendLine("D=M");
            sysInit.AppendLine("@5");
            sysInit.AppendLine("D=D-A");
            sysInit.AppendLine("@0");
            sysInit.AppendLine("D=D-A");
            sysInit.AppendLine("@ARG");
            sysInit.AppendLine("M=D");
            sysInit.AppendLine("@SP");
            sysInit.AppendLine("D=M");
            sysInit.AppendLine("@LCL");
            sysInit.AppendLine("M=D");
            sysInit.AppendLine("@Sys.init");
            sysInit.AppendLine("0;JMP");
            sysInit.AppendLine($"(RETURN_LABEL{functionLabelInc})");

            functionLabelInc++;

            return sysInit.ToString();
        }

        private string TranslateCmdType(string vmcodeLine)
        {
            var cmd_values = vmcodeLine.Split(' ');
            string cmd = cmd_values[0];

            if (arithmicCmds.Contains(cmd))
            {
                return ArithmicCommand(cmd); // Cmd
            }
            else if (memoryCmd.Contains(cmd))
            {
                if (cmd_values.Length < 3) throw new Exception("Not enough Memory command variables defined.");
                string segment = cmd_values[1];
                int index = int.Parse(cmd_values[2]);

                return MemoryCommand(cmd, segment, index); // Cmd Segment Value
            }
            else if (branchingCmds.Contains(cmd))
            {
                if (cmd_values.Length < 2) throw new Exception("Not enough Branching command variables defined.");
                string label = cmd_values[1];

                return BranchingCommands(cmd, label); // Cmd Label
            }
            else if (funcCmds.Contains(cmd))
            {
                string fnName = "";
                int args = 0;

                if (cmd != "return")
                {
                    if (cmd_values.Length < 2) throw new Exception("Not enough Function command variables defined.");

                    fnName = cmd_values[1];
                    args = int.Parse(cmd_values[2]);
                }

                return FunctionCommands(cmd, fnName, args); // Cmd FunctionName Args || Cmd=return
            }

            throw new Exception("Command not recognized");
        }

        private string ArithmicCommand(string cmd)
        {
            Dictionary<string, string> arithmicCmd = new Dictionary<string, string>()
            {
                {"add", "M+D" },
                {"sub", "M-D" },
                {"or", "M|D" },
                {"and", "M&D" },
            };

            Dictionary<string, string> comparisonCmd = new Dictionary<string, string>()
            {
                {"gt", "JLE" },
                {"lt", "JGE" },
                {"eq", "JNE" },
            };

            StringBuilder asmCode = new StringBuilder();

            if (arithmicCmd.TryGetValue(cmd, out string cmdAsm))
            { // add, sub, or, and
                asmCode.AppendLine("@SP");
                asmCode.AppendLine("AM=M-1");
                asmCode.AppendLine("D=M");
                asmCode.AppendLine("A=A-1");
                asmCode.AppendLine("M=" + cmdAsm);
            }
            else if (comparisonCmd.TryGetValue(cmd, out string jmpAsm))
            { // eq, lt, gt
                asmCode.AppendLine("@SP");
                asmCode.AppendLine("AM=M-1");
                asmCode.AppendLine("D=M");
                asmCode.AppendLine("A=A-1");
                asmCode.AppendLine("D=M-D");
                asmCode.AppendLine("@FALSE" + labelInc);
                asmCode.AppendLine("D;" + jmpAsm);
                asmCode.AppendLine("@SP");
                asmCode.AppendLine("A=M-1");
                asmCode.AppendLine("M=-1");
                asmCode.AppendLine("@CONTINUE" + labelInc);
                asmCode.AppendLine("0;JMP");
                asmCode.AppendLine($"(FALSE{labelInc})");
                asmCode.AppendLine("@SP");
                asmCode.AppendLine("A=M-1");
                asmCode.AppendLine("M=0");
                asmCode.AppendLine($"(CONTINUE{labelInc})");
                labelInc++;
            }
            else
            { // negate, not
                cmdAsm = "!M";

                if (cmd.Equals("neg"))
                {
                    asmCode.AppendLine("D=0");
                    cmdAsm = "D-M";
                }

                asmCode.AppendLine("@SP");
                asmCode.AppendLine("A=M-1");
                asmCode.AppendLine("M=" + cmdAsm);
            }

            return asmCode.ToString(); ;
        }

        private string MemoryCommand(string cmd, string segment, int index)
        {
            Dictionary<string, string> memSegments = new Dictionary<string, string>()
            {
                {"local", "LCL" },
                {"argument", "ARG" },
                {"temp", "R5" },
                {"this", "THIS" },
                {"that", "THAT" },
            };

            //Pointer 1 = THAT, Pointer 0 = THIS

            StringBuilder asmCode = new StringBuilder();


            if (cmd.Equals("push"))
            {
                if (memSegments.TryGetValue(segment, out string address))
                { // Local, Argument, This, That, Temp
                    address = address.Replace("i", (index - 1).ToString()); // Temp RAM address start + move
                    if (segment.Equals("temp")) index = 5 + index;

                    asmCode.AppendLine($"@{address}");
                    asmCode.AppendLine($"D=M");
                    asmCode.AppendLine($"@{index}");
                    asmCode.AppendLine($"A=D+A");
                    //asmCode.AppendLine($"M=D");
                    asmCode.AppendLine($"D=M");
                }
                else if (segment.Equals("static") || segment.Equals("pointer"))
                { // Static, Pointer
                    address = string.Empty;

                    if (segment.Equals("pointer"))
                    {
                        address = index.Equals(0) ? "THIS" : "THAT";
                    }
                    else //static
                    {
                        address = this.currentFileName + index;
                    }

                    asmCode.AppendLine($"@{address}");
                    asmCode.AppendLine($"D=M");
                }
                else
                { // Constant
                    asmCode.AppendLine($"@{index}");
                    asmCode.AppendLine($"D=A");
                }

                //Increment SP
                asmCode.AppendLine($"@SP");
                asmCode.AppendLine($"A=M");
                asmCode.AppendLine($"M=D");
                asmCode.AppendLine($"@SP");
                asmCode.AppendLine($"M=M+1");

            }
            else if (cmd.Equals("pop"))
            {
                if (memSegments.TryGetValue(segment, out string address))
                { // Local, Argument, This, That, Temp
                    if (segment.Equals("temp")) index = 5 + index;

                    asmCode.AppendLine($"@{address}");
                    asmCode.AppendLine($"D=M");
                    asmCode.AppendLine($"@{index}");
                    asmCode.AppendLine($"D=D+A");
                }
                else if (segment.Equals("static") || segment.Equals("pointer"))
                { // Static, Pointer
                    address = string.Empty;

                    if (segment.Equals("pointer"))
                    {
                        if (index.Equals(0)) address = "THIS";
                        else if (index.Equals(1)) address = "THAT";
                    }
                    else //static
                    {
                        address = this.currentFileName + index;
                    }


                    asmCode.AppendLine($"@{address}");
                    asmCode.AppendLine($"D=A");
                }

                //Decrement SP 
                asmCode.AppendLine($"@R13");
                asmCode.AppendLine($"M=D");
                asmCode.AppendLine($"@SP");
                asmCode.AppendLine($"AM=M-1");
                asmCode.AppendLine($"D=M");
                asmCode.AppendLine($"@R13");
                asmCode.AppendLine($"A=M");
                asmCode.AppendLine($"M=D");
            }


            return asmCode.ToString();
        }

        private string BranchingCommands(string cmd, string label)
        {
            StringBuilder asmCode = new StringBuilder();

            if (cmd.Equals("label")) // Label
            {
                asmCode.AppendLine($"({label})");
            }
            else if (cmd.Equals("goto")) // Go To
            {
                asmCode.AppendLine("@" + label);
                asmCode.AppendLine("0;JMP");
            }
            else if (cmd.Equals("if-goto")) // If Go To
            {
                asmCode.AppendLine("@SP");
                asmCode.AppendLine("AM=M-1");
                asmCode.AppendLine("D=M");
                asmCode.AppendLine("A=A-1");
                asmCode.AppendLine("@" + label);
                asmCode.AppendLine("D;JNE");
            }

            return asmCode.ToString();
        }

        private string FunctionCommands(string cmd, string fnName = "", int argCount = 0)
        {
            StringBuilder asmCode = new StringBuilder();

            if (cmd.Equals("function")) // Define function
            {
                asmCode.AppendLine($"({fnName})");

                for (int i = 0; i < argCount; i++)
                {
                    asmCode.AppendLine("@0");
                    asmCode.AppendLine("D=A");
                    asmCode.AppendLine("@SP");
                    asmCode.AppendLine("A=M");
                    asmCode.AppendLine("M=D");
                    asmCode.AppendLine("@SP");
                    asmCode.AppendLine("M=M+1");
                }
            }
            else if (cmd.Equals("call")) // Call function
            {
                asmCode.AppendLine("@RETURN_LABEL" + functionLabelInc);
                asmCode.AppendLine("D=A");
                asmCode.AppendLine("@SP");
                asmCode.AppendLine("A=M");
                asmCode.AppendLine("M=D");
                asmCode.AppendLine("@SP");
                asmCode.AppendLine("M=M+1");
                asmCode.AppendLine("@LCL");
                asmCode.AppendLine("D=M");
                asmCode.AppendLine("@SP");
                asmCode.AppendLine("A=M");
                asmCode.AppendLine("M=D");
                asmCode.AppendLine("@SP");
                asmCode.AppendLine("M=M+1");
                asmCode.AppendLine("@ARG");
                asmCode.AppendLine("D=M");
                asmCode.AppendLine("@SP");
                asmCode.AppendLine("A=M");
                asmCode.AppendLine("M=D");
                asmCode.AppendLine("@SP");
                asmCode.AppendLine("M=M+1");
                asmCode.AppendLine("@THIS");
                asmCode.AppendLine("D=M");
                asmCode.AppendLine("@SP");
                asmCode.AppendLine("A=M");
                asmCode.AppendLine("M=D");
                asmCode.AppendLine("@SP");
                asmCode.AppendLine("M=M+1");
                asmCode.AppendLine("@THAT");
                asmCode.AppendLine("D=M");
                asmCode.AppendLine("@SP");
                asmCode.AppendLine("A=M");
                asmCode.AppendLine("M=D");
                asmCode.AppendLine("@SP");
                asmCode.AppendLine("M=M+1");
                asmCode.AppendLine("@SP");
                asmCode.AppendLine("D=M");
                asmCode.AppendLine("@5");
                asmCode.AppendLine("D=D-A");
                asmCode.AppendLine("@" + argCount);
                asmCode.AppendLine("D=D-A");
                asmCode.AppendLine("@ARG");
                asmCode.AppendLine("M=D");
                asmCode.AppendLine("@SP");
                asmCode.AppendLine("D=M");
                asmCode.AppendLine("@LCL");
                asmCode.AppendLine("M=D");
                asmCode.AppendLine("@" + fnName);
                asmCode.AppendLine("0;JMP");
                asmCode.AppendLine($"(RETURN_LABEL{functionLabelInc})");

                functionLabelInc++;
            }
            else if (cmd.Equals("return")) // return from function
            {
                asmCode.AppendLine("@LCL");
                asmCode.AppendLine("D=M");
                asmCode.AppendLine("@R11");
                asmCode.AppendLine("M=D");
                asmCode.AppendLine("@5");
                asmCode.AppendLine("A=D-A");
                asmCode.AppendLine("D=M");
                asmCode.AppendLine("@R12");
                asmCode.AppendLine("M=D");
                asmCode.AppendLine("@ARG");
                asmCode.AppendLine("D=M");
                asmCode.AppendLine("@0");
                asmCode.AppendLine("D=D+A");
                asmCode.AppendLine("@R13");
                asmCode.AppendLine("M=D");
                asmCode.AppendLine("@SP");
                asmCode.AppendLine("AM=M-1");
                asmCode.AppendLine("D=M");
                asmCode.AppendLine("@R13");
                asmCode.AppendLine("A=M");
                asmCode.AppendLine("M=D");
                asmCode.AppendLine("@ARG");
                asmCode.AppendLine("D=M");
                asmCode.AppendLine("@SP");
                asmCode.AppendLine("M=D+1");
                asmCode.AppendLine("@R11");
                asmCode.AppendLine("D=M-1");
                asmCode.AppendLine("AM=D");
                asmCode.AppendLine("D=M");
                asmCode.AppendLine("@THAT");
                asmCode.AppendLine("M=D");
                asmCode.AppendLine("@R11");
                asmCode.AppendLine("D=M-1");
                asmCode.AppendLine("AM=D");
                asmCode.AppendLine("D=M");
                asmCode.AppendLine("@THIS");
                asmCode.AppendLine("M=D");
                asmCode.AppendLine("@R11");
                asmCode.AppendLine("D=M-1");
                asmCode.AppendLine("AM=D");
                asmCode.AppendLine("D=M");
                asmCode.AppendLine("@ARG");
                asmCode.AppendLine("M=D");
                asmCode.AppendLine("@R11");
                asmCode.AppendLine("D=M-1");
                asmCode.AppendLine("AM=D");
                asmCode.AppendLine("D=M");
                asmCode.AppendLine("@LCL");
                asmCode.AppendLine("M=D");
                asmCode.AppendLine("@R12");
                asmCode.AppendLine("A=M");
                asmCode.AppendLine("0;JMP");
            }

            return asmCode.ToString();
        }
    }
}
