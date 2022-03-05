using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using DataBase;

public class GeneralTesting : MonoBehaviour
{
    public static Player PlayerOne { get; private set; }

    [SerializeField]
    private MultiCardTransferSO transferCardsToDisplay;

    List<CardSO> cardsToDisplay = new List<CardSO>();

    bool hasSentInfo = false;

    // Start is called before the first frame update
    private void Awake()
    {
        CreateLocalDeck();
    }

    void Start()
    {
        foreach(Card c in PlayerOne.PlayerDeck)
        {
            CardSO cSO = (CardSO)ScriptableObject.CreateInstance(typeof(CardSO));
            cSO.Init(new ClientCardInfo {ID = c.ID, InstanceID = c.InstanceID.ToString() });
            cardsToDisplay.Add(cSO);
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        SendData();
    }

    private void CreateLocalDeck()
    {
        PlayerOne = new HumanPlayer(Guid.NewGuid());
        CardDataBase.MakePlayerDeck(PlayerOne);
        PlayerOne.PlayerDeck.GameStartSetup();
    }

    private void SendData()
    {
        if(!hasSentInfo)
        {
            transferCardsToDisplay.dataTransferReadyEvent.Invoke(cardsToDisplay);
            hasSentInfo = true;
        }
    }
}
