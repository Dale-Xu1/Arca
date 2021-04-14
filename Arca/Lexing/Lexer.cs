using Arca.Lexing.Constants;
using Arca.Lexing.StateMachines;
using Arca.Lexing.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arca.Lexing
{
    class Lexer
    {

        public Token Current { get; private set; }

        private readonly InputStream stream;

        private readonly Stack<int> indents = new Stack<int>(new int[] { 0 });
        private readonly Queue<Token> queue = new Queue<Token>(); // Only used for indents and dedents


        public Lexer(InputStream stream)
        {
            this.stream = stream;

            HandleIndentation();
            Next();
        }


        public void Next()
        {
            SkipWhitespace();

            Token? token = Run();
            if (token != null)
            {
                // Token was successfully created
                Current = (Token) token;
                return;
            }

            // Raise error
            ArcaException exception = new ArcaException(stream.Location, $"Unexpected {stream.CurrentFormatted}");
            Arca.Error(exception);

            // Skip character and try again
            stream.Next();
            Next();
        }

        public bool Check(params TokenType[] types)
        {
            foreach (TokenType type in types)
            {
                // Find match
                if (Current.Type == type) return true;
            }

            return false;
        }


        private Token? Run()
        {
            if (CharacterUtil.IsNewLine(stream.Current))
            {
                // Store location of new line
                Location location = stream.Location;
                stream.Next();

                if (HandleIndentation())
                {
                    // New line if the indentation is the same
                    return new Token(location, TokenType.NewLine);
                }
            }

            // Dequeue if tokens exist in queue
            if (queue.Count > 0) return queue.Dequeue();
            else if (stream.Current == '\0')
            {
                if (indents.Count == 1) // 1 because there is always a 0 as the first element
                {
                    // End of input was reached
                    return new Token(stream.Location, TokenType.EndOfInput);
                }

                QueueDedents();
                return Run();
            }

            // Run state machines if they can start
            return RunStateMachines();
        }

        private Token? RunStateMachines()
        {
            if (IdentifierMachine.CanStart(stream))
            {
                IdentifierMachine machine = new IdentifierMachine(stream);
                KeywordLexer keywordLexer = new KeywordLexer(machine);

                return keywordLexer.Run();
            }
            else if (NumberMachine.CanStart(stream))
            {
                NumberMachine machine = new NumberMachine(stream);
                return machine.Run();
            }
            else if (StringMachine.CanStart(stream))
            {
                StringMachine machine = new StringMachine(stream);
                return machine.Run();
            }

            SymbolLexer symbolLexer = new SymbolLexer(stream);
            return symbolLexer.Run();
        }


        private bool HandleIndentation()
        {
            int indent = GetIndentation();
            if (indent == indents.Peek()) return true; // Nothing happens if indentation doesn't change

            QueueDedents(indent);
            if (indent > indents.Peek())
            {
                indents.Push(indent);
                queue.Enqueue(new Token(stream.Location, TokenType.Indent));
            }

            return false;
        }

        private void QueueDedents(int indent = 0)
        {
            while (indent < indents.Peek())
            {
                // Add dedents until current indent level is reached
                indents.Pop();
                queue.Enqueue(new Token(stream.Location, TokenType.Dedent));
            }
        }

        private int GetIndentation()
        {
            int indent = SkipWhitespace();
            if (CharacterUtil.IsNewLine(stream.Current)) // Recount if a new line is found
            {
                stream.Next();
                return GetIndentation();
            }

            return indent;
        }

        private int SkipWhitespace()
        {
            int indent = 0;
            while (CharacterUtil.IsWhitespace(stream.Current))
            {
                // Count indentation
                if (stream.Current == '\t') indent += (4 - indent % 4); // Rounds to next multiple of 4
                else indent++;

                stream.Next();
            }

            // Test if there is a comment
            if (stream.Current == '/' && stream.Lookahead(1) == '/')
            {
                while (!CharacterUtil.IsNewLine(stream.Current) && stream.Current != '\0')
                {
                    // Skip everything until a new line or end of input is found
                    stream.Next();
                }
            }

            return indent;
        }

    }
}
