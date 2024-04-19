using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Draw : MonoBehaviour
{
    public GameObject Card;
    public GameObject Hand1;
    public GameObject Hand2;
    public deckManager deck1;
    public deckManager deck2;
   /* public TextMeshProUGUI deck1Size;
    public TextMeshProUGUI deck2Size;*/
    void Start()
    {
        for(int i = 0; i <10;i++)
        {
            Draw1();
            Draw2();
        }
    }
    public void Draw1()
    {
        List<GameObject> deckCards = deck1.GetCards();
        int random = Random.Range(0,deckCards.Count);
        GameObject selectedCard = deckCards[random];
        GameObject playerCard = Instantiate(selectedCard,new Vector3(0,0,0),Quaternion.identity);
        playerCard.transform.SetParent( Hand1.transform, false);
        deckCards.RemoveAt(random);
        /*deck1Size.text = deck1.GetCards().Count.ToString();*/
    }
    public void Draw2()
    {
        List<GameObject> deckCards = deck2.GetCards();
        int random = Random.Range(0,deckCards.Count);
        GameObject selectedCard = deckCards[random];
        GameObject playerCard = Instantiate(selectedCard,new Vector3(0,0,0),Quaternion.identity);
        playerCard.transform.SetParent( Hand2.transform, false);
        deckCards.RemoveAt(random);
        /*deck1Size.text = deck1.GetCards().Count.ToString();*/
    }
}
