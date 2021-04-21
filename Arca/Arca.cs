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

            while (true)
            {
                Console.WriteLine($"{$"[{lexer.Current.Location}]",-6}{(lexer.NewLine ? " \\n" : "")} {lexer.Current.Type} '{lexer.Current.Value}'");

                if (lexer.Current.Type == Lexing.Tokens.TokenType.EndOfInput) break;
                lexer.Next();
            }
            Console.WriteLine();

            BlockParser parser = new BlockParser(new Lexer(new InputStream(file)));
            BlockTree tree = parser.Parse();

            tree.Write();
        }


        public static void Error(ArcaException exception)
        {
            if (panic) Console.ForegroundColor = ConsoleColor.DarkRed;
            else Console.ForegroundColor = ConsoleColor.Red;

            //if (panic) return;
            panic = true;

            Console.Error.WriteLine($"[{exception.Location}] {exception.Message}");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public static void Synchronize() => panic = false;

    }
}
