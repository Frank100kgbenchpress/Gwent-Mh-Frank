using System.Collections.Generic;
using UnityEngine;
//aqui manejo todos los eventos relacionados con poner cartas//

public class DragAndDrop : MonoBehaviour 
{
    Vector2 startPosition;
    bool isDraggin = false;
    bool isOverDropZone = false;
    GameObject dropZone;
    public TurnSystem endturn;
    public TurnSystem turns;
    public effects effect;
    public Deck deck;
    public Draw draw;
    public Change change;
    


    void Update()
    {
        if(isDraggin)
        {
            transform.position = new Vector2(Input.mousePosition.x,Input.mousePosition.y);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        isOverDropZone = true;
        dropZone = collision.gameObject;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        isOverDropZone=false;
        dropZone = null;
    }
     

    public void StartDrag()
    {
        startPosition = transform.position;
        isDraggin = true;
    }
    public void EndDrag()
    {
        
        isDraggin = false;
        if(isOverDropZone && CorrectZone())
        {
            transform.SetParent(dropZone.transform, false);
            endturn = GameObject.Find("GameManager").GetComponent<TurnSystem>();
            effect = GameObject.Find("GameManager").GetComponent<effects>();
            //effect.UseEffect(gameObject.GetComponent<DisplayCard>().card.Effects,gameObject);
            //esto es para si activo una carta despues de un aumento tambien coja puntos//
            if(effect.effectLoop)
            {
                DisplayCard[] cards = new DisplayCard[6];
                cards[0] = GameObject.Find("SupportMeleeZone").GetComponentInChildren<DisplayCard>();
                cards[1] = GameObject.Find("SupportRangedZone").GetComponentInChildren<DisplayCard>();
                cards[2] = GameObject.Find("SupportSiegeZone").GetComponentInChildren<DisplayCard>();
                cards[3] = GameObject.Find("EnemySupportSiegeZone").GetComponentInChildren<DisplayCard>();
                cards[4] = GameObject.Find("EnemySupportMeleeZone").GetComponentInChildren<DisplayCard>();
                cards[5] = GameObject.Find("EnemySupportRangedZone").GetComponentInChildren<DisplayCard>();
                for(int i=0;i<6;i++)
                {
                    if(cards[i]!=null)
                    {
                        //effect.UseEffect(cards[i].card.Effects,cards[i].gameObject);
                    }
                }
            }
            //para que los climas sigan funcionando para otras cartas//
            if(effect.wheatherUse)
            {
                DisplayCard[] cards = GameObject.Find("WeatherZone").GetComponentsInChildren<DisplayCard>();
                if(cards != null)
                {
                    foreach(var card in cards)
                    {
                        //effect.UseEffect(card.card.Effects,card.gameObject);
                    }
                }
            }
            //este es para usar el decoy//
            if(!endturn.useDecoy)
            {
                endturn.EndTurn();
            }

           
        }
        else
        {
            transform.position = startPosition;
        }
        
    }
    //esto es para comprobar que la carta sea puesta donde va//
    public bool CorrectZone()
    {
        DisplayCard cardZone = gameObject.GetComponent<DisplayCard>();
        ZoneConditions conditions = dropZone.GetComponent<ZoneConditions>(); 
        string zoneName = conditions.Zone;
        string zoneOwner = conditions.OWner.ToString();
        foreach (var range in cardZone.card.Range)
        {
            if(range == zoneName && cardZone.Owner == zoneOwner) return true;
        }
        if(cardZone.Owner == "Neutral" && zoneName == "Wheather") return true;
        return false;
    }
    //esto era para que funcione el decoy//
    public void OnPointerClick()
    {
        DisplayCard cardDisplay = GetComponent<DisplayCard>();
        TurnSystem decoy = GameObject.Find("GameManager").GetComponent<TurnSystem>();
        if(decoy.useDecoy && decoy.Team==false)
        {
            if(cardDisplay.Owner=="Player" && cardDisplay.card.Type != "Oro")
            {
                GameObject zone1 = GameObject.Find("PlayerHand");
                cardDisplay.Points = cardDisplay.card.Attack;
                cardDisplay.AttackText.text = cardDisplay.Points.ToString();
                cardDisplay.Boost = false;
                transform.position = zone1.transform.position;
                transform.SetParent(zone1.transform,false);
                decoy.useDecoy = false;
                decoy.EndTurn();
            }
        }
        else if(decoy.useDecoy && decoy.Team)
        {
            if(cardDisplay.Owner=="Enemy" && cardDisplay.card.Type !="Oro")
            {
                GameObject zone1 = GameObject.Find("EnemyHand");
                cardDisplay.Points = cardDisplay.card.Attack;
                cardDisplay.AttackText.text = cardDisplay.Points.ToString();
                cardDisplay.Boost = false;
                transform.position = zone1.transform.position;
                transform.SetParent(zone1.transform,false);
                decoy.useDecoy = false;
                decoy.EndTurn();
            }
        }
    }
    //esto es para cuando quieras cambiar cartas la toques y se cambien//
    public void ChangingCards()
    {
        draw = GameObject.Find("GameManager").GetComponent<Draw>();
        endturn = GameObject.Find("GameManager").GetComponent<TurnSystem>();
        DisplayCard cardDisplay = GetComponent<DisplayCard>();
        if(endturn.isYourTurn)
        {
            change = GameObject.Find("Change").GetComponent<Change>();
            GameObject hand = GameObject.Find("PlayerHand");
            if(change.change)
            {
                deck = GameObject.Find("deckManager1").GetComponent<Deck>();
                List<GameObject> deckCards = deck.GetCards();
                if(cardDisplay.Owner == "Player")
                {
                    deckCards.Add(gameObject);
                    draw.DrawCard(1);
                    Destroy(gameObject);
                    change.counter++;
                    if(change.counter ==2)
                    {
                        endturn.NoMove(hand,true);
                        change.change = false;
                        change.Hide();
                    }
                }
            }
        }
        else if(endturn.isYourTurn==false)
        {
            change = GameObject.Find("EnemyChange").GetComponent<Change>();
            GameObject hand = GameObject.Find("EnemyHand");
            if(change.change)
            {
                deck = GameObject.Find("deckManager2").GetComponent<Deck>();
                List<GameObject> deckCards = deck.GetCards();
                if(cardDisplay.Owner == "Enemy")
                {
                    deckCards.Add(gameObject);
                    draw.DrawCard(2);
                    Destroy(gameObject);
                    change.counter++;
                    if(change.counter == 2)
                    {
                        endturn.NoMove(hand,true);
                        change.change = false;
                        change.Hide();
                    }
                }
            } 
        }
    }
}

