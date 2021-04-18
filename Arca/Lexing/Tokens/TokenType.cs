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

        If, Then, Else,
        While, For,

        Class, Function,

        Equal,
        Dot, Comma,
        Colon, Semicolon,
        ParenOpen, ParenClose,

        Plus, Minus,
        Star, Slash,
        Percent,
        Not, Or, And,

        IsEqual, NotEqual,
        Less, LessEqual,
        Greater, GreaterEqual

    }
}
