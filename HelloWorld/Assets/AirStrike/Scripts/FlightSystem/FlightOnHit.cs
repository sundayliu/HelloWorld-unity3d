using UnityEngine;
using System.Collections;

public class FlightOnHit : MonoBehaviour {
	
	public string[] Tag = new string[1]{"Scene"};
	public int Damage = 100;
	public AudioClip[] SoundOnHit;
	void Start(){
		
	}
	
    private void OnCollisionEnter(Collision collision)
    {
		bool hit = false;
		
		for(int i = 0;i<Tag.Length;i++){
			if(collision.gameObject.tag == Tag[i]){
				hit = true;
			}
		}
		
        if (hit)
        {
			if(SoundOnHit.Length>0)
			AudioSource.PlayClipAtPoint(SoundOnHit[Random.Range(0,SoundOnHit.Length)],this.transform.position);
			if(this.GetComponent<DamageManager>()){
				this.GetComponent<DamageManager>().ApplyDamage(Damage,collision.gameObject);
			}
		}	
    }
}
