using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DSL;
//los efectos//

public class effects : MonoBehaviour
{
    public GameObject zone1;
    public GameObject zone2;
    public GameObject zone3;
    public GameObject zone4;
    public GameObject zone5;
    public GameObject zone6;
    public Deck deck;
    public bool wheatherUse;
    public bool effectLoop;
    public effects effect;
    public TurnSystem decoy;
    public Draw draw;

    public void UseEffect(string effectName,GameObject card)
    {
        if(effectName == "Fila") RowPowerUpp(card,false);
        if(effectName == "Aumento") RowPowerUpp(card,true);
        if(effectName == "ClimaMelee") Weather("Melee");
        if(effectName == "ClimaRanged") Weather("Ranged");
        if(effectName == "CllimaSiege") Weather("Siege");
        if(effectName == "Multiply") Multiply(card);
        if(effectName == "a")
        {
            Evaluator evaluator = new Evaluator(card.GetComponent<DisplayCard>().card,card.GetComponent<DisplayCard>().card.Context);
            evaluator.EvaluateEffects();
        }
        if(effectName == "Draw")
        {
            draw = GameObject.Find("GameManager").GetComponent<Draw>();
            DisplayCard display = card.GetComponent<DisplayCard>();
            draw.DrawCard(display.Owner == "Player" ? 1 : 2);
        }
        if(effectName == "DestroyH") DestroyHightAttack();
        if(effectName == "Despeje") CleanWeather(card);
        if(effectName == "Lowest") Lowest(card);
        if(effectName == "CleanRow") CleanRow();
        if(effectName == "Call") CallWeather(card);
        if(effectName == "Average") Average();
    }
    //efecto de subir puntos el booleano es por si es una carta aumento ya que el efecto debe ser fijo por toda la ronda y en una carta normal es solo para las que estaban
    void RowPowerUpp(GameObject cardPlayed, bool loop)
    {
        string Owner = cardPlayed.GetComponent<DisplayCard>().Owner;
        string placedZone = cardPlayed.GetComponent<DragAndDrop>().PlacedZone;
        zone1 = GameObject.Find((Owner == "Player") ? placedZone + "Zone" : "Enemy" + placedZone + "Zone");
        DisplayCard[] cards = zone1.GetComponentsInChildren<DisplayCard>();
        AddAttack(cards,5);
        if(loop)effectLoop = true;
    }
    void AddAttack(DisplayCard[] cards,int ammount)
    {
        foreach (var card in cards.Where(c => c.Type == "Plata"))
        {
            card.Points += ammount;
            card.AttackText.text = card.Points.ToString();
        }
    }
    void Weather(string zone)
    {
        if(zone=="Ranged")
        {
            zone1 = GameObject.Find("RangedZone");
            zone2 = GameObject.Find("EnemyRangedZone");
        }
        if(zone=="Melee")
        {
            zone1 = GameObject.Find("MeleeZone");
            zone2 = GameObject.Find("EnemyMeleeZone");
        }
        if(zone=="Siege")
        {
            zone1 = GameObject.Find("SiegeZone");
            zone2 = GameObject.Find("EnemySiegeZone");
        }
        DisplayCard[] cards = zone1.GetComponentsInChildren<DisplayCard>();
        AttackToOneToWheatherEffect(cards);
        DisplayCard[] cards2 = zone2.GetComponentsInChildren<DisplayCard>();
        AttackToOneToWheatherEffect(cards2);
        wheatherUse = true;
    }
    void AttackToOneToWheatherEffect(DisplayCard[] cards)
    {
        foreach (var card in cards.Where(c => c.Type == "Plata"))
        {
            card.Points = 1;
            card.AttackText.text = card.Points.ToString();
        }
    }
    // en este no use la logica del string con la posicion porque una carta puede tener ese efecto y tener varias posiciones para colocarse
    void Multiply(GameObject cardPlayed)
    {
        string[] Range = cardPlayed.GetComponent<DisplayCard>().Range;
        int id = cardPlayed.GetComponent<DisplayCard>().card.Id;
        string owner = cardPlayed.GetComponent<DisplayCard>().Owner;
        int counter = 0;
        foreach (var range in Range)
        {
            if(owner =="Player")
            {
                if(range == "Melee")
                {
                    zone1 = GameObject.Find("MeleeZone");
                    CheckCardIDForMultiplyEffect(id,counter);
                }
                if(range=="Ranged")
                {
                    zone1 = GameObject.Find("RangedZone");
                    CheckCardIDForMultiplyEffect(id,counter);
                }
                if(range=="Siege")
                {
                    zone1 = GameObject.Find("SiegeZone");
                    CheckCardIDForMultiplyEffect(id,counter);
                }
            }
            if(owner == "Enemy")
            {
                if(range == "Melee")
                {
                    zone1 = GameObject.Find("EnemyMeleeZone");
                    CheckCardIDForMultiplyEffect(id,counter);
                }
                if(range=="Ranged")
                {
                    zone1 = GameObject.Find("EnemyRangedZone");
                    CheckCardIDForMultiplyEffect(id,counter);
                }
                if(range=="Siege")
                {
                    zone1 = GameObject.Find("EnemySiegeZone");
                    CheckCardIDForMultiplyEffect(id,counter);
                }
            }   
        }
        
    }
        void CheckCardIDForMultiplyEffect(int id,int counter)
        {
            DisplayCard[] cards = zone1.GetComponentsInChildren<DisplayCard>();
            counter = cards.Count(card => card.card.Id == id);
            foreach(var card in cards.Where(c => c.card.Id == id))
            {
                card.Points *= counter;
                card.AttackText.text = card.Points.ToString();
            }
        }
    void CleanWeather(GameObject cardPlayed)
    {
        zone1 = GameObject.Find("WeatherZone");
        foreach(Transform card in zone1.transform)
        {
            DisplayCard displayCard = card.gameObject.GetComponent<DisplayCard>();
            switch(displayCard.card.EffectText)
            {
                case "ClimaSiege":
                    RevertWeatherEffect("Siege");
                    break;
                case "ClimaMelee":
                    RevertWeatherEffect("Melee");
                    break;
                case "ClimaRanged":
                    RevertWeatherEffect("Ranged");
                    break;
            }
            Destroy(card.gameObject);
        }
        Destroy(cardPlayed);
        wheatherUse = false;
    }

    void RevertWeatherEffect(string zoneType)
    {
        zone2 = GameObject.Find(zoneType + "Zone");
        zone3 = GameObject.Find("Enemy" + zoneType + "Zone");
        RevertCardsInZone(zone2);
        RevertCardsInZone(zone3);
    }

    void RevertCardsInZone(GameObject zone)
    {
        DisplayCard[] cards = zone.GetComponentsInChildren<DisplayCard>();
        foreach(var card in cards)
        {
            card.Points = card.AttackOriginal;
            card.AttackText.text = card.Points.ToString();
            card.card.Boost = false;
        }
    }
    void DestroyHightAttack()
    {
        int maxPlayer = FindHighestAttack(GameObject.Find("UnitZones"), out GameObject destroyPlayer);
        int maxEnemy = FindHighestAttack(GameObject.Find("EnemyUnitsZones"), out GameObject destroyEnemy);
        Destroy(maxPlayer > maxEnemy ? destroyPlayer : destroyEnemy);
    }

        int FindHighestAttack(GameObject zoneGroup, out GameObject highestCard)
        {
            int max = int.MinValue;
            highestCard = null;

            foreach (Transform zone in zoneGroup.transform)
            {
                DisplayCard[] cards = zone.GetComponentsInChildren<DisplayCard>();
                foreach (var card in cards.Where(c => c.Type == "Plata"))
                {
                    if (card.Points > max)
                    {
                        max = card.Points;
                        highestCard = card.gameObject;
                    }
                }
            }

            return max;
        }
    void Lowest(GameObject cardPlayed)
    {
        int min = int.MaxValue;
        GameObject destroy = null;
        zone1 = cardPlayed.GetComponent<DisplayCard>().Owner == "Enemy" ? GameObject.Find("UnitZones") : GameObject.Find("EnemyUnitsZones");
        foreach (var Transform in zone1.transform)
        {
            DisplayCard[] cards = zone1.GetComponentsInChildren<DisplayCard>();
            foreach (var card in cards.Where(c => c.Type == "Plata"))
            {
                if(card.Points<min)
                {
                    min = card.Points;
                    destroy = card.gameObject;
                }
            }
            if(destroy != null)    Destroy(destroy);
        }
    }
    void CleanRow()
    {
        zone1 = GameObject.Find("MeleeZone");
        zone2 = GameObject.Find("EnemyMeleeZone");
        zone3 = GameObject.Find("RangedZone");
        zone4 = GameObject.Find("EnemyRangedZone");
        zone5 = GameObject.Find("SiegeZone");
        zone6 = GameObject.Find("EnemyRangedZone");

        int melee = RowCounter(zone1);
        int enemyMelee = RowCounter(zone2);
        int ranged = RowCounter(zone3);
        int enemyRanged = RowCounter(zone4);
        int siege = RowCounter(zone5);
        int enemySiege = RowCounter(zone6);
        
        if(melee==0) melee = int.MaxValue;
        if(enemyMelee==0) enemyMelee = int.MaxValue;
        if(ranged==0) ranged = int.MaxValue;
        if(enemyRanged==0) enemyRanged = int.MaxValue;
        if(siege==0) siege = int.MaxValue;
        if(enemySiege==0) enemySiege = int.MaxValue;

        int[] units = {melee,enemyMelee,ranged,enemyRanged,siege,enemySiege};
        int aux  = int.MaxValue;
        for (int i = 0; i < units.Length; i++)
        {
            if(units[i] < aux) aux = units[i];
        }
        if(melee == aux)    Clean(zone1);
        else if(enemyMelee == aux)    Clean(zone2);
        else if(ranged == aux)    Clean(zone3);
        else if(enemyRanged == aux)    Clean(zone4);
        else if(siege == aux)    Clean(zone5);
        else if(enemySiege == aux)    Clean(zone6);
    }
    int RowCounter(GameObject zone)
    {
        int counter = 0;
        foreach (var Transform in zone.transform)    counter++;   
        return counter;
    }
    void Clean(GameObject zone)
    {
        DisplayCard[] cards = zone.GetComponentsInChildren<DisplayCard>();
        foreach (var card in cards.Where(c => c.Type == "Plata"))
        {
            Destroy(card.gameObject);
        }
    }
    void CallWeather(GameObject cardPlayed)
    {
        DisplayCard cardUse = cardPlayed.GetComponent<DisplayCard>();
        zone1 = GameObject.Find("WeatherZone");
        
        string deckName = cardUse.Owner == "Player" ? "deckManager1" : "deckManager2";
        Debug.Log(deckName);
        CallWeatherFromDeck(deckName);
    }
    // accedo a la carta del display card porque como las asignaciones las hago en start llegan null al efecto
    void CallWeatherFromDeck(string deckName)
    {
        deck = GameObject.Find(deckName).GetComponent<Deck>();
        List<GameObject> cardsD = deck.GetCards();
        for (int i = 0; i < cardsD.Count; i++)
        {
            DisplayCard cardDisplay = cardsD[i].GetComponent<DisplayCard>();
            if(cardDisplay.card.Type == "Clima")
            {
                GameObject currentCard = Instantiate(cardsD[i], new Vector3(0,0,0), Quaternion.identity);
                currentCard.transform.SetParent(zone1.transform, false);
                cardsD.RemoveAt(i);
                effect = GameObject.Find("GameManager").GetComponent<effects>();
                effect.UseEffect(cardDisplay.card.EffectText, currentCard);
                break;
            }   
        }
    }
    void Average()
    {
        
        int totalSum = CalculateZoneSum(GameObject.Find("UnitZones") ,out int playerCount);
        totalSum += CalculateZoneSum(GameObject.Find("EnemyUnitsZones"), out int enemyCount);
        int totalCount = playerCount + enemyCount;

        int average = totalSum / totalCount;

        ApplyAverageToZone(GameObject.Find("UnitZones"), average);
        ApplyAverageToZone(GameObject.Find("EnemyUnitsZones"), average);
    }
    int CalculateZoneSum(GameObject zoneGroup, out int cardCount)
    {
        int sum = 0;
        cardCount = 0;
        foreach (Transform zone in zoneGroup.transform)
        {
            DisplayCard[] cards = zone.GetComponentsInChildren<DisplayCard>();
            foreach (var card in cards.Where(c => c.Type == "Plata"))
            {
                sum += card.Points;
                cardCount++;
            }
        }
        return sum;
    }
    void ApplyAverageToZone(GameObject zoneGroup, int average)
    {
        foreach (Transform zone in zoneGroup.transform)
        {
            DisplayCard[] cards = zone.GetComponentsInChildren<DisplayCard>();
            foreach (var card in cards.Where(c => c.Type == "Plata"))
            {
                card.Points = average;
                card.AttackText.text = card.Points.ToString();
            }
        }
    }
}
