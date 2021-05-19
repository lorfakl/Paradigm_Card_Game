using UnityEngine;
using System.Collections;

public class PlayerTurnMaker : TurnMaker 
{
    public override void OnTurnStart()
    {
        base.OnTurnStart();
        // dispay a message that it is Player2`s turn
        new ShowMessageCommand("Your Turn!", 2.0f).AddToQueue();
        p.DrawACard();
    }
}
