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
    /// Parser for SimpleC expressions. Used internally
    /// by the parser.
    /// </summary>
    /// <remarks>
    /// Uses the shunting-yard algorithm by Dijkstra.
    /// Good explanation here: http://wcipeg.com/wiki/Shunting_yard_algorithm
    /// </remarks>
    class ExpressionParser
    {
        private Stack<ExpressionNode> working = new Stack<ExpressionNode>();
        private Stack<ExpressionOperationType> operators = new Stack<ExpressionOperationType>();
        private Stack<int> arity = new Stack<int>();

        //taken from http://en.wikipedia.org/wiki/Operators_in_C_and_C++
        private static readonly Dictionary<ExpressionOperationType, int> operationPrecedence = new Dictionary<ExpressionOperationType, int>()
        {    
            { ExpressionOperationType.FunctionCall, 2 },
            { ExpressionOperationType.Negate, 3 },
            { ExpressionOperationType.Not, 3 },
            { ExpressionOperationType.Multiply, 5 },
            { ExpressionOperationType.Divide, 5 },
            { ExpressionOperationType.Modulo, 5 },
            { ExpressionOperationType.Add, 6 },
            { ExpressionOperationType.Substract, 6 },
            { ExpressionOperationType.LessThan, 8 },
            { ExpressionOperationType.LessEquals, 8 },
            { ExpressionOperationType.GreaterThan, 8 },
            { ExpressionOperationType.GreaterEquals, 8 },
            { ExpressionOperationType.Equals, 9 },
            { ExpressionOperationType.NotEquals, 9 },
            { ExpressionOperationType.And, 13 },
            { ExpressionOperationType.Or, 14 },
            { ExpressionOperationType.Assignment, 16 },
        };

        private static readonly ExpressionOperationType[] unaryOperators = { ExpressionOperationType.Negate, ExpressionOperationType.Not };

        private static readonly Dictionary<OperatorType, ExpressionOperationType> operatorToOperation = new Dictionary<OperatorType, ExpressionOperationType>()
        {
            { OperatorType.Add, ExpressionOperationType.Add},
            //{ OperatorType.SubstractNegate, /*not directly converitble, need to check for unary/binray*/},
            { OperatorType.Multiply, ExpressionOperationType.Multiply},
            { OperatorType.Divide, ExpressionOperationType.Divide},
            { OperatorType.Modulo, ExpressionOperationType.Modulo},
            { OperatorType.Assignment,ExpressionOperationType.Assignment},
            { OperatorType.Equals, ExpressionOperationType.Equals},
            { OperatorType.GreaterThan, ExpressionOperationType.GreaterThan},
            { OperatorType.LessThan, ExpressionOperationType.LessThan},
            { OperatorType.GreaterEquals, ExpressionOperationType.GreaterEquals},
            { OperatorType.LessEquals, ExpressionOperationType.LessEquals},
            { OperatorType.NotEquals, ExpressionOperationType.NotEquals},
            { OperatorType.Not, ExpressionOperationType.Not},
            { OperatorType.And, ExpressionOperationType.And},
            { OperatorType.Or, ExpressionOperationType.Or},
        };

        //used to distigush between unary and binary minus
        private bool lastTokenWasOperatorOrLeftBrace = true; //beginning of input is like a left brace

        /// <summary>
        /// Parses the given tokens to an AST.
        /// </summary>
        public ExpressionNode Parse(IEnumerable<Token> tokens)
        {
            bool sequenceWasEmpty = true;
            
            foreach (var token in tokens)
            {
                sequenceWasEmpty = false;
                if (token is NumberLiteralToken)
                {
                    working.Push(new NumberLiteralNode(((NumberLiteralToken)token).Number));
                    lastTokenWasOperatorOrLeftBrace = false;
                }
                else if (token is OperatorToken)
                {
                    ExpressionOperationType op;
                    if (((OperatorToken)token).OperatorType == OperatorType.SubstractNegate) //need to check if binary of unary
                        op = lastTokenWasOperatorOrLeftBrace ? ExpressionOperationType.Negate : ExpressionOperationType.Substract;
                    else //normal operator
                        op = operatorToOperation[((OperatorToken)token).OperatorType];

                    //TODO: Do we need to check for assosiativity? Only unary operators and assignments are rtl-assosiativ
                    while (operators.Count != 0 && operationPrecedence[operators.Peek()] > operationPrecedence[op]) //stack empty or only low precendence operators on stack
                    {
                        PopOperator();
                    }
                    operators.Push(op);
                    lastTokenWasOperatorOrLeftBrace = true;
                }
                else if (token is OpenBraceToken && ((OpenBraceToken)token).BraceType == BraceType.Round)
                {
                    if(working.Peek() is VariableReferenceExpressionNode) //we have a "variable" sitting on top of the op stack, this must be the name of a function to be called. Let's fix this.
                    {
                        var variable = (VariableReferenceExpressionNode)working.Pop();
                        working.Push(new FunctionCallExpressionNode(variable.VariableName));
                        arity.Push(1);
                    }
                    
                    operators.Push(ExpressionOperationType.OpenBrace);
                    lastTokenWasOperatorOrLeftBrace = true;
                }
                else if (token is CloseBraceToken && ((CloseBraceToken)token).BraceType == BraceType.Round)
                {
                    while (operators.Peek() != ExpressionOperationType.OpenBrace)
                        PopOperator();
                    operators.Pop(); //pop the opening brace from the stack

                    if(operators.Peek() == ExpressionOperationType.FunctionCall) //function call sitting on top of the stack
                        PopOperator();

                    lastTokenWasOperatorOrLeftBrace = false;
                }
                else if(token is IdentifierToken)
                {
                    //this could be a function call, but we would need a lookahead to check for an opening brace.
                    //just handle this as a variable reference and change it to a function when a open brace is found
                    working.Push(new VariableReferenceExpressionNode(((IdentifierToken)token).Content));
                }
                else if(token is ArgSeperatorToken)
                {
                    arity.Push(arity.Pop() + 1); //increase arity on top of the stack
                    
                    while (operators.Peek() != ExpressionOperationType.OpenBrace) //pop till the open brace of this function call
                        PopOperator();
                }
                else
                    throw new ParsingException("Found unknown token while parsing expression!");
            }

            if (sequenceWasEmpty)
                return null;

            //end of tokens, apply all the remaining operators
            while (operators.Count != 0)
                PopOperator();

            if (working.Count != 1)
                throw new ParsingException("Expression seems to be incomplete/invalid.");

            return working.Pop();
        }

        //pop and "apply" operator
        private void PopOperator()
        {
            var op = operators.Pop();
            if(op == ExpressionOperationType.FunctionCall)
            {
                //collect the args
                List<ExpressionNode> args = new List<ExpressionNode>();
                int functionArity = arity.Pop();
                for (int i = 0; i < functionArity; i++)
                    args.Add(working.Pop());
                //add them to the function call sitting on top of the stack
                ((FunctionCallExpressionNode)working.Peek()).AddArguments(args);
            }
            else if (unaryOperators.Contains(op))
                working.Push(new UnaryOperationNode(op, working.Pop()));
            else //binary
            {
                //reverse order of operands!
                var opB = working.Pop();
                var opA = working.Pop();

                working.Push(new BinaryOperationNode(op, opA, opB));
            }
        }
    }
}
