using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arca.Lexing
{
    class InputStream
    {

        public char Current { get; private set; }
        public Location Location { get; private set; } = new Location(1, 0);

        private StreamReader reader;
        private readonly List<char> queue = new List<char>();


        public InputStream(string path)
        {
            reader = new StreamReader(path);
            Current = NextCharacter();
        }


        public char Lookahead(int offset)
        {
            if (offset == 0) return Current;

            if (offset > queue.Count)
            {
                // Queue values if they haven't been seen yet
                for (int i = queue.Count; i < offset; i++)
                {
                    char current = NextCharacter();

                    // Can terminate early if end of input is reached
                    if (current == '\0') return current;
                    queue.Add(current);
                }
            }

            // -1 because the first element is the character after Current, so an offset of 1 is found at index 0
            return queue[offset - 1];
        }

        public void Next()
        {
            // Can't read farther if end was reached
            if (Current == '\0') return;

            int line = Location.Line;
            int col = Location.Col;

            if (Current == '\n')
            {
                // Next line and reset column
                line++;
                col = 0;
            }
            else
            {
                col++; // Next column
            }

            if (queue.Count > 0)
            {
                // Get value from queue if there are any
                Current = queue[0];
                queue.RemoveAt(0);
            }
            else
            {
                Current = NextCharacter();
            }

            Location = new Location(line, col);
        }


        private char NextCharacter()
        {
            if (reader != null)
            {
                // Read single character
                int character = reader.Read();

                // Return if not end of input
                if (character >= 0) return (char) character;

                reader.Close();
                reader = null;
            }

            return '\0';
        }

    }
}
