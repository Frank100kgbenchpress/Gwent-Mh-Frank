using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Card" , menuName = "Card")]
public class Cards : ScriptableObject
{
    public new string name;
    public int attack;
    public Sprite cardImage;
    public int id;
    public int zone;
    public string description;
    public bool golden;
    public int effect;
    public bool buff;
}
