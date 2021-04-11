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
            Expect(TokenType.True);
            Expect(TokenType.False);

            Expect(TokenType.Indent);
            while (!Match(TokenType.Dedent))
            {
                Expect(TokenType.Number);

                if (!Match(TokenType.NewLine, TokenType.Semicolon))
                {
                    Expect(TokenType.Dedent);
                    break;
                }
            }

            Expect(TokenType.EndOfInput);

            return new BlockTree();
        }

    }
}
