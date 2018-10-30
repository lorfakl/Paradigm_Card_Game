using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayable
{
    IEnumerator ChooseTerritoryChallengeCard(Location t);
    IEnumerator ChooseBarriers(int barrierCount);

    IEnumerator PerformGather();
    IEnumerator PerformAwaken();
    IEnumerator PerformCentral();
    IEnumerator PerformCrystal();
    void PlayCard();

}
