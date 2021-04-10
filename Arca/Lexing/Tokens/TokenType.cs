﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arca.Lexing.Tokens
{
    enum TokenType
    {

        Identifier, Number, String,
        EndOfInput,

        Indent, Dedent,

        True, False,
        Null

    }
}