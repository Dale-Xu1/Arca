using Arca.Lexing;
using Arca.Lexing.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arca.Parsing
{
    abstract class Parser<T> where T : struct
    {

        protected Lexer Lexer { get; }
        protected Location Location { get; }


        public Parser(Lexer lexer)
        {
            Lexer = lexer;
            Location = lexer.Current.Location;
        }


        public T? Parse()
        {
            try
            {
                return ParseTree();
            }
            catch (ArcaException exception)
            {
                Arca.Error(exception);
                return null;
            }
        }

        protected abstract T ParseTree();


        protected bool Match(params TokenType[] types)
        {
            bool result = Lexer.Check(types);

            // Move onto next token if successful
            if (result) Lexer.Next();
            return result;
        }

        protected Token Expect(params TokenType[] types)
        {
            bool result = Lexer.Check(types);
            Token token = Lexer.Current;

            if (result)
            {
                Lexer.Next();
                return token;
            }

            throw new ArcaException(token.Location, $"Unexpected {token}");
        }

    }
}
