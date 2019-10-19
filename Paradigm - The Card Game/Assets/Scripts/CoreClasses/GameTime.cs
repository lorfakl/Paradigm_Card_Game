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
       

    }
    
    
    
}
