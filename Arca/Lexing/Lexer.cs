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

        private readonly Stack<int> indents = new Stack<int>();
        private readonly Queue<Token> queue = new Queue<Token>(); // Only used for indents and dedents


        public Lexer(InputStream stream)
        {
            this.stream = stream;

            // Initialize data
            indents.Push(0);
            HandleIndentation();

            Next();
        }


        public void Next()
        {
            // Skip whitespace
            while (CharacterUtil.IsWhitespace(stream.Current))
            {
                stream.Next();
            }

            Token? token = RunStateMachines();
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
            // TODO: Check token type
            return true;
        }


        private Token? RunStateMachines()
        {
            if (CharacterUtil.IsNewLine(stream.Current))
            {
                stream.Next(); // Skip new line character
                HandleIndentation();
            }

        Queue:
            if (queue.Count > 0)
            {
                // Deque if tokens exist in queue
                return queue.Dequeue();
            }
            else if (stream.Current == '\0')
            {
                if (indents.Count == 1) // 1 because there is always a 0 as the first element
                {
                    // End of input was reached
                    return new Token(stream.Location, TokenType.EndOfInput);
                }

                QueueDedents(0);
                goto Queue;
            }

            // Run state machines if they can start
            if (IdentifierMachine.CanStart(stream))
            {
                IdentifierMachine machine = new IdentifierMachine(stream);
                return machine.Run(); // TODO: Keywords
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

            return null;
        }


        private void HandleIndentation()
        {
            int indent = GetIndentation();
            QueueDedents(indent);

            // Nothing happens if the indentation is the same
            if (indent == indents.Peek()) return;

            // Add indent
            indents.Push(indent);
            queue.Enqueue(new Token(stream.Location, TokenType.Indent));
        }

        private void QueueDedents(int indent)
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
            int indent = 0;
            while (CharacterUtil.IsWhitespace(stream.Current))
            {
                stream.Next();
                indent++; // Count indentation
            }

            if (CharacterUtil.IsNewLine(stream.Current))
            {
                // Recount if another new line is found
                stream.Next();
                return GetIndentation();
            }

            return indent;
        }

    }
}
