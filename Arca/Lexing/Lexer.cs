using Arca.Lexing.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arca.Lexing
{
    class Lexer
    {

        public Token Current { get; private set; }

        private readonly InputStream stream;


        public Lexer(InputStream stream)
        {
            this.stream = stream;

            Console.WriteLine(stream.Lookahead(100));

            while (stream.Current != '\0')
            {
                Console.WriteLine(stream.Current);
                stream.Next();
            }
        }

    }
}
