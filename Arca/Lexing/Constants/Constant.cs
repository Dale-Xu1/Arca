using Arca.Lexing.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arca.Lexing.Constants
{
    struct Constant
    {

        public TokenType Type { get; }

        public string Value { get; }


        public Constant(TokenType type, string value)
        {
            Type = type;
            Value = value;
        }

    }
}
