using Arca.Lexing;
using Arca.Lexing.Tokens;
using Arca.Parsing.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arca.Parsing.Statements
{
    struct BlockTree
    {

        public Location Location { get; }


        public BlockTree(Location location) => Location = location;

    }

    class BlockParser : Parser<BlockTree>
    {

        public BlockParser(Lexer lexer) : base(lexer) { }


        protected override BlockTree ParseTree()
        {
            Expect(TokenType.True);
            Expect(TokenType.False);

            //Expect(TokenType.Indent);
            //while (!Match(TokenType.Dedent))
            //{
            //    ExpressionParser parser = new ExpressionParser(Lexer);
            //    parser.Parse();

            //    if (!Match(TokenType.NewLine, TokenType.Semicolon))
            //    {
            //        Expect(TokenType.Dedent);
            //        break;
            //    }
            //}

            Expect(TokenType.EndOfInput);
            return new BlockTree(Location);
        }

    }
}
