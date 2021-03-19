using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CardControl : MonoBehaviour
{
    public BoxCollider collider;
    public Vector3 scaleFactor = new Vector3(2, 2, 2);
    public GameObject cardTemplate;

    private Vector3 defaultScale, defaultPosition;
    // Start is called before the first frame update
    void Start()
    {
        if(cardTemplate == null)
        {
            throw new System.Exception("No card template set in Inspector");
        }

        defaultScale = transform.localScale;
        defaultPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseEnter()
    {       
        Vector3 targetValueDoScale = Vector3.Scale(transform.localScale, scaleFactor);
        transform.DOScale(targetValueDoScale, 0.5f);
        transform.DOMove(transform.position + new Vector3(0.0f, 3f, 0.0f), 0.5f);
    }

    private void OnMouseExit()
    {
        Vector3 targetValueDoScale = Vector3.Scale(transform.localScale, Vector3.one);
        transform.DOScale(defaultScale, 0.5f);
        transform.DOMove(defaultPosition, 0.5f);
        
    }
}
