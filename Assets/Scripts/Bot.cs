using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot
{

	public BoxCollider boxCollider;
	public Rigidbody rigidbody;
	private GameObject gameObject;
	private GameObject cursor;
	private BotCharateristics charateristics;
	public Bot(GameObject bot, Transform StartPosition)
	{
		gameObject = bot;
		gameObject.transform.position = StartPosition.position;
		charateristics = gameObject.GetComponent<BotCharateristics>();
		boxCollider = gameObject.GetComponent<BoxCollider>();
		rigidbody = gameObject.GetComponent<Rigidbody>();
	}
	public void SetCursor(GameObject cursor)
	{
		this.cursor = cursor;
	}
	public bool Validate()
	{
		if (charateristics == null || gameObject == null || charateristics.BotType == MiniBotsManager.BotType.None)
		{
			return false;
		}
		return true;
	}
	public void Update()
	{
		gameObject.transform.position += charateristics.speed * Vector3.right * Time.deltaTime;
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
	public void JumpLaunch(Vector3 direction, float verticalForce, float horizontalForce)
	{
		rigidbody.AddForce(direction * verticalForce, ForceMode.Impulse);
		rigidbody.AddForce(Vector3.right * horizontalForce, ForceMode.VelocityChange);
	}

}
