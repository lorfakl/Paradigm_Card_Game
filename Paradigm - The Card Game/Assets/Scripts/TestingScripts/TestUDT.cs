using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TopLevelUIEvents;
using System;
using Utilities;

public class TestUDT : MonoBehaviour
{
    [SerializeField]
    private Button _generateNewBoardState;

    public delegate void playableCardsUpdate(object sender, PlayableCardEventArgs playableCardEvent);
    public static event playableCardsUpdate PlayableCardsEvent;
    //the PossiblePlaysIndicator subscribes to this handler and triggers the sprite Renderers on these objects

    public delegate void locationCountUpdate(object sender, LocationCountEventArgs locationCountEvent);
    public static event locationCountUpdate LocationCountEvent;
    //HUDGameInfo subscribes to this handler and updates the text components of Location UI

    public delegate void landscapeUpdate(object sender, LandscapeUpdateEventArgs locationCountEvent);
    public static event landscapeUpdate LandscapeUpdateEvent;
    //HUDGameInfo subscribes to this handler and updates the text components of Location UI

    public delegate void philosopherUpdate(object sender, PhilosopherUpdateEventArgs locationCountEvent);
    public static event philosopherUpdate PhilosopherUpdateEvent;
    //HUDGameInfo subscribes to this handler and updates the text components of Location UI
    private void Awake()
    {
        _generateNewBoardState.onClick.AddListener(GenerateNewBoardState);
    }


    private void ConfigureGameStart()
    {
        (Player Human, Player AI) testPlayers = HelperFunctions.PerformGameSetupTasks();

    }
    private void GenerateNewBoardState()
    {
        throw new NotImplementedException();
    }
}
