using UnityEngine;
using System.Collections;

public class PerformTerritoryChallengeCommand : Command {

    private IPlayable p;
    private Location temp;
   

    public PerformTerritoryChallengeCommand(IPlayable p, Location temp)
    {        
        this.p = p;
        this.temp = temp;
    }

    public override void StartCommandExecution()
    {
        p.ChooseTerritoryChallengeCard(temp);
    }
}
