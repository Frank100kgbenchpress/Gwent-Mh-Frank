using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour 
{
    private Vector2 startPosition;
    private bool isDraggin = false;
    private bool isOverDropZone = false;
    private GameObject dropZone;
    public TurnSystem endturn;
    public TurnSystem turns;
    public effects effect;
    public deckManager deck;
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
            effect.UseEffect(gameObject.GetComponent<displayCard>().card.effect,gameObject);
             
            if(effect.effectLoop)
            {
                displayCard[] cards = new displayCard[6];
                cards[0] = GameObject.Find("SupportMeleeZone").GetComponentInChildren<displayCard>();
                cards[1] = GameObject.Find("SupportDistanceZone").GetComponentInChildren<displayCard>();
                cards[2] = GameObject.Find("SupportAsediusZone").GetComponentInChildren<displayCard>();
                cards[3] = GameObject.Find("EnemySupportAsediusZone").GetComponentInChildren<displayCard>();
                cards[4] = GameObject.Find("EnemySupportMeleeZone").GetComponentInChildren<displayCard>();
                cards[5] = GameObject.Find("EnemySupportDistanceZone").GetComponentInChildren<displayCard>();
                for(int i=0;i<6;i++)
                {
                    if(cards[i]!=null)
                    {
                        effect.UseEffect(cards[i].card.effect,cards[i].gameObject);
                    }
                }
            }
            if(effect.wheatherUse)
            {
                displayCard[] cards = GameObject.Find("WeatherZone").GetComponentsInChildren<displayCard>();
                if(cards != null)
                {
                    foreach(var card in cards)
                    {
                        effect.UseEffect(card.card.effect,card.gameObject);
                    }
                }
            }
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
    public bool CorrectZone()
    {
        displayCard zone = gameObject.GetComponent<displayCard>();
        IDZone id = dropZone.GetComponent<IDZone>();
        if(zone.card.zone == id.idZone) return true;
        else return false;
    }
    public void OnPointerClick()
    {
        displayCard cardDisplay = GetComponent<displayCard>();
        TurnSystem decoy = GameObject.Find("GameManager").GetComponent<TurnSystem>();
        if(decoy.useDecoy && decoy.team==false)
        {
            if(cardDisplay.team==false && !cardDisplay.card.golden)
            {
                GameObject zone1 = GameObject.Find("PlayerHand");
                cardDisplay.points = cardDisplay.card.attack;
                cardDisplay.attackText.text = cardDisplay.points.ToString();
                cardDisplay.boost = false;
                transform.position = zone1.transform.position;
                transform.SetParent(zone1.transform,false);
                decoy.useDecoy = false;
                decoy.EndTurn();
            }
        }
        else if(decoy.useDecoy && decoy.team)
        {
            if(cardDisplay.team && !cardDisplay.card.golden)
            {
                GameObject zone1 = GameObject.Find("EnemyHand");
                cardDisplay.points = cardDisplay.card.attack;
                cardDisplay.attackText.text = cardDisplay.points.ToString();
                cardDisplay.boost = false;
                transform.position = zone1.transform.position;
                transform.SetParent(zone1.transform,false);
                decoy.useDecoy = false;
                decoy.EndTurn();
            }
        }
    }
    public void ChangingCards()
    {
        draw = GameObject.Find("GameManager").GetComponent<Draw>();
        endturn = GameObject.Find("GameManager").GetComponent<TurnSystem>();
        displayCard cardDisplay = GetComponent<displayCard>();
        if(endturn.isYourTurn)
        {
            change = GameObject.Find("Change").GetComponent<Change>();
            GameObject hand = GameObject.Find("PlayerHand");
            if(change.change)
            {
                deck = GameObject.Find("deckManager1").GetComponent<deckManager>();
                List<GameObject> deckCards = deck.GetCards();
                if(cardDisplay.team == false)
                {
                    deckCards.Add(gameObject);
                    draw.Draw1();
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
                deck = GameObject.Find("deckManager2").GetComponent<deckManager>();
                List<GameObject> deckCards = deck.GetCards();
                if(cardDisplay.team)
                {
                    deckCards.Add(gameObject);
                    draw.Draw2();
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
