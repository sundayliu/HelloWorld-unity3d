using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]

public class FlightView : MonoBehaviour
{

	public GameObject Target;
	public GameObject[] Cameras;
	private int indexCamera;
	public Vector3 Offset = new Vector3 (10, 0.5f, 10);
	
	public void SwitchCameras ()
	{
		indexCamera += 1;
		if (indexCamera >= Cameras.Length) {
			indexCamera = 0;
		}
		for (int i =0; i<Cameras.Length; i++) {
			if (Cameras [i] && Cameras [i].GetComponent<Camera>())
				Cameras [i].GetComponent<Camera>().enabled = false;
			if (Cameras [i] && Cameras [i].GetComponent<AudioListener> ())
				Cameras [i].GetComponent<AudioListener> ().enabled = false;
		}
		if (Cameras [indexCamera]) {
			if (Cameras [indexCamera] && Cameras [indexCamera].GetComponent<Camera>())
				Cameras [indexCamera].GetComponent<Camera>().enabled = true;
			if (Cameras [indexCamera] && Cameras [indexCamera].GetComponent<AudioListener> ())
				Cameras [indexCamera].GetComponent<AudioListener> ().enabled = true;
		}
	}

	void Awake ()
	{
		AddCamera(this.gameObject);
	}
	public void AddCamera(GameObject cam){
		GameObject[] temp = new GameObject[Cameras.Length+1];
		Cameras.CopyTo(temp, 0);
		Cameras = temp;
		Cameras[temp.Length-1] = cam;
	}
	void Start ()
	{
		if(!Target){
			PlayerManager player = (PlayerManager)GameObject.FindObjectOfType(typeof(PlayerManager));	
			Target = player.gameObject;
		}
	}
	
	void FixedUpdate ()
	{
		if (!Target)
			return;
		
		Quaternion lookAtRotation = Quaternion.LookRotation (Target.transform.position);
		this.transform.LookAt (Target.transform.position + Target.transform.forward * Offset.x);
		Vector3 positionTarget = Target.transform.position + ((-Target.transform.forward + (Target.transform.up * Offset.y)) * Offset.z);
		float distance = Vector3.Distance (positionTarget, this.transform.position);
		this.transform.position = Vector3.Lerp (this.transform.position, positionTarget, Time.fixedDeltaTime * distance);
		
	}

	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.C)) {
			SwitchCameras ();	
		}
		bool activecheck = false;
		for (int i =0; i<Cameras.Length; i++) {
			if (Cameras [i] && Cameras [i].GetComponent<Camera>().enabled) {
				activecheck = true;
				break;	
			}
		}
		if (!activecheck) {
			this.GetComponent<Camera>().enabled = true;
			if (this.gameObject.GetComponent<AudioListener> ())
				this.gameObject.GetComponent<AudioListener> ().enabled = true;
		}
	}
}
