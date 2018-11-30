using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBotsManager : MonoBehaviour {

	public enum BotType
	{
		None = -1,
		Mini = 0,
		Walk = 1,
		All = 100
	}
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
	[SerializeField] private GameObject CursorPrefab;
	[SerializeField] private GameObject WalkBotPrefab;
	[Range(1, 10)]
	[SerializeField] private int NumberOfWalkBots;
	[SerializeField] private Transform SpawnPostion;
	[SerializeField] private float TimePerSpawn;


	private TimeState timeState;
	private ActionState actionState;
	private int walkBotAlive;
	private bool clear;
	Bot bot;
	ArrayList walkBots;

	void Start () {
		timeState = TimeState.Play;
		actionState = ActionState.None;
		if (WalkBotPrefab == null || CursorPrefab == null)
		{
			print("Incorrect prefabs");
			clear = false;
		}
		walkBots = new ArrayList();
		walkBotAlive = 0;
		for (int i = 0; i < NumberOfWalkBots; i++)
		{
			StartCoroutine(CreateBot(i * TimePerSpawn, BotType.Walk));
		}
		clear = true;
	}
	
	
	void Update () {
		
		if (!clear) { return; }
		switch (actionState)
		{
			case ActionState.None: break;
			case ActionState.Power: break;
		}
		UpdateWalkBots();
		UpdateMiniBots();
	}
	private void UpdateWalkBots()
	{
		for (int i = 0; i < walkBots.Count; i++)
		{
			Bot temp = (Bot)walkBots[i];
			temp.Update();
		}
	}
	private void UpdateMiniBots()
	{

	}
	private void DoPower()
	{
		for (int i = 0; i < walkBots.Count; i++)
		{
			GameObject temp = Instantiate(CursorPrefab);
			
		}
		//cursor.SetActive(true);
		//Vector3 screenCords = Camera.main.WorldToScreenPoint(TestBot.transform.position);
		//cursor.transform.position = screenCords;
	}
	private void DoClimb()
	{

	}
	private void PauseGame()
	{

	}
	private void PlayGame()
	{

	}
	private void SetDefualts()
	{
		//cursor.GetComponent<CursorController>().Reset();
	}

	private void DisassembleCursors(BotType type)
	{
		switch (type)
		{
			case BotType.None: return;
			case BotType.All:
			case BotType.Mini:
			case BotType.Walk:
				{
					for (int i = 0; i < walkBots.Count; i++)
					{
						Bot tempBot = (Bot)walkBots[i];
						Destroy(tempBot.GetCursor());
						tempBot.SetCursor(null);
					}
				} break;
		}
	}
	private void InstantiateCursors(BotType type)
	{
		switch(type)
		{
			case BotType.None: return;
			case BotType.All:
			case BotType.Mini:
			case BotType.Walk:
				{
					for (int i = 0; i < walkBots.Count; i++)
					{
						Bot tempBot = (Bot)walkBots[i];
						GameObject tempCursor = Instantiate(CursorPrefab);
						tempCursor.transform.parent = canvas.transform;
						tempBot.SetCursor(tempCursor);
					}					
				} break;
		}
	}
	private void CreateCursorOnBot(Bot bot, GameObject cursorPrefab)
	{
		GameObject tempCursor = Instantiate(cursorPrefab);
		tempCursor.transform.parent = canvas.transform;
		bot.SetCursor(tempCursor);
	}
	IEnumerator CreateBot(float time, BotType type)
	{
		yield return new WaitForSeconds(time);
		switch (type)
		{
			case BotType.None: break;
			case BotType.All: break;
			case BotType.Mini:
			case BotType.Walk:
				{
					GameObject walkBot = Instantiate(WalkBotPrefab);
					Bot bot = new Bot(walkBot, SpawnPostion, BotType.Walk);
					if (!bot.Validate()) { print("Could not create bot"); }
					switch (actionState)
					{
						case ActionState.None: break;
						case ActionState.Power: CreateCursorOnBot(bot, CursorPrefab); break;
						case ActionState.Climb: CreateCursorOnBot(bot, CursorPrefab); break;
					}
					walkBots.Add(bot);
					walkBotAlive++;

				} break;
		}

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
			case ActionState.None: actionState = ActionState.Power; InstantiateCursors(BotType.All); break;
			case ActionState.Power: actionState = ActionState.None; DisassembleCursors(BotType.All); break;
			case ActionState.Climb: actionState = ActionState.None; DisassembleCursors(BotType.All); break;
		}
	}
	public void ClimbCommand()
	{
		switch (actionState)
		{
			case ActionState.None: actionState = ActionState.Climb; InstantiateCursors(BotType.All); break;
			case ActionState.Power: actionState = ActionState.None; DisassembleCursors(BotType.All); break;
			case ActionState.Climb: actionState = ActionState.None; DisassembleCursors(BotType.All); break;
		}
	}

}
