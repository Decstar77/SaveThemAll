using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBotsManager : MonoBehaviour {

	enum TimeState
	{
		Play = 0,
		Pause = 1
	}
	enum ActionState
	{
		None = -1,
		Power = 0,
		Climb = 1
	}
	[SerializeField] private Canvas canvas;
	[SerializeField] private GameObject cursor;
	[SerializeField] private GameObject TestBot;


	private TimeState timeState;
	private ActionState actionState;
	

	void Start () {
		timeState = TimeState.Play;
		actionState = ActionState.None;
		print(canvas.pixelRect.height);

	}
	
	
	void Update () {
		switch (actionState)
		{
			case ActionState.None: break;
			case ActionState.Power: DoPower(); break;
		}
		
	}

	private void DoPower()
	{
		cursor.SetActive(true);
		Vector3 screenCords = Camera.main.WorldToScreenPoint(TestBot.transform.position);
		cursor.transform.position = screenCords;
	}

	private void SetDefualts()
	{
		cursor.GetComponent<CursorController>().Reset();
	}

	/* <Summary>
		 Contains all public functions for UI commands
	   <Summary>*/
	
	public void PlayCommand()
	{
		switch (timeState)
		{
			case TimeState.Play: print("IsPlaying"); break;
			case TimeState.Pause: print("Playing now"); timeState = TimeState.Play; break;
		}
	
	}
	public void PauseCommand()
	{
		switch (timeState)
		{
			case TimeState.Play: print("Pausing"); timeState = TimeState.Pause; ; break;
			case TimeState.Pause: print("IsPause");  break;
		}
	}
	public void PowerCommand()
	{
		switch(actionState)
		{
			case ActionState.None: actionState = ActionState.Power; break;
			case ActionState.Power: actionState = ActionState.None; SetDefualts(); break;
			case ActionState.Climb: actionState = ActionState.None; SetDefualts(); break;
		}
	}
	public void ClimbCommand()
	{

	}

}
