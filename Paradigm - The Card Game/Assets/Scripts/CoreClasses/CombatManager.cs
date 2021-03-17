using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CombatManager : MonoBehaviour
{
    /*//public GameObject uiObject;
    private EventManager evntMngr;
    private Player ap;
    private Player vp;
    private bool cancelCombat = false;
    private bool hasCombatStarted = false;

    private bool madeBarriersTargets = false;
    private static bool updatedAPCombatTicket = false;
    private static bool updatedVPCombatTicket = false;
    private static List<ActionInfo> aggressorCombatTicket;
    private static List<ActionInfo> victimCombatTicket;

    private void Awake()
    {
        evntMngr = GameObject.FindGameObjectWithTag("GameManager").GetComponent<EventManager>();
    }
    // Start is called before the first frame update
    void Start()
    {
        if(!madeBarriersTargets) //This allows Barriers to be targeted in Combat
        {
            foreach(Card c in ap.GetLocation(ValidLocations.BZ))
            {
                c.AddDecoration(new CombatDecoration(), Decorations.Combat);
            }

            foreach (Card c in vp.GetLocation(ValidLocations.BZ))
            {
                c.AddDecoration(new CombatDecoration(), Decorations.Combat);
            }

            madeBarriersTargets = true;
        }
        
    }

    // Update is called once per frame
    void Update()
    {

        StartCombatPrompt();
          
        

        if (updatedVPCombatTicket && updatedAPCombatTicket)
        {
            updatedAPCombatTicket = false;
            updatedVPCombatTicket = false;
            ResolveCombatTickets();
        }
    }

    public void ModifyAction()
    {
        throw new System.NotImplementedException();
    }

    public static void UpdateCombatTicket(List<ActionInfo> combatTicket, bool isAggressor)
    {
        if (isAggressor)
        {
            aggressorCombatTicket = combatTicket;
            updatedAPCombatTicket = true;
            
        }
        else
        {
            victimCombatTicket = combatTicket;
            updatedVPCombatTicket = true;
        }
    }

    private void InitializePlayers(Player p1)
    {
        ap = p1;

        if(p1 == evntMngr.UIPlayer)
        {
            vp = evntMngr.NonUIPlayer;
        }
        else
        {
            vp = evntMngr.UIPlayer;
        }
    }

    private void StartCombatPrompt()
    {
        if (!hasCombatStarted)
        {
            hasCombatStarted = true;
            StartCoroutine(ap.ChooseAttackersAndTargets());
        }
        else if(aggressorCombatTicket != null)
        {
            StartCoroutine(vp.ChooseBlockers(aggressorCombatTicket));
        }
    }

    /// <summary>
    /// This function takes in the 2 combat tickets. Checks the victim ticket for blocks. If the victim is blocking an attack
    /// a new ActionInfo is created reflecting that. Once all the blocks have been checked any unblocked attacks are added to the 
    /// combined ticket
    /// </summary>
    private void ResolveCombatTickets()// like will have an issue in which all attacks will not be 
    {
        List<ActionInfo> combinedCombatTicket = new List<ActionInfo>();
        List<int> indicesForRemoval = new List<int>();
        if (victimCombatTicket.Count > 0)
        {
            foreach(ActionInfo action in victimCombatTicket)
            {
                foreach(ActionInfo aggAction in aggressorCombatTicket)
                {
                    action.Print();
                    aggAction.Print();
                    if (action.acted == aggAction.actor) //if the aggresive actor is being blocked
                    {
                        ActionInfo combinedAction = new ActionInfo { actor = aggAction.actor, acted = action.actor, act = NonMoveAction.Damage };
                        print("Combined Action");
                        combinedAction.Print();
                        combinedCombatTicket.Add(combinedAction);
                        indicesForRemoval.Add(aggressorCombatTicket.IndexOf(aggAction));
                    }
                }
            }

            foreach(int i in indicesForRemoval) //remove blocked attacks
            {
                aggressorCombatTicket.RemoveAt(i);
            }

            combinedCombatTicket.AddRange(aggressorCombatTicket); //combine unblocked attacks with combined

            ApplyCombatDamage(combinedCombatTicket);
        }

    }

    private void ApplyCombatDamage(List<ActionInfo> combinedCombatTicket)
    {
        foreach(ActionInfo damageInfo in combinedCombatTicket) //goes through combined ticket and applies to damage 
        {
            ((Accessor)damageInfo.acted).DecreaseHealth(((Accessor)damageInfo.actor).Power);
        }

        print("The job is done");
        Destroy(gameObject); 
    }*/
}
