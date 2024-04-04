using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleConsoleNeedCrack
{
    internal class Program
    {
        public static bool IsRegistered()
        {
            Register register = new Register();
            return register.IsRegistered();
        }
        static void Main(string[] args)
        {
            if (IsRegistered())
            {
                Console.Write("Registered ");
                if (License.IsValidActivation())
                {
                    Console.WriteLine("and Activated");
                    Console.WriteLine("You're done, Hello World!");
                }
                else
                {
                    Console.WriteLine("but not Activated");
                }
            }
            else
            {
                Console.WriteLine("Not Registered");
            }
            Console.ReadLine();
        }
    }
}
