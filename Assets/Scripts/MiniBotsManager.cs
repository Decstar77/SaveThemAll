using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
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
	[SerializeField] private float startTime = 60;
	[SerializeField] private Text Time;

	private TimeState timeState;
	private ActionState actionState;
	private int walkBotAlive;
	private bool clear;
	private bool isActing;
	Bot bot;
	ArrayList walkBots;

	void Start () {
		Time.text = startTime.ToString();
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
		StartCoroutine(Wait(1));
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
						tempCursor.transform.SetParent(canvas.transform);
						tempBot.SetCursor(tempCursor);
					}					
				} break;
		}
	}
	private void CreateCursorOnBot(Bot bot, GameObject cursorPrefab)
	{
		GameObject tempCursor = Instantiate(cursorPrefab);
		tempCursor.transform.SetParent(canvas.transform);
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
					Bot bot = new Bot(walkBot, SpawnPostion);
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
	IEnumerator Wait(float time)
	{
		yield return new WaitForSeconds(time);
		Time.text = (--startTime).ToString();
		StartCoroutine(Wait(time));
	}
	private int FindBot(GameObject id, BotType type)
	{
		switch (type)
		{
			case BotType.None: return -1;
			case BotType.All: return -1;
			case BotType.Mini: return -1;
			case BotType.Walk:
				{
					for (int i = 0; i < walkBots.Count; i++)
					{
						Bot temp = (Bot)walkBots[i];
						GameObject tempObj = temp.GetGameObject();
						if (id == tempObj)
							return i;
					}
				}break;
		}
		return -1;
	}
	public void AddForceToBot(GameObject id, BotType type, Vector3 direction, float VerticalForce, float HorizontalForce)
	{
		int pos = FindBot(id, type);
		if (pos != -1)
		{
			((Bot)(walkBots[pos])).JumpLaunch(direction, VerticalForce, HorizontalForce);
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
