using Arca.Lexing.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arca.Lexing.StateMachines
{
    class StringMachine : StateMachine<StringMachine.State>
    {

        public enum State
        {
            Start,
            Rest,
            Escape,
            End,
        }

        public static bool CanStart(InputStream stream) => (stream.Current == '"');


        public StringMachine(InputStream stream) : base(stream, State.Start) { }


        protected override State? Next(State state, char current)
        {
            switch (state)
            {
                case State.Start:
                {
                    // First character must be quote
                    if (current == '"') return State.Rest;
                    break;
                }

                case State.Rest:
                {
                    // Can end, escape, or continue string
                    if (current == '"') return State.End;
                    else if (current == '\\') return State.Escape;
                    else if (!CharacterUtil.IsNewLine(current) && current != '\0') return State.Rest;

                    break;
                }

                case State.Escape:
                {
                    // Only specific escape characters can be used
                    if (CharacterUtil.IsEscape(current)) return State.Rest;
                    break;
                }
            }

            return null;
        }

        protected override Token? CreateToken(State state, string value, Location location)
        {
            if (state != State.End) return null;
            return new Token(value, location, TokenType.String);
        }

    }
}
