using System;
using System.Collections.Generic;

namespace DSL
{
    public class EffectNode : Node
    {
        public Name Name;
        public Args Params;
        public Action Action;
        public EffectNode(){}
        public EffectNode(Name name,Args param,Action action) => (Name,Params,Action) = (name,param,action);
        public void Print(int indent = 0)
        {
            Console.WriteLine(new string(' ', indent) + "Effect:");
            Name?.Print(indent + 2);
            Params?.Print(indent + 2);
            Action?.Print(indent + 2);
        }
    }
    public class OnActivation : Node
    {
        public List<OnActivationElements> Elements;
        public OnActivation() => Elements = new();   
        public void Print(int pos = 0)
        {
            Console.WriteLine(new string(' ', pos) + "OnActivation:");
            foreach (var element in Elements)    element.Print(pos + 2);
        }
    }
    public class OnActivationElements : Node
    {
        public OAEffect OAEffect {get;set;}
        public Selector Selector {get;set;}
        public List<PostAction> PostActions {get;set;}
        public OnActivationElements(OAEffect oaEffect, Selector selector, List<PostAction> pA) => (OAEffect, Selector , PostActions) = (oaEffect,selector,pA);
        public void Print(int pos = 0)
        {
            Console.WriteLine(new string(' ', pos) + "OnActivationElements:");
            OAEffect?.Print(pos + 2);
            Selector?.Print(pos + 2);
            foreach (var postAction in PostActions)    postAction?.Print(pos+2);   
        }
    }
    public class OAEffect : Node
    {
        public string Name {get; set;}
        public List<Assignment> Assingments {get; set;}
        public OAEffect(string name, List<Assignment> assingments) => (Name,Assingments) = (name, assingments);
        public void Print(int pos = 0)
        {
            Console.WriteLine(new string(' ', pos) + "OAEffect:");
            Console.WriteLine(new string(' ', pos + 2) + "Name: " + Name);
            foreach (var assignment in Assingments)    assignment.Print(pos + 2);
        }
    }
    public class Selector : Node
    {
        public string Source {get;set;}
        public Single Single {get;set;}
        public Predicate Predicate {get;set;}
        public Selector(string source,Single single,Predicate predicate) => (Source, Single, Predicate) = (source, single, predicate);
        public void Print(int pos = 0)
        {
            Console.WriteLine(new string(' ', pos) + "Selector:");
            Console.WriteLine(new string(' ', pos + 2) + "Source: " + Source);
            Single?.Print(pos + 2);
            Predicate?.Print(pos + 2);
        }
    }
    public class Single : Node
    {
        public bool Value {get;set;}
        public Single(Token token)
        {
            if(token.Type == TokenType.BOOLEAN)
            {
                if(token.Lexeme == "true")  Value = true;
                else Value = false;
            }
        }
        public void Print(int pos = 0) =>    Console.WriteLine(new string(' ', pos) + "Single: " + Value);
    }
    public class Predicate : Node
    {
        public Variable Var;
        public Expression Condition;
        public Predicate(Variable var,Expression condition) => (Var,Condition) = (var, condition);
        public void Print(int pos = 0)
        {
            Console.WriteLine(new string(' ', pos) + "Predicate:");
            Var?.Print(pos + 2);
            Condition?.Print(pos + 2);
        }
    }
    public class PostAction : Node
    {
        public Expression Type;
        public Selector Selector;
        public List<Assignment> Assingments;
        public PostAction(Expression type,Selector selector) => (Type , Selector,Assingments) = (type,selector,new());
        public void Print(int pos = 0)
        {
            Console.WriteLine(new string(' ', pos) + "PostAction:");
            Type?.Print(pos + 2);
            Selector?.Print(pos + 2);
        }
    }
    public class Action : Node
    {
        public Variable Targets;
        public Variable Context;
        public StmsBlock Block;
        public Action(Variable targets,Variable context,StmsBlock block) => (Targets, Context, Block) = (targets,context , block);
        public void Print(int indent = 0)
        {
            Console.WriteLine(new string(' ', indent) + "Action:");
            Targets?.Print(indent + 2);
            Context?.Print(indent + 2);
            Block?.Print(indent + 2);
        }
    }
}