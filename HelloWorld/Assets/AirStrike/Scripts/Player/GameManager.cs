using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public int Score = 0;
	public int Killed = 0;
	
	void Start () {
		Score = 0;
		Killed = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void AddScore(int score){
		Score += score;
		Killed +=1;
	}
	
	void OnGUI(){
		//GUI.Label(new Rect(20,20,300,30),"Kills "+Score);
	}
	public void GameOver(){
		GameUI menu = (GameUI)GameObject.FindObjectOfType(typeof(GameUI));
		if(menu){
			menu.Mode = 1;	
		}
	}
}
