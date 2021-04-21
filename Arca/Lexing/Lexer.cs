using Arca.Lexing.Tokenizers;
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
        public Location Location => Current.Location;

        public bool NewLine { get; private set; } // If current token is on a new line
        public int Indent { get; set; } = 0;

        private readonly InputStream stream;


        public Lexer(InputStream stream)
        {
            this.stream = stream;
            Next();
        }


        public void Next()
        {
            NewLine = false;
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

        private void SkipWhitespace()
        {
            while (true)
            {
                // Skip tokens until non-whitespace character is found
                if (CharacterUtil.IsNewLine(stream.Current)) NewLine = true; // Track if new line was found
                else if (!CharacterUtil.IsWhitespace(stream.Current)) break;

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

                SkipWhitespace();
            }
        }

        private Token? Run()
        {
            if (stream.Current == '\0')
            {
                // End of input was reached
                return new Token(stream.Location, TokenType.EndOfInput);
            }

            // Tokenize literals
            LiteralLexer lexer = new LiteralLexer(stream);
            return lexer.Run();
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

    }
}
