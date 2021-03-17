using DataBase;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;
    public GameObject uiCamera;

    public static Player PlayerOne { get; private set; }
    public static Player PlayerTwo { get; private set; }
    public static GameObject UiCamera { get; private set; }
    private static Player FirstPlayer { get; set; }
    private static Player SecondPlayer { get; set; }
    public StateMachine TurnphaseStateMachine { get; private set; }
    void Awake()
    {
        PlayerOne = new HumanPlayer(Guid.NewGuid());
        PlayerTwo = new AIPlayer(Guid.NewGuid());
        UiCamera = uiCamera;

        CardDataBase.MakePlayerDeck(PlayerOne);
        print("Made player deck?");
        CardDataBase.MakePlayerDeck(PlayerTwo);

        PlayerOne.Majesty = PlayerOne.PlayerDeck.GetMajesty();
        PlayerTwo.Majesty = PlayerTwo.PlayerDeck.GetMajesty();
        print("Should be a full deck" + PlayerOne.PlayerDeck.Count);
        print("Human player ID:" + PlayerOne.ID);
        print("AI player ID:" + PlayerTwo.ID);
        PlayerOne.PlayerDeck.GameStartSetup();
        PlayerTwo.PlayerDeck.GameStartSetup();

        if (PlayerOne == null || PlayerTwo == null)
        {
            Debug.Log("Player in Game Master is Null as fuck!");
        }
        if (player1 != null && player2 != null)
        {
            GameObject player1Obj = Instantiate(player1);
            GameObject player2Obj = Instantiate(player2);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        TurnphaseStateMachine = new StateMachine("TurnPhase", new List<IState> {new TerritoryChallengeState(),
            new BarrierSelectState(), new EndPhaseState() });

        TurnphaseStateMachine.ProcessStates();
        //TurnphaseStateMachine.
        //SUBSCRIBE TO THE STATE MACHINES EVENT TO COUNT TURNCYCLES AND STUFF
    }

    public static void SetTurnOrder(List<IPlayable> playerTurnOrder)
    {
        FirstPlayer = (Player)playerTurnOrder[0];
        SecondPlayer = (Player)playerTurnOrder[1];
    }
}
