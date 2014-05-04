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

            while (!eof())
            {
                //at the top level there can only be two things:
                //either a variable declaration in the style of "int {name} = {value};"
                //or a function declaration in the style of "int {name}({args})\{{body}\}"
                //in both cases we must read the token sequence int/identifier

                KeywordToken typeToken = readKeyword(KeywordType.Int);
                IdentifierToken identifierToken = (IdentifierToken)readToken(typeof(IdentifierToken));
                
                //no switch(typeof()) in C# :(
                Token operatorOrBrace = readToken(typeof(Token));
                if (operatorOrBrace.GetType().IsAssignableFrom(typeof(OperatorToken)) && ((OperatorToken)operatorOrBrace).OperatorType == OperatorType.Assignment) //variable declaration
                {
                    
                }
                else if(operatorOrBrace.GetType().IsAssignableFrom(typeof(OpenBraceToken)) && ((OpenBraceToken)operatorOrBrace).BraceType == BraceType.Round) //function definition
                {

                }
                else
                    throw new Exception("The parser encountered an unexpected token.");
            }

            return rootNode;
        }

        private IEnumerable<Token> readTokenSeqence(params Type[] expectedTypes)
        {
            foreach (var t in expectedTypes)
            {
                yield return readToken(t);
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

        private Token readToken(Type expectedType)
        {
            if (expectedType.IsAssignableFrom(peekType()))
                return next();
            else
                throw new ParsingException("Unexpected token " + peek());
        }

        private KeywordToken readKeyword(KeywordType expectedKeyword)
        {
            var tk = (KeywordToken)readToken(typeof(KeywordToken));
            if (tk.KeywordType != expectedKeyword)
                throw new ParsingException("Unexpected keyword" + expectedKeyword);
            return tk;
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
