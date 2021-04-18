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
                try
                {
                    // Parse and save trees
                    S tree = ParseTree();
                    ExpectEnd();

                    trees.Add(tree);
                }
                catch (ArcaException exception)
                {
                    Arca.Error(exception);
                    Synchronize();
                }
            }

            // Restore indentation
            Lexer.Indent = previousIndent;
            return CreateTree(location, trees.ToArray());
        }

        protected abstract S ParseTree();
        protected abstract T CreateTree(Location location, S[] trees);

        protected virtual bool CanStart() => false;


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
                    throw new ArcaException(Lexer.Location, "Expected semicolon");
                }
                else if (!Lexer.NewLine) ignoreIndent = true; // Indentation can be ignored for the next statement
            }
        }


        private void Synchronize(bool initial = true)
        {
            // New line means end of statement, so the lexer is synchronized
            if (Lexer.NewLine && !initial)
            {
                Arca.Synchronize();
                return;
            }

            switch (Lexer.Current.Type)
            {
                case TokenType.EndOfInput: break;
                case TokenType.Semicolon:
                {
                    // Skip semicolon
                    Lexer.Next();
                    break;
                }

                default:
                {
                    if (CanStart()) break;
                    else
                    {
                        // Synchronize again for next token
                        Lexer.Next();
                        Synchronize(false);

                        return;
                    }
                }
            }

            // Ignore indentation because we know the next token is on the same line
            ignoreIndent = true;
            Arca.Synchronize();
        }

    }
}
