using UnityEngine;
using DSL;
[CreateAssetMenu(fileName = "New Card" , menuName = "Card")]
//estas son las propiedades de la  carta//
public class Card : ScriptableObject
{
    public string Name { get; protected set; }
    public string Description{get;set;}
    public int Attack{get;set;}
    public string CardOwner{get;set;}
    public int Id { get; set; }
    public string Type { get; set; }
    public OnActivation Effects{get;set;}
    public string[] Range = new string [3];
    public GameObject Prefab { get; set; }
    public string Faction{get;set;}
    public string EffectText{get;set;}
    public Sprite CardImage{get;set;} 
    public bool Boost{get;set;}
}