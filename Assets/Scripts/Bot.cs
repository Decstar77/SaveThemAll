using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot
{


	private GameObject gameObject;
	private GameObject cursor;
	private MiniBotsManager.BotType type;
	private BotCharateristics charateristics;
	public Bot(GameObject bot, Transform StartPosition, MiniBotsManager.BotType type)
	{
		gameObject = bot;
		gameObject.transform.position = StartPosition.position;
		this.type = type;
		charateristics = gameObject.GetComponent<BotCharateristics>();
	}
	public void SetCursor(GameObject cursor)
	{
		this.cursor = cursor;
	}
	public bool Validate()
	{
		if (charateristics == null || gameObject == null || type == MiniBotsManager.BotType.None)
		{
			return false;
		}
		return true;
	}
	public void Update()
	{
		gameObject.transform.position += Vector3.right * Time.deltaTime;
		if (cursor != null)
		{
			Vector3 screenCords = Camera.main.WorldToScreenPoint(gameObject.transform.position);
			cursor.transform.position = screenCords;
		}
	}
	public GameObject GetGameObject()
	{
		return gameObject;
	}
	public GameObject GetCursor()
	{
		return cursor;
	}

}
