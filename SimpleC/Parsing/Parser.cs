using SimpleC.Types;
using SimpleC.Types.AstNodes;
using SimpleC.Types.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleC.Parsing
{
    /// <summary>
    /// Parser for the SimpleC language.
    /// </summary>
    class Parser
    {
        /*
         * Some general notes: The part "implementing the parser" wasn't handled
         * in my lecuture until now. So this code has no theoretical background
         * is is most certianly crap. I should improve this as my lecture advances
         * or try to use something like Coco/R or Bison the generate the parser.
         * 
         * This stuff here should be enough for some simple math expression, but
         * please, don't try to take it any further.
         */
        
        public Token[] Tokens { get; private set; }

        private int readingPosition;

        public Parser(Token[] tokens)
        {
            this.Tokens = tokens;

            readingPosition = 0;
        }

        public AstNode ParseToAst()
        {
            var rootNode = new StatementSequenceNode();

            //This only supports (multiple) math expressions, seperated by semicolons. (See note above)

            while (!eof())
            {
                //no switch(typeof()) in C# :(
                Type type = peekType();
                if (type.IsAssignableFrom(typeof(NumberLiteralToken))) //we are starting a math expression, without braces. Read till end of statement.
                {
                    //TODO: INWORK
                }
                else
                    throw new Exception("The parser encountered an unknown token type.");
            }

            return rootNode;
        }

        private IEnumerable<Token> readTokenSeqence(params Type[] expectedTypes)
        {
            foreach (var t in expectedTypes)
            {
                if (t.IsAssignableFrom(peekType()))
                    yield return next();
                else
                    throw new ParsingException("Unexpected token " + peek());
            }
        }

        private IEnumerable<Token> readUntilClosingBrace()
        {
            while (!eof() && !typeof(CloseBraceToken).IsAssignableFrom(peekType()))
                yield return next();
        }

        private IEnumerable<Token> readUntilStatementSeperator()
        {
            while (!eof() && !typeof(StatementSperatorToken).IsAssignableFrom(peekType()))
                yield return next();
        }

        private Token peek()
        {
            //TODO: Check for eof()
            return Tokens[readingPosition];
        }

        private Type peekType()
        {
            return peek().GetType();
        }

        private Token next()
        {
            var ret = peek();
            readingPosition++;
            return ret;
        }

        private Type nextType()
        {
            return next().GetType();
        }

        private bool eof()
        {
            return readingPosition >= Tokens.Length;
        }
    }
}
