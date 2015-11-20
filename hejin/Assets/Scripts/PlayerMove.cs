using UnityEngine;
using System.Collections;
using System;
/// <summary>
/// 主角控制（主类）
/// </summary>
public class PlayerMove : MonoBehaviour
{
    bool isDie = false;
    GameObject Jet;
    //
    public AudioClip soundKnife;
    public AudioClip soundHeroDie;
    public AudioClip soundEnemyDie;
    public AudioClip soundTieGuo;
    public AudioClip soundThrow;
    public AudioClip soundShoot;
    public AudioClip soundBoom;
    public AudioClip soundBoomMoving;
    public AudioClip jetOverBoom;
    //
    float cameraY = -0.5f;
    float cameraX = 0f;
    float heroX = 0;
    bool isStarted = false;
    public int killCount = 0;
    public int playBlood = 100;
    /// <summary>
    /// 主角所处状态
    /// </summary>
    public enum PlayerStatus
    {
        GROUND, DOWN, JUMP,DIE
    }
    public Camera MainCamera;
    public PlayerStatus status = PlayerStatus.JUMP;
    public float Speed = 3;
    public float JumpSpeed = 5;
    public PlayerJump playJump;
    public PlayerGround playGround;
    public PlayerDown playDown;
    public PlayerDie playDie;
    public shoot shoot;
    private bool isGround = true;
    private int groundLayMask;
    private bool isDown = false;
    private UISlider bloodProgressbar;
    private UIWidget bloodProgressbarColor;
    public bool btnRightPressed = false;
    public bool btnLeftPressed = false;
    public bool btnJumpPressed = false;
    public bool btnDownPressed = false;
    public bool btnUpPressed = false;
    private GameObject overWindow;
    private GameObject result;
    float maxDistance = 2;
   public bool isJetDestroy = false;
    public bool isRightDir=true;
    public bool isShooting = false;
    public bool isThrowing = false;
    void Start()
    {
        Jet = GameObject.Find("Jet");
        Jet.SetActive(false);
        groundLayMask = LayerMask.GetMask("Ground");
        bloodProgressbar = (GameObject.Find("blood")).GetComponent<UISlider>();
        bloodProgressbarColor = (GameObject.Find("Foreground")).GetComponent<UIWidget>();
        overWindow = GameObject.Find("gameOverWindow");
        result = GameObject.Find("GameResult");
        overWindow.SetActive(false);
        gameObject.SetActive(false);
        GameObject btnJump = GameObject.Find("btnJump");
        UIEventListener.Get(btnJump).onPress = btnJumpPress;
        GameObject btnShoot = GameObject.Find("btnShoot");
        UIEventListener.Get(btnShoot).onPress = btnShootPress;
        GameObject btnThrow = GameObject.Find("btnThrow");
        UIEventListener.Get(btnThrow).onPress = btnThrowPress;

        shoot = gameObject.GetComponent<shoot>();
    }
    void btnShootPress(GameObject button, bool isPressed)
    {
        isShooting = isPressed;

        shoot.isShooting = isShooting;
    }
    void btnThrowPress(GameObject button, bool isPressed)
    {
        isThrowing = isPressed;
        shoot.isThrowing = isThrowing;
    }
    void btnJumpPress(GameObject button, bool a)
    {
        btnJumpPressed = a;
    }
    // Update is called once per frame
    void Update()
    {
        #region 死亡判定
        if (playBlood < 1)
        {
            dieControls();
            return;
        }
        #endregion

        //下面所有控制为非死亡状态控制和判定

        #region 主角运动跟踪用于控制主角摄像头的不可回退
        heroX = transform.position.x;
        if (heroX > maxDistance)
        {
            maxDistance = maxDistance < 19.78f ? heroX : 19.78f;
        }

        //摄像头控制
        if (heroX < 19.78)
        {
            if (heroX < 15 && maxDistance <= 15)
            {
                cameraY = -0.5f;
            }
            else
                if (heroX < 19.78 && MainCamera.transform.position.y <= 2)
                {
                    MainCamera.transform.Translate(Vector3.up * 2 * Time.deltaTime);
                    cameraY = MainCamera.transform.position.y > cameraY ? MainCamera.transform.position.y : cameraY;
                }
                else
                {
                    //cameraY = gameObject.transform.position.y + 0.6095399f;
                }
            MainCamera.transform.position = new Vector3(maxDistance, cameraY, -10);
        }
        else
        {

            if (Jet != null)//直升机出现
            {
                Jet.SetActive(true);
            }
            else
            {
                isJetDestroy = true;
                result.GetComponent<UILabel>().text = "Mission success";
                overWindow.SetActive(true);
            }
        }
        #endregion

        #region 行走控制
        if (!btnRightPressed && !btnLeftPressed)//静止
        {
            playDown.legStatus = LegStatus.Idle;
            playGround.legStatus = LegStatus.Idle;
        }
        else
        {   //行走
            playDown.legStatus = LegStatus.Walk;
            playGround.legStatus = LegStatus.Walk;
            if (btnRightPressed)
            {
                isRightDir = true;
                transform.localScale = new Vector3(-1, 1, 1);

                if (heroX >= 19.78 && !isJetDestroy)
                { }
                else
                {
                    RaycastHit hitRight;
                    if (!Physics.Raycast(transform.position, Vector3.right, out hitRight, 0.1f, groundLayMask))
                    {
                        transform.Translate(Vector3.right * 2f * Time.deltaTime);
                    }
                }
            }
            else
            {
                isRightDir = false;
                transform.localScale = new Vector3(1, 1, 1);

                RaycastHit hitLeft;
                if (!Physics.Raycast(transform.position, Vector3.left, out hitLeft, 0.1f, groundLayMask) && heroX > maxDistance - 2.2f)
                    transform.Translate(Vector3.left * 2f * Time.deltaTime);
            }
        }
        if (status == PlayerStatus.GROUND)
        {
            if (btnUpPressed)
            {
                if (playGround.isShoot)
                {

                }
                    //playGround.bodyStatus = BodyStatus.Up_Shoot;
                else
                    playGround.bodyStatus = BodyStatus.Up_Idle;
            }
            else
            {
                if (btnDownPressed)
                {
                    status = PlayerStatus.DOWN;
                }
                else
                {
                    if (playGround.isShoot)
                    {
                        //射击还是近身攻击有后面shoot判断
                    }
                    else
                        if (playGround.isThrow)
                            playGround.bodyStatus = BodyStatus.Hor_Throw;
                        else
                            playGround.bodyStatus = BodyStatus.Hor_Idle;
                }
            }
        }
        if(status== PlayerStatus.DOWN)
        {
            if (btnUpPressed)
            {
                status = PlayerStatus.JUMP;
            }
            else
            {
                if(playDown.isShoot)
                {
                    playDown.legStatus = LegStatus.DownShoot;
                }
                else
                    if (playDown.isThrow)
                    {
                        playDown.bodyStatus = BodyStatus.Hor_Throw;
                        playDown.legStatus = LegStatus.DownShoot;
                    }
                    else
                        playDown.bodyStatus = BodyStatus.Hor_Idle;
            }
        }
        if (status == PlayerStatus.JUMP)
        {
            if (btnUpPressed)
            {
                if (playJump.isShoot)
                {
                    playJump.bodyStatus = BodyStatus.Up_Shoot;
                }
                else
                    playJump.bodyStatus = BodyStatus.Up_Idle;
            }
            else
                if (btnDownPressed)
                {
                    if (playJump.isShoot)
                    {
                        playJump.bodyStatus = BodyStatus.Down_Shoot;
                    }
                    else
                        playJump.bodyStatus = BodyStatus.Down_Idle;
                }
                else
                {
                    if (playJump.isShoot)
                    {

                    }
                    else
                        if (playJump.isThrow)
                        {
                            playJump.bodyStatus = BodyStatus.Hor_Throw;
                        }
                        else
                            playJump.bodyStatus = BodyStatus.Hor_Idle;
                }
        }
        #endregion

        #region 控制左右距离阻挡刚体距离
        RaycastHit Right;
        if (Physics.Raycast(transform.position, Vector3.right, out Right, 0.2f, groundLayMask))
        {
            transform.Translate(Vector3.left * 0.5f * Time.deltaTime);
        }
        RaycastHit Left;
        if (Physics.Raycast(transform.position, Vector3.left, out Left, 0.2f, groundLayMask))
        {
            transform.Translate(Vector3.right * 0.5f * Time.deltaTime);
        }
        #endregion

        //if (!isShooting && !playGround.isShoot && !playJump.isShoot && !playDown.isShoot)//射击完毕恢复状态
        //{
        //    if (btnUpPressed)
        //    {
        //        playGround.bodyStatus = BodyStatus.Up_Idle;
        //    }
        //    else

        //        if (btnDownPressed)
        //        {
        //            //playDown.bodyStatus = BodyStatus.Up_Idle;
        //        }
        //        else
        //        {
        //            playGround.bodyStatus = BodyStatus.Hor_Idle;
        //            print("PP");
        //        }
        //}
        #region 跳跃判断
        float h = Input.GetAxis("Horizontal");
        Vector3 v = GetComponent<Rigidbody>().velocity;
        GetComponent<Rigidbody>().velocity = new Vector3(h * Speed, v.y, v.z);
        v = GetComponent<Rigidbody>().velocity;
        RaycastHit hit;
        isGround = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, out hit, 0.2f, groundLayMask);

        if (!isGround)
        {
            status = PlayerStatus.JUMP;
        }
        else
        {
            if (btnDownPressed)
            {
                status = PlayerStatus.DOWN;

            }
            else
            {
                status = PlayerStatus.GROUND;
            }
        }
        //控制跳跃
        if (Input.GetKeyDown(KeyCode.K) && isGround)
        {
            GetComponent<Rigidbody>().velocity = new Vector3(v.x, JumpSpeed, v.z);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            btnDownPressed = true;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            btnUpPressed = true;
        }
        //跳跃判定
        if (btnJumpPressed && isGround)
        {
            Vector3 v0 = GetComponent<Rigidbody>().velocity;
            GetComponent<Rigidbody>().velocity = new Vector3(v0.x, JumpSpeed, v0.z);
        }
        #endregion

        #region 根据状态启用游戏状态对象
        switch (status)
        {

            case PlayerStatus.JUMP:
                playDie.gameObject.SetActive(false);
                playJump.gameObject.SetActive(true);
                playGround.gameObject.SetActive(false);
                playDown.gameObject.SetActive(false);
                break;
            case PlayerStatus.GROUND:
                playDie.gameObject.SetActive(false);
                playJump.gameObject.SetActive(false);
                playGround.gameObject.SetActive(true);
                playDown.gameObject.SetActive(false);
                break;
            case PlayerStatus.DOWN:
                playDie.gameObject.SetActive(false);
                playJump.gameObject.SetActive(false);
                playGround.gameObject.SetActive(false);
                playDown.gameObject.SetActive(true);
                break;
        }
        #endregion
    }
    /// <summary>
    /// 死亡控制 执行一遍
    /// </summary>
    private void dieControls()
    {
        if (!isDie)
        {
            AudioSource.PlayClipAtPoint(soundHeroDie, new Vector3(0, 0, 0));
            bloodProgressbar.value = 0;
            status = PlayerStatus.DIE;
            playJump.gameObject.SetActive(false);
            playGround.gameObject.SetActive(false);
            playDown.gameObject.SetActive(false);
            playDie.gameObject.SetActive(true);
            result.GetComponent<UILabel>().text = "Try again";
            overWindow.SetActive(true);
            GameObject.Find("OverBG").GetComponent<UISprite>().color = new Color(1, 0, 0, 0.5f);
            isDie = true;
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Cube")
        {
            isStarted = true;
        }
    }

    public void OnGUI()
    {
        bloodProgressbar.value = (float)playBlood / 100;
        if (playBlood > 60)
            bloodProgressbarColor.color = Color.green;
        else if (playBlood > 20)
            bloodProgressbarColor.color = Color.yellow;
        else
        {
            bloodProgressbarColor.color = Color.red;
        }
    }



}
