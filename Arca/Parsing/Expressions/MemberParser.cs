using Arca.Lexing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arca.Parsing.Expressions
{
    class MemberTree : SyntaxTree
    {

        public SyntaxTree Expression { get; }
        public IdentifierTree Member { get; }


        public MemberTree(Location location, SyntaxTree expression, IdentifierTree member) : base(location)
        {
            Expression = expression;
            Member = member;
        }


        public override string ToString(int indent) => $"{Whitespace()}({Expression}.{Member})";

    }

    class MemberParser : Parser<MemberTree>
    {

        private readonly SyntaxTree expression;


        public MemberParser(Lexer lexer, SyntaxTree expression) : base(lexer) => this.expression = expression;


        protected override MemberTree ParseTree(Location location)
        {
            Lexer.Next();
            IdentifierTree member = new IdentifierParser(Lexer).Parse();

            return new MemberTree(location, expression, member);
        }

    }
}
