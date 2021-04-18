using Arca.Lexing;
using Arca.Lexing.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arca.Parsing.Expressions
{
    class LiteralTree<T> : SyntaxTree
    {

        public T Value { get; }


        public LiteralTree(Location location, T value) : base(location) => Value = value;


        public override string ToString(int indent) => $"{Whitespace()}{Value}";

    }

    class IntTree : LiteralTree<int>
    {
        public IntTree(Location location, int value) : base(location, value) { }
    }

    class FloatTree : LiteralTree<double>
    {
        public FloatTree(Location location, double value) : base(location, value) { }
    }

    class LiteralParser : Parser<SyntaxTree>
    {

        public LiteralParser(Lexer lexer) : base(lexer) { }


        protected override SyntaxTree ParseTree(Location location)
        {
            switch (Lexer.Current.Type)
            {
                case TokenType.Identifier: return new IdentifierParser(Lexer).Parse();

                case TokenType.Int: return ParseInt(location);
                case TokenType.Float: return ParseFloat(location);

                case TokenType.ParenOpen: return ParseGroup();
            }

            return null;
        }


        private IntTree ParseInt(Location location)
        {
            int value = int.Parse(Lexer.Current.Value);
            Lexer.Next();

            return new IntTree(location, value);
        }

        private FloatTree ParseFloat(Location location)
        {
            double value = double.Parse(Lexer.Current.Value);
            Lexer.Next();

            return new FloatTree(location, value);
        }

        private SyntaxTree ParseGroup()
        {
            Lexer.Next();
            SyntaxTree expression = new ExpressionParser(Lexer).Parse();

            Expect(TokenType.ParenClose);
            return expression;
        }

    }
}
