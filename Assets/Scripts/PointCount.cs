using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PointCount : MonoBehaviour
{
    public TextMeshProUGUI playerpoints;
    public TextMeshProUGUI enemyPoints;
    public GameObject meleeZone;
    public GameObject distanceZone;
    public GameObject asediusZone;
    public GameObject meleeZone1;
    public GameObject distanceZone1;
    public GameObject asediusZone1;
    // Update is called once per frame
    void Update()
    {
        CollectPoints();
        CollectEnemyPoints();
    }
    public void CollectPoints()
    {
        int meleepoints = 0;
        int distancePoints = 0;
        int asediusPoints = 0;
        displayCard[] cards = meleeZone.GetComponentsInChildren<displayCard>();
        foreach (displayCard card in cards)
        {
            meleepoints += card.points;
        }
        displayCard[] cards1 = distanceZone.GetComponentsInChildren<displayCard>();
        foreach(var card in cards1)
        {
            distancePoints += card.points;
        }
        displayCard[] cards2 = asediusZone.GetComponentsInChildren<displayCard>();
        foreach(var card in cards2)
        {
            asediusPoints += card.points;
        }
        int totalPoints = meleepoints + distancePoints + asediusPoints;
        playerpoints.text = totalPoints.ToString();        
    }
    public void CollectEnemyPoints()
    {
        int meleepoints = 0;
        int distancePoints = 0;
        int asediusPoints = 0;
        displayCard[] cards = meleeZone1.GetComponentsInChildren<displayCard>();
        foreach (displayCard card in cards)
        {
            meleepoints += card.points;
        }
        displayCard[] cards1 = distanceZone1.GetComponentsInChildren<displayCard>();
        foreach(var card in cards1)
        {
            distancePoints += card.points;
        }
        displayCard[] cards2 = asediusZone1.GetComponentsInChildren<displayCard>();
        foreach(var card in cards2)
        {
            asediusPoints += card.points;
        }
        int totalPoints = meleepoints + distancePoints + asediusPoints;
        enemyPoints.text = totalPoints.ToString();
    }
}
