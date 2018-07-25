using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameTimeManager
{
    private int gameTimeUnits;
    private int turnCycles;
    private int dTwists;
    private Player p1;
    private Player p2;
    private Player currentTurnPlayer;
    private Button endTurnButton;
    private Button nextPhaseButton;

    public GameTimeManager()
    {
        Debug.Log("Game Start!");
        gameTimeUnits = 0;
        GameStart();
    }

    public int GameTimeUnits
    {
        get { return gameTimeUnits; }
    }

    public Player UIPlayer
    {
        get { return p1; }
    }

    public Player NoUIPlayer
    {
        get { return p2; }
    }

    public Player CurrentTurn
    {
        get { return currentTurnPlayer; }
    }

    public void AdvanceGameTime()
    {
        gameTimeUnits++;
        if((gameTimeUnits % 5) == 0)
        {
            turnCycles++;
        }

        if(turnCycles % 3 == 0)
        {
           // Debug.Log("Twist Dimensions!");
        }
    }

    /// <summary>
    /// This function is created at the start of every card game, it creates
    /// both player objects and maps the UI buttons in the scene to the 
    /// human player or UIPlayer 
    /// </summary>
    private void GameStart()
    {
        p1 = new Player(this);
        p2 = new Player(this, 3, true); //arbitrary as fuck
        GameObject[] buttons = GameObject.FindGameObjectsWithTag("playerUI");
        if (buttons != null)
        {
            if (buttons[0].name == "endTurnButton")
            {
                endTurnButton = buttons[0].GetComponent<Button>();
                nextPhaseButton = buttons[1].GetComponent<Button>();
            }
            else
            {
                endTurnButton = buttons[1].GetComponent<Button>();
                nextPhaseButton = buttons[0].GetComponent<Button>();
            }

            endTurnButton.onClick.AddListener(p1.PlayerTurn.EndTurn);
            //nextPhaseButton.onClick.AddListener(p1.PlayerTurn.StartTurn);

        }
        else
        {
            Debug.Log("Why'd you create player objects with no UI, seems weird");
        }

    }
    
    public List<Player> StartTerritoryChallenge(Card p1Pick, Card p2Pick)
    {
        List<Player> playerTurnOrder = new List<Player>();

        //throw new Exception("The guts havent been made yet these card objects are hella null");
       
        if(Card.GetShape(p1Pick) > Card.GetShape(p2Pick))
        {
            if(Card.GetShape(p1Pick) == ShapeTrait.Triangle)
            {
                playerTurnOrder.Add(p2); //P2 loses TC goes first
                playerTurnOrder.Add(p1);
                return playerTurnOrder;
            }
            else
            {
                playerTurnOrder.Add(p1);//P1 loses TC goes first
                playerTurnOrder.Add(p2);
                return playerTurnOrder;
            }
        }
        else if(Card.GetShape(p2Pick) == ShapeTrait.Triangle)
        {
            playerTurnOrder.Add(p1);//P1 loses TC goes first
            playerTurnOrder.Add(p2);
            return playerTurnOrder;
        }
        else if(Card.GetShape(p1Pick) == Card.GetShape(p2Pick))
        {
            int num = UnityEngine.Random.Range(0, 100);
            if(num > 50)
            {
                playerTurnOrder.Add(p1);//P1 loses TC goes first
                playerTurnOrder.Add(p2);
                return playerTurnOrder;
            }
            else
            {
                playerTurnOrder.Add(p2);//P2 loses TC goes first
                playerTurnOrder.Add(p1);
                return playerTurnOrder;
            }
        }
        else
        {
            playerTurnOrder.Add(p2);//P2 loses TC goes first
            playerTurnOrder.Add(p1);
            return playerTurnOrder;
        }

    }

    
}
