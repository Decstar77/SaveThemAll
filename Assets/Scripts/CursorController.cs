using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine;

public class CursorController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	private bool DoWave = true;
	[SerializeField] private float amplitude = 0.5f;
	[SerializeField] private float period = 1f;
	[SerializeField] private float offset = 1.5f;
	[SerializeField] private float stepInc = 10;
	private float step = 1;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (DoWave)
		{
			float stepR = step * Mathf.Deg2Rad * period;
			float scale = amplitude * Mathf.Sin(stepR) + offset;
			step += stepInc;
			gameObject.transform.localScale = new Vector3(scale, scale, 0);
		}
		else
		{
			float scale = amplitude * Mathf.Sin(Mathf.PI *period * Mathf.Deg2Rad) + offset;
			gameObject.transform.localScale = new Vector3(scale, scale, 0);
		}
	}
	public void OnPointerEnter(PointerEventData eventData)
	{
		DoWave = false;
	}
	public void OnPointerExit(PointerEventData eventData)
	{
		DoWave = true;
	}
	public void Reset()
	{
		step = 1;
		gameObject.transform.localScale = new Vector3(1, 1, 1);
		gameObject.SetActive(false);
	}
}
