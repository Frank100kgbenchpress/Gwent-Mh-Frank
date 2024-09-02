using System.Collections.Generic;
using System.Linq;
using UnityEngine;
//los efectos//

public class effects : MonoBehaviour
{
    public GameObject zone1;
    public GameObject zone2;
    public GameObject zone3;
    public GameObject zone4;
    public GameObject zone5;
    public GameObject zone6;
    public deckManager deck;
    public bool wheatherUse;
    public bool effectLoop;
    public effects effect;
    public TurnSystem decoy;
    public Draw draw;

    public void UseEffect(int hability,GameObject card)
    {
        if(hability==1)
        {
            Supply(card);
        }
        if(hability==2)
        {
            Weather(card,"Melee"); // tengo que poner uno para cada uno 
        }
        if(hability==3)
        {
            Clean(card);
        }
        if(hability==4)
        {
            RowPowerUpp(card);
        }
        if(hability==5)
        {
            Multiply(card);
        }
        if(hability==6)
        {
            DestroyHightAttack();
        }
        if(hability==7)
        {
            Lowest(card);
        }
        if(hability==8)
        {
            CleanRow(card);
        }
        if(hability==9)
        {
            CallWeather(card);
        }
        if(hability==10)
        {
            Average(card);
        }
        if(hability==11)
        {
            decoy = GameObject.Find("GameManager").GetComponent<TurnSystem>();
            decoy.useDecoy = true;
            if(card.GetComponent<DisplayCard>().Owner =="Player")
            {
                decoy.Team = false;
            }
            decoy.Team = true;
        }
        if(hability==12)
        {
            draw = GameObject.Find("GameManager").GetComponent<Draw>();
            DisplayCard display = card.GetComponent<DisplayCard>();
            if(display.Owner == "Player")
            {
                draw.DrawCard(1);
            }
            else
            {
                draw.DrawCard(2);
            }
        }
    }
    void RowPowerUpp(GameObject cardPlayed)
    {
        string Owner = cardPlayed.GetComponent<DisplayCard>().Owner;
        string[] CardRange = cardPlayed.GetComponent<DisplayCard>().Range;
        if(Owner == "Player")
        {
            foreach (var range in CardRange)
            {
                if(range == "Melee") zone1 = GameObject.Find("MeleeZone");
                if(range == "Range") zone1 = GameObject.Find("DistanceZone");
                if(range == "Siege") zone1 = GameObject.Find("SiegeZone");
            }
        }
        if(Owner == "Enemy")
        {
            foreach (var range in CardRange)
            {
                if(range == "Melee") zone1 = GameObject.Find("EnemyMeleeZone");
                if(range == "Range") zone1 = GameObject.Find("EnemyDistanceZone");
                if(range == "Siege") zone1 = GameObject.Find("EnemySiegeZone");
            }
        }
        DisplayCard[] cards = zone1.GetComponentsInChildren<DisplayCard>();
        AddAttack(cards,5);
    }
    void Weather(GameObject cardPlayed, string zone)
    {
        string weather = cardPlayed.GetComponent<DisplayCard>().card.Type ="Clima";
        if(zone=="Distance")
        {
            zone1 = GameObject.Find("DistanceZone");
            zone2 = GameObject.Find("EnemyDistanceZone");
        }
        if(zone=="Melee")
        {
            zone1 = GameObject.Find("MeleeZone");
            zone2 = GameObject.Find("EnemyMeleeZone");
        }
        if(zone=="Siege")
        {
            zone1 = GameObject.Find("SiegeZone");
            zone2 = GameObject.Find("SiegeZone");
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
                if(range=="Distance")
                {
                    zone1 = GameObject.Find("DistanceZone");
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
                if(range=="Distance")
                {
                    zone1 = GameObject.Find("EnemyDistanceZone");
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
            foreach(var card in cards)
            {
                if(id==card.card.Id)    counter++;
            }
            foreach(var card in cards)
            {
                if(id==card.card.Id)
                {
                    card.Points *= counter;
                    card.AttackText.text = card.Points.ToString();
                }
            }
    }
    void Clean(GameObject cardPlayed)
    {
        zone1 = GameObject.Find("WeatherZone");
        bool[] check = new bool[3];
        foreach(Transform card in zone1.transform)
        {
            if(card.gameObject.GetComponent<DisplayCard>().card.name=="Tundra Zone")
            {
                check[0] = true;
                Destroy(card.gameObject);
            }
            if(card.gameObject.GetComponent<DisplayCard>().card.name=="Volcan Zone")
            {
                check[1] = true; 
                Destroy(card.gameObject);
            }
            if(card.gameObject.GetComponent<DisplayCard>().card.name=="Jungle Zone")
            {
                check[2] = true;
                Destroy(card.gameObject);
            }
        }
        if(check[0])
        {
            zone2 = GameObject.Find("AsediusZone");
            zone3 = GameObject.Find("EnemyAsediusZone");
            DisplayCard[] asedius = zone2.GetComponentsInChildren<DisplayCard>();
            foreach(var card in asedius)
            {
                card.Points = card.AttackOriginal;
                card.AttackText.text = card.Points.ToString();
                card.card.Boost = false;
            }
            DisplayCard[] enemyAsedius = zone2.GetComponentsInChildren<DisplayCard>();
            foreach(var card in enemyAsedius)
            {
                card.Points = card.AttackOriginal;
                card.AttackText.text = card.Points.ToString();
                card.card.Boost = false;
            }
        }
        if(check[1])
        {
            zone2 = GameObject.Find("MeleeZone");
            zone3 = GameObject.Find("EnemyMeleeZone");
            DisplayCard[] melee = zone2.GetComponentsInChildren<DisplayCard>();
            foreach(var card in melee)
            {
                card.Points = card.AttackOriginal;
                card.AttackText.text = card.Points.ToString();
                card.card.Boost = false;
            }
            DisplayCard[] enemyMelee = zone2.GetComponentsInChildren<DisplayCard>();
            foreach(var card in enemyMelee)
            {
                card.Points = card.AttackOriginal;
                card.AttackText.text = card.Points.ToString();
                card.card.Boost = false;
            }
        }
        if(check[2])
        {
            zone2 = GameObject.Find("DistanceZone");
            zone3 = GameObject.Find("EnemyDistanceZone");
            DisplayCard[] distance = zone2.GetComponentsInChildren<DisplayCard>();
            foreach(var card in distance)
            {
                card.Points = card.AttackOriginal;
                card.AttackText.text = card.Points.ToString();
                card.card.Boost = false;
            }
            DisplayCard[] enemyDistance = zone2.GetComponentsInChildren<DisplayCard>();
            foreach(var card in enemyDistance)
            {
                card.Points = card.AttackOriginal;
                card.AttackText.text = card.Points.ToString();
                card.card.Boost = false;
            }
        }
    Destroy(cardPlayed);
    wheatherUse = false;
    }
    void Supply(GameObject cardPlayed)
    {
        string[] Range = cardPlayed.GetComponent<DisplayCard>().Range;
        string owner = cardPlayed.GetComponent<DisplayCard>().Owner;
        if(owner=="Player")
        {
            if(Range[0]=="Melee")    zone1 = GameObject.Find("MeleeZone");
            if(Range[0]=="Distance") zone1 = GameObject.Find("DistanceZone");
            if(Range[0]=="Siege")    zone1 = GameObject.Find("SiegeZone");
        }
        if(owner=="Enemy")
        {
            if(Range[0]=="Melee")    zone1 = GameObject.Find("EnemyMeleeZone");
            if(Range[0]=="Distance") zone1 = GameObject.Find("EnemyDistanceZone");
            if(Range[0]=="Siege")    zone1 = GameObject.Find("EnemySiegeZone");
        }
        DisplayCard[] cards = zone1.GetComponentsInChildren<DisplayCard>();
        AddAttack(cards,5);
        effectLoop = true;
    }
    void AddAttack(DisplayCard[] cards,int ammount)
    {
        foreach (var card in cards.Where(c => c.Type == "Plata"))
        {
            card.Points += ammount;
            card.AttackText.text = card.Points.ToString();
        }
    }
    void DestroyHightAttack()
    {
        int max = int.MinValue;
        int maxenemy = int.MinValue;
        GameObject destroy = null;
        GameObject destroyE = null;
        zone1 = GameObject.Find("UnitZones");
        foreach (var zone in zone1.transform)
        {
            DisplayCard[] cards = zone1.GetComponentsInChildren<DisplayCard>();
            foreach (var card in cards)
            {
                if(card.card.golden)
                {
                    continue;
                }   
                if(card.points > max)
                {
                    max = card.points;
                    destroy = card.gameObject;
                }
            }  
        }
        zone2 = GameObject.Find("EnemyUnitsZones");
        foreach (var zone in zone2.transform)
        {
            DisplayCard[] cards = zone2.GetComponentsInChildren<DisplayCard>();
            foreach (var card in cards)
            {
                if(card.card.golden)
                {
                    continue;
                }   
                if(card.points > max)
                {
                    max = card.points;
                    destroyE = card.gameObject;
                }
            }  
        }
        if(max>maxenemy)
        {
            Destroy(destroy);
        }
        else
        {
            Destroy(destroyE);
        }
    }
    void Lowest(GameObject cardPlayed)
    {
        int min = int.MaxValue;
        GameObject destroy = null;
        if(cardPlayed.GetComponent<DisplayCard>().Team)
        {
            zone1 = GameObject.Find("UnitZones");
        }
        else
        {
            zone1 = GameObject.Find("EnemyUnitsZones");
        }
        foreach (var Transform in zone1.transform)
        {
            DisplayCard[] cards = zone1.GetComponentsInChildren<DisplayCard>();
            foreach (var card in cards)
            {
                if(card.card.golden)
                {
                    continue;
                }
                if(card.points<min)
                {
                    min = card.points;
                    destroy = card.gameObject;
                }   
            }
            if(destroy != null)
            {
                DisplayCard show = destroy.GetComponent<DisplayCard>();
                Destroy(destroy);
            }
        }
    }
    void CleanRow(GameObject cardPlayed)
    {
        int melee = 0;
        int enemyMelee = 0;
        int distance = 0;
        int enemyDistance = 0;
        int asedius = 0;
        int enemyAsedius = 0;
        zone1 = GameObject.Find("MeleeZone");
        zone2 = GameObject.Find("EnemyMeleeZone");
        zone3 = GameObject.Find("DistanceZone");
        zone4 = GameObject.Find("EnemyDistanceZone");
        zone5 = GameObject.Find("AsediusZone");
        zone6 = GameObject.Find("EnemyAsediusZone");
        foreach (var Transform in zone1.transform)
        {
            melee++;   
        }
         foreach (var Transform in zone2.transform)
        {
            enemyMelee++;   
        }
         foreach (var Transform in zone3.transform)
        {
            distance++;   
        }
         foreach (var Transform in zone4.transform)
        {
            enemyDistance++;   
        }
         foreach (var Transform in zone5.transform)
        {
            asedius++;   
        }
         foreach (var Transform in zone6.transform)
        {
            enemyAsedius++;   
        }
        if(melee==0) melee = int.MaxValue;
        if(enemyMelee==0) enemyMelee = int.MaxValue;
        if(distance==0) distance = int.MaxValue;
        if(enemyDistance==0) enemyDistance = int.MaxValue;
        if(asedius==0) asedius = int.MaxValue;
        if(enemyAsedius==0) enemyAsedius = int.MaxValue;
        int[] units = {melee,enemyMelee,distance,enemyDistance,asedius,enemyAsedius};
        int aux  = int.MaxValue;
        for (int i = 0; i < units.Length; i++)
        {
            if(units[i] < aux) aux = units[i];
        }
        if(melee == aux)
        {
            DisplayCard[] cards = zone1.GetComponentsInChildren<DisplayCard>();
            foreach (var card in cards)
            {
                if(card.card.golden) continue;  
                Destroy(card.gameObject);    
            }
        }
        else if(enemyMelee == aux)
        {
            DisplayCard[] cards = zone2.GetComponentsInChildren<DisplayCard>();
            foreach (var card in cards)
            {
                if(card.Type =="Oro") continue;  
                Destroy(card.gameObject);    
            }
        }
        else if(distance == aux)
        {
            DisplayCard[] cards = zone3.GetComponentsInChildren<DisplayCard>();
            foreach (var card in cards)
            {
                if(card.Type =="Oro") continue;  
                Destroy(card.gameObject);    
            }
        }
        else if(enemyDistance == aux)
        {
            DisplayCard[] cards = zone4.GetComponentsInChildren<DisplayCard>();
            foreach (var card in cards)
            {
                if(card.card.golden) continue;  
                Destroy(card.gameObject);    
            }
        }
        else if(asedius == aux)
        {
            DisplayCard[] cards = zone5.GetComponentsInChildren<DisplayCard>();
            foreach (var card in cards)
            {
                if(card.card.golden) continue;  
                Destroy(card.gameObject);    
            }
        }
        else if(enemyAsedius == aux)
        {
            DisplayCard[] cards = zone6.GetComponentsInChildren<DisplayCard>();
            foreach (var card in cards)
            {
                if(card.card.golden) continue;  
                Destroy(card.gameObject);    
            }
        }
    }
    void CallWeather(GameObject cardPlayed)
    {
        DisplayCard cardUse = cardPlayed.GetComponent<DisplayCard>();
        zone1 = GameObject.Find("WeatherZone");
        if(cardUse.Team==false)
        {
            deck = GameObject.Find("deckManager1").GetComponent<deckManager>();
            List<GameObject> cardsD = deck.GetCards();
            for (int i = 0; i < cardsD.Count; i++)
            {
                if(cardsD[i].GetComponent<DisplayCard>().name == "Tundra"|| cardsD[i].GetComponent<DisplayCard>().name == "Volcan"|| cardsD[i].GetComponent<DisplayCard>().name == "Jungle")
                {
                    GameObject currentCard = Instantiate(cardsD[i], new Vector3(0,0,0),Quaternion.identity);
                    currentCard.transform.SetParent(zone1.transform, false);
                    cardsD.RemoveAt(i);
                    effect = GameObject.Find("GameManager").GetComponent<effects>();
                    effect.UseEffect(currentCard.GetComponent<DisplayCard>().card.effect,currentCard);
                    break;
                }   
            }
        }
        else
        {
            deck = GameObject.Find("deckManager2").GetComponent<deckManager>();
            List<GameObject> cardsD = deck.GetCards();
            for (int i = 0; i < cardsD.Count; i++)
            {
                if(cardsD[i].GetComponent<DisplayCard>().name == "Tundra"||cardsD[i].GetComponent<DisplayCard>().name == "Volcan"||cardsD[i].GetComponent<DisplayCard>().name == "Jungle")
                {
                    GameObject currentCard = Instantiate(cardsD[i], new Vector3(0,0,0),Quaternion.identity);
                    currentCard.transform.SetParent(zone1.transform, false);
                    cardsD.RemoveAt(i);
                    effect = GameObject.Find("GameManager").GetComponent<effects>();
                    effect.UseEffect(currentCard.GetComponent<DisplayCard>().card.effect,currentCard);
                    break;
                }   
            }
        }
    }
    void Average(GameObject cardPlayed)
    {
        int sum = 0;
        int div = 0;
        zone1 = GameObject.Find("UnitZones");
        foreach (var Transform in zone1.transform)
        {
            DisplayCard[] cards = zone1.GetComponentsInChildren<DisplayCard>();
            foreach (var card in cards)
            {
                if(card.card.golden) continue;
                
                sum += card.points;
                div++;   
            }   
        }
        zone2 = GameObject.Find("EnemyUnitsZones");
        foreach (var Transform in zone2.transform)
        {
            DisplayCard[] cards = zone2.GetComponentsInChildren<DisplayCard>();
            foreach (var card in cards)
            {
                if(card.card.golden) continue;
                
                sum += card.points;
                div++;   
            }   
        }
        sum /= div;
        foreach (var Transform in zone1.transform)
        {
            DisplayCard [] cards = zone1.GetComponentsInChildren<DisplayCard>();
            foreach (var card in cards)
            {
                if(card.card.golden) continue;
                
                card.points = sum;
                card.attackText.text = card.points.ToString();   
            }    
        }
        foreach (var Transform in zone2.transform)
        {
            DisplayCard [] cards = zone2.GetComponentsInChildren<DisplayCard>();
            foreach (var card in cards)
            {
                if(card.card.golden) continue;
                
                card.points = sum;
                card.attackText.text = card.points.ToString();   
            }    
        }
    }
}
