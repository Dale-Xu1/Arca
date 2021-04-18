using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arca.Lexing.Tokens
{
    struct Token
    {

        public string Value { get; }
        public Location Location { get; }

        public TokenType Type { get; }


        public Token(string value, Location location, TokenType type)
        {
            Value = value;
            Location = location;
            Type = type;
        }

        public Token(Location location, TokenType type) : this("", location, type) { }


        public override string ToString()
        {
            switch (Type)
            {
                case TokenType.Identifier: return $"identifier '{Value}'";
                case TokenType.Int:
                case TokenType.Float: return $"number {Value}";
                case TokenType.String: return $"string {Value}";

                case TokenType.EndOfInput: return "end of input";

                // Keywords and symbols
                default: return $"'{Value}'";
            }
        }

    }
}
