﻿using Arca.Lexing.StateMachines;
using Arca.Lexing.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arca.Lexing.Constants
{
    class KeywordLexer
    {

        public static Dictionary<TokenType, string> Keywords { get; } = new Dictionary<TokenType, string>
        {
            [TokenType.True] = "true",
            [TokenType.False] = "false",
            [TokenType.Null] = "null"
        };


        private readonly IdentifierMachine machine;


        public KeywordLexer(IdentifierMachine machine) => this.machine = machine;


        public Token? Run()
        {
            // Run identifier state machine
            Token? token = machine.Run();
            if (token == null) return null;

            string identifier = ((Token) token).Value;

            // Test if identifier matches with keyword
            foreach (KeyValuePair<TokenType, string> keyword in Keywords)
            {
                if (string.Equals(identifier, keyword.Value))
                {
                    // Create keyword token
                    Location location = ((Token) token).Location;
                    return new Token(identifier, location, keyword.Key);
                }
            }

            return token;
        }

    }
}