using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arca.Lexing
{
    class CharacterUtil
    {

        public static bool IsIdentifier(char c) => (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || c == '_';

        public static bool IsDigit(char c) => (c >= '0' && c <= '9');


        public static bool IsIndent(char c) => (c == ' ' || c == '\t');

        public static bool IsNewLine(char c) => (c == '\n' || c == '\r');


        public static bool IsEscape(char c) => (c == '"') || (c == '\\') || (c == 'n') || (c == 'r') || (c == 't') || (c == '0');

    }
}
