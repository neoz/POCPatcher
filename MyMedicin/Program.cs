using dnlib.DotNet;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MyMedicin
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Using reflection to get the assembly
            Assembly assembly = Assembly.LoadFrom("SampleConsoleNeedCrack.exe");
            MyMedicin.Patch(assembly);
            assembly.EntryPoint.Invoke(null, new object[] { args });

        }
    }
}
