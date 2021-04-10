﻿using Arca.Lexing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arca
{
    class Arca
    {

        public static void Run(string file)
        {
            InputStream stream = new InputStream(file);
            Lexer lexer = new Lexer(stream);
        }

        public static void Error(ArcaException exception)
        {
            Console.Error.WriteLine($"[{exception.Location}] {exception.Message}");
        }

    }
}
