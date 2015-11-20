using UnityEngine;
using System.Collections;

public class GameUI : MonoBehaviour
{

	public GUISkin skin;
	public Texture2D Logo;
	public int Mode;
	
	void Start ()
	{
	
	}
	
	void Update ()
	{
	
	}
	
	public void OnGUI ()
	{
		GameManager game = (GameManager)GameObject.FindObjectOfType (typeof(GameManager));
		PlayerController play = (PlayerController)GameObject.FindObjectOfType (typeof(PlayerController));
		
		if (skin)
			GUI.skin = skin;
		
		
		switch (Mode) {
		case 0:
			if (Input.GetKeyDown (KeyCode.Escape)) {
				Mode = 2;	
			}
			
			if (play) {
				WeaponController weapon = play.GetComponent<WeaponController> ();
				play.Active = true;
			
				GUI.skin.label.alignment = TextAnchor.UpperLeft;
				GUI.Label (new Rect (20, 20, 200, 30), "Killed " + game.Killed.ToString ());
				GUI.Label (new Rect (20, 50, 200, 30), "Score " + game.Score.ToString ());
			
			
				GUI.skin.label.alignment = TextAnchor.UpperRight;
				GUI.Label (new Rect (Screen.width - 220, 20, 200, 30), "ARMOR " + play.GetComponent<DamageManager> ().HP);
				if (weapon.WeaponLists [weapon.CurrentWeapon].Icon)
					GUI.DrawTexture (new Rect (Screen.width - 100, Screen.height - 100, 80, 80), weapon.WeaponLists [weapon.CurrentWeapon].Icon);
				
				GUI.skin.label.alignment = TextAnchor.UpperRight;
				if (weapon.WeaponLists [weapon.CurrentWeapon].Ammo <= 0 && weapon.WeaponLists [weapon.CurrentWeapon].ReloadingProcess > 0) {
					if (!weapon.WeaponLists [weapon.CurrentWeapon].InfinityAmmo)
						GUI.Label (new Rect (Screen.width - 230, Screen.height - 120, 200, 30), "Reloading " + Mathf.Floor ((1 - weapon.WeaponLists [weapon.CurrentWeapon].ReloadingProcess) * 100) + "%");
				} else {
					if (!weapon.WeaponLists [weapon.CurrentWeapon].InfinityAmmo)
						GUI.Label (new Rect (Screen.width - 230, Screen.height - 120, 200, 30), weapon.WeaponLists [weapon.CurrentWeapon].Ammo.ToString ());
				}
				
				GUI.skin.label.alignment = TextAnchor.UpperLeft;
				GUI.Label (new Rect (20, Screen.height - 50, 250, 30), "R : Switch Guns C : Change Camera");
			
			}
			break;
		case 1:
			if (play)
				play.Active = false;
			
			Screen.lockCursor = false;
			
			GUI.skin.label.alignment = TextAnchor.MiddleCenter;
			GUI.Label (new Rect (0, Screen.height / 2 + 10, Screen.width, 30), "Game Over");
		
			GUI.DrawTexture (new Rect (Screen.width / 2 - Logo.width / 2, Screen.height / 2 - 150, Logo.width, Logo.height), Logo);
		
			if (GUI.Button (new Rect (Screen.width / 2 - 150, Screen.height / 2 + 50, 300, 40), "Restart")) {
				Application.LoadLevel (Application.loadedLevelName);
			
			}
			if (GUI.Button (new Rect (Screen.width / 2 - 150, Screen.height / 2 + 100, 300, 40), "Main menu")) {
				Application.LoadLevel ("Mainmenu");
			}
			break;
		
		case 2:
			if (play)
				play.Active = false;
			
			Screen.lockCursor = false;
			Time.timeScale = 0;
			GUI.skin.label.alignment = TextAnchor.MiddleCenter;
			GUI.Label (new Rect (0, Screen.height / 2 + 10, Screen.width, 30), "Pause");
		
			GUI.DrawTexture (new Rect (Screen.width / 2 - Logo.width / 2, Screen.height / 2 - 150, Logo.width, Logo.height), Logo);
		
			if (GUI.Button (new Rect (Screen.width / 2 - 150, Screen.height / 2 + 50, 300, 40), "Resume")) {
				Mode = 0;
				Time.timeScale = 1;
			}
			if (GUI.Button (new Rect (Screen.width / 2 - 150, Screen.height / 2 + 100, 300, 40), "Main menu")) {
				Time.timeScale = 1;
				Mode = 0;
				Application.LoadLevel ("Mainmenu");
			}
			break;
			
		}
		
	}
}
