using Arca.Lexing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arca.Parsing.Expressions
{
    class AssignmentTree : SyntaxTree
    {

        public SyntaxTree Target { get; }
        public SyntaxTree Expression { get; }


        public AssignmentTree(Location location, SyntaxTree target, SyntaxTree expression) : base(location)
        {
            Target = target;
            Expression = expression;
        }


        public override void Write(int indent = 0)
        {
            Console.Write("(");

            Target.Write();
            Console.Write(" = ");
            Expression.Write();

            Console.Write(")");
        }

    }

    class AssignmentParser : Parser<AssignmentTree>
    {

        private readonly SyntaxTree target;


        public AssignmentParser(Lexer lexer, SyntaxTree target) : base(lexer) => this.target = target;


        protected override AssignmentTree ParseTree(Location location)
        {
            // Test assignment target
            if (!(target is IdentifierTree) && !(target is MemberTree))
            {
                throw new ArcaException(location, "Invalid assignment target");
            }
            Lexer.Next();

            // Don't add 1 to precedence to get right associativity
            SyntaxTree expression = new ExpressionParser(Lexer, Precedence.Assignment).Parse();
            return new AssignmentTree(location, target, expression);
        }

    }
}
