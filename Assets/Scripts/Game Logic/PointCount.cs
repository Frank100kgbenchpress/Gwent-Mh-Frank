using UnityEngine;
using TMPro;

// Class to manage point counting for players and enemies
public class PointCount : MonoBehaviour
{
    public TextMeshProUGUI PlayerPoints;
    public TextMeshProUGUI EnemyPoints;
    //Player Zones
    public GameObject MeleeZone;
    public GameObject RangedZone;
    public GameObject SiegeZone;
    //Enemy Zones
    public GameObject MeleeZone1;
    public GameObject RangedZone1;
    public GameObject SiegeZone1;

    void Update()
    {
        CollectPoints(MeleeZone, RangedZone, SiegeZone, true);
        CollectPoints(MeleeZone1, RangedZone1, SiegeZone1, false);
    }

    public void CollectPoints(GameObject meleeZone, GameObject rangedZone, GameObject siegeZone, bool isPlayer)
    {
        int meleePoints = CollectPointsFromZone(meleeZone);
        int distancePoints = CollectPointsFromZone(rangedZone);
        int siegePoints = CollectPointsFromZone(siegeZone);
        
        int totalPoints = meleePoints + distancePoints + siegePoints;
        (isPlayer ? PlayerPoints : EnemyPoints).text = totalPoints.ToString();
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
