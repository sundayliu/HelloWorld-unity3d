using UnityEngine;
using System.Collections;

public class StartCamera : MonoBehaviour {
    public GameObject Enmy1Prefabs;
    public GameObject Enmy2Prefabs;
	// Use this for initialization
    void Start()
    {
        Invoke("CreateEnemy", 4f);
    }
	// Update is called once per frame
	void Update () {
	
	}
    void CreateEnemy()
    {
        Instantiate(Enmy1Prefabs, new Vector3(Random.Range(5, 20), 3f), new Quaternion());
        Instantiate(Enmy1Prefabs, new Vector3(Random.Range(5, 20), 3f), new Quaternion());
        Instantiate(Enmy1Prefabs, new Vector3(Random.Range(5, 20), 3f), new Quaternion());
        Instantiate(Enmy1Prefabs, new Vector3(Random.Range(5, 20), 3f), new Quaternion());
        Instantiate(Enmy1Prefabs, new Vector3(Random.Range(5, 20), 3f), new Quaternion());
        Instantiate(Enmy2Prefabs, new Vector3(Random.Range(5, 20), 3f), new Quaternion());
        Instantiate(Enmy2Prefabs, new Vector3(Random.Range(5, 20), 3f), new Quaternion());
        Instantiate(Enmy2Prefabs, new Vector3(Random.Range(5, 20), 3f), new Quaternion());
        Instantiate(Enmy2Prefabs, new Vector3(Random.Range(5, 20), 3f), new Quaternion());
    }
    public void TryAgain()
    {
        Application.LoadLevel(0);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

   
}
