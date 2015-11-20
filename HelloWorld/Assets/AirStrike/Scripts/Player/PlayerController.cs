using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	FlightSystem flight;
	public bool Active = true;
	
	void Start () {
		flight = this.GetComponent<FlightSystem>();
	}
	
	void Update () {
		if(!flight || !Active)
			return;
		
		
		Screen.lockCursor = true;
		
		flight.AxisControl(new Vector2(Input.GetAxis("Mouse X"),Input.GetAxis("Mouse Y")));
		
		if(Input.GetKey(KeyCode.A)){
			flight.TurnControl(-1);
		}
		if(Input.GetKey(KeyCode.D)){
			flight.TurnControl(1);
		}
		if(Input.GetKey(KeyCode.W)){
			flight.SpeedUp();
		}
		
		if(Input.GetButton("Fire1")){
            flight.WeaponControl.LaunchWeapon();
        }
		
		if(Input.GetKeyDown(KeyCode.R)){
            flight.WeaponControl.SwitchWeapon();
        }
       

	}
}
