using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUI : MonoBehaviour
{
    //public TargetingOptions Targets = TargetingOptions.AllCharacters;
   
    [SerializeField]
    private SpriteRenderer sr;
    [SerializeField]
    private LineRenderer lr;
    [SerializeField]
    private GameObject icon;

    private GameObject c;

    private static bool isEnabled = false;


    #region Unity Callbacks
    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        lr = GetComponentInChildren<LineRenderer>();
        //lr.sortingLayerName = "AboveEverything";
        
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    private void OnMouseDown()
    {
        print("battleUI click");
        Ray rayLine = Camera.main.ScreenPointToRay(Input.mousePosition); //cast a ray from the camera to the mouse position
        RaycastHit objectHit; //gameobject the ray hit
        
        if (Physics.Raycast(rayLine, out objectHit)) //if the ray hits a collider
        {
            if (objectHit.transform.GetComponent<CardScript>() != null) //check if the object hit contains a CardScript component
            {
                objectHit.transform.GetComponent<CardScript>().Print();
                print("We clicked this from battleUI");
            }
        }
    }

    private void OnMouseDrag()
    {
        
    }
    #endregion

    public static bool EnableBattleUI()
    {
        isEnabled = true;
        return isEnabled;
    }

    public void OnStartDrag()
    {
        sr.enabled = true;
        lr.enabled = true;
    }

    public void OnDraggingInUpdate()
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

    public void OnEndDrag()
    {
        // return target and arrow to original position
        // this position is special for spell cards to show the arrow on top
        //transform.localPosition = new Vector3(0f, 0f, 0.1f);
        //sr.enabled = false;
        //lr.enabled = false;
        //triangleSR.enabled = false;

    }

}
