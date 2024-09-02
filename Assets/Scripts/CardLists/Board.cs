using System.Collections.Generic;
using UnityEngine;

public class Board : CardList
{
    public GameObject Units1;
    public GameObject Units2;
    public GameObject Inspires1;
    public GameObject Inspires2;
    public override List<GameObject> GetCards()
    {
        List<GameObject> cards = new();
        CardsToAdd();
        return cards;
    }
    void CardsToAdd()
    {
        AddCards(Units1);
        AddCards(Units2);
        AddCards(Units2);
        AddCards(Inspires2);
    }
    void AddCards(GameObject zone)
    {
        foreach(Transform transform in zone.transform)
        {
            foreach(Transform trans in transform)
            {
                Cards.Add(trans.gameObject);
            }
        }
    }
    public override void Push(GameObject card)=>    Cards.Add(card);
    public override void SendBottom(GameObject card)=>    Cards.Insert(0,card);
    public override GameObject Pop()
    {
        if(Cards.Count == 0)    return null;
        
        else
        {
            GameObject card = Cards[Cards.Count-1];
            Cards.RemoveAt(Cards.Count-1);
            DeleteCard(card);
            return card;
        }
    }

    public override void Remove(GameObject card)
    {
        DeleteCard(card);
        Cards.Remove(card);
    }

    void DeleteCard(GameObject card)
    {
        CardToDelete(Units1,card);
        CardToDelete(Units2,card);
        CardToDelete(Inspires1,card);
        CardToDelete(Inspires2,card);
    }
    void CardToDelete(GameObject zone,GameObject card)
    {
        foreach (Transform transform in zone.transform)
        {
            foreach (Transform trans in transform)
            {
                if(trans.gameObject == card)
                {
                    Destroy(trans.gameObject);
                    break;
                }
            }   
        }
    }
}
