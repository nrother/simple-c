using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleC.Types.Tokens
{
    class KeywordToken : Token
    {
        static readonly Dictionary<string, KeywordType> validKeywords = new Dictionary<string, KeywordType>()
        {
            { "if", KeywordType.If },
            { "int", KeywordType.Int },
            { "return", KeywordType.Return },
            { "void", KeywordType.Void },
            { "while", KeywordType.While },
        };

        public KeywordType KeywordType { get; private set; }

        public KeywordToken(string content)
            : base(content)
        {
            if (!validKeywords.ContainsKey(content))
                throw new ArgumentException("The content is no valid keyword.", "content");

            KeywordType = validKeywords[content];
        }

        /// <summary>
        /// Returns true, if the given string is a known
        /// keyword, false otherwise.
        /// </summary>
        public static bool IsKeyword(string s)
        {
            return validKeywords.ContainsKey(s);
        }
    }
}
