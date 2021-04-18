using Arca.Lexing;
using Arca.Lexing.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arca.Parsing
{
    abstract class SyntaxTree
    {

        public Location Location { get; }


        protected SyntaxTree(Location location) => Location = location;


        public override string ToString() => ToString(0);
        public abstract string ToString(int indent);

        protected string Whitespace(int indent)
        {
            // Create whitespace characters
            string whitespace = "";
            for (int i = 0; i < indent; i++) whitespace += "    ";

            return $"{whitespace}[{Location}]";
        }

    }

    abstract class Parser<T> where T : SyntaxTree
    {

        protected Lexer Lexer { get; }

        private readonly Location location;


        protected Parser(Lexer lexer)
        {
            Lexer = lexer;
            location = lexer.Current.Location;
        }


        public T Parse()
        {
            try
            {
                return ParseTree(location);
            }
            catch (ArcaException exception)
            {
                Arca.Error(exception);
                return null;
            }
        }

        protected abstract T ParseTree(Location location);


        protected bool Match(params TokenType[] types)
        {
            bool result = Lexer.Check(types);

            // Move onto next token if successful
            if (result) Lexer.Next();
            return result;
        }

        protected Token Expect(params TokenType[] types)
        {
            Token token = Lexer.Current;

            // Same as match, but throws an error
            if (Match(types)) return token;
            throw new ArcaException(token.Location, $"Unexpected {token}");
        }

        protected void ExpectNewLine()
        {
            // Throws error if new line is not found
            if (!Lexer.NewLine) throw new ArcaException(Lexer.Location, "Expected new line");
        }

    }
}
