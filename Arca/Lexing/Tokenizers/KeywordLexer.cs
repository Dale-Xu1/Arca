using Arca.Lexing.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arca.Lexing.Tokenizers
{
    class KeywordLexer
    {

        private static readonly Dictionary<TokenType, string> keywords = new Dictionary<TokenType, string>
        {
            [TokenType.True] = "true",
            [TokenType.False] = "false",
            [TokenType.Null] = "null",

            [TokenType.If] = "if",
            [TokenType.Then] = "then",
            [TokenType.Else] = "else",
            [TokenType.While] = "while",
            [TokenType.For] = "for",

            [TokenType.Class] = "class",
            [TokenType.Function] = "function",
        };


        private readonly Token token;


        public KeywordLexer(Token token) => this.token = token;


        public Token? Run()
        {
            string identifier = token.Value;

            // Test if identifier matches with keyword
            foreach (KeyValuePair<TokenType, string> keyword in keywords)
            {
                if (string.Equals(identifier, keyword.Value))
                {
                    // Create keyword token
                    return new Token(identifier, token.Location, keyword.Key);
                }
            }

            return token;
        }

    }
}
