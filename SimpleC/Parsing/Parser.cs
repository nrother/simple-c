using SimpleC.Types;
using SimpleC.Types.AstNodes;
using SimpleC.Types.Tokens;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                        if (keyword.IsTypeKeyword)
                        {
                            var varType = keyword.ToVariableType();
                            //it must be followed by a identifier:
                            var name = readToken<IdentifierToken>();
                            //so see what it is (function or variable):
                            Token lookahead = peek();
                            if (lookahead is OperatorToken && (((OperatorToken)lookahead).OperatorType == OperatorType.Assignment) || lookahead is StatementSperatorToken) //variable declaration
                            {
                                if (lookahead is OperatorToken)
                                    next(); //skip the "="
                                scopes.Peek().AddStatement(new VariableDeclarationNode(varType, name.Content, ExpressionNode.CreateFromTokens(readUntilStatementSeperator())));
                            }
                            else if (lookahead is OpenBraceToken && (((OpenBraceToken)lookahead).BraceType == BraceType.Round)) //function definition
                            {
                                var func = new FunctionDeclarationNode(name.Content);
                                scopes.Peek().AddStatement(func); //add the function to the old (root) scope...
                                scopes.Push(func); //...and set it a the new scope!
                                //Read the argument list
                                next(); //skip the opening brace
                                lookahead = peek();
                                while (!(lookahead is CloseBraceToken && ((CloseBraceToken)lookahead).BraceType == BraceType.Round)) //TODO: Refactor using readUntilClosingBrace?
                                {
                                    var argType = readToken<KeywordToken>();
                                    if (!argType.IsTypeKeyword)
                                        throw new ParsingException("Expected type keyword!");
                                    var argName = readToken<IdentifierToken>();
                                    func.AddParameter(new ParameterDeclarationNode(argType.ToVariableType(), argName.Content));
                                    if (peek() is ArgSeperatorToken) //TODO: Does this allow (int a int b)-style functions? (No arg-seperator!)
                                        next(); //skip the sperator
                                }
                                next(); //skip the closing brace
                                var curlyBrace = readToken<OpenBraceToken>();
                                if (curlyBrace.BraceType != BraceType.Curly)
                                    throw new ParsingException("Wrong brace type found!");
                            }
                            else
                                throw new Exception("The parser encountered an unexpected token.");
                        }
                        else
                            throw new ParsingException("Found non-type keyword on top level.");
                    }
                    else //we are in a nested scope
                    {
                        //TODO: Allow Function-local variables!
                        switch (keyword.KeywordType)
                        {
                            case KeywordType.Return:
                                scopes.Peek().AddStatement(new ReturnStatementNode(ExpressionNode.CreateFromTokens(readUntilStatementSeperator())));
                                break;
                            case KeywordType.If:
                                var @if = new IfStatementNode(ExpressionNode.CreateFromTokens(readUntilClosingBrace()));
                                scopes.Peek().AddStatement(@if);
                                scopes.Push(@if);
                                break;
                            case KeywordType.While:
                                var @while = new WhileLoopNode(ExpressionNode.CreateFromTokens(readUntilClosingBrace()));
                                scopes.Peek().AddStatement(@while);
                                scopes.Push(@while);
                                break;
                            default:
                                throw new ParsingException("Unexpected keyword type.");
                        }
                    }
                }
                else if(peek() is IdentifierToken && scopes.Count > 1) //in nested scope
                {
                    var name = readToken<IdentifierToken>();
                    if(peek() is OperatorToken && ((OperatorToken)peek()).OperatorType == OperatorType.Assignment) //variable assignment
                    {
                        next(); //skip the "="
                        scopes.Peek().AddStatement(new VariableAssingmentNode(name.Content, ExpressionNode.CreateFromTokens(readUntilStatementSeperator())));
                    }
                    else //lone expression (incl. function calls!)
                        scopes.Peek().AddStatement(ExpressionNode.CreateFromTokens(readUntilStatementSeperator()));
                }
                else if(peek() is CloseBraceToken)
                {
                    var brace = readToken<CloseBraceToken>();
                    if (brace.BraceType != BraceType.Curly)
                        throw new ParsingException("Wrong brace type found!");
                    scopes.Pop(); //Scope has been closed!
                }
                else
                    throw new ParsingException("The parser ran into an unexpeted token.");
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
            //TODO: Only allow round braces, handle nested braces!
            while (!eof() && !(peek() is CloseBraceToken))
                yield return next();
            next(); //skip the closing brace
        }

        private IEnumerable<Token> readUntilStatementSeperator()
        {
            while (!eof() && !(peek() is StatementSperatorToken))
                yield return next();
            next(); //skip the semicolon
        }

        private TExpected readToken<TExpected>() where TExpected : Token
        {
            if (peek() is TExpected)
                return (TExpected)next();
            else
                throw new ParsingException("Unexpected token " + peek());
        }

        [DebuggerStepThrough]
        private Token peek()
        {
            //TODO: Check for eof()
            return Tokens[readingPosition];
        }

        [DebuggerStepThrough]
        private Token next()
        {
            var ret = peek();
            readingPosition++;
            return ret;
        }

        [DebuggerStepThrough]
        private bool eof()
        {
            return readingPosition >= Tokens.Length;
        }
    }
}
