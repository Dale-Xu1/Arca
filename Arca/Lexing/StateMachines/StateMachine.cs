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


        protected StateMachine(InputStream stream, T start)
        {
            this.stream = stream;
            this.start = start;

            location = stream.Location;
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

            return CreateToken(state, builder.ToString(), location);
        }


        protected abstract T? Next(T state, char current);

        protected abstract Token? CreateToken(T state, string value, Location location);

    }
}
