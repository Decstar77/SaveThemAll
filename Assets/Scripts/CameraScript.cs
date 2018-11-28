using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

	[SerializeField] float stop1;
	[SerializeField] float stop2;
	[SerializeField] float acceleration = 0.1f;
	[SerializeField] float cutOffPoint = 0.2f;
	[SerializeField] float friction = 0.75f;
	Vector3 touchStart;
	Vector3 touchEnd;
	Vector3 velocity;
	void Start () {
	}

	void Update()
	{
		Vector3 dir = Vector3.zero;
		if (Input.GetMouseButtonDown(0))
		{
			touchStart = GetWorldPositionOnPlane(Input.mousePosition, 0);
		}
		if (Input.GetMouseButtonUp(0))
		{
			touchEnd = GetWorldPositionOnPlane(Input.mousePosition, 0);
		}
		if (Input.GetMouseButton(0))
		{
			dir = touchStart - GetWorldPositionOnPlane(Input.mousePosition, 0);
			velocity = dir * acceleration;
		}
		else
		{
			if (velocity.x != 0)
			{
				velocity.x = Mathf.Lerp(velocity.x, 0, (1 / (touchStart - touchEnd).magnitude) * friction);
				if (Mathf.Abs(velocity.x) <= cutOffPoint)
				{
					velocity.x = 0;
				}
			}
		}
		CheckBounds();
		gameObject.transform.position += Vector3.right * velocity.x;			
	}
	/*<Summary>
	Shoots a ray through the world space from the Vec3
	Then creats a Plane with vec3.z = z
	Then returns the distance of which the ray collides with the plane
	<Summary>*/
	public Vector3 GetWorldPositionOnPlane(Vector3 screenPosition, float z)
	{
		Ray ray = Camera.main.ScreenPointToRay(screenPosition);
		Plane xy = new Plane(Vector3.forward, new Vector3(0, 0, z));
		float distance;
		xy.Raycast(ray, out distance);
		return ray.GetPoint(distance);
	}
	public void CheckBounds()
	{
		float x = gameObject.transform.position.x;
		if (x + velocity.x < stop1)
		{
			velocity.x = 0f;
		}
		if (x + velocity.x > stop2)
		{
			velocity.x = 0f;
		}
	}

}
