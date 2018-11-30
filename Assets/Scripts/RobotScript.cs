using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotScript : MonoBehaviour {

	void Start () {
		
	}
	
	void Update () {
		transform.position += Vector3.right * Time.deltaTime;
	}
}
