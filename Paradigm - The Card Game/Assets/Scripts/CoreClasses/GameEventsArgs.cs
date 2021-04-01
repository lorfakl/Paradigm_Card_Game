﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum MoveAction
{
    Break, Build, Collect, Crystallize, Delete, Despawn, Draw, Lock, Rest, Return, Search, Spawn, Unlock, None
}

public enum NonMoveAction
{
    Attack, Activate, Battle, Block, Damage, Forge, Heal, Initiate, Respond, TurnPhase, DimensionTwist, None, GameEnd
}

public enum EventType
{
    Gameplay, LegalCheck, UIUpdate
}

public struct GameAction
{
    MoveAction MoveAction { get; set; }
    NonMoveAction NonMoveAction { get; set; }
    public GameAction(MoveAction ma, NonMoveAction nma)
    {
        MoveAction = ma;
        NonMoveAction = nma;
    }

    
}

public class GameEventsArgs : EventArgs
{
    protected Player owner;
    protected Card cardSource;
    protected Turn turn;
    protected List<LocationChanges> boardMovements;
    protected MoveAction moveAction;
    protected NonMoveAction notMoveAction;
    protected Player playerTarget;
    protected List<Card> cardTargets;
    protected Card targetCard;
    protected EventType type;

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
        this.turn = this.owner.PlayerTurn;

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
        this.turn = this.owner.PlayerTurn;
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
        this.turn = this.owner.PlayerTurn;
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
    public GameEventsArgs(Player owner, Player target, NonMoveAction nonMoveAction)
    {
        this.owner = owner;
        this.cardSource = null;
        this.boardMovements = null;
        this.playerTarget = target;
        this.notMoveAction = nonMoveAction;
        this.moveAction = MoveAction.None;
        this.turn = owner.PlayerTurn;
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
        this.turn = this.owner.PlayerTurn;

        Debug.Log("Event Data Created!");
    }

    public void Print()
    {
        string data = "CardSource: " + cardSource.Name + "\n MoveAction: " + moveAction.ToString()
            + "\n Non-Move Action: " + notMoveAction.ToString() + "\n Owner: " + cardSource.Owner.Type;
        Utilities.HelperFunctions.Print(data);
    }

    public List<Card> CardTargets
    {
        get { return cardTargets; }
    }

    public Card TargetCard
    {
        get { return targetCard; }
    }

    public Player PlayerTarget
    {
        get { return playerTarget; }
    }

    public Card EventOriginCard
    {
        get { return cardSource; }
    }

    public Player EventOwner
    {
        get { return owner; }
    }

    public List<LocationChanges> GameBoardMovements
    {
        get { return boardMovements; }
    }

    public MoveAction MoveActionEvent
    {
        get { return moveAction; }
    }

    public NonMoveAction ActionEvent
    {
        get { return notMoveAction; }
    }

    public bool IsUIEvent
    {
        get;
        protected set;

    }

}
