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

	MeshCollider cameraColliderBounds;
	Vector3 touchStart;
	Vector3 touchEnd;
	Vector3 panningVelocity;
	Vector3 zoomVelocity;
	Vector3 startPos;
	Vector3 backScrollDirection;
	Vector3 backScrollStopPoint;

	void Start () {
		cameraColliderBounds = CameraBounds.GetComponent<MeshCollider>();
		startPos = transform.position;
		BoundingBox();
	}

	void Update()
	{
		#region//Panning
		Vector3 dir = Vector3.zero;
		if (Input.GetMouseButtonDown(0))
		{
			touchStart = UtilitFunctions.GetWorldPositionOnPlane(Input.mousePosition, 0);
		}
		if (Input.GetMouseButtonUp(0))
		{
			touchEnd = UtilitFunctions.GetWorldPositionOnPlane(Input.mousePosition, 0);
		}
		if (Input.GetMouseButton(0))
		{
			dir = touchStart - UtilitFunctions.GetWorldPositionOnPlane(Input.mousePosition, 0);
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


	public Vector3 NewZoom(Vector3 pos, float x)
	{
		Ray ray = Camera.main.ScreenPointToRay(pos);
		Vector3 currentpos = gameObject.transform.position;
		Vector3 futurePos = ray.GetPoint(x);
		if (x > 0) { return futurePos - currentpos; }

		Ray backRay = new Ray(CameraBounds.transform.position, backScrollDirection);
		
		return Vector3.zero;

	}
	public Vector3 Zoom(Vector3 pos, float x)
	{ 
		Ray ray = Camera.main.ScreenPointToRay(pos);
		Vector3 currentpos = gameObject.transform.position;
		Vector3 futurePos = ray.GetPoint(x);
		if (x > 0) { return futurePos - currentpos;	}
		Debug.DrawRay(transform.position, startPos - transform.position, Color.red);
		Ray nRay = new Ray(transform.position, startPos - transform.position);
		Vector3 vec = nRay.GetPoint(x);
		vec.Set(0, currentpos.y - vec.y, currentpos.z - vec.z);
		return vec;

	}
	public void BoundingBox()
	{
		backScrollDirection = CameraBounds.transform.TransformVector(Vector3.up);
		Ray r = new Ray(CameraBounds.transform.position, backScrollDirection);
		RaycastHit rr;
		if (cameraColliderBounds.Raycast(r, out rr, Mathf.Infinity))
		{
			print("Founds");
			print(rr.point);
		}
		Debug.DrawRay(CameraBounds.transform.position, backScrollDirection);
	}
	public void CheckBounds()
	{

		Vector3 posToCheck = transform.position + panningVelocity;
		Vector3 offset = cameraColliderBounds.bounds.center - posToCheck;
		Ray inputRay = new Ray(posToCheck, offset.normalized);
		RaycastHit rHit;
		Debug.DrawRay(posToCheck, offset);
		
		if (cameraColliderBounds.Raycast(inputRay, out rHit, offset.magnitude * 1.0f))
		{
			Vector3 hit = rHit.point;
			float check = CameraBounds.transform.position.x + cameraColliderBounds.bounds.size.x/2;
			float check1 = CameraBounds.transform.position.x - cameraColliderBounds.bounds.size.x/2;
			if (transform.position.x > CameraBounds.transform.position.x)
			{
				if (hit.x < check)
				{
					panningVelocity.y = 0;
				}
			}
			else
			{
				if (hit.x > check1)
				{
					panningVelocity.y = 0;
				}
			}
			if (transform.position.x + panningVelocity.x > check)
			{	
				panningVelocity.x = 0;
			}
			if (transform.position.x + panningVelocity.x < check1)
			{
				panningVelocity.x = 0;
			}
		}

		posToCheck = transform.position + zoomVelocity;
		offset = cameraColliderBounds.bounds.center - posToCheck;
		inputRay = new Ray(posToCheck, offset.normalized);
		
		if(cameraColliderBounds.Raycast(inputRay, out rHit, offset.magnitude * 1.1f))
		{
			if(Input.GetAxisRaw("Mouse ScrollWheel") > 0)
				zoomVelocity = Vector3.zero;
		}
	}
}
