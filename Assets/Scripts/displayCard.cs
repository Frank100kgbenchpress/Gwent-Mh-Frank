using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class displayCard : MonoBehaviour
{
    public Cards card;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI attackText;
    public Image artImage;
    public int position;
    public bool team;
    public bool boost;
    public int attackOriginal;

    // Start is called before the first frame update
    void Start()
    {
        nameText.text = card.name;
        attackText.text = card.attack.ToString();
        artImage.sprite = card.cardImage;
        attackOriginal = card.attack;
    }
}
