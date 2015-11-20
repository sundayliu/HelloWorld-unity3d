using UnityEngine;
using System.Collections;
/// <summary>
/// 直升机炸弹
/// </summary>
public class BoomMove : MonoBehaviour {
    GameObject hero;
    private PlayerMove playerMove;
    public SpriteRenderer boomRenderer;
    public Sprite[] boomSprites;
    private float boomAnimSpeed = 5;
    private float boomAnimInterval = 0;
    private float timeboomAnim = 0;
    private int boomAnimIndex = 0;
    private int boomSpriteCount = 0;

    public Sprite[] boomedSprites;
    private float boomedAnimSpeed = 20;
    private float boomedAnimInterval = 0;
    private float timeboomedAnim = 0;
    private int boomedAnimIndex = 0;
    private int boomedSpriteCount = 0;
    bool isBoomed = false;
    bool isBoomHero = true;
    Vector3 v;
	// Use this for initialization
    void Start()
    {
        hero = GameObject.Find("Player");
        if (hero == null)
            return;
        Destroy(gameObject, 2f);
        playerMove = hero.GetComponent<PlayerMove>();
        boomAnimInterval = 1 / boomAnimSpeed;
        boomSpriteCount = boomSprites.Length;

        boomedAnimInterval = 1 / boomedAnimSpeed;
        boomedSpriteCount = boomedSprites.Length;
        AudioSource.PlayClipAtPoint(playerMove.soundBoomMoving, new Vector3(0, 0, 0));
    }
	
	// Update is called once per frame
	void Update () {
        if (hero == null)
            return;
       
        if (!isBoomed)
        {
            timeboomAnim += Time.deltaTime;
            if (timeboomAnim > boomAnimInterval)
            {
                timeboomAnim -= boomAnimInterval;
                boomAnimIndex++;
                boomAnimIndex %= boomSpriteCount;
                boomRenderer.sprite = boomSprites[boomAnimIndex];
            }
        }
        else
        {
            transform.transform.position = v;
            timeboomedAnim += Time.deltaTime;
            if (timeboomedAnim > boomedAnimInterval)
            {
                timeboomedAnim -= boomedAnimInterval;
                boomedAnimIndex++;
                boomedAnimIndex %= boomedSpriteCount;
                boomRenderer.sprite = boomedSprites[boomedAnimIndex];
                if (isBoomHero)
                {
                    playerMove.playBlood -= 30;
                    isBoomHero = false;
                }
                if(boomedAnimIndex==boomedSpriteCount-1)
                {
                    Destroy(gameObject);
                }
            }
        }
	
	}
    
    public void OnCollisionEnter(Collision collision)
    {
        if (hero == null)
            return;
        AudioSource.PlayClipAtPoint(playerMove.soundThrow, hero.transform.position);
        isBoomed = true;
        v =new Vector3(transform.transform.position.x,transform.transform.position.y+0.5f,transform.transform.position.z);
        if(collision.gameObject.name=="Player")
        {
            //isBoomHero = true;
        }
        else
        {
            isBoomHero = false;
        }
    }
    
}
