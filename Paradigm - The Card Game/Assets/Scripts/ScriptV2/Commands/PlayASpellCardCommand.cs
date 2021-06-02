using UnityEngine;
using System.Collections;

public class PlayASpellCardCommand: Command
{
    private CardLogic card;
    private Player2 p;
    //private ICharacter target;

    public PlayASpellCardCommand(Player2 p, CardLogic card)
    {
        this.card = card;
        this.p = p;
    }

    public override void StartCommandExecution()
    {
        // move this card to the spot
        p.PArea.handVisual.PlayASpellFromHand(card.UniqueCardID);
        // do all the visual stuff (for each spell separately????)
    }
}
