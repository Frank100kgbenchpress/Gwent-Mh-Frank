using UnityEngine;
using TMPro;
using Unity.VisualScripting;
//aqui se manejan los turnos//

public class TurnSystem : MonoBehaviour
{
    public bool isYourTurn;
    public TextMeshProUGUI playerRounds;
    public TextMeshProUGUI enemyRounds; 
    public TextMeshProUGUI turnText;
    public TextMeshProUGUI playerpoints;
    public TextMeshProUGUI enemyPoints;
    public GameObject hand1;
    public GameObject hand2;
    public bool round;
    public bool useDecoy;
    public bool Team;
    public Draw draw;
    public Change change;
    public Change change1;
    public bool pass;
    public int counter=0;
    public int playerWin;
    public int enemyWin;
    public GameObject camera1;
    public GameObject camera2;
    public Board board;
    void Update() 
    {
        playerRounds.text = playerWin.ToString();
        enemyRounds.text = enemyWin.ToString();
    }
    void Start() 
    {
        playerRounds.text = playerWin.ToString();
        enemyRounds.text = enemyWin.ToString();
        draw = GameObject.Find("GameManager").GetComponent<Draw>();
        for (int i = 0; i < 10; i++)
        {
            if(i<7)
            {
                draw.DrawCard(1);
            }
            draw.DrawCard(2);
        }
        EndTurn();
        change = GameObject.Find("Change").GetComponent<Change>();
        change1 = GameObject.Find("EnemyChange").GetComponent<Change>();
        change.change = true;
        change1.change = true;
        change1.Hide();
        NoMove(hand1,false);
        NoMove(hand2,false);
    }   
    public void OnClick() //para cuando se pasa turno//
    {
       
        if(isYourTurn)
        {
            if(change.change)
            {
                NoMove(hand1,true);
                change.change = false;
                change.Hide();
            }
        }
        else if(!isYourTurn)
        {
            if(change1.change)
            {
                NoMove(hand2,true);
                change1.change = false;
                change1.Hide();
            }   
        }
         round =! round;
        isYourTurn =! isYourTurn;
        EndTurn();
        counter++;
        if(counter==2)
        {
            
            draw = GameObject.Find("GameManager").GetComponent<Draw>();
            for(int i=0;i<2;i++) 
            {
                draw.DrawCard(1);
                draw.DrawCard(2);
            }
            int playerPoints = int.Parse(playerpoints.text);
            int enemypoints  = int.Parse(enemyPoints.text);
            if(playerPoints >= enemypoints) 
            {
                playerWin++;
                if(playerWin ==2)
                {
                    Menu.WinnerScreen();
                }
                
                isYourTurn = true;
                EndTurn();
            }
            else if(enemypoints > playerPoints)
            {
                enemyWin++;
                if(enemyWin ==2)
                {
                    Menu.WinnerScreen2();
                }          
                isYourTurn = false;
                EndTurn();
            }
            Clean();
            counter = 0;
        }
        
    }
    public void EndTurn() //pa terminar turno//
    {
        if(!round)
        {
            isYourTurn =! isYourTurn;
            
        }
        UpdateTurnUI();
    }
    private void UpdateTurnUI()//enseña el texto de que cambiamos turnos//
    {
        if (isYourTurn)
        {
            turnText.text = "Your Turn";
            HideEnemyCards(hand1,true);
            HideEnemyCards(hand2,false); 
            //camera1.SetActive(true);
            //camera2.SetActive(false);
        }
        else
        {
            turnText.text = "Opponent Turn";
            HideEnemyCards(hand2,true);
            HideEnemyCards(hand1,false);
            RotateCards(hand2);
            //camera1.SetActive(false);
            //camera2.SetActive(true);
        }
    }
    //para cuando juegue yo esconden las cartas del otro//
    public void HideEnemyCards(GameObject hand,bool myHand)
    {
        if(myHand==true)
        {
            foreach(Transform card in hand.transform)
            {
                card.gameObject.SetActive(true);
            }
        }
        else
        {
            foreach(Transform card in hand.transform)
            {
                card.gameObject.SetActive(false);
            }
        }        
    }
    //para que las cartas no se puedan mover si toca lo de cambiar cartas//
    public void NoMove(GameObject hand,bool move)
    {
        DragAndDrop[] cards = hand.GetComponentsInChildren<DragAndDrop>();
        foreach(DragAndDrop card in cards)
        {
            card.enabled = move;
        }
    }
    void Clean() //limpia el campo cuando se acaba una ronda//
    {
        CleanZone(GameObject.Find("UnitZones"));
        CleanZone(GameObject.Find("EnemyUnitsZones"));
        CleanZone(GameObject.Find("SupportZone"));
        CleanZone(GameObject.Find("EnemySupportZones"));
        GameObject weather = GameObject.Find("WeatherZone");
        foreach(Transform card in weather.transform)
        {
            Destroy(card.gameObject);
        }
    }
    void CleanZone(GameObject objective)
    {
        foreach ( Transform zone in objective.transform)
        {
            foreach (Transform card in zone)
            {
                Destroy(card.gameObject);
            }   
        }
    } 
    public void RotateCards(GameObject Hand) //pa rotar las cartas pal cambio de camara//
    {
        UnityEngine.Quaternion pos = transform.rotation;
        foreach(Transform card in Hand.transform)
        {
            pos = card.transform.rotation;
            pos = UnityEngine.Quaternion.Euler(180f,180f,0);
            card.transform.rotation = pos;
        }
    }
    public string TriggerPlayer()=>   isYourTurn ? "Player" : "Enemy";
    public Hand HandOfPlayer(string player) => player == "Player" ? GameObject.Find("PlayerHand").GetComponent<Hand>() : GameObject.Find("EnemyHand").GetComponent<Hand>();
    public Deck DeckOfPlayer(string owner) => owner == "Player" ? GameObject.Find("deckManager1").GetComponent<Deck>() : GameObject.Find("deckManager2").GetComponent<Deck>();
    public Board Board()  => board;
    public Field FieldOfPlayer(string player) => player == "Player" ? GameObject.Find("UnitZones").GetComponent<Field>() : GameObject.Find("EnemyUnitsZones").GetComponent<Field>();
    public Graveyard GraveyardOfPlayer(string player) => player == "Player" ? GameObject.Find("Graveyard1").GetComponent<Graveyard>() : GameObject.Find("Graveyard2").GetComponent<Graveyard>();
    
}
