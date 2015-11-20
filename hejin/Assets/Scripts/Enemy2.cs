using UnityEngine;
using System.Collections;
/// <summary>
/// 第二种敌人（扔手雷的）
/// </summary>
public class Enemy2 : MonoBehaviour {
    private PlayerMove playMove;
    private int groundLayMask;
    public Transform throwPos;
    public GameObject EnemyBomb;
    public enum EnemyState { IDLE,WALK,KILL,DEAD,THROW}
    public EnemyState state = EnemyState.WALK;
    public GameObject hero;
    public SpriteRenderer enemy1SpriteRenderer;
    public Sprite[] enemy1WalkSprites;
    //private float enemy1Speed;
    private int spriteWalkCount;
    private int walkIndex=0;
    private float walkTimer = 0;
    private float animWalkSpeed = 10f;
    private float animWalkInterval = 0;

    public Sprite[] enemy1KillSprites;
    private int spriteKillCount;
    private float animKillSpeed = 6;
    private float animKillInterval = 0;
    private float killTimer = 0;
    private int killIndex;

    public Sprite[] enemy1ThrowSprites;
    private int spriteThrowCount;
    private float animThrowSpeed = 4f;
    private float animThrowInterval = 0;
    private float throwTimer = 0;
    private int throwIndex;

    public Sprite[] enemy1IdleSprites;
    private int spriteIdleCount;
    private float animIdleSpeed = 7f;
    private float animIdleInterval = 0;
    private float idleTimer = 0;
    private int idleIndex;

    public Sprite[] enemy1PartrolSprites;
    private int spritePartrolCount;
    private float animPartrolSpeed = 10f;
    private float animPartrolInterval = 0;
    private float partrolTimer = 0;
    private int partrolIndex;

    public Sprite[] enemy1DeadSprites;
    private int spriteDeadCount;
    private float animDeadSpeed = 10f;
    private float animDeadInterval = 0;
    private float deadTimer = 0;
    private int deadIndex;
    private Vector3 localScale;
	// Use this for initialization
    void Start()
    {      
        groundLayMask = LayerMask.GetMask("Ground");
        hero = GameObject.Find("Player");
        playMove = hero.GetComponent<PlayerMove>();
        animWalkInterval = 1f / animWalkSpeed;
        animKillInterval = 1f / animKillSpeed;
        animThrowInterval = 1f / animThrowSpeed;
        animIdleInterval = 1f / animIdleSpeed;
        animDeadInterval = 1f / animDeadSpeed;
        animPartrolInterval = 1f / animPartrolSpeed;
        spriteWalkCount = enemy1WalkSprites.Length;
        spriteKillCount = enemy1KillSprites.Length;
        spriteThrowCount = enemy1ThrowSprites.Length;
        spriteIdleCount = enemy1IdleSprites.Length;
        spriteDeadCount = enemy1DeadSprites.Length;
        spritePartrolCount = enemy1PartrolSprites.Length;
	}
	
	// Update is called once per frame
	void Update () {
        if (hero.transform.position.x > transform.position.x)
        {
            localScale = new Vector3(-1, 1, 1);
        }
        if (hero.transform.position.x < transform.position.x)
        {
            localScale = new Vector3(1, 1, 1);
        }
        transform.localScale = localScale;
        if (playMove.playBlood <= 0)
        {
            state = EnemyState.IDLE;
            Idle();
            return;
        }
        if(state == EnemyState.DEAD)
        {
            Dead();
            return;
        }
        float dis = Vector3.Distance(transform.position, hero.transform.position);
        if(dis>3f)
        {
            state = EnemyState.IDLE;
        }
        else if (dis < 0.5f)
        {
            state = EnemyState.KILL;
        }
        else
        {
            if (dis < 2)
            {
                state = EnemyState.THROW;
            }
            else
                state = EnemyState.WALK;
        }
        switch (state)
        {
            case EnemyState.IDLE:
                Idle();
                break;
            case EnemyState.WALK:
                Walk();
                break;
            case EnemyState.KILL:
                Kill();
                break;
            case EnemyState.THROW:
                Throw();
                break;
            default:
                break;
        }       
	}
    /// <summary>
    /// 站岗巡逻状态标识
    /// </summary>
    private bool isPartrol = false;
    private float idleKindTimer = 0;
    private bool partrolLeft = false;
    private void Idle()
    {
        idleKindTimer += Time.deltaTime;
        
        if(idleKindTimer>5)
        {
            idleKindTimer -= 5;
            if (Random.Range(0, 2) == 1)
            {
                partrolLeft = !partrolLeft;
            }
            if (Random.Range(0, 2) == 1)
            {
                isPartrol = !isPartrol;
            }
        }
        //站岗or巡逻
        if (!isPartrol)
        {
            try
            {
                idleTimer += Time.deltaTime;

                if (idleTimer > animIdleInterval)
                {
                    idleTimer -= animIdleInterval;
                    idleIndex++;
                    idleIndex %= spriteIdleCount;
                    enemy1SpriteRenderer.sprite = enemy1IdleSprites[idleIndex];
                }
            }
            catch(System.Exception ex)
            {
                print(ex.Message);
            }
        }
        else
        {
            partrolTimer += Time.deltaTime;

            if (partrolTimer > animPartrolInterval)
            {
                partrolTimer -= animPartrolInterval;
                partrolIndex++;
                partrolIndex %= spritePartrolCount;
                enemy1SpriteRenderer.sprite = enemy1PartrolSprites[partrolIndex];
            }
            if (partrolLeft)
            {
                transform.localScale = new Vector3(1, 1, 1);
                RaycastHit hitLeft;
                if (!Physics.Raycast(transform.position, Vector3.left, out hitLeft, 0.2f, groundLayMask))
                transform.Translate(Vector3.left * 1 * Time.deltaTime);
            }
            else
            {
                transform.localScale = new Vector3(-1, 1, 1);
                RaycastHit hitRight;
                if (!Physics.Raycast(transform.position, Vector3.right, out hitRight, 0.2f, groundLayMask))
                transform.Translate(Vector3.right * 1 * Time.deltaTime);
            }
        }
      
    }

    private void Dead()
    {
        deadTimer += Time.deltaTime;
        if (deadTimer > animDeadInterval)
        {
            deadTimer -= animDeadInterval;
            deadIndex++;
            deadIndex %= spriteDeadCount;
            if (deadIndex == 1)
            {
                AudioSource.PlayClipAtPoint(playMove.soundEnemyDie, new Vector3(0, 0, 0));
            }
            if (deadIndex == 0)
            {
                Destroy(gameObject);
            }
            enemy1SpriteRenderer.sprite = enemy1DeadSprites[deadIndex];
            
            
        }
    }
    private void Throw()
    {
        throwTimer += Time.deltaTime;

        if (throwTimer > animThrowInterval)
        {
            throwTimer -= animThrowInterval;
            throwIndex++;
            throwIndex %= spriteThrowCount;
            enemy1SpriteRenderer.sprite = enemy1ThrowSprites[throwIndex];
            if (throwIndex == 1)
            {
                if (localScale.x == -1)
                    EnemyBomb.GetComponent<EnemyBomb>().isLeft = false;
                else
                    EnemyBomb.GetComponent<EnemyBomb>().isLeft = true;
                Instantiate(EnemyBomb, new Vector3(throwPos.position.x, throwPos.position.y), Quaternion.Euler(0, 0, 0));

            }
        }
    }
    private void Kill()
    {
        killTimer += Time.deltaTime;

        if (killTimer > animKillInterval)
        {
            killTimer -= animKillInterval;
            killIndex++;
            killIndex %= spriteKillCount;
            enemy1SpriteRenderer.sprite = enemy1KillSprites[killIndex];
           
            if (killIndex == 2)
            {
                playMove.playBlood-=5;
                AudioSource.PlayClipAtPoint(playMove.soundKnife, new Vector3(0, 0, 0));
            }
        }
        if (partrolLeft)
        {
            transform.Translate(Vector3.left * 0.1f * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.right * 0.1f * Time.deltaTime);
        }
    }
    private void Walk()
    {
        walkTimer += Time.deltaTime;

        if (walkTimer > animWalkInterval)
         {
             walkTimer -= animWalkInterval;
             walkIndex++;
             walkIndex %= spriteWalkCount;
             enemy1SpriteRenderer.sprite = enemy1WalkSprites[walkIndex];
         }
         if (transform.position.x >= hero.transform.position.x)
         {
             transform.localScale = new Vector3(1, 1, 1);
             RaycastHit hitLeft;
             if (!Physics.Raycast(transform.position, Vector3.left, out hitLeft, 0.2f, groundLayMask))
             transform.Translate(Vector3.left * 1 * Time.deltaTime);
         }
         else
         {
             transform.localScale = new Vector3(-1, 1, 1);
             RaycastHit hitRight;
             if (!Physics.Raycast(transform.position, Vector3.right, out hitRight, 0.2f, groundLayMask))
             transform.Translate(Vector3.right * 1 * Time.deltaTime);
         }
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "projectile(Clone)" && state != EnemyState.DEAD)
        {
            state = EnemyState.DEAD;
            playMove.killCount++;

        }
    }

}
