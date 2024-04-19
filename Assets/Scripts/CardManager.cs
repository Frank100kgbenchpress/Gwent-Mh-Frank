/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public GameObject cardPrefab; // Tu prefab de carta
    public Cards cardData; // Tu ScriptableObject con los datos de la carta

    void Start()
    {
        // Crea una instancia del prefab de la carta
        GameObject cardInstance = Instantiate(cardPrefab);

        // Asigna el ScriptableObject a la instancia del prefab
        CardDisplay cardDisplay = cardInstance.GetComponent<CardDisplay>();
        if (cardDisplay != null)
        {
            cardDisplay.CardData = cardData;
        }
    }
}
*/