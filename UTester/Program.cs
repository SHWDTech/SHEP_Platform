using System;
using SHEP_Platform.Common;

namespace UTester
{
    class Program
    {
        static void Main(string[] args)
        {
            var b = Global.StringToBytes("1024");
            foreach (var b1 in b)
            {
                Console.WriteLine($"{b1:X2}");
            }

            Console.ReadKey();
        }
    }
}
