using UnityEngine;
using System.Collections;

public enum AIState
{
	Idle,
	Patrol,
	Attacking,
	TurnPosition,
}

public enum TargetBehavior
{
	Static,
	Moving,
	Flying,
}

public class AIController : MonoBehaviour
{
	
	public string[] TargetTag;
	public GameObject Target;
	public float TimeToLock;
	public float AttackDirection = 0.5f;
	public float DistanceLock = float.MaxValue;
	public float DistanceAttack = 300;
	public float FlyDistance = 1000;
	public Transform CenterOfBattle;
	public AIState AIstate = AIState.Patrol;
	public float AIstateTime = 5;
	[HideInInspector]
	public int WeaponSelected = 0;
	public int AttackRate = 80;
	private float timestatetemp;
	private float timetolockcount;
	private FlightSystem flight;
	private bool attacking;
	private Vector3 directionTurn;
	private TargetBehavior targetHavior;
	private Vector3 targetpositionTemp;
	
	void Start ()
	{
		timetolockcount = Time.time;
		flight = this.GetComponent<FlightSystem> ();
		flight.AutoPilot = true;
		timestatetemp = 0;
	}
	
	void TargetBehaviorCal ()
	{
		
		if (Target) {
			Vector3 delta = (targetpositionTemp - Target.transform.position);
			float deltaheight = Mathf.Abs (targetpositionTemp.y - Target.transform.position.y); 
			targetpositionTemp = Target.transform.position;
			
			if (delta == Vector3.zero) {
				targetHavior = TargetBehavior.Static;	
			} else {
				targetHavior = TargetBehavior.Moving;
				if (deltaheight > 0.5f) {
					targetHavior = TargetBehavior.Flying;	
				}
			}
		}
	}
	
	private float dot;
	private Vector3 battleposition;
	
	void Update ()
	{
		if (!flight)
			return;
		
		battleposition = Vector3.zero;
		if (CenterOfBattle)
			battleposition = CenterOfBattle.transform.position;
		
		TargetBehaviorCal ();
		
		switch (AIstate) {
		case AIState.Patrol:
			for (int t = 0; t<TargetTag.Length; t++) {
				if (GameObject.FindGameObjectsWithTag (TargetTag [t]).Length > 0) {
					GameObject[] objs = GameObject.FindGameObjectsWithTag (TargetTag [t]);
					float distance = int.MaxValue;
					for (int i = 0; i < objs.Length; i++) {
						if (objs [i]) {

							if (timetolockcount + TimeToLock < Time.time) {
                           	
								float dis = Vector3.Distance (objs [i].transform.position, transform.position);
								if (DistanceLock > dis) {
									if (!Target) {
										if (distance > dis && Random.Range (0, 100) > 80) {
											distance = dis;
											Target = objs [i];
											flight.FollowTarget = true;
											AIstate = AIState.Attacking;
											timestatetemp = Time.time;
											WeaponSelected = Random.Range (0, flight.WeaponControl.WeaponLists.Length);	
										}
									}
								}
							}
							shootTarget (objs [i].transform.position);
						}
					}
				}
			}
			break;
		case AIState.Idle:
			if (Vector3.Distance (flight.PositionTarget, this.transform.position) <= FlyDistance) {
				AIstate = AIState.Patrol;
				timestatetemp = Time.time;
			}
			
			break;
		case AIState.Attacking:
			if (Target) {
				flight.PositionTarget = Target.transform.position;
				if (!shootTarget (flight.PositionTarget)) {
					if (attacking) {
						if (Time.time > timestatetemp + 5) {
							turnPosition ();
						}	
					} else {
						if (Time.time > timestatetemp + 7) {
							turnPosition ();
						}		
					}
				}
				
			} else {
				AIstate = AIState.Patrol;
				timestatetemp = Time.time;
			}
			if (Vector3.Distance (battleposition, this.transform.position) > FlyDistance) {
				gotoCenter ();
			}
			break;
		case AIState.TurnPosition:
			if (Time.time > timestatetemp + 7) {
				timestatetemp = Time.time;
				AIstate = AIState.Attacking;
			}
			if (Vector3.Distance (battleposition, this.transform.position) > FlyDistance) {
				gotoCenter ();
			}
			float height = flight.PositionTarget.y;
			if (targetHavior == TargetBehavior.Static) {
				directionTurn.y = 0;
				flight.PositionTarget += (this.transform.forward + directionTurn) * flight.Speed;
				flight.PositionTarget.y = height;
				flight.PositionTarget.y += flight.Speed/2;
			} else {
				flight.PositionTarget += (this.transform.forward + directionTurn) * flight.Speed;
				flight.PositionTarget.y = height;
				flight.PositionTarget.y += flight.Speed/2;
				//flight.PositionTarget = battleposition + new Vector3 (Random.Range (-FlyDistance, FlyDistance), Random.Range (0, FlyDistance / 2), Random.Range (-FlyDistance, FlyDistance));	
			}
			break;
		}
	}
	
	bool shootTarget (Vector3 targetPos)
	{
		Vector3 dir = (targetPos - transform.position).normalized;
		dot = Vector3.Dot (dir, transform.forward);
		float distance = Vector3.Distance (targetPos, this.transform.position);	
		if (distance <= DistanceAttack) {
			if (dot >= AttackDirection) {
				attacking = true;
				if (Random.Range (0, 100) > AttackRate) {
					flight.WeaponControl.LaunchWeapon (WeaponSelected);
				}
				if (distance < DistanceAttack / 3) {
					turnPosition ();	
				}
			} else {
				WeaponSelected = Random.Range (0, flight.WeaponControl.WeaponLists.Length);	
				return false;
			}
		} else {
			flight.SpeedUp ();
		}
		return true;
	}

	void turnPosition ()
	{
		directionTurn = new Vector3(Random.Range(-2,1)+1,Random.Range(-2,1)+1,Random.Range(-2,1)+1);
		AIstate = AIState.TurnPosition;
		timestatetemp = Time.time;
		attacking = false;
	}

	void gotoCenter ()
	{
		flight.PositionTarget = battleposition;	
		timestatetemp = Time.time;
		Target = null;
		AIstate = AIState.Idle;	
	}

}
