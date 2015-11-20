using UnityEngine;
using System.Collections;
/// <summary>
/// 汽车车轮
/// </summary>
public class Wheel : MonoBehaviour {
    bool isStop = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
    void Update()
    {
        if (!isStop)
            transform.Rotate(-Vector3.forward * 600 * Time.deltaTime);
    }
    public void CarStop()
    {
        isStop = true;
    }
    public void CarMove()
    {
        isStop = false;
    }
}
