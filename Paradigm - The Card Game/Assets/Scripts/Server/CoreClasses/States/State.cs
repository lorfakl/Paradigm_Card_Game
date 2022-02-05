using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    public IPlayable Player1 { get; protected set; }
    public IPlayable Player2 { get; protected set; }
    public Player Owner { get; protected set; }
}
