using Arca.Lexing;
using Arca.Lexing.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arca.Parsing.Expressions
{
    class IdentifierTree : LiteralTree<string>
    {

        public IdentifierTree(Location location, string value) : base(location, value) { }

    }

    class IdentifierParser : Parser<IdentifierTree>
    {

        public IdentifierParser(Lexer lexer) : base(lexer) { }


        protected override IdentifierTree ParseTree(Location location)
        {
            Token token = Expect(TokenType.Identifier);
            return new IdentifierTree(location, token.Value);
        }

    }
}
