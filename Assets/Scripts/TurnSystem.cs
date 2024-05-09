using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Drawing;
using System.Diagnostics.Tracing;

public class TurnSystem : MonoBehaviour
{
    public bool isYourTurn;
    public TextMeshProUGUI turnText;
    public TextMeshProUGUI playerpoints;
    public TextMeshProUGUI enemyPoints;
    public GameObject hand1;
    public GameObject hand2;
    public PointCount playerpoint;
    public bool round;
    public bool useDecoy;
    public bool team;
    public Draw draw;
    public Change change;
    public Change change1;
    public bool pass;
    public int counter=0;
    public int playerWin;
    public int enemyWin;
    public WinnerScreen winnerScreen;
    
    private void Start() 
    {
        draw = GameObject.Find("GameManager").GetComponent<Draw>();
        for (int i = 0; i < 10; i++)
        {
            if(i<7)
            {
                draw.Draw1();
            }
            draw.Draw2();
        }
        EndTurn();
        change = GameObject.Find("Change").GetComponent<Change>();
        change1 = GameObject.Find("EnemyChange").GetComponent<Change>();
        change.change = true;
        change1.change = true;
        change1.Hide();
        NoMove(hand1,false);
        NoMove(hand2,false);
    }   
    public void OnClick() //This method is to manage the rounds
    {
       
        if(isYourTurn)
        {
            if(change.change)
            {
                NoMove(hand1,true);
                change.change = false;
                change.Hide();
            }
        }
        else if(!isYourTurn)
        {
            if(change1.change)
            {
                NoMove(hand2,true);
                change1.change = false;
                change1.Hide();
            }   
        }
         round =! round;
        isYourTurn =! isYourTurn;
        EndTurn();
        counter++;
        if(counter==2)
        {
            
            draw = GameObject.Find("GameManager").GetComponent<Draw>();
            for(int i=0;i<2;i++) 
            {
                draw.Draw1();
                draw.Draw2();
            }
            winnerScreen = GameObject.Find("GameManager").GetComponent<WinnerScreen>();
            int playerPoints = int.Parse(playerpoints.text);
            int enemypoints  = int.Parse(enemyPoints.text);
            if(playerPoints >= enemypoints) 
            {
                playerWin++;
                if(playerWin ==1)
                {
                    winnerScreen.SetUp(true);
                    //Image roundColor = GameObject.Find("RoundC1").GetComponent<Image>();
                    //roundColor.color = Color.green; //This turns the red circules to green;
                }
                else if(playerWin ==2)
                {
                    winnerScreen.Finish(true);
                    //Image roundColor = GameObject.Find("RoundC2").GetComponent<Image>();
                    //roundColor.color = Color.green;
                }
                else if(enemypoints > playerPoints)
                {
                    enemyWin++;
                    if(enemyWin ==1)
                    {
                        winnerScreen.SetUp(false);
                        //Image roundColor = GameObject.Find("RoundC3").GetComponent<Image>();
                        //roundColor.color = Color.green;
                    }
                else if(enemyWin ==2)
                {
                    winnerScreen.Finish(false);   
                    //Image roundColor = GameObject.Find("RoundC4").GetComponent<Image>();
                    //roundColor.color = Color.green;
                }
                //winner.RoundShow("P2");
                isYourTurn = true;
                EndTurn();
            }
            
            isYourTurn = false;
            EndTurn();
            }
            Clean();
            counter = 0;
        }
        
    }
    public void EndTurn() //This method changes turns every time is called
    {
        if(!round)
        {
            isYourTurn =! isYourTurn;
        }
        UpdateTurnUI();
    }
    private void UpdateTurnUI()
    {
        if (isYourTurn)
        {
            turnText.text = "Your Turn";
            HideEnemyCards(hand1,true);
            HideEnemyCards(hand2,false); 
        }
        else
        {
            turnText.text = "Opponent Turn";
            HideEnemyCards(hand2,true);
            HideEnemyCards(hand1,false);
            RotateCards(hand2);
        }
    }
    public void HideEnemyCards(GameObject hand,bool myHand)
    {
        if(myHand==true)
        {
            foreach(Transform card in hand.transform)
            {
                card.gameObject.SetActive(true);
            }
        }
        else
        {
            foreach(Transform card in hand.transform)
            {
                card.gameObject.SetActive(false);
            }
        }        
    }
    public void NoMove(GameObject hand,bool move)
    {
        DragAndDrop[] cards = hand.GetComponentsInChildren<DragAndDrop>();
        foreach(DragAndDrop card in cards)
        {
            card.enabled = move;
        }
    }
    void Clean() //This method eliminates the cards from the field
    {
        GameObject units = GameObject.Find("UnitsZones");
        foreach(Transform zone in units.transform)
        {
            foreach(Transform card in zone.transform)
            {
                Destroy(card.gameObject);
            }
        }
        GameObject units1 = GameObject.Find("EnemyUnitsZones");
        foreach(Transform zone in units1.transform)
        {
            foreach(Transform card in zone.transform)
            {
                Destroy(card.gameObject);
            }
        }
        GameObject support = GameObject.Find("SupportZone");
        foreach(Transform zone in support.transform)
        {
            foreach(Transform card in zone.transform)
            {
                Destroy(card.gameObject);
            }
        }
        GameObject support1 = GameObject.Find("EnemySupportZones");
        foreach(Transform zone in support1.transform)
        {
            foreach(Transform card in zone.transform)
            {
                Destroy(card.gameObject);
            }
        }
        GameObject weather = GameObject.Find("WeatherZone");
        foreach(Transform card in weather.transform)
        {
            displayCard cardDisplay = card.gameObject.GetComponent<displayCard>();
            Destroy(card.gameObject);
        }
    } 
    public void RotateCards(GameObject Hand) //This method rotates the player2 cards
    {
        UnityEngine.Quaternion pos = transform.rotation;
        foreach(Transform card in Hand.transform)
        {
            pos = card.transform.rotation;
            pos = UnityEngine.Quaternion.Euler(180f,180f,0);
            card.transform.rotation = pos;
        }
    }
}
