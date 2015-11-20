using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour
{
    public WeaponLauncher[] WeaponLists;
    public int CurrentWeapon = 0;

    private void Start()
    {
    }
	
	
	public void LaunchWeapon(int index){
		CurrentWeapon = index;
		if(CurrentWeapon < WeaponLists.Length && WeaponLists[index] != null){
			WeaponLists[index].Shoot();
		}
	}
	public void LaunchWeapon(){
		if(CurrentWeapon < WeaponLists.Length && WeaponLists[CurrentWeapon] != null){
			WeaponLists[CurrentWeapon].Shoot();
		}
	}
	
	public void SwitchWeapon(){
		CurrentWeapon +=1;
		if(CurrentWeapon>=WeaponLists.Length){
			CurrentWeapon = 0;	
		}
	}
	
}
