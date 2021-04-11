using Arca.Lexing;
using Arca.Lexing.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arca.Parsing.Statements
{
    struct BlockTree
    {

    }

    class BlockParser : Parser<BlockTree>
    {

        public BlockParser(Lexer lexer) : base(lexer) { }


        protected override BlockTree ParseTree()
        {
            Expect(TokenType.Number);
            Expect(TokenType.Add);
            Expect(TokenType.Number);
            Expect(TokenType.Multiply);
            Expect(TokenType.Number);

            return new BlockTree();
        }

    }
}
