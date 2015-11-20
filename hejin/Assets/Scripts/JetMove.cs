using UnityEngine;
using System.Collections;
/// <summary>
/// 直升机状态
/// </summary>
public class JetMove : MonoBehaviour {
    public GameObject jet;
    public GameObject jetUp;
    public GameObject jetBody;
    GameObject hero;
    private PlayerMove playerMove;
    public GameObject throwPos;
    public GameObject boom;
    public SpriteRenderer jetUpRenderer;
    public Sprite[] jetUpSprites;
    private float upAnimSpeed = 10;
    private float upAnimInterval = 0;
    private float timeUpAnim = 0;
    private int upAnimIndex = 0;
    private int upSpriteCount = 0;

    public Sprite[] jetOverSprites;
    private float OverAnimSpeed = 20;
    private float OverAnimInterval = 0;
    private float timeOverAnim = 0;
    private int OverAnimIndex = 0;
    private int OverSpriteCount = 0;

    private float movingInTime=0;
    public bool isCanThrowBoom = true;
    private float checkTime = 0;
    private float colorChangedTime = 0;
    private float throwTime = 0;
    private float heroX = 19;
    public int jetBlood = 100;
    private bool isOver = false;
    bool isLoading = false;
	// Use this for initialization
	void Start () {
        hero=GameObject.Find("Player");
        if (hero == null)
            return;
        playerMove = hero.GetComponent<PlayerMove>();
        upAnimInterval = 1 / upAnimSpeed;
        upSpriteCount = jetUpSprites.Length;
        OverAnimInterval = 1 / OverAnimSpeed;
        OverSpriteCount = jetOverSprites.Length;
	}
	
	// Update is called once per frame
    void Update()
    {
        if (movingInTime < 1f)
        {
            jet.transform.Translate(Vector3.down * 1.5f * Time.deltaTime);
            movingInTime += Time.deltaTime;
            isCanThrowBoom = false;
        }

        if (isLoading)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 30);
            jet.transform.Translate(Vector3.down * 2 * Time.deltaTime);
        }
        if (isOver)
        {
            jet.transform.position = overPos;
            jetBody.GetComponent<SpriteRenderer>().color = Color.white;
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            timeOverAnim += Time.deltaTime;
            if (timeOverAnim > OverAnimInterval)
            {
                timeOverAnim -= OverAnimInterval;
                OverAnimIndex++;
                OverAnimIndex %= OverSpriteCount;
                jetBody.GetComponent<SpriteRenderer>().sprite = jetOverSprites[OverAnimIndex];
                if (OverAnimIndex == OverSpriteCount - 1)
                {
                    playerMove.isJetDestroy = true;
                    Destroy(jet);
                }
            }


        }
        if (!isLoading && !isOver)
        {
            timeUpAnim += Time.deltaTime;
            if (timeUpAnim > upAnimInterval)
            {
                timeUpAnim -= upAnimInterval;
                upAnimIndex++;
                upAnimIndex %= upSpriteCount;
                jetUpRenderer.sprite = jetUpSprites[upAnimIndex];
            }
            if (isCanThrowBoom && playerMove.playBlood>0)
            {
                Instantiate(boom, throwPos.transform.position, Quaternion.Euler(0, 0, 0));
                isCanThrowBoom = false;
            }
            throwTime += Time.deltaTime;
            if (throwTime > 1)
            {
                throwTime -= 1;

                isCanThrowBoom = true;
            }
            checkTime += Time.deltaTime;
            if (checkTime > 2 && playerMove.playBlood > 0)
            {
                checkTime -= 2;
                heroX = hero.transform.position.x;
            }
            colorChangedTime += Time.deltaTime;
            if (colorChangedTime > 0.3f)
            {
                colorChangedTime -= 0.3f;
                jetBody.GetComponent<SpriteRenderer>().color = Color.white;
                jetUp.GetComponent<SpriteRenderer>().color = Color.white;
            }
            if (heroX > transform.position.x)
            {
                gameObject.transform.Translate(Vector3.right * 2 * Time.deltaTime);
            }
            if (heroX < transform.position.x)
            {
                gameObject.transform.Translate(Vector3.left * 2 * Time.deltaTime);
            }
        }
        if(playerMove.playBlood<=0)
        {
            jet.transform.Translate(Vector3.up * 0.5f * Time.deltaTime);
        }
    }
    Vector3 overPos;
    public void OnCollisionEnter(Collision collision)
    {
        if (hero == null)
            return;
        jetBody.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0, 50);
        jetUp.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0, 50);
        jetBlood -= 5;
        if (jetBlood <= 0)
        {
          isLoading   = true;

        }
        if (collision.gameObject.name == "Cube")
        {
            overPos = jet.transform.position;
            Destroy(GetComponent<Rigidbody>());
            isLoading = false;
            isOver = true;
            jetBody.GetComponent<SpriteRenderer>().sprite = null;
            jetUp.GetComponent<SpriteRenderer>().sprite = null;
            AudioSource.PlayClipAtPoint(playerMove.jetOverBoom, hero.transform.position);
        }
    }
    
}
