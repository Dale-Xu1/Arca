using Arca.Lexing.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arca.Lexing.StateMachines
{
    class NumberMachine : StateMachine<NumberMachine.State>
    {

        public enum State
        {
            StartDigit,
            Digit,

            StartDecimal,
            Decimal,

            Exponent,

            StartRest,
            Rest
        }

        public static bool CanStart(InputStream stream)
        {
            if (CharacterUtil.IsDigit(stream.Current)) return true;
            return (stream.Current == '.' && CharacterUtil.IsDigit(stream.Lookahead(1))); // Test for . with number after it
        }


        private bool isFloat = false;


        public NumberMachine(InputStream stream) : base(stream, State.StartDigit) { }


        protected override State? Next(State state, char current)
        {
            switch (state)
            {
                case State.StartDigit:
                {
                    // Number must start off with at least one digit, or immediately transition to decimals
                    if (CharacterUtil.IsDigit(current)) return State.Digit;
                    else if (current == '.') return State.StartDecimal;

                    break;
                }

                case State.Digit:
                {
                    // Can continue digits or switch to decimal or exponent
                    if (CharacterUtil.IsDigit(current)) return State.Digit;
                    else if (current == '.') return State.StartDecimal;
                    else if (char.ToLower(current) == 'e') return State.Exponent;

                    break;
                }

                case State.StartDecimal:
                {
                    isFloat = true;

                    // Decimals must start with at least one digit
                    if (CharacterUtil.IsDigit(current)) return State.Decimal;
                    break;
                }

                case State.Decimal:
                {
                    // Can continue decimals or switch to exponent
                    if (CharacterUtil.IsDigit(current)) return State.Decimal;
                    else if (char.ToLower(current) == 'e') return State.Exponent;

                    break;
                }

                case State.Exponent:
                {
                    isFloat = true;

                    // Can immediately start remaining numbers or specify exponent sign
                    if (CharacterUtil.IsDigit(current)) return State.Rest;
                    else if (current == '+' || current == '-') return State.StartRest;

                    break;
                }

                case State.StartRest: // Two states used to differentiate between start and rest in success test
                case State.Rest:
                {
                    // Remaining characters are digits
                    if (CharacterUtil.IsDigit(current)) return State.Rest;
                    break;
                }
            }

            return null;
        }

        protected override Token? CreateToken(State state, string value, Location location)
        {
            if ((state == State.Digit) || (state == State.Decimal) || (state == State.Rest))
            {
                TokenType type = isFloat ? TokenType.Float : TokenType.Int;
                return new Token(value, location, type);
            }

            return null;
        }

    }
}
