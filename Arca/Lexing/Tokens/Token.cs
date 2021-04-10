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

    }
}
