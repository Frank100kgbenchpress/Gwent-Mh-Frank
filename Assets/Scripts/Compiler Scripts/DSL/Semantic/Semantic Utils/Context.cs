using System;
using System.Collections.Generic;

namespace DSL
{
    
    public class Context
    {
        public Dictionary<string,Card> cards = new Dictionary<string,Card>();
        public Dictionary<string,EffectNode> effects = new Dictionary<string,EffectNode>();
        public Dictionary<string,object> variables = new Dictionary<string,object>();
        public TurnSystem turnSystem;

        public void AddCard(string name)
        {
            if(cards.ContainsKey(name))
            {
                throw new Exception($"Hay otra carta con el nombre: '{name}'.");
            }
            cards[name] = new Card();
        }
        public void AddEffect(string name)
        {
            if(effects.ContainsKey(name))
            {
                throw new Exception($"Hay otra carta con el nombre: '{name}'.");
            }
            effects[name] = new EffectNode();
        }

        public EffectNode GetEffect(string name)
        {
            if(effects.ContainsKey(name))
            {
                return effects[name];
            }
            else throw new Exception($"There is no effect with the name: '{name}'.");;
        }

    }   
}