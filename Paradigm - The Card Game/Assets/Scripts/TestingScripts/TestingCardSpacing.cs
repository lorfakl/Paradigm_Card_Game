using DataBase;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestingCardSpacing : MonoBehaviour
{

    public Button drawButton;
    public Button arrangeButton;
    public GameObject prefab;
    public UIManager UIManager;

    public static Player PlayerOne { get; private set; }
    public static Player Player2{ get; private set; }

    //private Vector3 midPoint;
    // Start is called before the first frame update
    void Start()
    {
        PlayerOne = new HumanPlayer(Guid.NewGuid());
        Player2 = new AIPlayer(Guid.NewGuid());
        UIManager = gameObject.GetComponent<UIManager>();

        CardDataBase.MakePlayerDeck(PlayerOne);
        CardDataBase.MakePlayerDeck(Player2);

        if(Player2.PlayerDeck.GetMajesty() == PlayerOne.PlayerDeck.GetMajesty())
        {
            throw new Exception("It didnt take apparently");
        }

        PlayerOne.PlayerDeck.GameStartSetup();
        Player2.PlayerDeck.GameStartSetup();
        //PlayerOne.PlayerDeck.Shuffle();
        drawButton.onClick.AddListener(StupidButton);
        arrangeButton.onClick.AddListener(AttackBtn);
        //arrangeButton.onClick.AddListener(UIManager.Rearrange);
        print("Mid point fomula");
        

       
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void AttackBtn()
    {
        StartCoroutine(PlayerOne.PerformAttack());
    }

    private void StupidButton()
    {
        print("This work?");
        if (PlayerOne == null)
        {
            throw new Exception("this is wrong");
        }
        StartCoroutine(PlayerOne.PerformGather());
        //print(PlayerOne.PlayerDeck.Count);
    }
}
