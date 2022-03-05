using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;
using DataBase;
using System.Linq;

[CreateAssetMenu(fileName = "NewCard", menuName = "Card")]
public class CardSO : ScriptableObject
{

    #region Private Fields
    [SerializeField] private string cardName;
    [SerializeField] private int id;
    [SerializeField] private string instanceId;
    [SerializeField] private List<string> abilities = new List<string>();
    [SerializeField] private GameObject g;
    [SerializeField] private string currentLocation;
    [SerializeField] private string classType;
    [SerializeField] private string fam;
    [SerializeField] private Sprite cardArt;
    #endregion

    #region Properties

    //Properties
    public int ID
    {
        get { return this.id; }
        set { this.id = value; }
    }

    public string Name
    {
        get { return this.cardName; }
        set { this.cardName = value; }
    }

    public List<string> Abilities
    {
        get { return this.abilities; }
    }

    public string CurrentLocation
    {
        get { return currentLocation; }
        set { currentLocation = value; }
    }

    public GameObject GameObj
    {
        get { return g; }
        set { g = value; }
    }

    public string InstanceId
    {
        get { return instanceId; }

    }

    public string CardType
    {
        get { return classType; }

    }

    public string Fam
    {
        get { return fam; }

    }

    public Sprite CardArt
    {
        set { cardArt = value; }
    }

    #endregion

    public void Init(ClientCardInfo c)
    {
        Dictionary<string, string> cardInfo = CardDataBase.GetClientSafeCardInfo(c.ID);
        this.ID = c.ID;
        this.instanceId = c.InstanceID;
        this.cardName = cardInfo["Name"];
        this.classType = cardInfo["Type"];
        this.fam = cardInfo["Fam"];

        var ablKeys = cardInfo.Keys.ToList().Where(key => key.Contains("Abl")).ToList();

        foreach (string key in ablKeys)
        {
            this.abilities.Add(cardInfo[key]);
        }
    }
}

   

