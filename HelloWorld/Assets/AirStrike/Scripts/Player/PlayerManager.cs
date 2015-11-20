using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(Indicator))]
[RequireComponent(typeof(RadarSystem))]
[RequireComponent(typeof(PlayerDead))]

public class PlayerManager : MonoBehaviour {
	[HideInInspector]
	public PlayerController PlayerControl;
	[HideInInspector]
	public Indicator Indicate;
	
	void Awake(){
		Indicate = this.GetComponent<Indicator>();
		PlayerControl = this.GetComponent<PlayerController>();
	}
	
	void Start () {
		FlightView view = (FlightView)GameObject.FindObjectOfType(typeof(FlightView));
		if(Indicate.CockpitCamera){
			view.AddCamera(Indicate.CockpitCamera);
		}
	}

	void Update () {
	
	}
}
