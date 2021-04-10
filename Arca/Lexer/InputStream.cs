using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arca.Lexer
{
    class InputStream
    {

        public char Current { get; private set; }
        public Location Location { get; private set; } = new Location(1, 0);

        private readonly StreamReader reader;


        public InputStream(string path)
        {
            reader = new StreamReader(path);
            Current = NextCharacter();
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

            Current = NextCharacter();
            Location = new Location(line, col);
        }


        private char NextCharacter()
        {
            // Read single character
            int character = reader.Read();

            // Return if not end of input
            if (character >= 0) return (char) character;
            reader.Close();

            return '\0';
        }

    }
}
