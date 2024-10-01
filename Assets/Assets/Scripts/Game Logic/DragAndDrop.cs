using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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
    public string PlacedZone;
    EventTrigger eventTrigger;


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
     void Start() =>    eventTrigger = GetComponent<EventTrigger>();
    

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
            effect.UseEffect(gameObject.GetComponent<DisplayCard>().card.EffectText,gameObject);
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
                        effect.UseEffect(cards[i].card.EffectText,cards[i].gameObject);
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
                        effect.UseEffect(card.card.EffectText,card.gameObject);
                    }
                }
            }
            //este es para usar el decoy//
            if(!endturn.useDecoy)
            {
                endturn.EndTurn();
            }
            if (eventTrigger != null)
            {
                Destroy(eventTrigger);
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
        if (cardZone.Type == "Decoy")
        {
            // Find the card in the drop zone
            DisplayCard targetCard = dropZone.GetComponentInChildren<DisplayCard>();
            if (targetCard != null && targetCard.Owner == cardZone.Owner && targetCard.Type != "Oro")
            {
                // Move the target card to the hand
                GameObject handZone = cardZone.Owner == "Player" ? GameObject.Find("PlayerHand") : GameObject.Find("EnemyHand");
                targetCard.transform.SetParent(handZone.transform, false);
                targetCard.transform.position = handZone.transform.position;

                // Reset the target card's stats
                targetCard.Points = targetCard.card.Attack;
                targetCard.AttackText.text = targetCard.Points.ToString();
                targetCard.Boost = false;

                // Allow the Decoy to be placed
                PlacedZone = zoneName;
                return true;
            }
        }
        foreach (var range in cardZone.card.Range)
        {
            if(range == zoneName && cardZone.Owner == zoneOwner && !conditions.isInspire && cardZone.card.Type != "Aumento")
            {
                PlacedZone = zoneName;
                return true;
            } 
            if(range == zoneName && cardZone.Owner == zoneOwner && cardZone.Type is "Aumento" && conditions.isInspire)
            {
                PlacedZone = zoneName;
                return true;
            }  
        }
        if(cardZone.Type is "Clima" or "Despeje"&& zoneName == "Wheather") return true;
        return false;
    }
    //esto era para que funcione el decoy//
    
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
                if(cardDisplay.Owner == "Player")
                {
                    deck.GetComponent<Deck>().Push(gameObject);
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
                if(cardDisplay.Owner == "Enemy")
                {
                    deck.GetComponent<Deck>().Push(gameObject);
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

