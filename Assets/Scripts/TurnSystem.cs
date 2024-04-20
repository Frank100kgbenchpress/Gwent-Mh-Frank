using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Drawing;

public class TurnSystem : MonoBehaviour
{
    public bool isYourTurn;
    public TextMeshProUGUI turnText;
    public GameObject hand1;
    public GameObject hand2;
    public PointCount playerpoints ;
    public bool round;
    int counter;
    public void OnClick()
    {
        counter++;
        if(counter==2)
        {
            counter=0;
            int pPoints = int.Parse(playerpoints.playerpoints.text);
            int ePoints = int.Parse(playerpoints.enemyPoints.text);;
            if(pPoints<ePoints)
            {

            }
            else
            {
                
            }
        }
        round =! round;
        isYourTurn =! isYourTurn;
        EndYourTurn();
        
    }
    void Start()
    {
        EndYourTurn();
    }
    public void EndYourTurn()
    {
        if(!round)
        {
            isYourTurn =! isYourTurn;
        
        }
        if(isYourTurn)
        {
            turnText.text = "Your Turn";
            HideEnemyCards(hand1,true);
            HideEnemyCards(hand2,false);
        }
        else
        {
            turnText.text = "Opponent Turn"; 
            HideEnemyCards(hand1,false);
            HideEnemyCards(hand2,true);
        }     
        
    }
    public void HideEnemyCards(GameObject hand,bool myHand)
    {
        if(myHand==true)
        {
            foreach(Transform card in hand.transform)
            {
                Vector3 hidecard = card.transform.position;
                hidecard.z = 0;
                card.transform.position = hidecard;
            }
        }
        else
        {
            foreach(Transform card in hand.transform)
            {
                Vector3 hidecard = card.transform.position;
                hidecard.z = -10;
                card.transform.position = hidecard;
            }
        }        
    }    
}
