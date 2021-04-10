using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


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
            
    }

    public Accessor(Card c):base(c)
    {

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

    public int NumOfAttacks
    {
        get { return numOfAttacks; }
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

    protected void Attack()
    {

    }
    
    protected void Block()
    {

    }
}

