using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneFeedBack : MonoBehaviour {

	[SerializeField] float step = 1;
	[SerializeField] float CutOffPointScale = 1;
	float sStep;
	void Start () {
		sStep = 0;
	}
	
	void Update () {
		sStep += step;
		//transform.localScale = Vector3.one * (Amplitude * Mathf.Sin(peroid * sStep * Mathf.Deg2Rad) + offSet);
		transform.localScale = Vector3.one * Mathf.Exp(sStep);
		if (transform.localScale.x >= CutOffPointScale)
		{
			print("Destroyingh");
			Destroy(gameObject);
		}
	}

}
