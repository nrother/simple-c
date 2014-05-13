using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleC.Types.Tokens
{
    class KeywordToken : Token
    {
        private static readonly Dictionary<string, KeywordType> validKeywords = new Dictionary<string, KeywordType>()
        {
            { "if", KeywordType.If },
            { "int", KeywordType.Int },
            { "return", KeywordType.Return },
            { "void", KeywordType.Void },
            { "while", KeywordType.While },
        };

        private static readonly Dictionary<KeywordType, VariableType> keywordTypeToVariableType = new Dictionary<KeywordType, VariableType>
        {
            { KeywordType.Int, VariableType.Int },
            { KeywordType.Void, VariableType.Void },
        };

        public KeywordType KeywordType { get; private set; }

        /// <summary>
        /// Returns true, if this keyword is a keyword
        /// for a type, false otherwise.
        /// </summary>
        public bool IsTypeKeyword
        {
            get { return keywordTypeToVariableType.ContainsKey(KeywordType); }
        }

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

        /// <summary>
        /// Returns the assisated VariableType for this keyword,
        /// if this keyword represents a variable type.
        /// Throws an excepion otherwise.
        /// </summary>
        public VariableType ToVariableType()
        {
            return keywordTypeToVariableType[KeywordType];
        }
    }
}
