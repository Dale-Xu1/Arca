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
            return $"{Whitespace()}{symbol}({Expression})";
        }

    }

    class PrefixParser : Parser<UnaryTree>
    {

        private static readonly TokenType[] operations =
        {
            TokenType.Minus,
            TokenType.Not
        };


        public PrefixParser(Lexer lexer) : base(lexer) { }


        protected override UnaryTree ParseTree(Location location)
        {
            TokenType operation = Lexer.Current.Type;
            if (Match(operations))
            {
                // Parse expression and wrap in unary operation
                SyntaxTree expression = new ExpressionParser(Lexer, Precedence.Prefix).Parse();
                return new UnaryTree(location, expression, operation);
            }

            return null;
        }

    }
}
