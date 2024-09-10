using UnityEngine;
public class Draw : MonoBehaviour 
{
    public Hand Hand1;
    public Hand Hand2;
    public Deck deck1;
    public Deck deck2;
    public void DrawCard(int player) 
    {
        if(player == 1)    Hand1.Push(deck1.Pop());
        else    Hand2.Push(deck2.Pop());
    }
}
