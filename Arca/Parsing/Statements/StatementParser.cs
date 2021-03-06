﻿using Arca.Lexing;
using Arca.Parsing.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arca.Parsing.Statements
{
    class ExpressionTree : SyntaxTree
    {

        public SyntaxTree Expression { get; }


        public ExpressionTree(Location location, SyntaxTree expression) : base(location) => Expression = expression;


        public override void Write(int indent = 0)
        {
            Whitespace(indent);

            Expression.Write();
            Console.WriteLine();
        }

    }

    class StatementParser : Parser<SyntaxTree>
    {

        public static bool CanStart(Lexer lexer) => IfParser.CanStart(lexer);


        public StatementParser(Lexer lexer) : base(lexer) { }


        protected override SyntaxTree ParseTree(Location location)
        {
            if (IfParser.CanStart(Lexer)) return new IfParser(Lexer).Parse();

            SyntaxTree expression = new ExpressionParser(Lexer).Parse();
            return new ExpressionTree(location, expression);
        }

    }
}
