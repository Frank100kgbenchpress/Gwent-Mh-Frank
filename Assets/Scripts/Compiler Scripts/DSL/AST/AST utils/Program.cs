using System.Collections.Generic;
using System;

namespace DSL
{
    public class Program : Node
    {
        public List<CardNode> CardNodes;
        public List<EffectNode> EffectNodes;
        public Program() => (CardNodes,EffectNodes) = (new(),new());
        public void Print(int pos = 0)
        {
            UnityEngine.Debug.Log(new string(' ', pos) + "Program:");
            foreach (var card in CardNodes)    card.Print( pos + 2);

            foreach (var effect in EffectNodes)    effect.Print( pos + 2);
        }
    }   
}