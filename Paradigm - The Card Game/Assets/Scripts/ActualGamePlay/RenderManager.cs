using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RenderManager : MonoBehaviour
{
    private int[] playerInfo = { 0, 0, 0, 0, 0, 0, 0 };
    private string[] playerInfoTags = { "yourDeckCount","yourGraveCount","yourBarrierCount","enemyDeckCount","enemyGraveCount","enemyHandCount","enemyBarrierCount" };
    private string[] playerInfoPreambles = { "Deck: ", "Grave: ", "Barriers: ", "EDeck: ", "EGrave: ", "EHand: ", "EBarriers: " };
    private Text[] playerInfoObjects = new Text[7];
    // Use this for initialization

    private void Awake()
    {
        GameEventsManager.UpdateUI += ChangePlayerInfo;
    }
    void Start ()
    {
        for(int i = 0; i < 7; i++)
        {
            playerInfoObjects[i] = GameObject.FindWithTag(playerInfoTags[i]).GetComponent<Text>();
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        
    }

    private void ChangePlayerInfo(int[] data)
    {
        for(int i = 0; i < 7; i++)
        {
            playerInfoObjects[i].text = playerInfoPreambles[i] + data[i];
        }
    }

}
