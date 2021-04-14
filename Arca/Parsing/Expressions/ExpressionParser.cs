using Arca.Lexing;
using Arca.Lexing.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arca.Parsing.Expressions
{
    class ExpressionParser
    {

        private readonly Lexer lexer;


        public ExpressionParser(Lexer lexer) => this.lexer = lexer;


        public void Parse()
        {
            Token a = Expect(TokenType.Number);
            Expect(TokenType.Plus);
            Token b = Expect(TokenType.Number);

            Console.WriteLine($"{a.Value} + {b.Value}");
        }


        private bool Match(params TokenType[] types)
        {
            bool result = lexer.Check(types);

            // Move onto next token if successful
            if (result) lexer.Next();
            return result;
        }

        private Token Expect(params TokenType[] types)
        {
            bool result = lexer.Check(types);
            Token token = lexer.Current;

            if (result)
            {
                lexer.Next();
                return token;
            }

            throw new ArcaException(token.Location, $"Unexpected {token}");
        }

    }
}
