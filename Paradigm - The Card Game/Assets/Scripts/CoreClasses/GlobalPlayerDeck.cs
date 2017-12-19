using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalPlayerDeck
{ 

    private static Deck playerDeck = new Deck();
    
    public static void setPlayerDeck(Deck d)
    {
        playerDeck = d;
    }

    public static Deck getPlayerDeck()
    {
        return playerDeck;
    }

    private static Deck aiPlayerDeck = new Deck();

    public static void setAIPlayerDeck(Deck d)
    {
        aiPlayerDeck = d;
    }

    public static Deck getAIPlayerDeck()
    {
        return aiPlayerDeck;
    }

}
