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

            // Initialize data
            HandleIndentation();
            Next();
        }


        public void Next() => GetNext(true);
        public void NextToken() => GetNext(false);

        public void GetNext(bool indent)
        {
            // Skip whitespace
            while (CharacterUtil.IsWhitespace(stream.Current))
            {
                stream.Next();
            }

            Token? token = Run(indent);
            if (token != null)
            {
                // Token was successfully created
                Current = (Token) token;
                Console.WriteLine($"[{Current.Location}] {Current.Type} {Current.Value}");

                return;
            }

            // Raise error
            ArcaException exception = new ArcaException(stream.Location, $"Unexpected {stream.CurrentFormatted}");
            Arca.Error(exception);

            // Skip character and try again
            stream.Next();
            GetNext(indent);
        }

        public bool Check(params TokenType[] types)
        {
            foreach (TokenType type in types)
            {
                // Find match
                if (Current.Type == type) return true;
            }

            // Check again if current was a new line
            if (Current.Type == TokenType.NewLine)
            {
                Next();
                return Check(types);
            }

            return false;
        }


        private Token? Run(bool indent)
        {
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
            else if (CharacterUtil.IsNewLine(stream.Current))
            {
                // Output new line token
                Token token = new Token(stream.Location, TokenType.NewLine);
                stream.Next();

                if (indent) HandleIndentation();
                else GetIndentation(); // Skips adding indentation tokens

                return token;
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
                // Count indentation
                if (stream.Current == '\t') indent += 4;
                else indent++;

                stream.Next();
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
