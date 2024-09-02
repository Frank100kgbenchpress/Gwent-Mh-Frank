using System.Collections.Generic;
using UnityEngine;
using DSL;

public abstract class CardList : MonoBehaviour
{
    public Context Context{get;set;}
    public List<GameObject> Cards = new();
    public abstract List<GameObject> GetCards();
    public virtual List<GameObject> Find(Predicate predicate)
    {
        List<GameObject> filtredCards = new List<GameObject>();
        foreach(var card in Cards)
        {
            Context.variables[predicate.Var.Value] = card;
            if((bool)predicate.Condition.Evaluate(Context)) filtredCards.Add(card);
            Context.variables.Remove(predicate.Var.Value);
        }
        return filtredCards;
    }
    public virtual void Push(GameObject card) =>    Cards.Add(card);
    public virtual void SendBottom(GameObject card) =>        Cards.Insert(0,card);
    public virtual GameObject Pop()
    {
        if(Cards.Count == 0)
        {
            return null;
        }
        GameObject card = Cards[Cards.Count-1];
        Cards.RemoveAt(Cards.Count-1);
        return card;
    }
    public virtual void Remove(GameObject card)=>    Cards.Remove(card);
    public virtual void Shuffle()
    {
        List<GameObject> cards = GetCards();
        for(int i=0;i<cards.Count;i++)
        {
            int randomIndex = UnityEngine.Random.Range(0,cards.Count);
            GameObject temp = cards[i];
            cards[i] = cards[randomIndex];
            cards[randomIndex] = temp;
        }
    }
}
