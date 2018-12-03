using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugTraps : MonoBehaviour {

public GameObject electroTrap;
public GameObject bladeTrap;
public GameObject flameThrower;
public GameObject gasTrap;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Instantiate (electroTrap, new Vector3(transform.position.x, transform.position.y, -0.5f), transform.rotation);
			Debug.Log("Electro trap!");
        }
				        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Instantiate (bladeTrap, new Vector3(transform.position.x, transform.position.y, -0.5f), transform.rotation);
			Debug.Log("Blade trap!");
        }
	}
}
