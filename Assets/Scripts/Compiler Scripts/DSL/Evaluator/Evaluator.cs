using System;
using System.Collections.Generic;
namespace DSL
{
    public class Evaluator
    {
        Card Card{get;set;}
        Context Context{get;set;}
        public Evaluator(Card card,Context context)=> (Card,Context) = (card,context);
        public void EvaluateEffects()
        {
            foreach(var oAElement in Card.Effects.Elements)
            {
                EvaluateOAE(oAElement);
            }
        }
        void EvaluateOAE(OnActivationElements oAElement)
        {
            EvaluateOAEffect(oAElement.OAEffect,oAElement.Selector);
            if(oAElement.PostActions != null)
            {
                foreach(var postAction in oAElement.PostActions)
                {
                    EvaluatePostActions(postAction,oAElement.Selector.Source);
                }
            }
        }
        void EvaluateOAEffect(OAEffect oAEffect,Selector selector)
        {
            EffectNode effect = Context.effects[oAEffect.Name];
            foreach(var assignment in oAEffect.Assingments)
            {
                Context.variables[assignment.Left.Value] = assignment.Right.Evaluate(Context); 
            }
            if(selector != null)
            {
                Context.variables["targets"] = EvaluateSelector(selector);
                EvaluateAction(effect.Action);
                Context.variables.Remove("targets");
            }
            else    EvaluateAction(effect.Action);
        }
        void EvaluatePostActions(PostAction postAction,string source)
        {
            EffectNode effect = Context.effects[(string)postAction.Type.Evaluate(Context)];
            foreach(var assignment in postAction.Assingments)
            {
                Context.variables[assignment.Left.Value] = assignment.Right.Evaluate(Context); 
            }
            if(postAction.Selector != null)
            {
                Context.variables["targets"] = EvaluateSelector(postAction.Selector,source);
                EvaluateAction(effect.Action);
                Context.variables.Remove("targets");
            }
            else
            {
                EvaluateAction(effect.Action);
            }
        }
        List<GameObject> EvaluateSelector(Selector selector, string source = null)
        {
            List<GameObject> cards = new();
            List<GameObject> filtredCards = new();
            if(selector.Source == "parent") cards = EvaluateSource(source);
            else cards = EvaluateSource(selector.Source);
            
            foreach(var card in cards)
            {
                Context.variables[selector.Predicate.Var.Value] = card;
                if((bool)selector.Predicate.Condition.Evaluate(Context) && card.GetComponent<DisplayCard>().type != "Oro") filtredCards.Add(card);
                Context.variables.Remove(selector.Predicate.Var.Value);
            }
            if(selector.Single.Value && filtredCards.Count!=0)
            {
                List<GameObject> result = new List<GameObject>{filtredCards[0]};
                return result;
            }
            else
            {
                return filtredCards;
            }
        }
        List<GameObject> EvaluateSource(string source)
        {
            switch(source)
            {
                case "hand": return Context.turnSystem.HandOfPlayer(Context.turnSystem.TriggerPlayer()).GetCards();
                case "otherHand":if(Context.turnSystem.TriggerPlayer()==2) return Context.turnSystem.HandOfPlayer(1).GetCards();
                else return Context.turnSystem.HandOfPlayer(2).GetCards();
                case "deck": return Context.turnSystem.DeckOfPlayer(Context.turnSystem.TriggerPlayer()).GetCards();
                case "otherDeck":if(Context.turnSystem.TriggerPlayer()==2) return Context.turnSystem.DeckOfPlayer(1).GetCards();
                else return Context.turnSystem.DeckOfPlayer(2).GetCards();
                case "field": return Context.turnSystem.FieldOfPlayer(Context.turnSystem.TriggerPlayer()).GetCards();
                case "otherField":if(Context.turnSystem.TriggerPlayer()==2) return Context.turnSystem.FieldOfPlayer(1).GetCards();
                else return Context.turnSystem.FieldOfPlayer(2).GetCards();
                default: return Context.turnSystem.Board().GetCards();
            }
        }
        void EvaluateAction(Action action) => ExecuteStmtBlock(action.Block);
        void ExecuteStmtBlock(StmsBlock stmsBlock)
        {
            foreach(var stm in stmsBlock.statements)    stm.Execute(Context);
        }
    }
    
}
