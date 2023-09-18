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

        public string[] ConvertVMtoASM(string[] content)
        {
            string[] vmcode = CleanedVMcode(content);
            List<string> convertedToAsm = new List<string>();

            for (int i = 0; i < vmcode.Length; i++)
            {
                var cmd_values = vmcode[i].Split(' ');
                string cmd = cmd_values[0];

                if (arithmicCmds.Contains(cmd))
                {
                    convertedToAsm.Add(ArithmicCommand(cmd)); // Cmd
                }
                else if (memoryCmd.Contains(cmd))
                {
                    if (cmd_values.Length < 3) throw new Exception("Not enough Memory command variables defined.");
                    string segment = cmd_values[1];
                    int index = int.Parse(cmd_values[2]);

                    convertedToAsm.Add(MemoryCommand(cmd, segment, index)); // Cmd Segment Value
                }
                else if (branchingCmds.Contains(cmd))
                {
                    if (cmd_values.Length < 2) throw new Exception("Not enough Branching command variables defined.");
                    string label = cmd_values[1];

                    convertedToAsm.Add(BranchingCommands(cmd, label)); // Cmd Label
                }
                else if (funcCmds.Contains(cmd))
                {
                    string fnName = "";
                    string args = "";

                    if (cmd != "return")
                    {
                        if (cmd_values.Length < 2) throw new Exception("Not enough Function command variables defined.");

                        fnName = cmd_values[1];
                        args = cmd_values[2];
                    }

                    convertedToAsm.Add(FunctionCommands(cmd, fnName, args)); // Cmd FunctionName Args || Cmd=return
                }
            }

            return convertedToAsm.ToArray();
        }

        private string[] CleanedVMcode(string[] content)
        {
            string[] cleanedCode = new string[content.Length];

            for (int i = 0; i < content.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(content[i])) cleanedCode[i] = content[i].Trim().Substring(0, content[i].IndexOf("//"));
            }

            return cleanedCode;
        }

        private string ArithmicCommand(string cmd)
        {

            return string.Empty;
        }

        private string MemoryCommand(string cmd, string segment, int index)
        {
            Dictionary<string, string> memSegments = new Dictionary<string, string>()
            {
                {"local", "LCL" },
                {"argument", "ARG" },
                {"pointer", "POINTER" },
            };

            //Pointer 1 = THAT, Pointer 0 = THIS

            StringBuilder asmCode = new StringBuilder();


            if (cmd.Equals("push"))
            {
                if (memSegments.TryGetValue(segment, out var address))
                { // Local, Argument, This, That
                    if (address.Equals("POINTER"))
                    {
                        if (index.Equals(0)) address = "THIS";
                        else if (index.Equals(1)) address = "THAT";
                    }

                    asmCode.Append($"@{index}");
                    asmCode.Append($"D=A");
                    asmCode.Append($"@{address}");
                    asmCode.Append($"D=D+A");
                    asmCode.Append($"M=D");
                    asmCode.Append($"D=A");
                    asmCode.Append($"@SP");
                    asmCode.Append($"A=D");
                    asmCode.Append($"M=M+1");
                }
                else
                { // Constant
                    asmCode.Append($"@{index}");
                    asmCode.Append($"D=A");
                    asmCode.Append($"@SP");
                    asmCode.Append($"A=M");
                    asmCode.Append($"M=D");
                    asmCode.Append($"@SP");
                    asmCode.Append($"M=M+1");
                }
            } 
            else if (cmd.Equals("pop"))
            {
                if (memSegments.TryGetValue(segment, out var address))
                { // Local, Argument, This, That
                    asmCode.Append($"@{index}");
                    asmCode.Append($"D=A");
                    asmCode.Append($"@{address}");
                    asmCode.Append($"D=D+A");
                    asmCode.Append($"M=D");
                    asmCode.Append($"@SP");
                    asmCode.Append($"M=M-1");
                    asmCode.Append($"D=A");
                    asmCode.Append($"@{address}");
                    asmCode.Append($"A=D");

                }
                else
                { // Constant
                    asmCode.Append($"@{index}");
                    asmCode.Append($"D=A");
                    asmCode.Append($"@SP");
                    asmCode.Append($"A=M");
                    asmCode.Append($"M=D");
                    asmCode.Append($"@SP");
                    asmCode.Append($"M=M+1");
                }
            }


            return asmCode.ToString();
        }

        private string BranchingCommands(string cmd, string label)
        {
            return string.Empty;
        }

        private string FunctionCommands(string cmd, string fnName = "", string args = "")
        {
            return string.Empty;
        }
    }
}
