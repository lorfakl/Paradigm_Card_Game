using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

// this class will take care of switching turns and counting down time until the turn expires
public class MatchTurnManager : MonoBehaviour {


    // for Singleton Pattern
    public static MatchTurnManager Instance;

    // PRIVATE FIELDS
    // reference to a timer to measure 
    private GlobalTimer timer;
    public static PlayerInteraction[] CurrentPlayers = new PlayerInteraction[2];

    // PROPERTIES
    private Player2 _whoseTurn;
    public Player2 whoseTurn
    {
        get
        {
            return _whoseTurn;
        }

        set
        {
            _whoseTurn = value;
            timer.StartTimer();

            //GlobalSettings.Instance.EnableEndTurnButtonOnStart(_whoseTurn);

            //TurnMaker tm = whoseTurn.GetComponent<TurnMaker>();
            // Player2`s method OnTurnStart() will be called in tm.OnTurnStart();
            /*tm.OnTurnStart();
            if (tm is PlayerTurnMaker)
            {
                whoseTurn.HighlightPlayableCards();
            }
            // remove highlights for opponent.
            whoseTurn.otherPlayer.HighlightPlayableCards(true);*/
                
        }
    }


    // METHODS
    void Awake()
    {
        Instance = this;
        timer = GetComponent<GlobalTimer>();
    }

    void Start()
    {
        GameStart();
    }

    public void GameStart()
    {
        Debug.Log("In TurnManager.OnGameStart()");
        foreach(PlayerInteraction player in CurrentPlayers)
        {
            
        }
        Debug.Log("Do Territory Challenge");

        Debug.Log("Do Barrier selection");
        
        /*CardLogic.CardsCreatedThisGame.Clear();
        CreatureLogic.CreaturesCreatedThisGame.Clear();

        foreach (Player2 p in Player2.Players)
        {
            p.ManaThisTurn = 0;
            p.ManaLeft = 0;
            p.LoadCharacterInfoFromAsset();
            p.TransmitInfoAboutPlayerToVisual();
            p.PArea.PDeck.CardsInDeck = p.Deck2.cards.Count;
            // move both portraits to the center
            p.PArea.Portrait.transform.position = p.PArea.InitialPortraitPosition.position;
        }

        Sequence s = DOTween.Sequence();
        s.Append(Player2.Players[0].PArea.Portrait.transform.DOMove(Player2.Players[0].PArea.PortraitPosition.position, 1f).SetEase(Ease.InQuad));
        s.Insert(0f, Player2.Players[1].PArea.Portrait.transform.DOMove(Player2.Players[1].PArea.PortraitPosition.position, 1f).SetEase(Ease.InQuad));
        s.PrependInterval(3f);
        s.OnComplete(() =>
            {
                // determine who starts the game.
                int rnd = Random.Range(0,2);  // 2 is exclusive boundary
                // Debug.Log(Player2.Players.Length);
                Player2 whoGoesFirst = Player2.Players[rnd];
                // Debug.Log(whoGoesFirst);
                Player2 whoGoesSecond = whoGoesFirst.otherPlayer;
                // Debug.Log(whoGoesSecond);
         
                // draw 4 cards for first Player2 and 5 for second Player2
                int initDraw = 4;
                for (int i = 0; i < initDraw; i++)
                {            
                    // second Player2 draws a card
                    whoGoesSecond.DrawACard(true);
                    // first Player2 draws a card
                    whoGoesFirst.DrawACard(true);
                }
                // add one more card to second Player2`s hand
                whoGoesSecond.DrawACard(true);
                //new GivePlayerACoinCommand(null, whoGoesSecond).AddToQueue();
                whoGoesSecond.GetACardNotFromDeck(CoinCard);
                new StartATurnCommand(whoGoesFirst).AddToQueue();
            });*/
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            EndTurn();
    }

    // FOR TEST PURPOSES ONLY
    public void EndTurnTest()
    {
        timer.StopTimer();
        timer.StartTimer();
    }

    public void EndTurn()
    {
        // stop timer
        timer.StopTimer();
        // send all commands in the end of current Player2`s turn
        whoseTurn.OnTurnEnd();

        new StartATurnCommand(whoseTurn.otherPlayer).AddToQueue();
    }

    public void StopTheTimer()
    {
        timer.StopTimer();
    }

    public void StartTimer()
    {
        timer.StartTimer();
    }
}

