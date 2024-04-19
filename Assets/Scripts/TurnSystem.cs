using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TurnSystem : MonoBehaviour
{
    public bool isYourTurn;
    public TextMeshProUGUI turnText;
    public GameObject hand1;
    public GameObject hand2;
    void Start()
    {
        EndYourTurn();
    }
    public void EndYourTurn()
    {
        isYourTurn =! isYourTurn;
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
