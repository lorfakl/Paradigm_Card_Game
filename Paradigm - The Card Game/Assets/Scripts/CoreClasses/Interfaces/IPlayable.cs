using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    IEnumerator PerformAttack();
    void PlayCard(Card c);

}
