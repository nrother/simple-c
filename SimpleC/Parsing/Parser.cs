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
        private Stack<StatementSequenceNode> scopes;

        private static readonly KeywordType[] typeKeywords = { KeywordType.Int, KeywordType.Void };

        public Parser(Token[] tokens)
        {
            this.Tokens = tokens;

            readingPosition = 0;
            scopes = new Stack<StatementSequenceNode>();
        }

        public ProgramNode ParseToAst()
        {
            scopes.Push(new ProgramNode());

            while (!eof())
            {
                if (peek() is KeywordToken)
                {
                    var keyword = (KeywordToken)next();

                    if (scopes.Count == 1) //we are a top level, the only valid keywords are variable types, starting a variable or function definition
                    {
                        if (!keyword.IsTypeKeyword)
                        {
                            var varType = keyword.ToVariableType();
                            //it must be followed by a identifier:
                            var name = readToken<IdentifierToken>();
                            //so see what it is (function or variable):
                            Token lookahed = peek();
                            if (lookahed is OperatorToken && (((OperatorToken)lookahed).OperatorType == OperatorType.Assignment) || lookahed is StatementSperatorToken) //variable declaration
                            {
                                scopes.Peek().AddStatement(new VariableDeclarationNode(varType, name.Content ,ExpressionNode.CreateFromTokens(readUntilStatementSeperator())));
                            }
                            else if (lookahed is OpenBraceToken && (((OpenBraceToken)lookahed).BraceType == BraceType.Round)) //function definition
                            {
                                var func = new FunctionDeclarationNode(name.Content);
                                scopes.Peek().AddStatement(func); //add the function to the old (root) scope...
                                scopes.Push(func); //...and set it a the new scope!
                            }
                            else
                                throw new Exception("The parser encountered an unexpected token.");
                        }
                        else
                            throw new ParsingException("Found non-type keyword on top level.");
                    }
                } //TODO: More keywords, expression in functions!
            }

            if (scopes.Count != 1)
                throw new ParsingException("The scopes are not correctly nested.");

            return (ProgramNode)scopes.Pop();
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
