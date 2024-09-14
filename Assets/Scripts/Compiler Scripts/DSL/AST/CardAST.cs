using UnityEngine;
namespace DSL
{
    public class CardNode : Node
    {
        public CardType Type {get;set;}
        public Name Name {get;set;}
        public Faction Faction {get;set;}
        public Power Power {get;set;}
        public Range Range {get;set;}
        public OnActivation OnActivation {get;set;}
        public CardNode(){}
        public CardNode(CardType type,Name name,Faction faction,Power power,Range range,OnActivation onActivation) =>(Type,Name,Faction,Power,Range,OnActivation) = (type,name,faction,power,range,onActivation);
        public void Print(int pos = 0)
        {
            Debug.Log(new string(' ', pos) + "Card:");
            Type?.Print(pos + 2);
            Name?.Print(pos + 2);
            Faction?.Print(pos + 2);
            Power?.Print(pos + 2);
            Range?.Print(pos + 2);
            OnActivation?.Print(pos + 2);
        }
    }   
    #region Card Atributes (Name, CardType, Faction, Power , Range and owner)
    public class Name : Node
    {
        public Expression name {get;set;}
        public Name (Expression expression) => name = expression;
        public void Print(int pos = 0)
        {
            Debug.Log(new string(' ', pos) + "Name:");
            name.Print(pos + 2);
        }
    }
    public class CardType : Node
    {
        public Expression Type {get; set;}
        public CardType (Expression expression) => Type = expression;
        public void Print(int pos = 0)
        {
            Debug.Log(new string(' ', pos) + "Type:");
            Type.Print(pos + 2);
        }
    }
    public class Faction : Node
    {
        public Expression faction {get; set;}
        public Faction(Expression expression) => faction = expression;
        public void Print(int pos = 0)
        {
            Debug.Log(new string(' ', pos) + "Faction:");
            faction.Print(pos + 2);
        }
    }
    public class Power : Node
    {
        public Expression power {get; set;}
        public Power(Expression expression) => power = expression;
        public void Print(int pos = 0)
        {
            Debug.Log(new string(' ', pos) + "Power:");
            power.Print(pos + 2);
        }
    }
    public class Range : Node
    {
        public Expression[] range {get;set;}
        string Lexeme {get; set;}
        public Range(Expression[] expressions) => range = expressions;
        public Range(string lexeme) => Lexeme = lexeme;
    
        public void Print(int pos = 0)
        {
            Debug.Log(new string(' ', pos) + "Range:");
            if(range != null) foreach(var expr in range)    expr.Print(pos + 2);
            else Debug.Log(new string(' ', pos) + "Lexeme: " + Lexeme);
        }
    }
    public class PowerAsField : Node
    {
        public PowerAsField(){}
        public void Print(int pos) {}
    }
    public class Owner : Node
    {
        public string Owner_;
        public Owner(string owner)=>    Owner_ = owner;
        public void Print(int indent = 0)=>    Debug.Log(new string(' ', indent) + "Owner: " + Owner_);
    }
    #endregion
}