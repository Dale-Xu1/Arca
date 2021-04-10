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


        public Lexer(InputStream stream)
        {
            this.stream = stream;
            Next();
        }


        public void Next()
        {
            SkipWhitespace();
            Token? token = RunStateMachines();

            if (token != null)
            {
                // Token was successfully created
                Current = (Token) token;
                return;
            }

            // Raise error
            ArcaException exception = new ArcaException(stream.Location, $"Unexpected '{stream.Current}'");
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


        private void SkipWhitespace()
        {
            while (CharacterUtil.IsIndent(stream.Current) || CharacterUtil.IsNewLine(stream.Current))
            {
                stream.Next();
            }
        }

        private Token? RunStateMachines()
        {
            if (stream.Current == '\0')
            {
                // End of input was reached
                return new Token(stream.Location, TokenType.EndOfInput);
            }

            // Run state machines if they can start
            if (IdentifierMachine.CanStart(stream))
            {
                IdentifierMachine machine = new IdentifierMachine(stream);
                return machine.Run(); // TODO: Keywords
            }

            return null;
        }

    }
}
