using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WinnerScreen : MonoBehaviour
{
    public TextMeshProUGUI roundWinner;
    public TextMeshProUGUI winnerGame;
    public void SetUp(bool winner)
    {
        gameObject.SetActive(true);
        if(winner)
        {
            roundWinner.text = "Player 1 wins Round";
        }
        else
        {
            roundWinner.text = "Player 2 Wins Round";
        }
    }
    public void Finish(bool winner)
    {
        gameObject.SetActive(true);
        if(winner)
        {
            winnerGame.text = "Player 1 wins";
        }
        else
        {
            winnerGame.text = "Player 2 Wins";
        }
    } 
}
