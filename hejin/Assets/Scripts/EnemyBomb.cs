using UnityEngine;
using System.Collections;
/// <summary>
/// 敌人扔的手雷
/// </summary>
public class EnemyBomb : MonoBehaviour
{
    private PlayerMove playMove;
    private GameObject hero;
    public SpriteRenderer bombSpriteRenderer;
    public Sprite spriteBomb0;
    public Sprite[] spriteBomb1;
    public bool isLeft = false;
    bool isBombed = false;
    float i = 1;
	// Use this for initialization
    void Start()
    {
        Destroy(gameObject, 3);
        hero = GameObject.Find("Player");
        if (hero == null)
            return;
        playMove = hero.GetComponent<PlayerMove>();
        GetComponent<Rigidbody>().useGravity = false;
        float dis = Vector3.Distance(transform.position, hero.transform.position);
        i = dis / 1.6f;
        if (isLeft)
            GetComponent<Rigidbody>().AddForce(new Vector3(-10 * i, 20 * i));
        else
            GetComponent<Rigidbody>().AddForce(new Vector3(10 * i, 20 * i));


        animKillInterval = 1f / animKillSpeed;
        spriteKillCount = spriteBomb1.Length;
    }
	
	// Update is called once per frame
    void Update()
    {
        if (GetComponent<Rigidbody>() != null)
        {
            GetComponent<Rigidbody>().AddForce(new Vector3(0, -1));
        }

        if (!isBombed)
        {
            //bombSpriteRenderer.sprite = spriteBomb0;
            transform.Rotate(0, 0, 360 * Time.deltaTime);
        }
        else
        {
            transform.position = v;
            killTimer += Time.deltaTime;

            if (killTimer > animKillInterval)
            {
                killTimer -= animKillInterval;
                killIndex++;
                killIndex %= spriteKillCount;
                bombSpriteRenderer.sprite = spriteBomb1[killIndex];
                transform.rotation = Quaternion.Euler(0, 0, 0);
                if(killIndex==spriteKillCount-1)
                { Destroy(gameObject); }
            }
        }
    }
    Vector3 v;
    bool isEntered = false;
    public void OnCollisionEnter(Collision collision)
    {
        if (isEntered)
            return;
       playMove.playBlood -= 10;
        isEntered = true;
        if (hero != null)
            AudioSource.PlayClipAtPoint(playMove.soundThrow, new Vector3(0, 0, 0));
        v = transform.position;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        if (collision.gameObject.name != "Enemy2(Clone)" || collision.gameObject.name != "Enemy1(Clone)")
        {
            bombSpriteRenderer.sprite = null;
            isBombed = true;
        }
    }
    private void PlaySound()
    {
        GetComponent<AudioSource>().Play();
    }


    private int spriteKillCount;
    private float animKillSpeed = 15f;
    private float animKillInterval = 0;
    private float killTimer = 0;
    private int killIndex;
}
