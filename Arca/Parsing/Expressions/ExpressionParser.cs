using Arca.Lexing;
using Arca.Lexing.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arca.Parsing.Expressions
{
    class ExpressionTree : SyntaxTree
    {

        public ExpressionTree(Location location) : base(location) { }


        public override string ToString(int indent) => $"{Whitespace(indent)} expression";

    }

    class ExpressionParser : Parser<ExpressionTree>
    {

        public ExpressionParser(Lexer lexer) : base(lexer) { }


        protected override ExpressionTree ParseTree(Location location)
        {
            Expect(TokenType.Int, TokenType.Float);
            return new ExpressionTree(location);
        }

    }
}
