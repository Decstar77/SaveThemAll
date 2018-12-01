using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilitFunctions  {

	public static Vector3 GetWorldPositionOnPlane(Vector3 screenPosition, float z)
	{
		Ray ray = Camera.main.ScreenPointToRay(screenPosition);
		Plane xy = new Plane(Vector3.forward, new Vector3(0, 0, z));
		float distance;
		xy.Raycast(ray, out distance);
		return ray.GetPoint(distance);
	}
	public static GameObject GetObjectRayIntersected(Ray ray, string tag = "")
	{
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit))
		{
			if (tag == "")
				return hit.transform.gameObject;
			if (hit.transform.tag == tag)
			{
				return hit.transform.gameObject;
			}
		}
		return null;
	}
}
