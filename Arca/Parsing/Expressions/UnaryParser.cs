﻿using Arca.Lexing;
using Arca.Lexing.Constants;
using Arca.Lexing.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arca.Parsing.Expressions
{
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

    class UnaryParser : Parser<SyntaxTree>
    {

        private static readonly TokenType[] operations =
        {
            TokenType.Minus,
            TokenType.Not
        };


        public UnaryParser(Lexer lexer) : base(lexer) { }


        protected override SyntaxTree ParseTree(Location location)
        {
            TokenType operation = Lexer.Current.Type;
            if (Match(operations))
            {
                // Parse expression and wrap in unary operation
                SyntaxTree expression = new ExpressionParser(Lexer).Parse();
                return new UnaryTree(location, expression, operation);
            }

            return null;
        }

    }
}
