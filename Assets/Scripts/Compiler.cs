using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DSL;
public class Compiler : MonoBehaviour
{
    public TMP_Text myTextMeshPro;
    public GameObject panel;
    public GameObject scroll;
    public GameObject prefab;
    public Hand hand;
    public Card card;
    public Transform canvas;

    public void Compile(TMP_Text myTextMeshPro)
    {
        string input = myTextMeshPro.text;
        List<string> errors = new();
        Lexer lexer = new Lexer(input,errors);
        List<Token> tokens = lexer.ScanTokens();
        Debug.Log(errors.Count+ " errores lexer");
        errors.RemoveAt(errors.Count-1);
        if(errors.Count > 0)
        {
            Debug.Log("Manzana");
            string joinedText = string.Join("\n", errors);
            PrintErrors(joinedText);
            return;
        }
        Parser parser = new Parser(tokens);
        Node node = parser.Parse();
        if(parser.Ex != null)
        {
            PrintErrors(parser.Ex.ToString());
            Debug.Log(parser.Ex.ToString() + "pancontimba");
            return;
        }
        Context context = new Context();
        context.turnSystem = GameObject.Find("GameManager").GetComponent<TurnSystem>();
        SemanticalCheck semanticalCheck = new SemanticalCheck(node,context,errors);
        foreach (var er in errors)
        {
            Debug.Log(er + " Tomatico");
        }
        if(errors.Count ==1) errors.RemoveAt(0);
        if(errors.Count > 0)
        {
            string joinedText = string.Join("\n", errors);
            PrintErrors(joinedText);
            Debug.Log(myTextMeshPro.text+"timba");
            return;
        }
        Debug.Log("pirip");
        foreach(CardNode card in (node as Program).CardNodes)
        {
            Debug.Log("azucar");
            SpawnCard(card,context);
        }
    }
    void PrintErrors(string joinedText)
    {
        myTextMeshPro.text += joinedText;
        TogglePanelVisibility();
    }
    
    public void TogglePanelVisibility()
    {
        bool isActive = panel.activeSelf;
        panel.SetActive(!isActive);
    }

    public void ToggleEditPanel()
    {
        bool isActive = scroll.activeSelf;
        scroll.SetActive(!isActive);
    }

    void SpawnCard(CardNode cardnode, Context context)
    {
        Debug.Log("wiiki");
        Card card = new Card();
        card.name = cardnode.Name.name.Evaluate(new Context()).ToString();
        card.Type = cardnode.Type.Type.Evaluate(new Context()).ToString();
        card.Faction = cardnode.Faction.faction.Evaluate(new Context()).ToString();
        card.Attack = (int)cardnode.Power.power.Evaluate(new Context());
        card.Context = context;
        int i=0;
        foreach(var zone in cardnode.Range.range)
        {
            card.Range[i++] = zone.Evaluate(new Context()) as string;
        }
        card.Effects = cardnode.OnActivation;
        card.Prefab = prefab;
        card.CardOwner = "Player";
        card.EffectText = "a";
        card.Id = 50;
        GameObject gameObject = Instantiate(prefab);
        gameObject.GetComponent<DisplayCard>().card = card;
        hand.Push(gameObject);
    }
}
