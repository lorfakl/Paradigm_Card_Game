using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class GlobalCardTransit
{
    private static List<Card> cardTransit = new List<Card>();
    private static int value = 0;

    public static Utilities.Tuple getCards()
    {
        Utilities.Tuple combo;
        combo.cardList = cardTransit;
        combo.value = value;
        return combo;
    }

    public static void sendCards(List<Card> l, int selection)
    {
        cardTransit = l;
        value = selection;
    }
    

}
