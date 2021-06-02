using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New UI Display Object", menuName = "UI Configuration Object")]
public class UIScriptableObject : ScriptableObject
{
    [SerializeField] private GameObject UIPlayerEntryPoint;
    public GameObject UiEntryPoint { get { return UIPlayerEntryPoint; } }

    [SerializeField] private GameObject NonUIPlayerEntryPoint;
    public Transform NonUiEntryPoint { get { return NonUIPlayerEntryPoint.transform; } }

    [SerializeField] private Vector3 cardScale;
    public Vector3 Scale { get { return cardScale; } }

    [SerializeField] private float moveSpeed;
    public float MoveSpeed { get { return moveSpeed; } }




}
