using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodEffect : MonoBehaviour {


public GameObject bloodParticles;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

void Die () {
Instantiate(bloodParticles, new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.2f), transform.rotation);
}

}
