using Arca.Lexing;
using Arca.Lexing.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arca.Parsing.Expressions
{
    enum Precedence
    {

        Assignment,

        Or,
        And,

        Equality,
        Comparison,

        Term,
        Factor,

        Prefix,
        Postfix

    }

    class ExpressionParser : Parser<SyntaxTree>
    {

        private readonly Precedence precedence;


        public ExpressionParser(Lexer lexer, Precedence precedence = Precedence.Assignment) : base(lexer) => this.precedence = precedence;


        protected override SyntaxTree ParseTree(Location location)
        {
            // Parse left side of expression
            SyntaxTree left = new LiteralParser(Lexer).Parse();
            if (left == null) left = new PrefixParser(Lexer).Parse();

            if (left == null) throw new ArcaException(location, "Expected expression");

            // Parse infix operations
            while (true)
            {
                SyntaxTree result = new InfixParser(Lexer, left, precedence).Parse();
                if (result == null) break;

                left = result;
            }

            return left;
        }

    }
}
