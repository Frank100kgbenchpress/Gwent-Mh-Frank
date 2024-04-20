using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class effects : MonoBehaviour
{
    public GameObject zone1;
    public GameObject zone2;
    public GameObject zone3;
    public bool wheatherUse;
    public bool effectLoop;

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

        }
        if(hability==5)
        {

        }
        if(hability==6)
        {

        }
        if(hability==7)
        {

        }
        if(hability==8)
        {

        }
        if(hability==9)
        {

        }
        if(hability==10)
        {

        }
        if(hability==11)
        {

        }
        if(hability==12)
        {

        }
    }
    void RowPowerUpp(GameObject cardPlayed)
    {
        int cardPosition = cardPlayed.GetComponent<displayCard>().position;
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
        if(cardPosition==1)
        {
            zone1 = GameObject.Find("MeleeZone");
        }
        if(cardPosition==2)
        {
            zone1 = GameObject.Find("DistanceZone");
        }
        if(cardPosition==3)
        {
            zone1 = GameObject.Find("AsediusZone");
        }
        if(cardPosition==8)
        {
            zone1 = GameObject.Find("EnemyMeleeZone");
        }
        if(cardPosition==9)
        {
            zone1 = GameObject.Find("EnemyDistanceZone");
        }
        if(cardPosition==10)
        {
            zone1 = GameObject.Find("EnemyDistanceZone");
        }
    }
    void Weather(GameObject cardPlayed)
    {
        string weather = cardPlayed.GetComponent<displayCard>().card.name;
        if(weather=="Tundra")
        {
            zone1 = GameObject.Find("DistanceZone");
            zone2 = GameObject.Find("EnemyDistanceZone");
        }
        if(weather=="Volcan")
        {
            zone1 = GameObject.Find("MeleeZone");
            zone2 = GameObject.Find("EnemyMeleeZone");
        }
        if(weather=="Jungle")
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
            displayCard[] cards = zone1.GetComponentsInChildren<displayCard>();
            zone1 = GameObject.Find("MeleeZone");
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
            displayCard[] cards = zone1.GetComponentsInChildren<displayCard>();
            zone1 = GameObject.Find("DistanceZone");
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
        zone1 = GameObject.Find("WheatherZone");
        bool[] check = new bool[3];
        foreach(Transform card in zone1.transform)
        {
            if(card.gameObject.GetComponent<displayCard>().card.name=="Tundra")
            {
                check[0] = true;
            }
            if(card.gameObject.GetComponent<displayCard>().card.name=="Volcan")
            {
                check[1] = true; 
            }
            if(card.gameObject.GetComponent<displayCard>().card.name=="Jungle")
            {
                check[2] = true;
            }
        }
        if(check[0])
        {
            zone2 = GameObject.Find("AsediusZone");
            zone3 = GameObject.Find("EnemyAsediusZone");
            displayCard[] asedius = zone2.GetComponentsInChildren<displayCard>();
            foreach(var card in asedius)
            {
                card.points = card.card.attack;
                card.attackText.text = card.points.ToString();
                card.card.buff = false;
            }
            displayCard[] enemyAsedius = zone2.GetComponentsInChildren<displayCard>();
            foreach(var card in enemyAsedius)
            {
                card.points = card.card.attack;
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
                card.points = card.card.attack;
                card.attackText.text = card.points.ToString();
                card.card.buff = false;
            }
            displayCard[] enemyMelee = zone2.GetComponentsInChildren<displayCard>();
            foreach(var card in enemyMelee)
            {
                card.points = card.card.attack;
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
                card.points = card.card.attack;
                card.attackText.text = card.points.ToString();
                card.card.buff = false;
            }
            displayCard[] enemyDistance = zone2.GetComponentsInChildren<displayCard>();
            foreach(var card in enemyDistance)
            {
                card.points = card.card.attack;
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
            zone1 = GameObject.Find("SupportMeleeZone");
        }
        if(cardPosition==5)
        {
            zone1 = GameObject.Find("SupportDistanceZone");
        }
        if(cardPosition==6)
        {
            zone1 = GameObject.Find("SupportAsediusZone");
        }
        if(cardPosition==11)
        {
            zone1 = GameObject.Find("EnemySupportMeleeZone");
        }
        if(cardPosition==12)
        {
            zone1 = GameObject.Find("EnemySupportDistanceZone");
        }
        if(cardPosition==13)
        {
            zone1 = GameObject.Find("EnemySupportDistanceZone");
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
}
