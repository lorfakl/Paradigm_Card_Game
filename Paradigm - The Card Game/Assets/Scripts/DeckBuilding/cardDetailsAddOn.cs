using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cardDetailsAddOn : MonoBehaviour {

    public Card expandedCard;
    public bool selected = false;
    public void setExpandedCard(Card c)
    {
        expandedCard = c;
    }

    public Card getExpandedCard()
    {
        return expandedCard;
    }
}
