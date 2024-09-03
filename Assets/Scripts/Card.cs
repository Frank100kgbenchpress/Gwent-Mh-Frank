using UnityEngine;
using DSL;
[CreateAssetMenu(fileName = "New Card" , menuName = "Card")]
//estas son las propiedades de la  carta//
public class Card : ScriptableObject
{
    public string Name;
    public string Description;
    public int Attack;
    public string CardOwner;
    public int Id;
    public string Type;
    public OnActivation Effects;
    public string[] Range = new string [3];
    public GameObject Prefab;
    public string Faction;
    public string EffectText;
    public Sprite CardImage; 
    public bool Boost;
}