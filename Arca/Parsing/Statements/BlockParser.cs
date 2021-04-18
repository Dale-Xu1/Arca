using Arca.Lexing;
using Arca.Lexing.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arca.Parsing.Statements
{
    class BlockTree : SyntaxTree
    {

        public Location Location { get; }
        public SyntaxTree[] Statements { get; }


        public BlockTree(Location location, SyntaxTree[] statements)
        {
            Location = location;
            Statements = statements;
        }

    }

    class BlockParser : StackParser<BlockTree, SyntaxTree>
    {

        public BlockParser(Lexer lexer) : base(lexer) { }


        protected override SyntaxTree ParseTree()
        {
            if (Match(TokenType.True))
            {
                ExpectNewLine();
                return new BlockParser(Lexer).Parse();
            }
            
            Expect(TokenType.False);
            return null;
        }

        protected override BlockTree CreateTree(Location location, SyntaxTree[] statements) => new BlockTree(location, statements);

    }
}
