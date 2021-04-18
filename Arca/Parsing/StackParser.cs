using Arca.Lexing;
using Arca.Lexing.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arca.Parsing
{
    abstract class StackParser<T, S> : Parser<T>
        where T : SyntaxTree
        where S : SyntaxTree
    {

        private readonly List<S> trees = new List<S>();

        private int previousIndent;
        private bool ignoreIndent = false;


        protected StackParser(Lexer lexer) : base(lexer) { }


        protected override T ParseTree(Location location)
        {
            InitializeIndent();
            while (ExpectIndent())
            {
                // Parse and save trees
                S tree = ParseTree();
                trees.Add(tree);

                ExpectEnd();
            }

            // Restore indentation
            Lexer.Indent = previousIndent;
            return CreateTree(location, trees.ToArray());
        }

        protected abstract S ParseTree();
        protected abstract T CreateTree(Location location, S[] trees);


        private void InitializeIndent()
        {
            // Store previous indentation
            previousIndent = Lexer.Indent;

            Location location = Lexer.Location;
            int indent = location.Col;

            if (indent <= previousIndent || Lexer.Check(TokenType.EndOfInput))
            {
                // Indentation must increase
                throw new ArcaException(location, "Expected indent");
            }

            Lexer.Indent = indent;
        }

        private bool ExpectIndent()
        {
            // End of input ends stack
            if (Lexer.Check(TokenType.EndOfInput)) return false;

            Location location = Lexer.Location;
            int indent = location.Col;

            if (ignoreIndent)
            {
                // Skip indentation check
                ignoreIndent = false;
                return true;
            }
            else if (indent > Lexer.Indent)
            {
                // Indentation cannot increase
                Arca.Error(new ArcaException(location, "Unexpected indent"));
                return true;
            }

            return (indent == Lexer.Indent);
        }

        private void ExpectEnd()
        {
            // Make sure statements are deliminated
            if (!Lexer.NewLine && !Lexer.Check(TokenType.EndOfInput))
            {
                if (!Match(TokenType.Semicolon))
                {
                    Arca.Error(new ArcaException(Lexer.Location, "Expected semicolon"));
                }
                else if (!Lexer.NewLine) ignoreIndent = true; // Indentation can be ignored for the next statement
            }
        }

    }
}
