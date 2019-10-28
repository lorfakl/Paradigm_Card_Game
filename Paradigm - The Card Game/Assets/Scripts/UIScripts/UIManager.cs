using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public Text deckCount;
    public Text graveCount;
    public Text barrierCount;
    public Text nonUIDeckCount;
    public Text nonUIGraveCount;
    public Text nonUIHandCount;
    public Text nonUIBarrierCount;
    public GameObject gm;
    private bool isDictPrepared = false;
    private Dictionary<Location, Text> uiDict = new Dictionary<Location, Text>();
    static private Player ai;
    static private Player human;

    // Start is called before the first frame update
    void Start()
    {
        ai = gm.GetComponent<EventManager>().NonUIPlayer;
        human = gm.GetComponent<EventManager>().UIPlayer;
        PrepareDict(ai, human);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void DisplayNewUI(Location l)
    {
        if(!isDictPrepared)
        {
            PrepareDict(ai, human);
        }

        uiDict[l].text = l.Count.ToString();
    }

    private void PrepareDict(Player aip, Player man)
    {
        aip.GetLocation(ValidLocations.BZ).locationChangesEvent += DisplayNewUI;
        uiDict.Add(aip.GetLocation(ValidLocations.BZ), nonUIBarrierCount);

        aip.GetLocation(ValidLocations.Grave).locationChangesEvent += DisplayNewUI;
        uiDict.Add(aip.GetLocation(ValidLocations.Grave), nonUIGraveCount);

        aip.GetLocation(ValidLocations.Deck).locationChangesEvent += DisplayNewUI;
        uiDict.Add(aip.GetLocation(ValidLocations.Deck), nonUIDeckCount);

        aip.GetLocation(ValidLocations.Hand).locationChangesEvent += DisplayNewUI;
        uiDict.Add(aip.GetLocation(ValidLocations.Hand), nonUIHandCount);

        man.GetLocation(ValidLocations.BZ).locationChangesEvent += DisplayNewUI;
        uiDict.Add(man.GetLocation(ValidLocations.BZ), barrierCount);

        man.GetLocation(ValidLocations.Grave).locationChangesEvent += DisplayNewUI;
        uiDict.Add(man.GetLocation(ValidLocations.Grave), graveCount);

        man.GetLocation(ValidLocations.Deck).locationChangesEvent += DisplayNewUI;
        uiDict.Add(man.GetLocation(ValidLocations.Deck), deckCount);
        isDictPrepared = true;
    }
}
