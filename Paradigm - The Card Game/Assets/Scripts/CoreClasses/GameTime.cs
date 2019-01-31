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
        p1 = new HumanPlayer(this, 5);
        p2 = new AIPlayer(this, 3); //arbitrary as fuck

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
