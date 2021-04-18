using Arca.Lexing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arca.Parsing.Expressions
{
    class LiteralParser : Parser<SyntaxTree>
    {

        public LiteralParser(Lexer lexer) : base(lexer) { }


        protected override SyntaxTree ParseTree(Location location)
        {
            throw new NotImplementedException();
        }

    }
}
