using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HelperFunctions;

public interface IPlayable
{
    PlayerInteraction GetInteraction();
    bool GetPlayerUIStatus();

    IEnumerator ChooseTerritoryChallengeCard(Location t);
    IEnumerator ChooseBarriers(int barrierCount);

    IEnumerator PerformGather();
    IEnumerator PerformAwaken();
    IEnumerator PerformCentral();
    IEnumerator PerformCrystal();
    IEnumerator PerformEnd();
    IEnumerator ChooseAttackersAndTargets();
    IEnumerator ChooseBlockers(List<ActionInfo> apCombatTicket);
    void PlayCard();

}
