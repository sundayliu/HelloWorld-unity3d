using UnityEngine;
using System.Collections;

public class PlayerDead : FlightOnDead
{

	// Use this for initialization
	void Start ()
	{
	
	}
	
	public override void OnDead (GameObject killer)
	{

		GameManager gamemanger = (GameManager)GameObject.FindObjectOfType (typeof(GameManager));
		gamemanger.GameOver ();
			
		
		base.OnDead (killer);
	}
}
