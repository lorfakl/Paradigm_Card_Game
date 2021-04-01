using DataBase;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


public delegate void StateProcessingComplete();

public class GameMaster : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;
    public GameObject uiCamera;

    public static Player PlayerOne { get; private set; }
    public static Player PlayerTwo { get; private set; }
    public static bool IsTurnActive { get; set; }
    public static GameObject UiCamera { get; private set; }
    public StateMachine GameSetupStateMachince { get; private set; }
    public List<Func<IEnumerator>> FirstPlayerTurn { get; private set; }
    public List<Func<IEnumerator>> SecondPlayerTurn { get; private set; }



    private StateMachine[] turnStateMachines = new StateMachine[2];
    private static Player FirstPlayer { get; set; }
    private static Player SecondPlayer { get; set; }
    
    private bool firstTurnHasStarted = false;
    private bool isPhaseComplete = false;
    private static bool isTurnOrderSet = false;

    private bool areTurnsInitialized;

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
        GameSetupStateMachince = new StateMachine("TurnPhase", new List<IState> {new TerritoryChallengeState(),
            new BarrierSelectState(), new EndPhaseState() }, MarkTurnCompleted);

        GameSetupStateMachince.ProcessStates();
        //GameSetupStateMachince.
        //SUBSCRIBE TO THE STATE MACHINES EVENT TO COUNT TURNCYCLES AND STUFF

        print("GameMaster Start Exit: Done");
        
    }

    void Update()
    {
       

        print("Game Master Upfate");
        if(isTurnOrderSet && !areTurnsInitialized)
        {
            InitalizeTurns();
            print("Turn order is set and turns initialized");

            
        }

        if(areTurnsInitialized && isPhaseComplete)
        {
            print("Turns are happening");
            //if (isPhaseComplete)
            //{
                
                StartCoroutine(ExecuteTurnPhase());
                isPhaseComplete = false;
            //}
        }
        
        
    }

    private void InitalizeTurns()
    {
        FirstPlayerTurn = new List<Func<IEnumerator>> { FirstPlayer.PerformGather, FirstPlayer.PerformAwaken, FirstPlayer.PerformCentral, FirstPlayer.PerformCrystal, FirstPlayer.PerformEnd };
        SecondPlayerTurn = new List<Func<IEnumerator>> { SecondPlayer.PerformGather, SecondPlayer.PerformAwaken, SecondPlayer.PerformCentral, SecondPlayer.PerformCrystal, SecondPlayer.PerformEnd };
        areTurnsInitialized = true;
        
    }

    private IEnumerator ExecuteTurnPhase()
    {
        print("execute turn pjases");
        if (!firstTurnHasStarted)
        {
            firstTurnHasStarted = true;
            foreach ( Func<IEnumerator> turnphaseFunc in FirstPlayerTurn)
            {
                //if(isPhaseComplete == false)
                //{
                    yield return StartCoroutine(turnphaseFunc());
                    //yield return new WaitUntil(() => isPhaseComplete);
                //}
            }
            print("first player turn done");
            isPhaseComplete = true;
            //print("RUN ONLY ONCE");
        }
        else
        {
            firstTurnHasStarted = false;
            foreach (Func<IEnumerator> turnphaseFunc in SecondPlayerTurn)
            {               
                yield return StartCoroutine(turnphaseFunc());
            }
            print("Second Player Turn Done");
            isPhaseComplete = true;
            //print("RUN ONLY ONCE");
        }
    }
    public static void SetTurnOrder(List<IPlayable> playerTurnOrder)
    {
        FirstPlayer = (Player)playerTurnOrder[0];
        SecondPlayer = (Player)playerTurnOrder[1];
        isTurnOrderSet = true;
        //TESTING IF THE TERRITORY CHALLENGE WORKS: LATER
        print("TURN ORDER SET");
        //print("going second: " + SecondPlayer.ID);
    }

    public void MarkTurnCompleted()
    {
        isPhaseComplete = true;
        print("Turn complete");
    }
}
