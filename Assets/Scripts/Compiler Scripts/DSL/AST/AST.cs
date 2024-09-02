using System;
using System.Collections.Generic;
using UnityEngine;

namespace DSL
{
    public abstract class Expression : Node
    {
        public abstract void Print(int pos = 0);
        public abstract object Evaluate(Context context);
    }   

    #region Primitive stuff Number, string and bool
    public class Number : Expression
    {
        public int Value;
        public Number(int value) => Value = value;

        public override object Evaluate(Context context) => Value;

        public override void Print(int pos = 0) => Console.WriteLine(new string(' ', pos) + "Number: " + Value);
    }
    public class String : Expression
    {
        public string Value;
        public String(string value) => Value = value;
        public override object Evaluate(Context context) => Value;
        public override void Print(int pos = 0) => Console.WriteLine(new string(' ', pos) + "String: " + Value);
    }
    public class Bool : Expression
    {
        public bool Value;
        public Bool(bool value) => Value = value;
        public override object Evaluate(Context context) => Value;
        public override void Print(int indent = 0) => Console.WriteLine(new string(' ', indent) + "Bool: " + Value);
    }
    #endregion
    #region Operators (binary)
    public class BinaryOperator : Expression
    {
        protected Expression Left;
        protected Token Operators;
        protected Expression Right;
        public BinaryOperator(Expression left,Token operators,Expression right) => (Left, Operators, Right) = (left,operators,right);
        public override object Evaluate(Context context)  
        {  
            object leftValue = Left.Evaluate(context);  
            object rightValue = Right.Evaluate(context);  
  
            if (leftValue is double leftDouble && rightValue is double rightDouble) return EvaluateNumericOperators(leftDouble, rightDouble);  
  
            if (leftValue is string leftString && rightValue is string rightString) return EvaluateStringOperators(leftString, rightString);  

            throw new InvalidOperationException("Unsupported operator: " + Operators.Lexeme);  
        }  

    object EvaluateNumericOperators(double leftValue, double rightValue)  
    {  
        return Operators.Type switch  
        {  
            TokenType.PLUS => leftValue + rightValue,  
            TokenType.MINUS => leftValue - rightValue,  
            TokenType.MULTIPLY => leftValue * rightValue,  
            TokenType.DIVIDE => leftValue / rightValue,  
            TokenType.MODULUS => leftValue % rightValue,  
            TokenType.POWER => Math.Pow(leftValue, rightValue),  
            TokenType.GREATER => leftValue > rightValue,  
            TokenType.GREATER_EQUAL => leftValue >= rightValue,  
            TokenType.LESS => leftValue < rightValue,  
            TokenType.LESS_EQUAL => leftValue <= rightValue,  
            TokenType.NOT_EQUAL => !leftValue.Equals(rightValue),  
            TokenType.EQUAL => leftValue.Equals(rightValue),  
            _ => throw new InvalidOperationException("Unsupported operator: " + Operators.Lexeme)  
        };  
    }
    object EvaluateStringOperators(string leftValue, string rightValue)  
    {  
        return Operators.Type switch  
        {  
            TokenType.CONCAT => leftValue + rightValue,  
            TokenType.CONCAT_CONCAT => $"{leftValue} {rightValue}",  
            _ => throw new InvalidOperationException("Unsupported operator: " + Operators.Lexeme)  
        };  
    }  
        public override void Print(int pos = 0)
        {
            Console.WriteLine(new string(' ', pos) + "BinaryOperator: " + Operators.Lexeme);
            Left.Print(pos + 2);
            Right.Print(pos + 2);
        }
    }
    
    #endregion
    #region Expressions (Unary , Groups and binary expressions)
    public class UnaryExpression : Expression
    {
        public Token Operators;
        public Expression Right;
        public UnaryExpression(Token operators,Expression right) => (Operators, Right ) = (operators,right);
        public override object Evaluate(Context context) => Operators.Type switch  
        {  
            TokenType.MINUS => -Convert.ToDouble(Right.Evaluate(context)),  
            TokenType.NOT => !(bool)Right.Evaluate(context), 
            TokenType.PLUS_PLUS_LEFT => Convert.ToInt32(Right.Evaluate(context)) + 1, 
            TokenType.MINUS_MINUS_LEFT => Convert.ToInt32(Right.Evaluate(context))-1,
            TokenType.PLUS_PLUS_RIGHT => RightChangesInt(Right,true,context),
            TokenType.MINUS_MINUS_RIGHT => RightChangesInt(Right,false,context),
            _ => throw new InvalidOperationException($"Unsupported operator: {Operators.Lexeme}")  
        }; 
        int RightChangesInt(Expression right,bool plusOrMinus,Context context)
        {
            int originalValue = Convert.ToInt32(right.Evaluate(context));
            int newVal = originalValue + (plusOrMinus ? 1 : -1);
            context.variables[(right as Variable).Value] = newVal;
            return originalValue;
        }
        
        public override void Print(int pos = 0)
        {
            Console.WriteLine(new string(' ', pos) + "UnaryExpression: " + Operators.Lexeme);
            Right.Print(pos + 2);
        }
    }
    public class ExpressionGroup : Expression
    {
        public Expression Exp;
        public ExpressionGroup(Expression expression) => Exp = expression;
        public override object Evaluate(Context context) => Exp.Evaluate(context);
        public override void Print(int indent = 0)
        {
            Console.WriteLine(new string(' ', indent) + "ExpressionGroup:");
            Exp.Print(indent + 2);
        }
    }
    public class BinaryExpression : Expression
    {
        public Expression Left{get;set;}
        public Token Operators{get;set;}
        public Expression Right{get;set;}
        public BinaryExpression(Expression left,Token operators,Expression right) => (Left,Operators,Right) = (left,operators,right);
        public override object Evaluate(Context context)
        {
            object leftValue = Left.Evaluate(context);
            object rightValue = Right.Evaluate(context);
            if(leftValue is int && rightValue is int)
            {
                switch(Operators.Type)
                {
                    case TokenType.PLUS:          return Convert.ToInt32(leftValue) + Convert.ToInt32(rightValue);    
                    case TokenType.MINUS:         return Convert.ToInt32(leftValue) - Convert.ToInt32(rightValue);
                    case TokenType.MULTIPLY:      return Convert.ToInt32(leftValue) * Convert.ToInt32(rightValue);
                    case TokenType.DIVIDE:        return Convert.ToInt32(leftValue) / Convert.ToInt32(rightValue);
                    case TokenType.MODULUS:       return Convert.ToInt32(leftValue) % Convert.ToInt32(rightValue);
                    case TokenType.POWER:         return Math.Pow(Convert.ToInt32(leftValue),Convert.ToInt32(rightValue));
                    case TokenType.GREATER:       return Convert.ToInt32(leftValue) > Convert.ToInt32(rightValue);
                    case TokenType.GREATER_EQUAL: return Convert.ToInt32(leftValue) >= Convert.ToInt32(rightValue);
                    case TokenType.LESS:          return Convert.ToInt32(leftValue) < Convert.ToInt32(rightValue);
                    case TokenType.LESS_EQUAL:    return Convert.ToInt32(leftValue) <= Convert.ToInt32(rightValue);
                    case TokenType.NOT_EQUAL:     return !leftValue.Equals(rightValue);
                    case TokenType.EQUAL:         return leftValue.Equals(rightValue);
                    default: throw new InvalidOperationException("Unsupported operator: " + Operators.Lexeme);
                }
            }
            else if(leftValue is string && rightValue is string)
            {
                switch (Operators.Type)
                {
                    case TokenType.CONCAT : return leftValue.ToString() + rightValue.ToString();
                    case TokenType.CONCAT_CONCAT : return leftValue.ToString() + " " + rightValue.ToString();
                    default: throw new InvalidOperationException("Unsupported operator: " + Operators.Lexeme);
                }
            }
            else throw new InvalidOperationException("Unsupported operator: " + Operators.Lexeme);
        }
        public override void Print(int pos = 0)
        {
            Console.WriteLine(new string(' ', pos) + "BinaryOperator: " + Operators.Lexeme);
            Left.Print(pos + 2);
            Right.Print(pos + 2);
        }
    }
    #endregion
    #region Variables (variables and variables comp)
    public class Variable : Expression
    {
        public Token Token { get; }
        public string Value { get; }
        public Type VariableType { get; set; } 

        public enum Type
        {
            TARGETS, CONTEXT, CARD, FIELD, INT, STRING, BOOL, VOID, NULL
        }

        public Variable(Token token) => (Token, Value, VariableType) = (token, token.Lexeme,Type.NULL);

        public void TypeParam(TokenType tokenType)
        {
            VariableType = tokenType switch
            {
                TokenType.BOOLEANTYPE => Type.BOOL,
                TokenType.NUMBERTYPE => Type.INT,
                TokenType.STRINGTYPE => Type.STRING,
                _ => VariableType // Mantiene el tipo actual si no coincide
            };
        }
        public override object Evaluate(Context context) => context.variables[Value];
        public override void Print(int pos = 0) => Console.WriteLine(new string(' ', pos) + "Variable: " + Value + " (" + VariableType.ToString() + ")"); 
    }
    public class VariableComp : Variable,Stmt
    {
        public Args args;
        public VariableComp(Token token) : base(token) => (args) = new Args();
        public override void Print(int pos = 0)
        {
            Console.WriteLine(new string(' ', pos) + "VariableComp: " + Value);
            args?.Print(pos + 2);
        }
        public void Execute(Context context)
        {
            object last = null;
            foreach(var arg in args.Arguments)
            {
                if(arg is Function)
                {
                    last = (arg as Function).GetValue(context,last);
                }
                else if(arg is Pointer)
                {
                    Pointer pointer = arg as Pointer;
                    switch(pointer.Pointer_)
                    {
                        case "Hand": last = context.turnSystem.HandOfPlayer(context.turnSystem.TriggerPlayer());break;
                        case "Deck": last = context.turnSystem.DeckOfPlayer(context.turnSystem.TriggerPlayer());break;
                        case "Graveyard": last = context.turnSystem.GraveyardOfPlayer(context.turnSystem.TriggerPlayer());break;
                        case "Field": last = context.turnSystem.FieldOfPlayer(context.turnSystem.TriggerPlayer());break;
                        case "Board": last = context.turnSystem.Board();break;
                    }
                }
            }
        }
        public override object Evaluate(Context context)
        {
            object last = context.variables[Value];
            foreach(var arg in args.Arguments)
            {
                if(arg is Function)
                {
                    last = (arg as Function).GetValue(context,last);
                }
                else if(arg is Indexer)
                {
                    if(last is CardList)
                    {
                        List<GameObject> cards = (last as CardList).Cards;
                        Indexer indexer = arg as Indexer;
                        last = cards[indexer.Index];
                    }
                    else
                    {
                        string[] range = last as string[];
                        Indexer indexer = arg as Indexer;
                        last = range[indexer.Index];
                    }
                }
                else if(arg is Pointer)
                {
                    Pointer pointer = arg as Pointer;
                    switch(pointer.Pointer_)
                    {
                        case "Hand": last = context.turnSystem.HandOfPlayer(context.turnSystem.TriggerPlayer());break;
                        case "Deck": last = context.turnSystem.DeckOfPlayer(context.turnSystem.TriggerPlayer());break;
                        case "Graveyard": last = context.turnSystem.GraveyardOfPlayer(context.turnSystem.TriggerPlayer());break;
                        case "Field": last = context.turnSystem.FieldOfPlayer(context.turnSystem.TriggerPlayer());break;
                        case "Board": last = context.turnSystem.Board();break;
                    }
                }
                else
                {
                    Card card = last as Card;
                    switch(arg)
                    {
                        case CardType: last = card.Type;break;
                        case Name: last = card.name;break;
                        case Faction: last = card.Faction;break;
                        case PowerAsField: last = card.Attack;break;
                        case Range: last = card.Range;break;
                        case Owner: last = card.CardOwner;break;
                    }
                }
            }
            return last;
        }

        public void AssignValue(Context context, object value)
        {
            object last = null;
            if(Value == "target")
            {
                last = context.variables[Value];
            }
            foreach(var arg in args.Arguments)
            {
                if(arg is Function)
                {
                    last = (arg as Function).GetValue(context,last);
                }
                else if(arg is Indexer)
                {
                    if(last is CardList)
                    {
                        List<GameObject> cards = (last as CardList).Cards;
                        Indexer indexer = arg as Indexer;
                        last = cards[indexer.Index];
                    }
                    else
                    {
                        string[] range = last as string[];
                        Indexer indexer = arg as Indexer;
                        range[indexer.Index] = value as string;
                    }
                }
                else if(arg is Pointer)
                {
                    Pointer pointer = arg as Pointer;
                    switch(pointer.Pointer_)
                    {
                        case "Hand": last = context.turnSystem.HandOfPlayer(context.turnSystem.TriggerPlayer());break;
                        case "Deck": last = context.turnSystem.DeckOfPlayer(context.turnSystem.TriggerPlayer());break;
                        case "Graveyard": last = context.turnSystem.GraveyardOfPlayer(context.turnSystem.TriggerPlayer());break;
                        case "Field": last = context.turnSystem.FieldOfPlayer(context.turnSystem.TriggerPlayer());break;
                        case "Board": last = context.turnSystem.Board();break;
                    }
                }
                else
                {
                    Card card = last as Card;
                    switch(arg)
                    {
                        case CardType: card.Type = value as string ;break;
                        case Name: card.name = value as string ; break;
                        case Faction: card.Faction = value as string ; break;
                        case PowerAsField: card.Attack = Convert.ToInt32(value) ; break;
                        case Range: last = card.Range ; break ;
                    }
                }
            }
        }
    }
    #endregion
    #region Stmt Blocks , while , for and function 
    public class StmsBlock : Node
    {
        public List<Stmt> statements;
        public StmsBlock() => statements = new();
        public void Print(int indent = 0)
        {
            Console.WriteLine(new string(' ', indent) + "StmsBlock:");
            foreach (var stmt in statements)   stmt.Print(indent + 2);  
        }
    }
    public class WhileStatement : Stmt
    {
        public Expression Condition;
        public StmsBlock Body;
        public WhileStatement(Expression condition,StmsBlock body) => (Condition,Body) = (condition,body);
        public void Print(int pos = 0)
        {
            Console.WriteLine(new string(' ', pos) + "WhileStatement:");
            Console.WriteLine(new string(' ', pos + 2) + "Condition:");
            Condition?.Print(pos + 2);
            Console.WriteLine(new string(' ', pos + 2) + "Body:");
            Body?.Print(pos + 2);
        }
        public void Execute(Context context)
        {
            while((bool)Condition.Evaluate(context))
            {
                foreach(var stmt in Body.statements)
                {
                    stmt.Execute(context);
                }
            }
        }
    }
    public class ForStatement : Stmt
    {
        public Variable Target;
        public Variable Targets;
        public StmsBlock Body;
        public ForStatement(Variable target, Variable targets, StmsBlock body) => (Target,Targets,Body) = (target,targets,body);
        public void Print(int indent = 0)
        {
            Console.WriteLine(new string(' ', indent) + "ForStatement:");
            Console.WriteLine(new string(' ', indent + 2) + "Target:");
            Target?.Print(indent + 2);
            Console.WriteLine(new string(' ', indent + 2) + "Targets:");
            Targets?.Print(indent + 2);
            Console.WriteLine(new string(' ', indent + 2) + "Body:");
            Body?.Print(indent + 2);
        }
        public void Execute(Context context)
        {
            foreach(Card target in context.variables["targets"] as List<Card>)
            {
                context.variables["target"] = target;
                foreach(var stmt in Body.statements)
                {
                    stmt.Execute(context);
                }
                context.variables.Remove("target");
            }
        }
    }
    public class Function : Stmt
    {
        public string FunctionName { get; }
        public Args Args { get; }
        public Variable.Type Type { get; private set; } = Variable.Type.NULL;
        public Function(string functionName, Args args) => (FunctionName,Args,Type) = (functionName,args,DetermineReturnType(functionName));
        Variable.Type DetermineReturnType(string functionName) => functionName switch
        {
            "FieldOfPlayer" => Variable.Type.CONTEXT,
            "HandOfPlayer" => Variable.Type.FIELD,
            "GraveyardOfPlayer" => Variable.Type.FIELD,
            "DeckOfPlayer" => Variable.Type.FIELD,
            "Find" => Variable.Type.TARGETS,
            "Push" => Variable.Type.VOID,
            "SendBottom" => Variable.Type.VOID,
            "Pop" => Variable.Type.CARD,
            "Remove" => Variable.Type.VOID,
            "Shuffle" => Variable.Type.VOID,
            "Add" => Variable.Type.VOID,
            _ => Variable.Type.NULL
        };
        public void Print(int pos = 0)
        {
            Console.WriteLine(new string(' ', pos) + "Function:");
            Console.WriteLine(new string(' ', pos + 2) + "FunctionName: " + FunctionName);
            Args?.Print(pos + 2);
            Console.WriteLine(new string(' ', pos + 2) + "Return Type: " + Type.ToString());
        }
        public void Execute(Context context)=>    throw new NotImplementedException();
        public object GetValue(Context context, object value)
        {
            switch(FunctionName)
            {
                case "TriggerPlayer": return context.turnSystem.TriggerPlayer();
                case "HandOfPlayer": if(Args.Arguments[0] is Function) return context.turnSystem.HandOfPlayer(Convert.ToInt32((Args.Arguments[0] as Function).GetValue(context,value)));
                else return context.turnSystem.HandOfPlayer(Convert.ToInt32((Args.Arguments[0] as Expression).Evaluate(context)));
                case "DeckOfPlayer": if(Args.Arguments[0] is Function) return context.turnSystem.DeckOfPlayer(Convert.ToInt32((Args.Arguments[0] as Function).GetValue(context,value)));
                else return context.turnSystem.DeckOfPlayer(Convert.ToInt32((Args.Arguments[0] as Expression).Evaluate(context)));
                case "GraveyardOfPlayer": if(Args.Arguments[0] is Function) return context.turnSystem.GraveyardOfPlayer(Convert.ToInt32((Args.Arguments[0] as Function).GetValue(context,value)));
                else return context.turnSystem.GraveyardOfPlayer(Convert.ToInt32((Args.Arguments[0] as Expression).Evaluate(context)));
                case "FieldOfPlayer": if(Args.Arguments[0] is Function) return context.turnSystem.FieldOfPlayer(Convert.ToInt32((Args.Arguments[0] as Function).GetValue(context,value)));
                else return context.turnSystem.FieldOfPlayer(Convert.ToInt32((Args.Arguments[0] as Expression).Evaluate(context)));
                //case "Find": return (value as CardList).Find()
                case "Push": (value as CardList).Push((Args.Arguments[0] as Expression).Evaluate(context) as GameObject);return null;
                case "SendBottom": (value as CardList).SendBottom((Args.Arguments[0] as Expression).Evaluate(context) as GameObject);return null;
                case "Pop": return (value as CardList).Pop();
                case "Remove": (value as CardList).Remove((Args.Arguments[0] as Expression).Evaluate(context) as GameObject);return null;
                case "Shuffle": (value as CardList).Shuffle();return null; 
                default: return null;
            }
        }
    }

    #endregion
    #region Assignement
    public class Assignment : Stmt
    {
        public Variable Left {get;}
        public Token Op {get;}
        public Expression Right {get;}
        public Assignment(Variable left, Token op, Expression right) =>(Left,Op,Right) = (left,op,right);
        public void Print(int pos = 0)
        {
            Console.WriteLine(new string(' ', pos) + "Assignment:");
            Left?.Print(pos + 2);
            Console.WriteLine(new string(' ', pos + 2) + "Op: " + Op.Lexeme);
            Right?.Print(pos + 2);
        }
        public void Execute(Context context)
        {
            if(Op.Type == TokenType.ASSIGN)
            {
                if(Left is VariableComp)
                {
                    (Left as VariableComp).AssignValue(context,Right.Evaluate(context));
                }
                else
                {
                    context.variables[Left.Value] = Right.Evaluate(context);
                }
            }
            else if(Op.Type == TokenType.PLUS_EQUALS)
            {
                if(Left is VariableComp)
                {
                    (Left as VariableComp).AssignValue(context,Convert.ToInt32(Left.Evaluate(context))+Convert.ToInt32(Right.Evaluate(context)));
                }
                else
                {
                    int result = Convert.ToInt32(context.variables[Left.Value]);
                    result += Convert.ToInt32(Right.Evaluate(context));
                    context.variables[Left.Value] = result;
                }
            }
            else if(Op.Type == TokenType.MINUS_EQUALS)
            {
                if(Left is VariableComp)
                {
                    (Left as VariableComp).AssignValue(context,Convert.ToInt32(Left.Evaluate(context))-Convert.ToInt32(Right.Evaluate(context)));
                }
                else
                {
                    int result = Convert.ToInt32(context.variables[Left.Value]);
                    result -= Convert.ToInt32(Right.Evaluate(context));
                    context.variables[Left.Value] = result;
                }
            }
        }
    }
    #endregion
}