using UnityEngine;
using TMPro;

// Class to manage point counting for players and enemies
public class PointCount : MonoBehaviour
{
    public TextMeshProUGUI PlayerPoints;
    public TextMeshProUGUI EnemyPoints;
    
    public GameObject MeleeZone;
    public GameObject DistanceZone;
    public GameObject SiegeZone;
    
    public GameObject MeleeZone1;
    public GameObject DistanceZone1;
    public GameObject SiegeZone1;

    void Update()
    {
        CollectPoints(MeleeZone, DistanceZone, SiegeZone, false);
        CollectPoints(MeleeZone1, DistanceZone1, SiegeZone1, true);
    }

    public void CollectPoints(GameObject meleeZone, GameObject distanceZone, GameObject siegeZone, bool isPlayer)
    {
        int meleePoints = CollectPointsFromZone(meleeZone);
        int distancePoints = CollectPointsFromZone(distanceZone);
        int siegePoints = CollectPointsFromZone(siegeZone);
        
        int totalPoints = meleePoints + distancePoints + siegePoints;
        UpdateScoreDisplay(totalPoints, isPlayer);
    }

    int CollectPointsFromZone(GameObject zone)
    {
        DisplayCard[] cards = zone.GetComponentsInChildren<DisplayCard>();
        int totalPoints = 0;
        foreach (var card in cards)    totalPoints += card.Points;
        return totalPoints;
    }

    void UpdateScoreDisplay(int totalPoints, bool isPlayer)=> (isPlayer ? PlayerPoints : EnemyPoints).text = totalPoints.ToString();
}
