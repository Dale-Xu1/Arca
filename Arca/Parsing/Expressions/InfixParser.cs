﻿using Arca.Lexing;
using Arca.Lexing.Tokenizers;
using Arca.Lexing.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arca.Parsing.Expressions
{
    class BinaryTree : SyntaxTree
    {

        public SyntaxTree Left { get; }
        public SyntaxTree Right { get; }

        public TokenType Operation { get; }


        public BinaryTree(Location location, SyntaxTree left, SyntaxTree right, TokenType operation) : base(location)
        {
            Left = left;
            Right = right;
            Operation = operation;
        }


        public override void Write(int indent)
        {
            Console.Write("(");
            Left.Write();

            string symbol = SymbolLexer.Symbols[Operation];
            Console.Write($" {symbol} ");

            Right.Write();
            Console.Write(")");
        }

    }

    class InfixParser : Parser<SyntaxTree>
    {

        public static Dictionary<TokenType, Precedence> Operations { get; } = new Dictionary<TokenType, Precedence>
        {
            [TokenType.Equal] = Precedence.Assignment,

            [TokenType.Or] = Precedence.Or,
            [TokenType.And] = Precedence.And,

            [TokenType.IsEqual] = Precedence.Equality,
            [TokenType.NotEqual] = Precedence.Equality,

            [TokenType.Less] = Precedence.Comparison,
            [TokenType.LessEqual] = Precedence.Comparison,
            [TokenType.Greater] = Precedence.Comparison,
            [TokenType.GreaterEqual] = Precedence.Comparison,

            [TokenType.Plus] = Precedence.Term,
            [TokenType.Minus] = Precedence.Term,

            [TokenType.Star] = Precedence.Factor,
            [TokenType.Slash] = Precedence.Factor,
            [TokenType.Percent] = Precedence.Factor,

            [TokenType.Dot] = Precedence.Postfix,
            [TokenType.ParenOpen] = Precedence.Postfix
        };


        private readonly SyntaxTree left;
        private readonly Precedence basePrecedence;


        public InfixParser(Lexer lexer, SyntaxTree left, Precedence basePrecedence) : base(lexer)
        {
            this.left = left;
            this.basePrecedence = basePrecedence;
        }


        protected override SyntaxTree ParseTree(Location location)
        {
            // Test if next token is a valid operator
            TokenType operation = Lexer.Current.Type;
            if (!Operations.ContainsKey(operation)) return null;

            Precedence precedence = Operations[operation];
            if (precedence < basePrecedence) return null; // Next operator's precedence is too low

            // Handle special cases
            switch (operation)
            {
                case TokenType.Equal: return new AssignmentParser(Lexer, left).Parse();
                case TokenType.Dot: return new MemberParser(Lexer, left).Parse();
                case TokenType.ParenOpen: break; // TODO: Calls
            }

            // Create binary tree
            Lexer.Next();
            SyntaxTree right = new ExpressionParser(Lexer, precedence + 1).Parse();

            return new BinaryTree(location, left, right, operation);
        }

    }
}
