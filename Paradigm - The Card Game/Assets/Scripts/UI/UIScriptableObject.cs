using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New UI Display Object", menuName = "UI Configuration Object")]
public class UIScriptableObject : ScriptableObject
{
    [SerializeField] private GameObject UIPlayerEntryPoint;
    public Transform UiEntryPoint { get { return UIPlayerEntryPoint.transform; } }

    [SerializeField] private GameObject NonUIPlayerEntryPoint;
    public Transform NonUiEntryPoint { get { return NonUIPlayerEntryPoint.transform; } }
    // Start is called before the first frame update



}
