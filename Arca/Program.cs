using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arca
{
    class Program
    {

        private static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.Error.WriteLine("Usage: arca <file>");
                return;
            }

            Arca.Run(args[0]);
            Console.ReadLine(); // TODO: Remove
        }

    }
}
