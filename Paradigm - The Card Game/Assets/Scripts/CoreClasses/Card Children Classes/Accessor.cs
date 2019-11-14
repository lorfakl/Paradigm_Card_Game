﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using HelperFunctions;


public class Accessor: Card
{
    private int hp;
    private int maxHp;
    private int power;
    private int numOfAttacks = 1;
    private bool elemental = false;
    private List<Card> bonds;

    public Accessor()
    {

    }

    public Accessor(string n, string k, System.Int64 p, System.Int64 h, string t, string a, string a2, string a3)
    {
        this.setName(n);
        this.SetAbilities(a,a2,a3);
        this.SetTraits(t);
        this.SetPower((int)p);
        this.SetMaxHp((int)h);
        this.SetHp((int)h);
        Family fam = new Family(k);
        this.setFam(fam);
        bonds = new List<Card>();
        AddDecoration(new CombatDecoration(), Decorations.Combat);
            
    }

    public int Power
    {
        get { return this.power; }
        set { this.power = value; }
    }

    public int HP
    {
        get { return this.hp; }
        set { this.hp = value; }
    }

    public void SetPower(int p) { power = p; }
    public void SetMaxHp(int h) { maxHp = h; }
    public void SetHp(int h) { hp = h; }
    public void AddAttacks(int a) { numOfAttacks = numOfAttacks + a; }
    public void ReduceAttacks(int a)
    {
        numOfAttacks = numOfAttacks - a;
        if (numOfAttacks < 0)
        {
            numOfAttacks = 0;
        }

    }

    public int GetPower() { return power; }
    public int GetHp() { return hp; }
    public bool GetElementStatus() { return elemental; }

    public int DecreaseHealth(int damage)
    {
        this.HP -= damage;
        Utilities.RaiseNewEvent(this, this, NonMoveAction.Damage, this);
        return HP;
    }

    public override void PlayCard()
    {
        if(this.Owner.PlayerTurn.Phase == TurnPhase.Central)
        {

        }
        else
        {

        }
    }

    public override void UseEffect()
    {
        throw new NotImplementedException();
    }

    protected void Attack()
    {

    }
    
    protected void Block()
    {

    }
}

