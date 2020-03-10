using UnityEngine;
using System.Collections;

public abstract class TurnMaker : MonoBehaviour {

    protected Player2 p;

    void Awake()
    {
        p = GetComponent<Player2>();
    }

    public virtual void OnTurnStart()
    {
        // add one mana crystal to the pool;
        p.OnTurnStart();
    }

}
