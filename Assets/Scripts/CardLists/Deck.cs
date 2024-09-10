using System.Collections.Generic;
using UnityEngine;
public class Deck : CardList
{
    public List<GameObject> gameObjects;

    void Awake()
    {
        foreach(var card in gameObjects)
        {
            Cards.Add(card);
        }
    }
    void Start()    => Shuffle();      
    public override void Push(GameObject card) => Cards.Add(card);
    public override void SendBottom(GameObject card) => Cards.Insert(0,card);
    public override GameObject Pop()
    {
        GameObject card = Cards[Cards.Count-1];
        Cards.RemoveAt(Cards.Count-1);
        return card;
    }
    public override void Remove(GameObject card) =>    Cards.Remove(card); 
    public override void Shuffle()
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
    public override List<GameObject> GetCards() =>  Cards;
}
