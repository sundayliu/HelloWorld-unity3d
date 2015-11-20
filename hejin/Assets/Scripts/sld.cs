using UnityEngine;
using System.Collections;

public class sld : MonoBehaviour {

    private PlayerMove playerMove;
    public SpriteRenderer bombSpriteRenderer;
    public Sprite spriteBomb0;
    public Sprite[] spriteBomb1;
    bool isBombed = false;
    float i = 12;
    // Use this for initialization
    void Start()
    {
        Destroy(gameObject, 3);
        playerMove = GameObject.Find("Player").GetComponent<PlayerMove>();
        if (!playerMove.isRightDir)
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
                if (killIndex == spriteKillCount - 1)
                { Destroy(gameObject); }
            }
        }
    }
    Vector3 v;
    public void OnCollisionEnter(Collision collision)
    {
        AudioSource.PlayClipAtPoint(playerMove.soundThrow, new Vector3(0, 0, 0));
        v = transform.position;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        bombSpriteRenderer.sprite = null;
        isBombed = true;
        GameObject g = collision.gameObject;
        if (g.name == "Enemy1(Clone)")
        {
            g.GetComponent<Enemy1>().state = Enemy1.EnemyState.DEAD;
        }
        if (g.name == "Enemy2(Clone)")
        {
            g.GetComponent<Enemy2>().state = Enemy2.EnemyState.DEAD;
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
