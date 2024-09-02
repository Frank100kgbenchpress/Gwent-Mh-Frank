using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Graveyard : CardList
{
    public TextMeshProUGUI GraveyardText;
    public override List<GameObject> GetCards() =>  Cards;
    public override void Push(GameObject card)
    {
        Cards.Add(card);
        int aux = GetNumber();
        GraveyardText.text = aux+1.ToString();
    }
    public override void SendBottom(GameObject card)
    {
        Cards.Insert(0,card);
        int aux = GetNumber();
        GraveyardText.text = aux+1.ToString();
    }
    public override GameObject Pop()
    {
        GameObject card = Cards[Cards.Count-1];
        Cards.RemoveAt(Cards.Count-1);
        int aux = GetNumber();
        GraveyardText.text = aux--.ToString();
        return card;
    }
    public override void Remove(GameObject card)
    {
        int aux = GetNumber();
        GraveyardText.text = aux--.ToString();
        Cards.Remove(card);
    }
    public int GetNumber()
    {
        string aux = GraveyardText.text.ToString();
        int temp = Convert.ToInt32(aux);
        return temp;
    }
}
