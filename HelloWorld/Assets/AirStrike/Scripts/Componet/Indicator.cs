using UnityEngine;
using System.Collections;

public enum NavMode
{
	Third,
	Cockpit,
	None
}

public class Indicator : MonoBehaviour
{
	public string[] TargetTag;
	public Texture2D[] NavTexture;
	public Texture2D Crosshair;
	public Texture2D Crosshair_in;
	public Vector2 CrosshairOffset;
	public float DistanceSee = 800;
	public float Alpha = 0.7f;
	public GameObject CockpitCamera;
	public bool Show = true;
	[HideInInspector]
	public NavMode Mode = NavMode.Third;
	[HideInInspector]
	public FlightSystem flight;
	void Awake ()
	{
		if (!CockpitCamera){
			if(this.transform.GetComponentInChildren (typeof(Camera))){
				CockpitCamera = this.transform.GetComponentInChildren (typeof(Camera)).gameObject;
			}
		}
		flight = this.GetComponent<FlightSystem>();
	}
	
	void Start ()
	{

	}

	public void DrawNavEnemy ()
	{
		for (int t=0; t<TargetTag.Length; t++) {
			if (GameObject.FindGameObjectsWithTag (TargetTag [t]).Length > 0) {
				GameObject[] objs = GameObject.FindGameObjectsWithTag (TargetTag [t]);
				for (int i = 0; i < objs.Length; i++) {
					if (objs [i]) {
						Vector3 dir = (objs [i].transform.position - transform.position).normalized;
						float direction = Vector3.Dot (dir, transform.forward);
						if (direction >= 0.7f) {
							float dis = Vector3.Distance (objs [i].transform.position, transform.position);
							if (DistanceSee > dis) {
								DrawTargetLockon (objs [i].transform, t);
							}
						}
					}
				}
			}
		}
	}

	void OnGUI ()
	{
		if (Show) {
			GUI.color = new Color (1, 1, 1, Alpha);
			switch (Mode) {
			case NavMode.Third:
				if (Crosshair)
					GUI.DrawTexture (new Rect ((Screen.width / 2 - Crosshair.width / 2) + CrosshairOffset.x, (Screen.height / 2 - Crosshair.height / 2) + CrosshairOffset.y, Crosshair.width, Crosshair.height), Crosshair);	
				DrawNavEnemy ();
				break;
			case NavMode.Cockpit:
				if (Crosshair_in)
				GUI.DrawTexture (new Rect ((Screen.width / 2 - Crosshair_in.width / 2) + CrosshairOffset.x, (Screen.height / 2 - Crosshair_in.height / 2) + CrosshairOffset.y, Crosshair_in.width, Crosshair_in.height), Crosshair_in);	
				DrawNavEnemy ();
				break;
			case NavMode.None:
				
				break;
			}

			
		}
	}
	
	public void DrawTargetLockon (Transform aimtarget, int type)
	{
		if (Camera.current && Camera.current.GetComponent<Camera>() != null) {
			Vector3 dir = (aimtarget.position - Camera.current.GetComponent<Camera>().transform.position).normalized;
			float direction = Vector3.Dot (dir, Camera.current.GetComponent<Camera>().transform.forward);
			if (direction > 0.5f) {
				Vector3 screenPos = Camera.current.GetComponent<Camera>().WorldToScreenPoint (aimtarget.transform.position);
				float distance = Vector3.Distance (transform.position, aimtarget.transform.position);
				
				GUI.DrawTexture (new Rect (screenPos.x - NavTexture [type].width / 2, Screen.height - screenPos.y - NavTexture [type].height / 2, NavTexture [type].width, NavTexture [type].height), NavTexture [type]);
            	
			}
		}
	}
	
	void Update ()
	{
		if(CockpitCamera){
			if (CockpitCamera.GetComponent<Camera>().enabled) {
				Mode = NavMode.Cockpit;	
			} else {
				Mode = NavMode.Third;
			}
		}else{
			Mode = NavMode.Third;
		}
	}
}
