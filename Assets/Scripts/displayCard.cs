using UnityEngine;
using UnityEngine.UI;
using TMPro;
//aqui es para que se muestren los datos de la carta//

public class DisplayCard : MonoBehaviour
{
    public Card card;
    public string Owner;
    public TextMeshProUGUI NameText;
    public TextMeshProUGUI AttackText;
    public TextMeshProUGUI Description;
    public string Type;
    public string Faction;
    public string[] Range = new string [3];
    public Image ArtImage;
    public bool Boost;
    public int Points;
    public int AttackOriginal;

    // Start is called before the first frame update
    void Start()
    {
        NameText.text = card.Name;
        ArtImage.sprite = card.CardImage;
        AttackOriginal = card.Attack;
        Points = card.Attack;
        Description.text = card.Description;                                                                                                                                                                                                                                               
        Type = card.Type;
        Faction = card.Faction;
        Range = card.Range;
        GetOwner();
        CollectCardPoints();
    }
    public void CollectCardPoints()
    {
        if(Type == "Oro" || Type == "Plata")  AttackText.text = Points.ToString();
    }
    void GetOwner() => Owner = card.Type == "Clima" || card.Type == "Despeje"? "Neutral" : card.CardOwner;
}
