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


        public override string ToString(int indent)
        {
            string whitespace = Whitespace(indent);
            StringBuilder builder = new StringBuilder(whitespace);

            builder.Append($"if {Condition}\n");

            builder.Append(ThenBranch.ToString(indent + 1));
            if (ElseBranch != null)
            {
                builder.Append($"\n{whitespace}else\n");
                builder.Append(ElseBranch.ToString(indent + 1));
            }

            return builder.ToString();
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

            if (Match(TokenType.Then))
            {
                if (Lexer.NewLine) thenBranch = new BlockParser(Lexer).Parse();
                else thenBranch = new StatementParser(Lexer).Parse();
            }
            else
            {
                // A new line is required if then is not provided
                ExpectNewLine();
                thenBranch = new BlockParser(Lexer).Parse();
            }

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
