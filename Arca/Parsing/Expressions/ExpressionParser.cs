using Arca.Lexing;
using Arca.Lexing.Constants;
using Arca.Lexing.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arca.Parsing.Expressions
{
    class ValueTree : SyntaxTree
    {

        public double Value { get; }


        public ValueTree(Location location, double value) : base(location) => Value = value;


        public override string ToString(int indent) => $"{Whitespace()} {Value}";

    }

    class UnaryTree : SyntaxTree
    {

        public SyntaxTree Expression { get; }
        public TokenType Operation { get; }


        public UnaryTree(Location location, SyntaxTree expression, TokenType operation) : base(location)
        {
            Expression = expression;
            Operation = operation;
        }


        public override string ToString(int indent)
        {
            string symbol = SymbolLexer.Symbols[Operation];
            return $"{Whitespace(indent)} {symbol}({Expression})";
        }

    }

    class ExpressionParser : Parser<SyntaxTree>
    {

        public ExpressionParser(Lexer lexer) : base(lexer) { }


        protected override SyntaxTree ParseTree(Location location)
        {
            Token token = Expect(TokenType.Int, TokenType.Float);
            return new ValueTree(location, double.Parse(token.Value));
        }

    }
}
