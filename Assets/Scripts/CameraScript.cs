using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

	[SerializeField] float acceleration = 0.1f;
	[SerializeField] float cutOffPoint = 0.2f;
	[SerializeField] float friction = 0.75f;
	[SerializeField] float ZoomSpeedMultiplier = 0.5f;
	[SerializeField] float ZoomInc = 0.5f;
	[SerializeField] GameObject CameraBounds;

	BoxCollider cameraBox;
	Vector3 touchStart;
	Vector3 touchEnd;
	Vector3 panningVelocity;
	Vector3 zoomVelocity;
	void Start () {
		cameraBox = CameraBounds.GetComponent<BoxCollider>();
	}

	void Update()
	{
		#region//Panning
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
			panningVelocity = dir * acceleration;
		}
		else
		{
			if (panningVelocity.x != 0)
			{
				panningVelocity.x = Mathf.Lerp(panningVelocity.x, 0, (1 / (touchStart - touchEnd).magnitude) * friction);
				
				if (Mathf.Abs(panningVelocity.x) <= cutOffPoint)
				{
					panningVelocity.x = 0;
				}
			}
			if (panningVelocity.y != 0)
			{
				panningVelocity.y = Mathf.Lerp(panningVelocity.y, 0, (1 / (touchStart - touchEnd).magnitude) * friction);
				if (Mathf.Abs(panningVelocity.y) <= cutOffPoint)
				{
					panningVelocity.y = 0;
				}
			}
		}
		#endregion
		#region//Zooming
		float zoomAmount = Input.GetAxisRaw("Mouse ScrollWheel") * ZoomInc;
		zoomVelocity = Vector3.zero;
		if (zoomAmount != 0) { zoomVelocity = Zoom(Input.mousePosition, zoomAmount) * ZoomSpeedMultiplier; }
		#endregion


		CheckBounds();
		gameObject.transform.Translate(Vector3.right * panningVelocity.x);
		gameObject.transform.Translate(Vector3.up * panningVelocity.y, Space.Self);
		gameObject.transform.position += zoomVelocity;
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
	public Vector3 Zoom(Vector3 pos, float x)
	{
		Ray ray = Camera.main.ScreenPointToRay(pos);
		Vector3 currentpos = gameObject.transform.position;
		Vector3 futurePos = ray.GetPoint(x);
		if (x > 0)
		{

			return futurePos - currentpos;
		}
		return currentpos - futurePos;
		
	}
	public void CheckBounds()
	{
		Vector3 posToCheck = transform.position + panningVelocity;
		Vector3 offset = cameraBox.bounds.center - posToCheck;
		Ray inputRay = new Ray(posToCheck, offset.normalized);
		RaycastHit rHit;
		Debug.DrawRay(posToCheck, offset);

		if (cameraBox.Raycast(inputRay, out rHit, offset.magnitude * 1.1f))
		{
			panningVelocity = Vector3.zero;
		}


		posToCheck = transform.position + zoomVelocity;
		offset = cameraBox.bounds.center - posToCheck;
		inputRay = new Ray(posToCheck, offset.normalized);

		if(cameraBox.Raycast(inputRay, out rHit, offset.magnitude * 1.1f))
		{
			zoomVelocity = Vector3.zero;
		}

	}
}
