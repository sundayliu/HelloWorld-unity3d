using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(DamageManager))]
[RequireComponent(typeof(WeaponController))]


public class FlightSystem : MonoBehaviour {
	
	public float Speed = 50.0f;
	public float SpeedMax = 100;
	private float MoveSpeed = 10;
    public float RotationSpeed = 1.0f;
	public float TurnSpeed = 2;
	public float SpeedPitch = 1;
	public float SpeedRoll = 1;
	public float SpeedYaw = 1;
	public float DampingTarget = 10.0f;
	public bool AutoPilot = false;
	
	[HideInInspector]
	public bool FollowTarget = false;
	[HideInInspector]
	public Vector3 PositionTarget = Vector3.zero;
	[HideInInspector]
	public DamageManager DamageManage;
	[HideInInspector]
	public WeaponController WeaponControl;
	
	private Vector3 positionTarget = Vector3.zero;
	private Quaternion mainRot = Quaternion.identity;
	[HideInInspector]
	public float roll = 0;
	[HideInInspector]
    public float pitch = 0;
	[HideInInspector]
    public float yaw = 0;
	
	
	void Start () 
	{
		DamageManage = this.gameObject.GetComponent<DamageManager>();
		WeaponControl = this.gameObject.GetComponent<WeaponController>();
	}

	void FixedUpdate()
	{
		if(!this.GetComponent<Rigidbody>())
			return;

		Quaternion AddRot = Quaternion.identity;
		Vector3 velocityTarget = Vector3.zero;
		
		if(AutoPilot){
			if(FollowTarget){
				positionTarget = Vector3.Lerp(positionTarget,PositionTarget,Time.fixedDeltaTime * DampingTarget);
				Vector3 relativePoint = this.transform.InverseTransformPoint(positionTarget).normalized;
				mainRot = Quaternion.LookRotation(positionTarget - this.transform.position);
				GetComponent<Rigidbody>().rotation = Quaternion.Lerp(GetComponent<Rigidbody>().rotation,mainRot,Time.fixedDeltaTime * RotationSpeed);
				this.GetComponent<Rigidbody>().rotation *= Quaternion.Euler(-relativePoint.y * 2,0,-relativePoint.x * 10);

			}
			velocityTarget = (GetComponent<Rigidbody>().rotation * Vector3.forward) * (Speed + MoveSpeed);
		}else{
			AddRot.eulerAngles = new Vector3(pitch, yaw, -roll);
       	 	mainRot *= AddRot;
        	GetComponent<Rigidbody>().rotation = Quaternion.Lerp(GetComponent<Rigidbody>().rotation,mainRot,Time.fixedDeltaTime * RotationSpeed);
			velocityTarget = (GetComponent<Rigidbody>().rotation * Vector3.forward) * (Speed + MoveSpeed);
			
		}
        GetComponent<Rigidbody>().velocity = Vector3.Lerp(GetComponent<Rigidbody>().velocity,velocityTarget,Time.fixedDeltaTime);
		
		yaw = Mathf.Lerp(yaw,0,Time.deltaTime);
		MoveSpeed = Mathf.Lerp(MoveSpeed,Speed,Time.deltaTime);
	}
	
	

	public void AxisControl(Vector2 axis){
		roll = Mathf.Lerp(roll,Mathf.Clamp(axis.x,-2,2) * SpeedRoll,Time.deltaTime);
        pitch = Mathf.Lerp(pitch,Mathf.Clamp(axis.y,-1,1) * SpeedPitch,Time.deltaTime);
	}
	
	public void TurnControl(float turn){
		yaw += turn * Time.deltaTime * SpeedYaw;
	}
	
	public void SpeedUp(){
		MoveSpeed = Mathf.Lerp(MoveSpeed,SpeedMax,Time.deltaTime * 10);
	}
	
	void Update(){
		
	}


}
