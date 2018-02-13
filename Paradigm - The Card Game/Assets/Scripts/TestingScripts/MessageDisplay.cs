using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageDisplay : MonoBehaviour {

    public GameObject messagePanel;

    public delegate void CommandFunction();
    CommandFunction commandFunction;
    private bool isPrepared = false;
    private Dictionary<string, CommandFunction> commandDict = new Dictionary<string, CommandFunction>();

    // Use this for initialization
    void Start()
    {
        PrepareDict();
    }

    // Update is called once per frame
    void Update() {

    }

    public void RunCommand(string command)
    {
        commandFunction = null;
        if(commandDict.TryGetValue(command, out commandFunction))
        {
            commandFunction();
        }
        else
        {
            print(command + " is not a valid command, here are the commands");
            ShowCommands();
        }
    }


    void PrepareDict()
    {
        if (!isPrepared)
        {
            commandDict.Add("!commands", ShowCommands);
            commandDict.Add("draw", Draw);
            commandDict.Add("show hand", DisplayHand);
            commandDict.Add("end", EndTurn);
            commandDict.Add("attack", Attack);
            commandDict.Add("show field", DisplayField);
            commandDict.Add("show deck", DisplayDeck);
        }

        isPrepared = true;
    }

    private void Attack()
    {
        print("Attack");
    }

    private void DisplayDeck()
    {
        print("Showing the deck");
    }

    private void DisplayField()
    {
        print("showing the field");
    }

    private void DisplayHand()
    {
        print("showing the hand");
    }

    private void Draw()
    {
        print("draw from the deck");
    }

    private void EndTurn()
    {
        print("I end my turn");
    }

    private void ShowCommands()
    {
        Debug.Log("Commands: '!commands', 'draw', 'show hand', 'end': Ends the turn, 'attack', 'show field', 'show deck' ");
    }

    
    

    

    
    
    
}
