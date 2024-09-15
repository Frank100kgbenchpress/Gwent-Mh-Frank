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

        public override void Print(int pos = 0) => Debug.Log(new string(' ', pos) + "Number: " + Value);
    }
    public class String : Expression
    {
        public string Value;
        public String(string value) => Value = value;
        public override object Evaluate(Context context) => Value;
        public override void Print(int pos = 0) => Debug.Log(new string(' ', pos) + "String: " + Value);
    }
    public class Bool : Expression
    {
        public bool Value;
        public Bool(bool value) => Value = value;
        public override object Evaluate(Context context) => Value;
        public override void Print(int pos = 0) => Debug.Log(new string(' ', pos) + "Bool: " + Value);
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
        int RightChangesInt(Expression right,bool plusOrMinus,Context context) // is to actualizate value for operators ++ and --//
        {
            int originalValue = Convert.ToInt32(right.Evaluate(context));
            int newVal = originalValue + (plusOrMinus ? 1 : -1);
            context.variables[(right as Variable).Value] = newVal;
            return originalValue;
        }
        
        public override void Print(int pos = 0)
        {
            Debug.Log(new string(' ', pos) + "UnaryExpression: " + Operators.Lexeme);
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
            Debug.Log(new string(' ', indent) + "ExpressionGroup:");
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
                    case TokenType.EQUAL: return leftValue.Equals(rightValue);
                    case TokenType.NOT_EQUAL: return !leftValue.Equals(rightValue);
                    default: throw new InvalidOperationException("Unsupported operator: " + Operators.Lexeme);
                }
            }
            else
            {
                switch (Operators.Type)
                {
                    case TokenType.EQUAL: return leftValue.Equals(rightValue);
                    case TokenType.NOT_EQUAL: return !leftValue.Equals(rightValue);
                    default: throw new InvalidOperationException("Unsupported operator: " + Operators.Lexeme);
                }
            }
        }
        public override void Print(int pos = 0)
        {
            Debug.Log(new string(' ', pos) + "BinaryOperator: " + Operators.Lexeme);
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
            TARGETS, CONTEXT, CARD, FIELD, INT, STRING, BOOL, VOID, NULL ,LIST, RANGE
        }

        public Variable(Token token) => (Token, Value, VariableType) = (token, token.Lexeme,Type.NULL);

        public void TypeParam(TokenType tokenType)
        {
            VariableType = tokenType switch
            {
                TokenType.BOOLEANTYPE => Type.BOOL,
                TokenType.NUMBERTYPE => Type.INT,
                TokenType.STRINGTYPE => Type.STRING,
                _ => VariableType // Manteins actual type if cant find coincidences//
            };
        }
        public override object Evaluate(Context context) => context.variables[Value];
        public override void Print(int pos = 0) => Debug.Log(new string(' ', pos) + "Variable: " + Value + " (" + VariableType.ToString() + ")"); 
    }
    public class VariableComp : Variable,Stmt
    {
        public Args args;
        public VariableComp(Token token) : base(token) => (args) = new Args();
        public override void Print(int pos = 0)
        {
            Debug.Log(new string(' ', pos) + "VariableComp: " + Value);
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
            object last = Value != "context"? context.variables[Value] : null;
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
                        List<GameObject> cards = (last as CardList).GetCards();
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
                    last = CheckPointer(pointer,context);
                }
                else
                {
                    GameObject card = last as GameObject;
                    switch(arg)
                    {
                        case CardType: last = card.GetComponent<DisplayCard>().Type;break;
                        case Name: last = card.GetComponent<DisplayCard>().NameText.text;break;
                        case Faction: last = card.GetComponent<DisplayCard>().Faction;break;
                        case PowerAsField: last = card.GetComponent<DisplayCard>().Points;break;
                        case Range: last = card.GetComponent<DisplayCard>().Range;break;
                        case Owner: last = card.GetComponent<DisplayCard>().card.CardOwner;break;
                    }
                }
            }
            return last;
        }

        public void AssignValue(Context context, object value)
        {
            
            object last = Value != null ? context.variables[Value]: null;
            foreach(var arg in args.Arguments)
            {
                if(arg is Function)    last = (arg as Function).GetValue(context,last);
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
                    last = CheckPointer(pointer,context);
                }
                else
                {
                    GameObject card = last as GameObject;
                    switch(arg)
                    {
                        case CardType: card.GetComponent<DisplayCard>().Type = value as string;break;
                        case Name: card.GetComponent<DisplayCard>().NameText.text = value as string;break;
                        case Faction: card.GetComponent<DisplayCard>().Faction = value as string;break;
                        case PowerAsField: card.GetComponent<DisplayCard>().Points = Convert.ToInt32(value);
                             card.GetComponent<DisplayCard>().CollectCardPoints(); break;
                        case Range: last = card.GetComponent<DisplayCard>().Range;break;
                    }
                }
            }
        }
        object CheckPointer(Pointer pointer,Context context)
        {
            switch(pointer.Pointer_)
            {
                case "Hand": return context.turnSystem.HandOfPlayer(context.turnSystem.TriggerPlayer());
                case "Deck": return  context.turnSystem.DeckOfPlayer(context.turnSystem.TriggerPlayer());
                case "Graveyard": return  context.turnSystem.GraveyardOfPlayer(context.turnSystem.TriggerPlayer());
                case "Field": return  context.turnSystem.FieldOfPlayer(context.turnSystem.TriggerPlayer());
                case "Board": return context.turnSystem.Board();
            }
            return null;
        }
    }
    #endregion
    #region Stmt Blocks , while , for , function and Assignment
    public class StmsBlock : Node
    {
        public List<Stmt> statements;
        public StmsBlock() => statements = new();
        public void Print(int pos = 0)
        {
            Debug.Log(new string(' ', pos) + "StmsBlock:");
            foreach (var stmt in statements)   stmt.Print(pos + 2);  
        }
    }
    public class WhileStatement : Stmt
    {
        public Expression Condition;
        public StmsBlock Body;
        public WhileStatement(Expression condition,StmsBlock body) => (Condition,Body) = (condition,body);
        public void Print(int pos = 0)
        {
            Debug.Log(new string(' ', pos) + "WhileStatement:");
            Debug.Log(new string(' ', pos + 2) + "Condition:");
            Condition?.Print(pos + 2);
            Debug.Log(new string(' ', pos + 2) + "Body:");
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
            Debug.Log(new string(' ', indent) + "ForStatement:");
            Debug.Log(new string(' ', indent + 2) + "Target:");
            Target?.Print(indent + 2);
            Debug.Log(new string(' ', indent + 2) + "Targets:");
            Targets?.Print(indent + 2);
            Debug.Log(new string(' ', indent + 2) + "Body:");
            Body?.Print(indent + 2);
        }
        public void Execute(Context context)
        {
            foreach(GameObject target in context.variables["targets"] as List<GameObject>)
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
        public Variable.Type Type = Variable.Type.NULL;
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
            Debug.Log(new string(' ', pos) + "Function:");
            Debug.Log(new string(' ', pos + 2) + "FunctionName: " + FunctionName);
            Args?.Print(pos + 2);
            Debug.Log(new string(' ', pos + 2) + "Return Type: " + Type.ToString());
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
                case "Find": (value as CardList).Find(Args.Arguments[0] as Predicate);return null;
                case "Push": (value as CardList).Push((Args.Arguments[0] as Expression).Evaluate(context) as GameObject);return null;
                case "SendBottom": (value as CardList).SendBottom((Args.Arguments[0] as Expression).Evaluate(context) as GameObject);return null;
                case "Pop": return (value as CardList).Pop();
                case "Remove": (value as CardList).Remove((Args.Arguments[0] as Expression).Evaluate(context) as GameObject);return null;
                case "Shuffle": (value as CardList).Shuffle();return null; 
                default: return null;
            }
        }
    }
    #region Assignement
    public class Assignment : Stmt
    {
        public Variable Left {get;}
        public Token Op {get;}
        public Expression Right {get;}
        public Assignment(Variable left, Token op, Expression right) =>(Left,Op,Right) = (left,op,right);
        public void Print(int pos = 0)
        {
            Debug.Log(new string(' ', pos) + "Assignment:");
            Left?.Print(pos + 2);
            Debug.Log(new string(' ', pos + 2) + "Op: " + Op.Lexeme);
            Right?.Print(pos + 2);
        }
        public void Execute(Context context)
        {
            if(Op.Type == TokenType.ASSIGN)
            {
                if(Left is VariableComp)    (Left as VariableComp).AssignValue(context,Right.Evaluate(context));
                else    context.variables[Left.Value] = Right.Evaluate(context);
            }
            else if(Op.Type == TokenType.PLUS_EQUALS)
            {
                if(Left is VariableComp)    (Left as VariableComp).AssignValue(context,Convert.ToInt32(Left.Evaluate(context))+Convert.ToInt32(Right.Evaluate(context)));
                else
                {
                    int result = Convert.ToInt32(context.variables[Left.Value]);
                    result += Convert.ToInt32(Right.Evaluate(context));
                    context.variables[Left.Value] = result;
                }
            }
            else if(Op.Type == TokenType.MINUS_EQUALS)
            {
                if(Left is VariableComp)    (Left as VariableComp).AssignValue(context,Convert.ToInt32(Left.Evaluate(context))-Convert.ToInt32(Right.Evaluate(context)));
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

    #endregion
    
}