using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiEvents : GameEventsArgs
{
    public ValidLocations Source { get; private set; }
    public ValidLocations Destination { get; private set; }
    public GameObject ObjectToMove { get; private set; }
 


    /// <summary>
    /// This function is called when we are instanstiating a gameobject from some data and moving that
    /// gameobject somewhere in the scene
    /// </summary>
    /// <param name="source"></param>
    /// <param name="destination"></param>
    /// <param name="changes"></param>
    /// <param name="moveAction"></param>
    public UiEvents(ValidLocations source, ValidLocations destination, MoveAction moveAction, Card c)
    {
        Source = source;
        Destination = destination;
        IsUIEvent = true;
        Source = source;
        Destination = destination;
        IsUIEvent = true;
        this.cardSource = c;
        this.owner = c.Owner;
        this.moveAction = moveAction;
        this.notMoveAction = NonMoveAction.None;
        this.type = EventType.UIUpdate;
        //Utilities.HelperFunctions.Print("UI Event Data Created!");
        //Utilities.HelperFunctions.Print("Moved a card from " + source + " to " + destination);

    }

    /// <summary>
    /// This constructor is called when we want to move a gameobject that already exists in the scene some
    /// where else in the scene
    /// </summary>
    /// <param name="source"></param>
    /// <param name="destination"></param>
    /// <param name="go"></param>
    /// <param name="changes"></param>
    /// <param name="moveAction"></param>
    public UiEvents(ValidLocations source, ValidLocations destination, GameObject go, MoveAction moveAction, Card c) : base(moveAction)
    {
        Source = source;
        Destination = destination;
        ObjectToMove = go;
        IsUIEvent = true;
        Source = source;
        Destination = destination;
        IsUIEvent = true;
        this.cardSource = c;
        this.moveAction = moveAction;
        this.notMoveAction = NonMoveAction.None;
        this.type = EventType.UIUpdate;
        //Utilities.HelperFunctions.Print("UI Event Data Related to moving a new GameObject was Created!");

    }

    public UiEvents(Player owner, Player target, GameAction gameAction) : base(owner, target, gameAction)
    {
        IsUIEvent = true;
    }

    public new void Print()
    {
        string data = "Source " + Source.ToString() + "\n Destination: " + Destination
            + "\n cardSource: " + cardSource.Name + "\n MoveAction: " + moveAction.ToString()
            + "\n Non-Move Action: " + notMoveAction.ToString();
        Utilities.HelperFunctions.Print(data);
    }
}
