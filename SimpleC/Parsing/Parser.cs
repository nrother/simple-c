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
                //either a variable declaration in the style of "int {name}[ = {value}];"
                //or a function declaration in the style of "int {name}({args})\{{body}\}"
                //in both cases we must read the token sequence int/identifier

                KeywordToken typeToken = readKeyword(KeywordType.Int);
                IdentifierToken identifierToken = readToken<IdentifierToken>();
                
                //no switch(typeof()) in C# :(
                Token lookahed = peek();
                if (lookahed is OperatorToken && (((OperatorToken)lookahed).OperatorType == OperatorType.Assignment) || lookahed is StatementSperatorToken) //variable declaration
                {
                    rootNode.AddStatement(new VariableDeclarationNode(typeToken, ExpressionNode.CreateFromTokens(readUntilStatementSeperator())));
                }
                else if(lookahed is OpenBraceToken && (((OpenBraceToken)lookahed).BraceType == BraceType.Round)) //function definition
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
                if (!t.IsAssignableFrom(peek().GetType()))
                    throw new ParsingException("Unexpected token");
                yield return next();
            }
        }

        private IEnumerable<Token> readUntilClosingBrace()
        {
            while (!eof() && !(peek() is CloseBraceToken))
                yield return next();
        }

        private IEnumerable<Token> readUntilStatementSeperator()
        {
            while (!eof() && !(peek() is StatementSperatorToken))
                yield return next();
        }

        private TExpected readToken<TExpected>() where TExpected : Token
        {
            if (peek() is TExpected)
                return (TExpected)next();
            else
                throw new ParsingException("Unexpected token " + peek());
        }

        private KeywordToken readKeyword(KeywordType expectedKeyword)
        {
            var tk = readToken<KeywordToken>();
            if (tk.KeywordType != expectedKeyword)
                throw new ParsingException("Unexpected keyword" + expectedKeyword);
            return tk;
        }

        private Token peek()
        {
            //TODO: Check for eof()
            return Tokens[readingPosition];
        }

        private Token next()
        {
            var ret = peek();
            readingPosition++;
            return ret;
        }

        private bool eof()
        {
            return readingPosition >= Tokens.Length;
        }
    }
}
