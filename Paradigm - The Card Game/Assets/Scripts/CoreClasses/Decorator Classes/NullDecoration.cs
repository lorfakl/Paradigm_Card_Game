using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NullDecoration : CardDecoration
{
    public NullDecoration()
    {
        this.type = Decorations.Null;
    }

    public override void PerformAction()
    {
        throw new System.NotImplementedException();
    }

}
