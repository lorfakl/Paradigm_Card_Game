using UnityEngine;
using System.Collections;

public class StartATurnCommand : Command {

    private Player2 p;

    public StartATurnCommand(Player2 p)
    {
        this.p = p;
    }

    public override void StartCommandExecution()
    {
        TurnManager.Instance.whoseTurn = p;
        // this command is completed instantly
        CommandExecutionComplete();
    }
}
