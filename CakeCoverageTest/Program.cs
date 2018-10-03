using System;
using System.Linq;

namespace CakeCoverageTest
{
    class Program
    {
        public static void Main(string[] args)
        {
            var arg = args.FirstOrDefault();
            var greeter = new Greeter();
            var salutation = greeter.Greet(arg);
            Console.WriteLine(salutation);
        }
    }
}
