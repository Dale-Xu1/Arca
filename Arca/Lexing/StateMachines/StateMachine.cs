using Arca.Lexing.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arca.Lexing.StateMachines
{
    abstract class StateMachine<T> where T : struct
    {

        private readonly InputStream stream;
        private readonly T start;

        private readonly Location location;
        private readonly TokenType type;


        protected StateMachine(InputStream stream, T start, TokenType type)
        {
            this.stream = stream;
            this.start = start;

            location = stream.Location;
            this.type = type;
        }


        public Token? Run()
        {
            StringBuilder builder = new StringBuilder();
            T state = start;

            while (true)
            {
                // Get next state
                char current = stream.Current;
                T? next = Next(state, current);

                // Stop when end is reached
                if (next == null) break;

                // Add character to string
                builder.Append(current);
                stream.Next();

                state = (T) next;
            }

            // Ask implementation whether run was successful
            if (IsSuccess(state)) return new Token(builder.ToString(), location, type);
            return null;
        }


        protected abstract T? Next(T state, char current);

        protected abstract bool IsSuccess(T state);

    }
}
