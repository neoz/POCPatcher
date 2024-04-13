using dnlib.DotNet;
using dnlib.DotNet.Emit;
using dnlib.DotNet.Writer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MedicinPatcher
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var targetFile = "";
            bool isNative = false;
            if (args.Length > 0)
            {
                // "-n" : native mode patching
                // "-h" : print help
                // else : print help
                if (args[0] == "-n")
                {
                    isNative = true;
                }
                else if (args[0] == "-h")
                {
                    printHelp();
                    return;
                }
                else
                {
                    if (args[0].StartsWith("-"))
                    {
                        // print  invalid argument
                        Console.WriteLine("Invalid argument: " + args[0]);
                        Console.WriteLine();
                        printHelp();
                        return;
                    }
                    else
                    { 
                        // get argument as target file and check if it exists, if not then print error and return
                        if (!System.IO.File.Exists(args[0]))
                        {
                            Console.WriteLine("File not found: " + args[0]);
                            Console.WriteLine();
                            printHelp();
                            return;
                        }
                        targetFile = args[0];
                    }
                }
            } else
            {
                printHelp();
                return;
            }

            var asm = typeof(Program).Assembly;

            /*
             * try to generate output file name based on target file name
             * if target file is SampleConsoleNeedCrack.exe then output file will be SampleConsoleNeedCrack-patched.exe
             * if isNative is true then output file will be SampleConsoleNeedCrack-patched-native.exe
            */
            var outputFileName = targetFile.Replace(".exe", "-patched");
            if (isNative)
            {
                outputFileName += "-native";
            }
            outputFileName += ".exe";

            // Load target file
            Console.WriteLine($"Loading target file {targetFile}");
            ModuleDefMD module = ModuleDefMD.Load(targetFile);

            var type1 = typeof(mypcode.Medicin);
            Importer importer = new Importer(module);
            ITypeDefOrRef consoleRef = importer.Import(type1);
            var method = type1.GetMethod("Patch");
            IMethod patchMethod = importer.Import(method);

            var init = module.GlobalType.FindOrCreateStaticConstructor();
            init.Body.Instructions.Insert(0, Instruction.Create(OpCodes.Call, patchMethod));

            if (isNative)
            {
                NativeModuleWriterOptions options = new NativeModuleWriterOptions(module,true);
                options.MetadataOptions.Flags |= MetadataFlags.PreserveAll;
                Console.WriteLine($"Writing native mode patched file {outputFileName}");
                module.NativeWrite(outputFileName,options);
            }
            else
            {
                ModuleWriterOptions options = new ModuleWriterOptions(module);
                options.MetadataOptions.Flags |= MetadataFlags.PreserveAll;
                Console.WriteLine($"Writing patched file {outputFileName}");
                module.Write(outputFileName, options);
            }
        }

        private static void printHelp()
        {
            Console.WriteLine("Usage: MedicinPatcher.exe [option] targetfile");
            Console.WriteLine();
            Console.WriteLine("Options:");
            Console.WriteLine("  -n : Native mode patching");
            Console.WriteLine("  -h : Print this help");
        }
    }
}
