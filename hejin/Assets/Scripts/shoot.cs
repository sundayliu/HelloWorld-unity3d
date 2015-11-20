using UnityEngine;
using System.Collections;
public enum ShootDir
{
    UP,LEFT,RIGHT
}
/// <summary>
/// 攻击控制（近战，射击，扔雷）
/// </summary>
public class shoot : MonoBehaviour
{
    private float shootInterval = 0.2f;
    private float throwInterval = 1f;
    public PlayerMove playMove;
    private float shootTime = 0;
    private float throwTime = 0;
    public PlayerJump playJump;
    public PlayerGround playGround;
    public PlayerDown playDown;
    public bool isShooting = false;
    public bool isThrowing = false;
    void Start()
    {
        playMove = gameObject.GetComponent<PlayerMove>();
    }
  
    // Update is called once per frame
    void Update()
    {
        bool isAtk = false;
        if (playMove.isShooting && Time.time > shootTime + shootInterval)
        {
            isShooting = true;
            Ray ray1 = new Ray(gameObject.transform.position, Vector3.right);
            RaycastHit hit1;
            if (Physics.Raycast(ray1, out hit1, 1f))
            {
                if (hit1.transform.gameObject.name == "Enemy1(Clone)")
                {
                    isAtk = true;
                    hit1.transform.gameObject.GetComponent<Enemy1>().state = Enemy1.EnemyState.DEAD;
                }
                if (hit1.transform.gameObject.name == "Enemy2(Clone)")
                {
                    isAtk = true;
                    hit1.transform.gameObject.GetComponent<Enemy2>().state = Enemy2.EnemyState.DEAD;
                }
            }
            Ray ray2 = new Ray(gameObject.transform.position, Vector3.left);
            RaycastHit hit2;
            if (Physics.Raycast(ray2, out hit2, 1f))
            {
                if (hit2.transform.gameObject.name == "Enemy1(Clone)")
                {
                    isAtk = true;
                    hit2.transform.gameObject.GetComponent<Enemy1>().state = Enemy1.EnemyState.DEAD;
                }
                if (hit2.transform.gameObject.name == "Enemy2(Clone)" )
                {
                    isAtk = true;
                    hit2.transform.gameObject.GetComponent<Enemy2>().state = Enemy2.EnemyState.DEAD;
                }
            }
            if (!isAtk)
            {

                switch (playMove.status)
                {
                    case PlayerMove.PlayerStatus.GROUND:
                        playGround.Shoot();
                        break;
                    case PlayerMove.PlayerStatus.DOWN:
                        playDown.Shoot();
                        break;
                    case PlayerMove.PlayerStatus.JUMP:
                        playJump.Shoot();
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (playMove.status)
                {
                    case PlayerMove.PlayerStatus.GROUND:
                        playGround.isShoot = true;
                        playGround.bodyStatus = BodyStatus.Hor_Atk;
                        break;
                    case PlayerMove.PlayerStatus.DOWN:
                        playDown.isShoot = true;
                        playDown.bodyStatus = BodyStatus.Hor_Atk;
                        playDown.legStatus = LegStatus.DownShoot;
                        break;
                    case PlayerMove.PlayerStatus.JUMP:
                        playJump.isShoot = true;
                        playJump.bodyStatus = BodyStatus.Hor_Atk;
                        break;
                    default:
                        break;
                }
            }
            shootTime = Time.time;
        }
        else
        {
            //btnShootPressed = false;
        }
        if (playMove.isThrowing && Time.time > throwTime + throwInterval&&!playMove.btnUpPressed)
        {
            switch (playMove.status)
            {
                case PlayerMove.PlayerStatus.GROUND:
                    playGround.Throw();
                    break;
                case PlayerMove.PlayerStatus.DOWN:
                    playDown.Throw();
                    break;
                case PlayerMove.PlayerStatus.JUMP:
                    playJump.Throw();
                    break;
                default:
                    break;
            }
            throwTime = Time.time;
        }
        else
        {

        }
    }
}