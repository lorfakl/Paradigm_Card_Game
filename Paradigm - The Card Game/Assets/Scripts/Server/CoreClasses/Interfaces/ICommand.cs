using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICommand 
{
    IEnumerator Execute(GameEventsArgs g);
}
