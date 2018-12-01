using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class EnivromentManager : MonoBehaviour {

	private enum Mode
	{
		None = -1,
		Play = 0, 
		Pause = 1,
		Launchpad = 2
	}

	[SerializeField] GameObject psTest;
	[SerializeField] GameObject launchPad;
	[SerializeField] GameObject launchPadFeedBack;
	[SerializeField] Text launchAmountTexts;
	private bool placing;
	private float test;
	private int LaunchPadAmount = 1;
	private Mode mode;
	private string LaunchPadTag = "LaunchPadPlanes";

	ArrayList launchPadParticles;
	GameObject []launchPadPlanes;


	void Start () {
		launchPadPlanes = GameObject.FindGameObjectsWithTag(LaunchPadTag);
		mode = Mode.None;
		placing = false;
		launchPadParticles = new ArrayList();
	}
	
	void Update () {
		switch (mode)
		{
			case Mode.None: return;
			case Mode.Play: return;
			case Mode.Pause: return;
			case Mode.Launchpad: PlaceLaunchPad(); break;
		}
		
	}
	private void OnDrawGizmos()
	{

	}

	private void PlaceLaunchPad()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		GameObject plane = UtilitFunctions.GetObjectRayIntersected(ray, LaunchPadTag);
		if (plane != null)
		{
			if (Input.GetMouseButtonDown(0))
			{
				LaunchPadAmount--;
				launchAmountTexts.text = LaunchPadAmount.ToString();
				placing = false;
				DisassembleParticles();
				mode = Mode.None;
				plane.transform.tag = "Untagged";
				GameObject feedBack = Instantiate(launchPadFeedBack);
				GameObject LaunchPad = Instantiate(launchPad);
				feedBack.transform.position = GetCenterOfPlane(plane) + Vector3.up;
				LaunchPad.transform.position = GetCenterOfPlane(plane) + Vector3.up;
			}
		}

	}

	private bool RemovePlane(GameObject obj, Mode rmode)
	{
		return false;
	}
	private int FindPlane(GameObject obj, Mode rmode)
	{
		switch (rmode)
		{
			case Mode.None: return -1;
			case Mode.Play: return -1;
			case Mode.Pause: return -1;
			case Mode.Launchpad:
				{
					for (int i = 0; i < launchPadPlanes.Length; i++)
					{
						if (obj == launchPadPlanes[i])
							return i;
					}
				} break;
		}
		return -1;
	}
	private void InstantiateParticles()
	{
		switch (mode)
		{
			case Mode.None: return;
			case Mode.Play: return;
			case Mode.Pause: return;
			case Mode.Launchpad:
				{
					
					for (int i = 0; i < launchPadPlanes.Length; i++)
					{
						if (launchPadPlanes[i].transform.tag != LaunchPadTag)
							continue;
						GameObject temp = Instantiate(psTest);
						temp.transform.position = GetCenterOfPlane(launchPadPlanes[i]);
						launchPadParticles.Add(temp);
					}
				} break;
		}

	}
	private void DisassembleParticles()
	{
		switch (mode)
		{
			case Mode.None: return;
			case Mode.Play: return;
			case Mode.Pause: return;
			case Mode.Launchpad:
				{

					for (int i = 0; i < launchPadParticles.Count; i++)
					{
						Destroy(((GameObject)launchPadParticles[i]));
					}
					launchPadParticles.Clear();
				}
				break;
		}
	}
	private Vector3 GetCenterOfPlane(GameObject plane)
	{
		Vector3 launchPadPlaneCenter = plane.GetComponent<BoxCollider>().center;
		return plane.transform.TransformPoint(launchPadPlaneCenter);
	}
	IEnumerator LaunchPadFeedBack(GameObject feedBack, float step)
	{
		//step += 15;
		//feedBack.transform.localScale = Vector3.one * (2 * Mathf.Sin(step * Mathf.Deg2Rad) + 1);
		//print(Vector3.one * (2 * Mathf.Sin(step * Mathf.Deg2Rad) + 1));
		yield return new WaitUntil(() => feedBack.transform.localScale.x <= 1);
		//print("Ended");
	}
	/* <Summary>
		Contains all public functions for UI commands
	<Summary>*/

	public void LaunchPadCommand()
	{
		if (!placing && LaunchPadAmount > 0)
		{
			placing = true;
			mode = Mode.Launchpad;
			InstantiateParticles();
		}
		else
		{
			placing = false;
			DisassembleParticles();
			mode = Mode.None;
		}
	}
}
