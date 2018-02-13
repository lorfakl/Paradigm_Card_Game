using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataBase;
using Utilities;


public static class MakeTestPlayer
{

    private static Player p1;
    private static Player p2;

    public static Player P1
    {
        get { return p1; }
    }

    public static Player P2
    {
        get { return p2; }
    }

    public static Player MakePlayer()
    {
        p1 = new Player();
        p2 = new Player();
        return p1;
    }
}
