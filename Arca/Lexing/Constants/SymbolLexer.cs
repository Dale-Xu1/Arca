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

        public static Dictionary<TokenType, string> Symbols { get; } = new Dictionary<TokenType, string>
        {
            [TokenType.Semicolon] = ";",

            [TokenType.Add] = "+",
            [TokenType.Subtract] = "-",
            [TokenType.Multiply] = "*",
            [TokenType.Dedent] = "/",
            [TokenType.Modulo] = "%"
        };


        private readonly InputStream stream;


        public SymbolLexer(InputStream stream) => this.stream = stream;


        public Token? Run()
        {
            Location location = stream.Location;

            // Test each symbol until one is successful
            foreach (KeyValuePair<TokenType, string> symbol in Symbols)
            {
                for (int i = 0; i < symbol.Value.Length; i++)
                {
                    // Discard if one character doesn't match
                    if (stream.Lookahead(i) != symbol.Value[i]) goto Continue;
                }

                // Move stream pointer to end of symbol
                for (int i = 0; i < symbol.Value.Length; i++) stream.Next();
                return new Token(symbol.Value, location, symbol.Key);

            Continue:;
            }

            return null;
        }

    }
}
