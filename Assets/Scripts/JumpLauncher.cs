using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpLauncher : MonoBehaviour {

	[SerializeField] float Verticalforce = 10;
	[SerializeField] float Horizontalforce = 5;
	[SerializeField] MiniBotsManager miniBots;
	private ParticleSystem psys;
	private void Start()
	{
		psys = GetComponent<ParticleSystem>();
		psys.Stop();
		if (miniBots == null)
		{
			GameObject temp = GameObject.FindGameObjectWithTag("MainCamera");
			miniBots = temp.GetComponent<MiniBotsManager>();
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		BotCharateristics botCharateristics = other.gameObject.GetComponent<BotCharateristics>();
		other.gameObject.transform.position = transform.position;
		psys.Play();
		miniBots.AddForceToBot(other.gameObject, botCharateristics.BotType , Vector3.up, Verticalforce, Horizontalforce);
	}

}
