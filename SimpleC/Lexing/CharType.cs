using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleC.Lexing
{
    /// <summary>
    /// The possible classes for a character.
    /// This is a Flags-enum, please note that there are
    /// compund values.
    /// </summary>
    [Flags]
    enum CharType
    {
        /// <summary>
        /// Unknown. This is 0x00, so it can't be checked with HasFlag!
        /// (A character can only be unknown or anything else)
        /// </summary>
        Unknown = 0x00,
        /// <summary>
        /// a-z,A-Z,_. Anything acceptable to start an identifier.
        /// </summary>
        Alpha = 0x01, //we need binary literals!
        /// <summary>
        /// 0-9.
        /// </summary>
        Numeric = 0x02,
        /// <summary>
        /// Spaces,tabs. Whitespace, but no newline.
        /// </summary>
        LineSpace = 0x04,
        /// <summary>
        /// A newline character.
        /// </summary>
        NewLine = 0x08,
        /// <summary>
        /// +,-,*,/,%,&,|,=,&gt;,&lt;,!.
        /// </summary>
        Operator = 0x10,
        /// <summary>
        /// (,[,{.
        /// </summary>
        OpenBrace = 0x20,
        /// <summary>
        /// ),],}.
        /// </summary>
        CloseBrace = 0x40,
        /// <summary>
        /// ,. Comma used to seperate arguments to functions.
        /// </summary>
        ArgSeperator = 0x80,
        /// <summary>
        /// ;. Semicolon used to seperate statements.
        /// </summary>
        StatementSeperator = 0x100,

        //compund values:
        AlphaNumeric = Alpha | Numeric,
        WhiteSpace = LineSpace | NewLine,
        Brace = OpenBrace | CloseBrace,
        /// <summary>
        /// Chars that "have a special meaning".
        /// </summary>
        MetaChar = Operator | Brace |ArgSeperator | StatementSeperator,
        All = AlphaNumeric | WhiteSpace | MetaChar,
    }
}
