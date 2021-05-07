using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using Utilities;

public class MessageDisplay : MonoBehaviour {

    public GameObject messagePanel;
    public GameObject messageObject;
    public GameObject content;
    public float offset = 226;

    private int turnPhaseCounter = 0;
    private float runningYOffset = 0;
    private bool isInitialValueSet = false;
    public delegate void CommandFunction();
    CommandFunction commandFunction;
    private bool isPrepared = false;
    List<GameObject> createdMessageObjects = new List<GameObject>();
    private Dictionary<string, CommandFunction> commandDict = new Dictionary<string, CommandFunction>();
    Player testPlayer;
    Player testAI;
    // Use this for initialization
    void Start()
    {
        PrepareDict();
        testAI = new AIPlayer(Guid.NewGuid());
        testPlayer = new HumanPlayer(Guid.NewGuid());
        DataBase.CardDataBase.MakePlayerDeck(testPlayer);
        DataBase.CardDataBase.MakePlayerDeck(testAI);
        testPlayer.PlayerDeck.GameStartSetup();
        testPlayer.PlayerDeck.Shuffle();

       
        GameEventsManager.NotifySubsOfEvent += CatchEvents;
       
        //testPlayer.PlayerDeck.Draw();
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

    void CatchEvents(object sender, GameEventsArgs e)
    {
        LogMessage(e);

    }

    private void LogMessage(GameEventsArgs e)
    {
        print("DID MEssage diSPLAY geT A mesSAGe");
        string eventData = e.Print();
        if(e.IsUIEvent)
        {
            eventData = eventData + "\n UI Event: True";
        }
        GameObject msg = MakeMessageObject(); //.GetComponent<TextMeshProUGUI>();
        msg.GetComponent<TextMeshProUGUI>().text = eventData;
    }

    GameObject MakeMessageObject()
    {
        foreach(var msg in createdMessageObjects)
        {
            Vector2 offsetAnchoredPos = msg.GetComponent<RectTransform>().anchoredPosition;
            offsetAnchoredPos.y -= offset;
            msg.GetComponent<RectTransform>().anchoredPosition = offsetAnchoredPos;
        }
        //Vector3 pos = new Vector3(0, offset);
        Vector2 loc = new Vector2(0, runningYOffset);
        GameObject msgObject = Instantiate(messageObject, messagePanel.transform, false);
        createdMessageObjects.Add(msgObject);
        //print("Local Pos: " + msgObject.transform.localPosition);
        //print("Pos: " + msgObject.transform.position);
        /*RectTransform msgRect = msgObject.GetComponent<RectTransform>();
        print("Anchored Pos: " + msgRect.anchoredPosition);
        if (!isInitialValueSet)
        {
            runningYOffset = msgRect.anchoredPosition.y - offset;
            print("Initial Running Y: " + runningYOffset);
            isInitialValueSet = true;
        }
        else
        {
            msgRect.anchoredPosition = loc;
            runningYOffset -= offset;
        }
        
        
        print("New Running Y: " + runningYOffset);*/
        return msgObject;
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
            commandDict.Add("next", ChangeTurnPhase);
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
        testPlayer.PlayerDeck.Draw();
    }

    private void ChangeTurnPhase()
    {
        if(turnPhaseCounter >= 6)
        {
            turnPhaseCounter = 0;
        }
        else
        {
            turnPhaseCounter++;
        }

        switch(turnPhaseCounter)
        {
            case 1:
                HelperFunctions.RaiseNewEvent(this, testPlayer, testPlayer, new GameAction(NonMoveAction.Turn, TurnPhase.Gather));
                break;
            case 2:
                HelperFunctions.RaiseNewEvent(this, testPlayer, testPlayer, new GameAction(NonMoveAction.Turn, TurnPhase.Awaken));
                break;
            case 3:
                HelperFunctions.RaiseNewEvent(this, testPlayer, testPlayer, new GameAction(NonMoveAction.Turn, TurnPhase.Central));
                break;
            case 4:
                HelperFunctions.RaiseNewEvent(this, testPlayer, testPlayer, new GameAction(NonMoveAction.Turn, TurnPhase.Crystallization));
                break;
            case 5:
                HelperFunctions.RaiseNewEvent(this, testPlayer, testPlayer, new GameAction(NonMoveAction.Turn, TurnPhase.End));
                break;
        }
       
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
