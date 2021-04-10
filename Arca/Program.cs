using Arca.Lexing;
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

            InputStream stream = new InputStream(args[0]);
            Lexer lexer = new Lexer(stream);

            Console.ReadLine();
        }

    }
}
