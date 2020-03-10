using UnityEngine;
using System.Collections;

public class DamageOpponentBattlecry : CreatureEffect
{
    public DamageOpponentBattlecry(Player2 owner, CreatureLogic creature, int specialAmount): base(owner, creature, specialAmount)
    {}

    // BATTLECRY
    public override void WhenACreatureIsPlayed()
    {
        new DealDamageCommand(owner.otherPlayer.PlayerId, specialAmount, owner.otherPlayer.Health - specialAmount).AddToQueue();
        owner.otherPlayer.Health -= specialAmount;
    }
}
