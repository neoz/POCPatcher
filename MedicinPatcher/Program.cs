using dnlib.DotNet;
using dnlib.DotNet.Emit;
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
            var asm = typeof(Program).Assembly;
            
            ModuleDefMD module = ModuleDefMD.Load("SampleConsoleNeedCrack.exe");

            var type1 = asm.GetType("MedicinPatcher.Medicin");
            Importer importer = new Importer(module);
            ITypeDefOrRef consoleRef = importer.Import(type1);
            var method = type1.GetMethod("Patch");
            IMethod patchMethod = importer.Import(method);

            var init = module.GlobalType.FindOrCreateStaticConstructor();
            init.Body.Instructions.Insert(0, Instruction.Create(OpCodes.Call, patchMethod));

            module.Write("SampleConsoleNeedCrack-patched.exe");
        }
    }
}
