using Arca.Lexing;
using Arca.Lexing.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arca.Parsing.Expressions
{
    struct Tree
    {

        public Location Location { get; }


        public Tree(Location location) => Location = location;

    }

    class ExpressionParser : Parser<Tree>
    {

        public ExpressionParser(Lexer lexer) : base(lexer) { }


        protected override Tree ParseTree()
        {
            Token a = Expect(TokenType.Number);
            Expect(TokenType.Plus);
            Token b = Expect(TokenType.Number);

            Console.WriteLine($"{a.Value} + {b.Value}");
            return new Tree(Location);
        }

    }
}
