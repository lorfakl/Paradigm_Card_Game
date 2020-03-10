using UnityEngine;
using System.Collections;

public class CreatureDieCommand : Command 
{
    private Player2 p;
    private int DeadCreatureID;

    public CreatureDieCommand(int CreatureID, Player2 p)
    {
        this.p = p;
        this.DeadCreatureID = CreatureID;
    }

    public override void StartCommandExecution()
    {
        p.PArea.tableVisual.RemoveCreatureWithID(DeadCreatureID);
    }
}
