using Arca.Lexing.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arca.Lexing.Constants
{
    class SymbolLexer
    {

        private static readonly Constant[] symbols =
        {
            new Constant(TokenType.Add, "+"), new Constant(TokenType.Subtract, "-"),
            new Constant(TokenType.Multiply, "*"), new Constant(TokenType.Divide, "/"),
            new Constant(TokenType.Modulo, "%")
        };


        private readonly InputStream stream;


        public SymbolLexer(InputStream stream) => this.stream = stream;


        public Token? Run()
        {
            Location location = stream.Location;

            // Test each symbol until one is successful
            foreach (Constant symbol in symbols)
            {
                for (int i = 0; i < symbol.Value.Length; i++)
                {
                    // Discard if one character doesn't match
                    if (stream.Lookahead(i) != symbol.Value[i]) goto Continue;
                }

                // Move stream pointer to end of symbol
                for (int i = 0; i < symbol.Value.Length; i++) stream.Next();
                return new Token(symbol.Value, location, symbol.Type);

            Continue:;
            }

            return null;
        }

    }
}
