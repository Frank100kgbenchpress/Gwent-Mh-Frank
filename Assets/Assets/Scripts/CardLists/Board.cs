using System.Collections.Generic;
using UnityEngine;

public class Board : CardList
{
    public GameObject Units1;
    public GameObject Units2;
    public GameObject Inspires1;
    public GameObject Inspires2;
    void Awake()
    {
        GetCards();
    }  
    void Start()
    {
        GetCards();
    }
    public override List<GameObject> GetCards()
    {
        List<GameObject> cards = new();
        foreach(Transform transform in Units1.transform)
        {
            foreach(Transform trans in transform)
            {
                cards.Add(trans.gameObject);
            }
        }
        foreach(Transform transform in Units2.transform)
        {
            foreach(Transform trans in transform)
            {
                cards.Add(trans.gameObject);
            }
        }
        foreach(Transform transform in Inspires1.transform)
        {
            foreach(Transform trans in transform)
            {
                cards.Add(trans.gameObject);
            }
        }
        foreach(Transform transform in Inspires2.transform)
        {
            foreach(Transform trans in transform)
            {
                cards.Add(trans.gameObject);
            }
        }
        return cards;
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
        foreach (Transform transform in Units1.transform)
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
        foreach (Transform transform in Units2.transform)
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
        foreach (Transform transform in Inspires1.transform)
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
        foreach (Transform transform in Inspires2.transform)
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
