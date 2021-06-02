using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Utilities;

public enum MoveAction
{
    Break, Build, Collect, Crystallize, Delete, Despawn, Destroy, Draw, Lock, Rest, Restore, Return, Search, Spawn, Unlock, UndefinedMove, None
}

public enum NonMoveAction
{
    Attack, Activate, Battle, Block, Damage, Forge, Heal, Initiate, Respond, Turn, DimensionTwist, None, GameEnd, DeclaredAttack,
    Active
}

public enum TurnPhase
{
    Gather, Awaken, Central, Crystallization, End, Start, None
}

public enum EventType
{
    Gameplay, LegalCheck, UIUpdate, StackNotification
}

public struct GameAction
{
    public MoveAction MoveAction { get; private set; }
    public NonMoveAction NonMoveAction { get; private set; }

    public TurnPhase TurnPhase { get; private set; }
    public GameAction(MoveAction ma, NonMoveAction nma)
    {
        MoveAction = ma;
        NonMoveAction = nma;
        TurnPhase = TurnPhase.None;
    }

    public GameAction(NonMoveAction non, TurnPhase phase)
    {
        NonMoveAction = non;
        TurnPhase = phase;
        MoveAction = MoveAction.None;
    }

    public GameAction(MoveAction ma, NonMoveAction nma, TurnPhase tp)
    {
        MoveAction = ma;
        NonMoveAction = nma;
        TurnPhase = tp;
    }
}


public class GameEventsArgs : EventArgs
{
    protected Player owner;
    protected Card cardSource;
    protected List<LocationChanges> boardMovements;
    protected MoveAction moveAction;
    protected NonMoveAction notMoveAction;
    protected TurnPhase phase;
    protected Player playerTarget;
    protected List<Card> cardTargets;
    protected Card targetCard;
    protected EventType type;
    protected Ability abilityTrigger;

    #region Properties

    private void SetUIEvent(EventType t)
    {
        if (t == EventType.UIUpdate)
        {
            this.IsUIEvent = true;
        }
    }

    public List<Card> CardTargets
    {
        get { return cardTargets; }
        set { cardTargets = value; }
    }

    public Card TargetCard
    {
        get { return targetCard; }
        set { targetCard = value; }
    }

    public Player PlayerTarget
    {
        get { return playerTarget; }
        set { playerTarget = value; }
    }

    public Card EventOriginCard
    {
        get { return cardSource; }
        set { cardSource = value; }
    }

    public Player EventOwner
    {
        get { return owner; }
        set { owner = value; }
    }

    public MoveAction MoveActionEvent
    {
        get { return moveAction; }
        set { moveAction = value; }
    }

    public NonMoveAction ActionEvent
    {
        get { return notMoveAction; }
        set { notMoveAction = value; }
    }

    public GameEventsArgs InitiateEventDetails
    {
        get;
        set;
    }

    public TurnPhase TurnPhase
    {
        get { return phase; }
        set { phase = value; }
    }

    public Ability TriggeringAbility
    {
        get { return abilityTrigger; }
        set { abilityTrigger = value; }
    }
    public bool IsUIEvent
    {
        get;
        set;

    }

    public EventType Type
    {
        get;
        set;
    }
    #endregion

    #region Constructors
    public GameEventsArgs() 
    {
    }

  
    public GameEventsArgs(List<LocationChanges> boardMovements, Card cardSource, MoveAction moveAction,
                                NonMoveAction notMoveAction, List<Card> cardTargets)
    {
        this.boardMovements = boardMovements;
        this.cardSource = cardSource;
        this.owner = cardSource.getOwner();
        this.moveAction = moveAction;
        this.notMoveAction = notMoveAction;
        this.cardTargets = cardTargets;
        

        Debug.Log("Event Data Created!");

    }

    /// <summary>
    /// This game event constructor is for movement based actions
    /// </summary>
    /// <param name="boardMovements"></param>
    /// <param name="moveAction"></param>
    public GameEventsArgs(List<LocationChanges> boardMovements, MoveAction moveAction)
    {
        this.boardMovements = boardMovements;
        this.cardSource = null;
        this.owner = boardMovements[0].destination.Owner;
        this.moveAction = moveAction;
        this.notMoveAction = NonMoveAction.None;
        
        List<Card> cardsMoved = new List<Card>();
        foreach (LocationChanges l in boardMovements)
        {
            cardsMoved.Add(l.c);
        }
        this.cardTargets = cardsMoved;
        this.playerTarget = cardsMoved[0].getOwner();

        Debug.Log("Event Data Created!");
    }

    /// <summary>
    /// This game event constructor is for movement based actions specifically those initiated by the Game
    /// and not the Player 
    /// </summary>
    /// <param name="boardMovements"></param>
    /// <param name="moveAction"></param>
    public GameEventsArgs(MoveAction moveAction)
    {
        this.cardSource = null;
        this.owner = boardMovements[0].destination.Owner;
        this.moveAction = moveAction;
        this.notMoveAction = NonMoveAction.None;
        
        List<Card> cardsMoved = new List<Card>();
        foreach (LocationChanges l in boardMovements)
        {
            cardsMoved.Add(l.c);
        }
        this.cardTargets = cardsMoved;
        this.playerTarget = cardsMoved[0].getOwner();

        Debug.Log("Event Data Created!");
    }

    /// <summary>
    /// This constructor is for game events that aren't sources from a card, like turn phase changes
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="target"></param>
    /// <param name="nonMoveAction"></param>
    public GameEventsArgs(Player owner, Player target, GameAction gameAction)
    {
        this.owner = owner;
        this.cardSource = null;
        this.boardMovements = null;
        this.playerTarget = target;
        this.notMoveAction = gameAction.NonMoveAction;
        this.phase = gameAction.TurnPhase;
        this.moveAction = gameAction.MoveAction;
        this.cardTargets = null;

    }

    /// <summary>
    /// The constructor to publish Attack declarations
    /// </summary>
    /// <param name="cardSource"></param>
    /// <param name="notMoveAction"></param>
    /// <param name="cardTarget"></param>
    public GameEventsArgs(Card cardSource, NonMoveAction notMoveAction, Card cardTarget)
    {
        this.boardMovements = null;
        this.cardSource = cardSource;
        this.owner = cardSource.getOwner();
        this.moveAction = MoveAction.None;
        this.notMoveAction = notMoveAction;
        this.targetCard = cardTarget;
        this.playerTarget = cardTarget.getOwner();
        

        Debug.Log("Event Data Created!");
    }

    public GameEventsArgs(Player owner, Player target, List<Card> cardTargets, GameAction action, EventType type)
    {
        this.owner = owner;
        this.playerTarget = target;
        this.cardTargets = cardTargets;
        this.moveAction = action.MoveAction;
        this.notMoveAction = action.NonMoveAction;
        this.type = type;
        SetUIEvent(type);
    }

    /// <summary>
    /// For single Card move actions that other Card Abls should know about
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="source"></param>
    /// <param name="destination"></param>
    /// <param name="moveAction"></param>
    /// <param name="target"></param>
    public GameEventsArgs(Card origin, ValidLocations source, ValidLocations destination, MoveAction moveAction, Card target)
    {
        EventOriginCard = origin;
        this.owner = target.Owner;
        this.playerTarget = target.Owner;
        this.TargetCard = target;
        this.moveAction = moveAction;
        if(moveAction == MoveAction.Spawn)
        {
            foreach(var abl in target.Abilities)
            {
                if(abl.Name == "OnSpawn")
                {
                    TriggeringAbility = abl;
                }
            }
        }
        this.notMoveAction = NonMoveAction.None;
        this.Type = EventType.Gameplay;
    }
    #endregion

    public string Print()
    {
        //HelperFunctions.Print("event data print:");
        try
        {
            string data = "CardSource: " + this?.EventOriginCard?.Name + "\n MoveAction: " + this?.moveAction.ToString()
            + "\n Non-Move Action: " + this?.notMoveAction.ToString() + "\n Owner: " + this?.EventOriginCard?.Owner.Type
            + "\n Ability Trigger: " + this?.abilityTrigger?.Name;
            HelperFunctions.Print(data);
            return data;
        }
        catch(Exception ex)
        {
            HelperFunctions.CatchException(ex);
            return "Error Printing Event Data";
        }
        
    }

    
}
