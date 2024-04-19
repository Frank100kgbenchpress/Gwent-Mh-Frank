using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deckManager : MonoBehaviour
{
    public List<GameObject> deck = new List<GameObject>(); // El mazo
    public List<GameObject> hand = new List<GameObject>(); // La mano
    public GameObject Card1;
    public GameObject Card2;
    public GameObject Card3;
    public GameObject Card4;
    public GameObject Card5;
    public GameObject Card6;
    public GameObject Card7;
    public GameObject Card8;
    public GameObject Card9;
    public GameObject Card10;
    public GameObject Card11;
    public GameObject Card12;
    public GameObject Card13;
    public GameObject Card14;
    public GameObject Card15;
    public GameObject Card16;
    public GameObject Card17;
    public GameObject Card18;
    public GameObject Card19;
    public GameObject Card20;
    public GameObject Card21;
    public GameObject Card22;
    public GameObject Card23;
    public GameObject Card24;
    public GameObject Card25;
     void Awake()
    {
        deck.Add(Card1);
        deck.Add(Card2);
        deck.Add(Card3);
        deck.Add(Card4);
        deck.Add(Card5);
        deck.Add(Card6);
        deck.Add(Card7);
        deck.Add(Card8);
        deck.Add(Card9);
        deck.Add(Card10);
        deck.Add(Card11);
        deck.Add(Card12);
        deck.Add(Card13);
        deck.Add(Card14);
        deck.Add(Card15);
        deck.Add(Card16);
        deck.Add(Card17);
        deck.Add(Card18);
        deck.Add(Card19);
        deck.Add(Card20);
        deck.Add(Card21);
        deck.Add(Card22);
        deck.Add(Card23);
        deck.Add(Card24);
        deck.Add(Card25);
    }
    public List<GameObject> GetCards()
    {
        return deck;
    }
}
