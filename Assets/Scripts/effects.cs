using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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
            Weather(card);
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
            if(card.GetComponent<displayCard>().team==false)
            {
                decoy.team = false;
            }
            decoy.team = true;
        }
        if(hability==12)
        {
            draw = GameObject.Find("GameManager").GetComponent<Draw>();
            displayCard display = card.GetComponent<displayCard>();
            if(display.team==false)
            {
                draw.Draw1();
            }
            else
            {
                draw.Draw2();
            }
        }
    }
    void RowPowerUpp(GameObject cardPlayed)
    {
        int cardPosition = cardPlayed.GetComponent<displayCard>().position;
        if(cardPosition==1)
        {
            zone1 = GameObject.Find("MeleeZone");
        }
        else if(cardPosition==2)
        {
            zone1 = GameObject.Find("DistanceZone");
        }
        else if(cardPosition==3)
        {
            zone1 = GameObject.Find("AsediusZone");
        }
        else if(cardPosition==8)
        {
            zone1 = GameObject.Find("EnemyMeleeZone");
        }
        else if(cardPosition==9)
        {
            zone1 = GameObject.Find("EnemyDistanceZone");
        }
        else if(cardPosition==10)
        {
            zone1 = GameObject.Find("EnemyDistanceZone");
        }
        displayCard[] cards = zone1.GetComponentsInChildren<displayCard>();
        foreach(var card in cards)
        {
            if(card.card.golden)
            {
                continue;
            }
            card.points += 5;
            card.attackText.text = card.points.ToString();
        }
    }
    void Weather(GameObject cardPlayed)
    {
        string weather = cardPlayed.GetComponent<displayCard>().card.name;
        if(weather=="Tundra Zone")
        {
            zone1 = GameObject.Find("DistanceZone");
            zone2 = GameObject.Find("EnemyDistanceZone");
        }
        if(weather=="Volcan Zone")
        {
            zone1 = GameObject.Find("MeleeZone");
            zone2 = GameObject.Find("EnemyMeleeZone");
        }
        if(weather=="Jungle Zone")
        {
            zone1 = GameObject.Find("AsediusZone");
            zone2 = GameObject.Find("EnemyAsediusZone");
        }
        displayCard[] cards = zone1.GetComponentsInChildren<displayCard>();
        foreach(var card in cards)
        {
            if(card.card.golden)
            {
                continue;
            }
            card.points = 1;
            card.attackText.text = card.points.ToString();
        }
        displayCard[] cards2 = zone2.GetComponentsInChildren<displayCard>();
        foreach(var card in cards2)
        {
            if(card.card.golden)
            {
                continue;
            }
            card.points = 1;
            card.attackText.text = card.points.ToString();
        }
        wheatherUse = true;
    }
    void Multiply(GameObject cardPlayed)
    {
        int cardPosition = cardPlayed.GetComponent<displayCard>().position;
        int id = cardPlayed.GetComponent<displayCard>().card.id;
        int counter = 0;
        
        
        if(cardPosition==1)
        {
            zone1 = GameObject.Find("MeleeZone");
            displayCard[] cards = zone1.GetComponentsInChildren<displayCard>();
            foreach(var card in cards)
            {
                if(id==card.card.id)
                {
                    counter++;
                }
            }
            foreach(var card in cards)
            {
                if(id==card.card.id)
                {
                    card.points *= counter;
                    card.attackText.text = card.points.ToString();
                }
            }
        }
        if(cardPosition==2)
        {
            zone1 = GameObject.Find("DistanceZone");
            displayCard[] cards = zone1.GetComponentsInChildren<displayCard>();
            foreach(var card in cards)
            {
                if(id==card.card.id)
                {
                    counter++;
                }

            }
            foreach(var card in cards)
            {
                if(id==card.card.id)
                {
                    card.points *= counter;
                    card.attackText.text = card.points.ToString();
                }
            }
        }
        if(cardPosition==3)
        {
            zone1 = GameObject.Find("AsediusZone");
            displayCard[] cards = zone1.GetComponentsInChildren<displayCard>();
            foreach(var card in cards)
            {
                if(id==card.card.id)
                {
                    counter++;
                }

            }
            foreach(var card in cards)
            {
                if(id==card.card.id)
                {
                    card.points *= counter;
                    card.attackText.text = card.points.ToString();
                }
            }
        }
        if(cardPosition==8)
        {
            zone1 = GameObject.Find("EnemyMeleeZone");
            displayCard[] cards = zone1.GetComponentsInChildren<displayCard>();
            foreach(var card in cards)
            {
                if(id==card.card.id)
                {
                    counter++;
                }

            }
            foreach(var card in cards)
            {
                if(id==card.card.id)
                {
                    card.points *= counter;
                    card.attackText.text = card.points.ToString();
                }
            }
        }
        if(cardPosition==9)
        {
            zone1 = GameObject.Find("EnemyDistanceZone");
            displayCard[] cards = zone1.GetComponentsInChildren<displayCard>();
            foreach(var card in cards)
            {
                if(id==card.card.id)
                {
                    counter++;
                }

            }
            foreach(var card in cards)
            {
                if(id==card.card.id)
                {
                    card.points *= counter;
                    card.attackText.text = card.points.ToString();
                }
            }
        }
        if(cardPosition==10)
        {
            zone1 = GameObject.Find("EnemyDistanceZone");
            displayCard[] cards = zone1.GetComponentsInChildren<displayCard>();
            foreach(var card in cards)
            {
                if(id==card.card.id)
                {
                    counter++;
                }

            }
            foreach(var card in cards)
            {
                if(id==card.card.id)
                {
                    card.points *= counter;
                    card.attackText.text = card.points.ToString();
                }
            }
        }
    }
    void Clean(GameObject cardPlayed)
    {
        zone1 = GameObject.Find("WeatherZone");
        bool[] check = new bool[3];
        foreach(Transform card in zone1.transform)
        {
            if(card.gameObject.GetComponent<displayCard>().card.name=="Tundra Zone")
            {
                check[0] = true;
                Destroy(card.gameObject);
            }
            if(card.gameObject.GetComponent<displayCard>().card.name=="Volcan Zone")
            {
                check[1] = true; 
                Destroy(card.gameObject);
            }
            if(card.gameObject.GetComponent<displayCard>().card.name=="Jungle Zone")
            {
                check[2] = true;
                Destroy(card.gameObject);
            }
        }
        if(check[0])
        {
            zone2 = GameObject.Find("AsediusZone");
            zone3 = GameObject.Find("EnemyAsediusZone");
            displayCard[] asedius = zone2.GetComponentsInChildren<displayCard>();
            foreach(var card in asedius)
            {
                card.points = card.attackOriginal;
                card.attackText.text = card.points.ToString();
                card.card.buff = false;
            }
            displayCard[] enemyAsedius = zone2.GetComponentsInChildren<displayCard>();
            foreach(var card in enemyAsedius)
            {
                card.points = card.attackOriginal;
                card.attackText.text = card.points.ToString();
                card.card.buff = false;
            }
        }
        if(check[1])
        {
            zone2 = GameObject.Find("MeleeZone");
            zone3 = GameObject.Find("EnemyMeleeZone");
            displayCard[] melee = zone2.GetComponentsInChildren<displayCard>();
            foreach(var card in melee)
            {
                card.points = card.attackOriginal;
                card.attackText.text = card.points.ToString();
                card.card.buff = false;
            }
            displayCard[] enemyMelee = zone2.GetComponentsInChildren<displayCard>();
            foreach(var card in enemyMelee)
            {
                card.points = card.attackOriginal;
                card.attackText.text = card.points.ToString();
                card.card.buff = false;
            }
        }
        if(check[2])
        {
            zone2 = GameObject.Find("DistanceZone");
            zone3 = GameObject.Find("EnemyDistanceZone");
            displayCard[] distance = zone2.GetComponentsInChildren<displayCard>();
            foreach(var card in distance)
            {
                card.points = card.attackOriginal;
                card.attackText.text = card.points.ToString();
                card.card.buff = false;
            }
            displayCard[] enemyDistance = zone2.GetComponentsInChildren<displayCard>();
            foreach(var card in enemyDistance)
            {
                card.points = card.attackOriginal;
                card.attackText.text = card.points.ToString();
                card.card.buff = false;
            }
        }
    Destroy(cardPlayed);
    wheatherUse = false;
    }
    void Supply(GameObject cardPlayed)
    {
        int cardPosition = cardPlayed.GetComponent<displayCard>().position;
        if(cardPosition==4)
        {
            zone1 = GameObject.Find("MeleeZone");
        }
        if(cardPosition==5)
        {
            zone1 = GameObject.Find("DistanceZone");
        }
        if(cardPosition==6)
        {
            zone1 = GameObject.Find("AsediusZone");
        }
        if(cardPosition==11)
        {
            zone1 = GameObject.Find("EnemyMeleeZone");
        }
        if(cardPosition==12)
        {
            zone1 = GameObject.Find("EnemyDistanceZone");
        }
        if(cardPosition==13)
        {
            zone1 = GameObject.Find("EnemyDistanceZone");
        }
        displayCard[] cards = zone1.GetComponentsInChildren<displayCard>();
        foreach(var card in cards)
        {
            if(!card.card.buff)
            {
                if(card.card.golden)
                {
                    continue;
                }
                card.points +=5;
                card.attackText.text = card.points.ToString();
            }
        }
        effectLoop = true;
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
            displayCard[] cards = zone1.GetComponentsInChildren<displayCard>();
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
            displayCard[] cards = zone2.GetComponentsInChildren<displayCard>();
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
        if(cardPlayed.GetComponent<displayCard>().team)
        {
            zone1 = GameObject.Find("UnitZones");
        }
        else
        {
            zone1 = GameObject.Find("EnemyUnitsZones");
        }
        foreach (var Transform in zone1.transform)
        {
            displayCard[] cards = zone1.GetComponentsInChildren<displayCard>();
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
                displayCard show = destroy.GetComponent<displayCard>();
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
            displayCard[] cards = zone1.GetComponentsInChildren<displayCard>();
            foreach (var card in cards)
            {
                if(card.card.golden) continue;  
                Destroy(card.gameObject);    
            }
        }
        else if(enemyMelee == aux)
        {
            displayCard[] cards = zone2.GetComponentsInChildren<displayCard>();
            foreach (var card in cards)
            {
                if(card.card.golden) continue;  
                Destroy(card.gameObject);    
            }
        }
        else if(distance == aux)
        {
            displayCard[] cards = zone3.GetComponentsInChildren<displayCard>();
            foreach (var card in cards)
            {
                if(card.card.golden) continue;  
                Destroy(card.gameObject);    
            }
        }
        else if(enemyDistance == aux)
        {
            displayCard[] cards = zone4.GetComponentsInChildren<displayCard>();
            foreach (var card in cards)
            {
                if(card.card.golden) continue;  
                Destroy(card.gameObject);    
            }
        }
        else if(asedius == aux)
        {
            displayCard[] cards = zone5.GetComponentsInChildren<displayCard>();
            foreach (var card in cards)
            {
                if(card.card.golden) continue;  
                Destroy(card.gameObject);    
            }
        }
        else if(enemyAsedius == aux)
        {
            displayCard[] cards = zone6.GetComponentsInChildren<displayCard>();
            foreach (var card in cards)
            {
                if(card.card.golden) continue;  
                Destroy(card.gameObject);    
            }
        }
    }
    void CallWeather(GameObject cardPlayed)
    {
        displayCard cardUse = cardPlayed.GetComponent<displayCard>();
        zone1 = GameObject.Find("WeatherZone");
        if(cardUse.team==false)
        {
            deck = GameObject.Find("deckManager1").GetComponent<deckManager>();
            List<GameObject> cardsD = deck.GetCards();
            for (int i = 0; i < cardsD.Count; i++)
            {
                if(cardsD[i].GetComponent<displayCard>().name == "Tundra"|| cardsD[i].GetComponent<displayCard>().name == "Volcan"|| cardsD[i].GetComponent<displayCard>().name == "Jungle")
                {
                    GameObject currentCard = Instantiate(cardsD[i], new Vector3(0,0,0),Quaternion.identity);
                    currentCard.transform.SetParent(zone1.transform, false);
                    cardsD.RemoveAt(i);
                    effect = GameObject.Find("GameManager").GetComponent<effects>();
                    effect.UseEffect(currentCard.GetComponent<displayCard>().card.effect,currentCard);
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
                if(cardsD[i].GetComponent<displayCard>().name == "Tundra"||cardsD[i].GetComponent<displayCard>().name == "Volcan"||cardsD[i].GetComponent<displayCard>().name == "Jungle")
                {
                    GameObject currentCard = Instantiate(cardsD[i], new Vector3(0,0,0),Quaternion.identity);
                    currentCard.transform.SetParent(zone1.transform, false);
                    cardsD.RemoveAt(i);
                    effect = GameObject.Find("GameManager").GetComponent<effects>();
                    effect.UseEffect(currentCard.GetComponent<displayCard>().card.effect,currentCard);
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
            displayCard[] cards = zone1.GetComponentsInChildren<displayCard>();
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
            displayCard[] cards = zone2.GetComponentsInChildren<displayCard>();
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
            displayCard [] cards = zone1.GetComponentsInChildren<displayCard>();
            foreach (var card in cards)
            {
                if(card.card.golden) continue;
                
                card.points = sum;
                card.attackText.text = card.points.ToString();   
            }    
        }
        foreach (var Transform in zone2.transform)
        {
            displayCard [] cards = zone2.GetComponentsInChildren<displayCard>();
            foreach (var card in cards)
            {
                if(card.card.golden) continue;
                
                card.points = sum;
                card.attackText.text = card.points.ToString();   
            }    
        }
    }
}
