using Arca.Lexing.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arca.Lexing.Tokenizers
{
    class LiteralLexer
    {

        private readonly InputStream stream;

        private readonly Location location;
        private readonly StringBuilder builder = new StringBuilder();


        public LiteralLexer(InputStream stream)
        {
            this.stream = stream;
            location = stream.Location;
        }


        public Token? Run()
        {
            if (CharacterUtil.IsIdentifier(stream.Current)) return Identifier();
            else if (NumberCanStart()) return Number();
            else if (stream.Current == '"') return String();

            SymbolLexer symbolLexer = new SymbolLexer(stream);
            return symbolLexer.Run();
        }


        private Token? Identifier()
        {
            Next();
            while (CharacterUtil.IsIdentifier(stream.Current) || CharacterUtil.IsDigit(stream.Current)) Next();

            // Test if identifier is a reserved keyword
            Token token = new Token(builder.ToString(), location, TokenType.Identifier);
            KeywordLexer lexer = new KeywordLexer(token);

            return lexer.Run();
        }


        private bool NumberCanStart()
        {
            if (CharacterUtil.IsDigit(stream.Current)) return true;
            return (stream.Current == '.' && CharacterUtil.IsDigit(stream.Lookahead(1))); // Test for . with number after it
        }

        private Token? Number()
        {
            bool isFloat = false;
            while (CharacterUtil.IsDigit(stream.Current)) Next();

            if (stream.Current == '.' && CharacterUtil.IsDigit(stream.Lookahead(1)))
            {
                // Numbers after decimal place
                Next();
                while (CharacterUtil.IsDigit(stream.Current)) Next();

                isFloat = true;
            }

            if (char.ToLower(stream.Current) == 'e')
            {
                Next();
                if (stream.Current == '+' || stream.Current == '-') Next();

                // At least one digit is required
                if (!CharacterUtil.IsDigit(stream.Current)) return null;
                while (CharacterUtil.IsDigit(stream.Current)) Next();

                isFloat = true;
            }

            return new Token(builder.ToString(), location, isFloat ? TokenType.Float : TokenType.Int);
        }


        private Token? String()
        {
            stream.Next();
            while (stream.Current != '"')
            {
                // String was left unterminated
                if (CharacterUtil.IsNewLine(stream.Current) || stream.Current == '\0') return null;
                else if (stream.Current == '\\')
                {
                    // Only specific escape characters can be used in escape sequence
                    stream.Next();
                    if (!CharacterUtil.IsEscape(stream.Current)) return null;

                    Escape();
                    continue;
                }

                Next();
            }

            stream.Next();
            return new Token(builder.ToString(), location, TokenType.String);
        }

        private void Escape()
        {
            // Convert escape codes to corresponding characters
            switch (stream.Current)
            {
                case '"': builder.Append('"'); break;
                case '\\': builder.Append('\\'); break;
                case 'n': builder.Append('\n'); break;
                case 'r': builder.Append('\r'); break;
                case 't': builder.Append('\t'); break;
                case '0': builder.Append('\0'); break;
            }

            stream.Next();
        }


        private void Next()
        {
            builder.Append(stream.Current);
            stream.Next();
        }

    }
}
