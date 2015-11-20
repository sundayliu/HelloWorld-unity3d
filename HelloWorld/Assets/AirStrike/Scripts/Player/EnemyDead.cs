using UnityEngine;
using System.Collections;

public class EnemyDead : FlightOnDead {

	public int ScoreAdd = 250;
	void Start () {
	
	}
	
	public override void OnDead (GameObject killer)
	{
		if(killer){
			if(killer.gameObject.GetComponent<PlayerManager>()){
				GameManager score = (GameManager)GameObject.FindObjectOfType(typeof(GameManager));
				score.AddScore(ScoreAdd);
			}
		}
		base.OnDead (killer);
	}
}
