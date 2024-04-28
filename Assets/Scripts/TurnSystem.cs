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
    public PointCount playerpoint ;
    public bool round;
    public bool useDecoy;
    public bool team;
    public Draw draw;
    public Change change;
    public Change change1;
    public bool pass;
    public int counter=0;
    public void OnClick() //This method is to manage the rounds
    {
        round =! round;
        isYourTurn =! isYourTurn;
        EndTurn();
        counter++;
        if(counter==2)
        {
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
            // Aquí puedes agregar más lógica para preparar el turno del jugador
        }
        else
        {
            turnText.text = "Opponent Turn";
            // Aquí puedes agregar más lógica para preparar el turno del enemigo
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
    public void NoMove(GameObject hand,bool move)
    {
        DragAndDrop[] cards = hand.GetComponentsInChildren<DragAndDrop>();
        foreach(DragAndDrop card in cards)
        {
            card.enabled = move;
        }
    }    
}
