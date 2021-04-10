using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTarget : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer sr;
    [SerializeField]
    private LineRenderer lr;
    private GameObject c;
    private Card card;
    private bool isBeingDragged = false;
    private float zDisplacement;
    private Vector3 pointerDisplacement = Vector3.zero;
    private Vector3 originalPosition;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        lr = GetComponentInChildren<LineRenderer>();
        //lr.sortingLayerName = "AboveEverything";
        c = gameObject.transform.parent.gameObject;
        card = c.GetComponent<CardScript>().Card;
        originalPosition = transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isBeingDragged)
        {
            Vector3 mousePos = MouseInWorldCoords();
            OnDraggingInUpdate();
            transform.position = new Vector3(mousePos.x, mousePos.y);
            //transform.position = mousePos;
            //transform.position = new Vector3(mousePos.x - pointerDisplacement.x, mousePos.y - pointerDisplacement.y, transform.position.z);
        }

        if(Input.GetMouseButtonDown(1))
        {
            isBeingDragged = false;
            transform.position = originalPosition;
            lr.enabled = false;
            gameObject.layer = LayerMask.NameToLayer("Default");
        }

        if(Input.GetMouseButtonDown(0))
        {
            if (isBeingDragged)
            {
                Ray rayLine = Camera.main.ScreenPointToRay(Input.mousePosition); //cast a ray from the camera to the mouse position
                RaycastHit objectHit; //gameobject the ray hit
                if (Physics.Raycast(rayLine, out objectHit)) //if the ray hits a collider
                {
                    print(objectHit.transform.gameObject.name);
                    if (objectHit.transform.gameObject.tag == "card")
                    {
                        print("attacking a card");
                        objectHit.transform.GetComponent<CardScript>().Print();
                        
                        Utilities.HelperFunctions.RaiseNewEvent(this, card, NonMoveAction.DeclaredAttack, objectHit.transform.GetComponent<CardScript>().Card);
                        isBeingDragged = false;
                    }
                }
            }
        }
    }

    private void OnMouseUpAsButton()
    {
        print("Hit the icon");
        sr.enabled = true;
        lr.enabled = true;
        isBeingDragged = true;
        //gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        //gameObject.transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
    }
   
    
    

    private void OnDraggingInUpdate()
    {
        // This code only draws the arrow
        Vector3 notNormalized = transform.position - c.transform.position;
        Debug.DrawLine(notNormalized, c.transform.position, Color.black, 2, false);
        Vector3 direction = notNormalized.normalized;
        Debug.DrawLine(direction, c.transform.position, Color.green, 2, false);
        float distanceToTarget = (direction * 2.3f).magnitude;
        if (notNormalized.magnitude > distanceToTarget)
        {
            // draw a line between the creature and the target
            lr.SetPositions(new Vector3[] { transform.parent.position, transform.position - direction * 2.3f });
            lr.enabled = true;

            // position the end of the arrow between near the target.
            //triangleSR.enabled = true;
            //triangleSR.transform.position = transform.position - 1.5f * direction;

            // proper rotarion of arrow end
            //float rot_z = Mathf.Atan2(notNormalized.y, notNormalized.x) * Mathf.Rad2Deg;
            //triangleSR.transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
        }
        else
        {
            // if the target is not far enough from creature, do not show the arrow
            lr.enabled = false;
            //triangleSR.enabled = false;
        }

    }

    private Vector3 MouseInWorldCoords()
    {
        var screenMousePos = Input.mousePosition;
        //Debug.Log(screenMousePos);
        screenMousePos.z = zDisplacement;
        return Camera.main.ScreenToWorldPoint(screenMousePos);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<CardScript>() != null && other.gameObject != gameObject)
        {
            print("This is a different card");
        }
    }
}
