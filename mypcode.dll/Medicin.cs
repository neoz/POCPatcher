using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace mypcode
{
    public static class Medicin
    {
        public static Harmony harmony = new HarmonyLib.Harmony("com.mymedicin.patch");

        public static void Postfix(ref bool __result)
        {
            __result = true;
        }
        public static void Patch()
        {
            var asm = Assembly.GetCallingAssembly();
            Type type1 = asm.GetType("SampleConsoleNeedCrack.Program");
            var method1 = type1.GetMethod("IsRegistered");

            var cls = typeof(Medicin);
            var postfix = cls.GetMethod("Postfix");
            var p1 = harmony.CreateProcessor(method1);
            p1.AddPostfix(postfix);
            p1.Patch();

            Type type2 = asm.GetType("SampleConsoleNeedCrack.License");
            var method2 = type2.GetMethod("IsValidActivation");
            var p2 = harmony.CreateProcessor(method2);
            p2.AddPostfix(postfix);
            p2.Patch();
        }
    }
}
