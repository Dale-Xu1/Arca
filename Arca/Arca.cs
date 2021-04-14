using Arca.Lexing;
using Arca.Parsing.Statements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arca
{
    class Arca
    {

        private static bool panic = false;


        public static void Run(string file)
        {
            InputStream stream = new InputStream(file);
            Lexer lexer = new Lexer(stream);

            while (lexer.Current.Type != Lexing.Tokens.TokenType.EndOfInput)
            {
                Console.WriteLine($"[{lexer.Current.Location}] {lexer.Current.Type} {lexer.Current.Value}");
                lexer.Next();
            }

            //BlockParser parser = new BlockParser(lexer);
            //parser.Parse();
        }


        public static void Error(ArcaException exception)
        {
            if (panic) return;
            panic = true;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine($"[{exception.Location}] {exception.Message}");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public static void Synchronize() => panic = false;

    }
}
