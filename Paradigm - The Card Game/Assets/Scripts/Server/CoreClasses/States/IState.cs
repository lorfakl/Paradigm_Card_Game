using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public interface IState 
{
    void SetOwner(Player p);
    bool IsUniqueToPlayer();
    void OnEntry();

    Task Operation();

    void OnExit();


  
}
