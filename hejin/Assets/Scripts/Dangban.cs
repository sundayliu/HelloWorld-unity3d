using UnityEngine;
using System.Collections;
/// <summary>
/// 汽车车后挡板动画控制
/// </summary>
public class Dangban : MonoBehaviour {
    public bool isStop=false;

    float angle;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
    void Update()
    {
        if (isStop && angle < 135)
        {
            angle += 360 * Time.deltaTime;
            transform.Rotate(Vector3.forward * 360 * Time.deltaTime);
        }
    }
}
