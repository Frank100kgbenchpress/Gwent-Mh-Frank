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
    // Start is called before the first frame update
   public void Print()
   {
        Debug.Log(name + ": ");
   }
}
