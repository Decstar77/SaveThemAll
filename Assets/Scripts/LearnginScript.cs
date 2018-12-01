using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LearnginScript : MonoBehaviour {


	Vector3 vec;
	Vector3 vec1;
	Vector3 vec2;
	[SerializeField] GameObject bounds;
	void Start () {
		Vector3 temp = new Vector3(5, 0, 0);
		vec = temp;
	}
	
	void Update () {

		BoxCollider box = GetComponent<BoxCollider>();
		print(box.bounds.size.z);
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Vector3 temp = (Vector3.right);
		Ray ray = new Ray(transform.position, temp);
		Gizmos.DrawRay(ray);
		RaycastHit hit;
		MeshCollider meshCollider = bounds.GetComponent<MeshCollider>();
		bool test = meshCollider.Raycast(ray, out hit, 1f);
		if (test)
		{
			vec = Vector3.zero;
		}
	}


}
