using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arca.Lexing.Tokens
{
    enum TokenType
    {

        Identifier, Int, Float, String,
        EndOfInput,

        True, False,
        Null,

        Semicolon,

        Plus, Minus,
        Star, Slash,
        Percent,
        Not, Or, And

    }
}
