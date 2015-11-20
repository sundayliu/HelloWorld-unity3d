using UnityEngine;
using System.Collections;
/// <summary>
/// 子弹
/// </summary>
public class projectile : MonoBehaviour {
    public Sprite spritHit;
	// Use this for initialization
	void Start () {
        Destroy(gameObject, 1);
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(Vector3.left * 6 * Time.deltaTime);
	}

    public void OnCollisionEnter(Collision collision)
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = spritHit;
        transform.localRotation = Quaternion.Euler(0, 0, 0);
        if (collision.gameObject.name != "Player")
            Destroy(gameObject,0.02f);
    }
    
}
