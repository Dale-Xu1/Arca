using Arca.Lexing;
using Arca.Parsing.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arca.Parsing.Statements
{
    class ExpressionTree : SyntaxTree
    {

        public SyntaxTree Expression { get; }


        public ExpressionTree(Location location, SyntaxTree expression) : base(location) => Expression = expression;


        public override string ToString(int indent) => $"{Whitespace(indent)}{Expression}";

    }

    class StatementParser : Parser<SyntaxTree>
    {

        public static bool CanStart(Lexer lexer) => false;


        public StatementParser(Lexer lexer) : base(lexer) { }


        protected override SyntaxTree ParseTree(Location location)
        {
            SyntaxTree expression = new ExpressionParser(Lexer).Parse();
            return new ExpressionTree(location, expression);
        }

    }
}
