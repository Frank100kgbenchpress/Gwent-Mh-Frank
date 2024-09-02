using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//para tener la cuenta de los puntos//

public class PointCount : MonoBehaviour
{
    public TextMeshProUGUI Playerpoints;
    public TextMeshProUGUI EnemyPoints;
    public GameObject MeleeZone;
    public GameObject DistanceZone;
    public GameObject SiegeZone;
    public GameObject MeleeZone1;
    public GameObject DistanceZone1;
    public GameObject SiegeZone1;
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
        DisplayCard[] cards = MeleeZone.GetComponentsInChildren<DisplayCard>();
        foreach (DisplayCard card in cards)
        {
            meleepoints += card.Points;
        }
        DisplayCard[] cards1 = DistanceZone.GetComponentsInChildren<DisplayCard>();
        foreach(var card in cards1)
        {
            distancePoints += card.Points;
        }
        DisplayCard[] cards2 = SiegeZone.GetComponentsInChildren<DisplayCard>();
        foreach(var card in cards2)
        {
            asediusPoints += card.Points;
        }
        int totalPoints = meleepoints + distancePoints + asediusPoints;
        Playerpoints.text = totalPoints.ToString();        
    }
    public void CollectEnemyPoints()
    {
        int meleepoints = 0;
        int distancePoints = 0;
        int asediusPoints = 0;
        DisplayCard[] cards = MeleeZone1.GetComponentsInChildren<DisplayCard>();
        foreach (DisplayCard card in cards)
        {
            meleepoints += card.Points;
        }
        DisplayCard[] cards1 = DistanceZone1.GetComponentsInChildren<DisplayCard>();
        foreach(var card in cards1)
        {
            distancePoints += card.Points;
        }
        DisplayCard[] cards2 = SiegeZone1.GetComponentsInChildren<DisplayCard>();
        foreach(var card in cards2)
        {
            asediusPoints += card.Points;
        }
        int totalPoints = meleepoints + distancePoints + asediusPoints;
        EnemyPoints.text = totalPoints.ToString();
    }
}
