using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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
            //Dimension Twist
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
        p2 = new Player(this, 3); //arbitrary as fuck
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
            nextPhaseButton.onClick.AddListener(p1.PlayerTurn.StartTurn);

        }
        else
        {
            Debug.Log("Why'd you create player objects with no UI, seems weird");
        }
    }

    
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
