using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour 
{
    private Vector2 startPosition;
    private bool isDraggin = false;
    private bool isOverDropZone = false;
    private GameObject dropZone;
    public TurnSystem endturn;
    public TurnSystem turns;
    public effects effect;
    public deckManager deck;
    public Draw draw;

    


    
    void Update()
    {
        if(isDraggin)
        {
            transform.position = new Vector2(Input.mousePosition.x,Input.mousePosition.y);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        isOverDropZone = true;
        dropZone = collision.gameObject;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        isOverDropZone=false;
        dropZone = null;
    }
     

    public void StartDrag()
    {
        startPosition = transform.position;
        isDraggin = true;
    }
    public void EndDrag()
    {
        
        isDraggin = false;
        if(isOverDropZone && CorrectZone())
        {
            turns = GameObject.Find("GameManager").GetComponent<TurnSystem>();
            transform.SetParent(dropZone.transform, false);
            turns.EndTurn();
            effect = GameObject.Find("GameManager").GetComponent<effects>();
            effect.UseEffect(gameObject.GetComponent<displayCard>().card.effect,gameObject);
        }
        else
        {
            transform.position = startPosition;
        }
        
    }
    public bool CorrectZone()
    {
        displayCard zone = gameObject.GetComponent<displayCard>();
        IDZone id = dropZone.GetComponent<IDZone>();
        if(zone.card.zone == id.idZone) return true;
        else return false;
        
    }
}
