using Arca.Lexing;
using Arca.Lexing.Tokens;
using Arca.Parsing.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arca.Parsing.Statements
{
    class IfTree : SyntaxTree
    {

        public SyntaxTree Condition { get; }

        public SyntaxTree ThenBranch { get; }
        public SyntaxTree ElseBranch { get; }


        public IfTree(Location location, SyntaxTree condition, SyntaxTree thenBranch, SyntaxTree elseBranch) : base(location)
        {
            Condition = condition;
            ThenBranch = thenBranch;
            ElseBranch = elseBranch;
        }


        public override void Write(int indent = 0)
        {
            Whitespace(indent);

            Console.Write("if ");
            Condition.Write();

            Console.WriteLine();

            ThenBranch.Write(indent + 1);
            if (ElseBranch != null)
            {
                Whitespace(indent);
                Console.WriteLine("else");

                ElseBranch.Write(indent + 1);
            }
        }

    }

    class IfParser : Parser<IfTree>
    {

        public static bool CanStart(Lexer lexer) => lexer.Check(TokenType.If);


        public IfParser(Lexer lexer) : base(lexer) { }


        protected override IfTree ParseTree(Location location)
        {
            // Parse condition
            Expect(TokenType.If);
            SyntaxTree condition = new ExpressionParser(Lexer).Parse();

            SyntaxTree thenBranch;
            SyntaxTree elseBranch = null;

            Expect(TokenType.Then); // Deliminate condition expression from first statement

            if (Lexer.NewLine) thenBranch = new BlockParser(Lexer).Parse();
            else thenBranch = new StatementParser(Lexer).Parse();

            if (Lexer.Check(TokenType.Else))
            {
                // If else is on a new line, it must be of the same indentation to be part of the if statement
                int indent = Lexer.Location.Col;
                if (Lexer.NewLine && (indent != Lexer.Indent)) goto End;

                Lexer.Next();
                if (Lexer.NewLine) elseBranch = new BlockParser(Lexer).Parse();
                else elseBranch = new StatementParser(Lexer).Parse();
            }

        End:
            return new IfTree(location, condition, thenBranch, elseBranch);
        }

    }
}
