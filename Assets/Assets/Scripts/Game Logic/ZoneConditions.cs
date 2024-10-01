using UnityEngine;
public class ZoneConditions : MonoBehaviour 
{
    public string Zone;
    public bool isInspire;
    public ZoneOWner OWner;
}
public enum ZoneOWner
{
    Player,
    Enemy
}
