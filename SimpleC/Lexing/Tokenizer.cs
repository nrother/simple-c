using SimpleC.Types;
using SimpleC.Types.Tokens;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleC.Lexing
{
    /// <summary>
    /// Tokenizer for the SimpleC language.
    /// </summary>
    class Tokenizer
    {
        public string Code { get; private set; }

        private int readingPosition;

        public Tokenizer(string code)
        {
            this.Code = code;

            readingPosition = 0;
        }

        public Token[] Tokenize()
        {
            var tokens = new List<Token>();

            var builder = new StringBuilder();

            while (!eof())
            {
                skip(CharType.WhiteSpace); //white space has no meaning besides sperating tokens (we not python!)
                switch (peekType())
                {
                    case CharType.Alpha: //start of identifier
                        readToken(builder, CharType.AlphaNumeric);
                        tokens.Add(new IdentifierToken(builder.ToString()));
                        builder.Clear();
                        break;
                    case CharType.Numeric: //start of number literal
                        readToken(builder, CharType.Numeric);
                        tokens.Add(new NumberLiteralToken(builder.ToString()));
                        builder.Clear();
                        break;
                    case CharType.Operator:
                        readToken(builder, CharType.Operator);
                        tokens.Add(new OperatorToken(builder.ToString()));
                        builder.Clear();
                        break;
                    case CharType.OpenBrace:
                        tokens.Add(new OpenBraceToken(next().ToString()));
                        break;
                    case CharType.CloseBrace:
                        tokens.Add(new CloseBraceToken(next().ToString()));
                        break;
                    case CharType.ArgSeperator:
                        tokens.Add(new ArgSeperatorToken(next().ToString()));
                        break;
                    case CharType.StatementSeperator:
                        tokens.Add(new StatementSperatorToken(next().ToString()));
                        break;
                    default:
                        throw new Exception("The tokenizer found an unidentifiable character.");
                }
            }

            return tokens.ToArray();
        }

        /// <summary>
        /// Reads characters into the StringBuilder while they match
        /// the given type(s).
        /// </summary>
        private void readToken(StringBuilder builder, CharType typeToRead)
        {
            while (!eof() && peekType().HasAnyFlag(typeToRead))
                builder.Append(next());
        }

        /// <summary>
        /// Skips any count of occurences of the given char class(es).
        /// </summary>
        private void skip(CharType typeToSkip)
        {
            while (peekType().HasAnyFlag(typeToSkip))
                next();
        }

        /// <summary>
        /// Returns the CharType of the next char,
        /// without advancing the pointer.
        /// </summary>
        /// <returns></returns>
        private CharType peekType()
        {
            return charTypeOf(peek());
        }

        /// <summary>
        /// Returns the CharType of the next char,
        /// advancing the pointer.
        /// </summary>
        /// <returns></returns>
        private CharType nextType()
        {
            return charTypeOf(next());
        }

        private CharType charTypeOf(char c)
        {
            //TODO: What says the C/C# spec for this?

            //First the small sets
            switch (c)
            {
                case '+':
                case '-':
                case '*':
                case '/':
                case '%':
                case '&':
                case '|':
                case '=':
                    return CharType.Operator;
                case '(':
                    return CharType.OpenBrace;
                case ')':
                    return CharType.CloseBrace;
                case ',':
                    return CharType.ArgSeperator;
                case ';':
                    return CharType.StatementSeperator;
            }

            //than the categories
            switch (char.GetUnicodeCategory(c))
            {
                case UnicodeCategory.DecimalDigitNumber:
                    return CharType.Numeric;
                case UnicodeCategory.LineSeparator:
                    return CharType.NewLine;
                case UnicodeCategory.ParagraphSeparator:
                case UnicodeCategory.LowercaseLetter:
                case UnicodeCategory.OtherLetter:
                case UnicodeCategory.UppercaseLetter:
                    return CharType.Alpha;
                case UnicodeCategory.SpaceSeparator:
                    return CharType.LineSpace;
            }

            return CharType.Unknown; //something really odd, we could probably allow it as a CharType.Alpha, when its not a Control-Char.
        }

        /// <summary>
        /// Returns the next character without advancing the pointer.
        /// </summary>
        private char peek()
        {
            //TODO: Check for eof()
            return Code[readingPosition];
        }

        /// <summary>
        /// Returns the next character and advances the pointer.
        /// </summary>
        private char next()
        {
            var ret = peek();
            readingPosition++;
            return ret;
        }

        /// <summary>
        /// Returns true if there are no more chars left.
        /// </summary>
        private bool eof()
        {
            return readingPosition >= Code.Length;
        }
    }
}
