using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Change : MonoBehaviour
{
    public bool change;
    public TurnSystem movement;
    public GameObject hand;
    public int counter;
    public void OnClick()
    {
        movement = GameObject.Find("BattleSystem").GetComponent<TurnSystem>();
        if(change == true)
        {
            movement.NoMove(hand,true);
            Hide();
            change = false;
        }
    }
    public void Hide() 
    {
        Vector3 pos = transform.position;
        pos.z = -10;
        transform.position = pos;
    }
}
