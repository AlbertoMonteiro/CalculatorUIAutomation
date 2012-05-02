using System;
using System.Threading;

namespace ConsoleApplication4
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var calc = new WindowsCalculator())
            {
                Console.WriteLine(calc.Result);
                Thread.Sleep(1000);
                calc.PressDigit(1);
                Thread.Sleep(1000);
                calc.PressAdd();
                Thread.Sleep(1000);
                calc.PressDigit(2);
                Thread.Sleep(1000);
                calc.PressEquals();
                Thread.Sleep(1000);
                Console.WriteLine(calc.Result);
                Console.WriteLine("Done");
                Console.ReadLine();
            }
        }
    }
}
