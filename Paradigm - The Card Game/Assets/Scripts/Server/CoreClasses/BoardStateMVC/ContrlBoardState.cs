using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContrlBoardState : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void ValidateClientInput(BoardState e)
    {
        if(BoardStateModel.BoardState.CurrentTurnPlayer == e.CurrentTurnPlayer)
        {
            string changedKey = e.GetPlayer(e.CurrentTurnPlayer).FindModifiedKey(BoardStateModel.BoardState, e.CurrentTurnPlayer);
            if(changedKey == "same")
            {
                BoardStateModel.UpdateBoardModel(e);
            }
            else
            {
                
            }
        }
        else
        {
            throw new System.Exception("The turn player IDs dont match, this is very likely a bug. Please fix it");
        }
    }
}
