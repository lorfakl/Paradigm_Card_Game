using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CardRotation : MonoBehaviour
{
    // parent game object for all the card face graphics
    public RectTransform cardFront;
    // parent game object for all the card back graphics
    public RectTransform cardBack;
    // an empty game object that is placed a bit above the face of the card, in the center of the card
    public Transform targetFacePoint;
    // 3d collider attached to the card (2d colliders like BoxCollider2D won`t work in this case)
    public Collider col;

    private bool isBackShowing = false;
    
    // Update is called once per frame
    void Update()
    {
        // Raycast from Camera to a target point on the face of the card
        // If it passes through the card`s collider, we should show the back of the card
        RaycastHit[] hits;
        hits = Physics.RaycastAll(origin: Camera.main.transform.position,
                                  direction: (-Camera.main.transform.position + targetFacePoint.position).normalized,
            maxDistance: (-Camera.main.transform.position + targetFacePoint.position).magnitude);
        bool passedThroughColliderOnCard = false;
        foreach (RaycastHit h in hits)
        {
            if (h.collider == col)
                passedThroughColliderOnCard = true;
        }
        //Debug.Log("TotalHits: " + hits.Length); 
        if (passedThroughColliderOnCard != isBackShowing)
        {
            // something changed
            isBackShowing = passedThroughColliderOnCard;
            if (isBackShowing)
            {
                // show the back side
                cardFront.gameObject.SetActive(false);
                cardBack.gameObject.SetActive(true);
            }
            else
            {
                // show the front side
                cardFront.gameObject.SetActive(true);
                cardBack.gameObject.SetActive(false);
            }

        }
    }
}
