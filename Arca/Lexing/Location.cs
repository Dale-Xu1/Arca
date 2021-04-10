using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arca.Lexing
{
    struct Location
    {

        public int Line { get; }
        public int Col { get; }


        public Location(int line, int col)
        {
            Line = line;
            Col = col;
        }


        public override string ToString()
        {
            return $"{Line}:{Col}";
        }

    }
}
