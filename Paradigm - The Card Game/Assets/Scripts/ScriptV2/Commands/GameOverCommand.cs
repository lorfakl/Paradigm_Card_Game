using UnityEngine;
using System.Collections;

public class GameOverCommand : Command{

    private Player2 looser;

    public GameOverCommand(Player2 looser)
    {
        this.looser = looser;
    }

    public override void StartCommandExecution()
    {
        looser.PArea.Portrait.Explode();
    }
}
