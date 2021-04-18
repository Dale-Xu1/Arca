using Arca.Lexing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arca.Parsing.Expressions
{
    class ExpressionParser : Parser<SyntaxTree>
    {

        public ExpressionParser(Lexer lexer) : base(lexer) { }


        protected override SyntaxTree ParseTree(Location location)
        {
            SyntaxTree expression = new LiteralParser(Lexer).Parse();
            if (expression == null) expression = new UnaryParser(Lexer).Parse();

            if (expression == null) throw new ArcaException(location, "I dunno");
            return expression;
        }

    }
}
