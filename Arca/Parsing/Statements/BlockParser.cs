using Arca.Lexing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arca.Parsing.Statements
{
    class BlockTree : SyntaxTree
    {

        public SyntaxTree[] Statements { get; }


        public BlockTree(Location location, SyntaxTree[] statements) : base(location) => Statements = statements;


        public override void Write(int indent = 0)
        {
            for (int i = 0; i < Statements.Length; i++)
            {
                SyntaxTree statement = Statements[i];
                statement.Write(indent);
            }
        }

    }

    class BlockParser : StackParser<BlockTree, SyntaxTree>
    {

        public BlockParser(Lexer lexer) : base(lexer) { }


        protected override SyntaxTree ParseTree() => new StatementParser(Lexer).Parse();

        protected override BlockTree CreateTree(Location location, SyntaxTree[] statements) => new BlockTree(location, statements);

        protected override bool CanStart() => StatementParser.CanStart(Lexer);

    }
}
