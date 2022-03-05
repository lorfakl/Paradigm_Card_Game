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
        //GameStart();
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


    
}
