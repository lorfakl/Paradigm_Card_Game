using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalGameConfiguration : MonoBehaviour
{
    public static int BarrierCount { get; private set; }
    public static int barriers = 12;

    public static int TimeOnTimerInSeconds
    {
        get { return timeOnTimerInSeconds; }
    }

    private static int timeOnTimerInSeconds = 45;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
