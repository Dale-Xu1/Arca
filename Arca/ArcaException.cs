using Arca.Lexing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arca
{
    class ArcaException : Exception
    {

        public Location Location { get; }


        public ArcaException(Location location, string message) : base(message)
        {
            Location = location;
        }

    }
}
