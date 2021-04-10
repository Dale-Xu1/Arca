using Arca.Lexing.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arca.Lexing.StateMachines
{
    class IdentifierMachine : StateMachine<IdentifierMachine.State>
    {

        public enum State
        {
            Start,
            Rest
        }

        public static bool CanStart(InputStream stream) => CharacterUtil.IsIdentifier(stream.Current);


        public IdentifierMachine(InputStream stream) : base(stream, State.Start, TokenType.Identifier) { }


        protected override State? Next(State state, char current)
        {
            switch (state)
            {
                case State.Start:
                {
                    // First character can only be letters or an underscore
                    if (CharacterUtil.IsIdentifier(current)) return State.Rest;
                    break;
                }

                case State.Rest:
                {
                    // Remaining characters can also be digits
                    if (CharacterUtil.IsIdentifier(current) || CharacterUtil.IsDigit(current)) return State.Rest;
                    break;
                }
            }

            return null;
        }

        protected override bool IsSuccess(State state) => (state == State.Rest);

    }
}
