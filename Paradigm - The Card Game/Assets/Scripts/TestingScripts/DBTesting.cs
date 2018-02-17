using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataBase;
using Utilities;

public class DBTesting : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        CardDataBase.WriteEncodedAbilities();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
