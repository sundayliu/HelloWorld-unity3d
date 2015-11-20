using UnityEngine;
using System.Collections;
/// <summary>
/// 游戏开始汽车控制
/// </summary>
public class Car : MonoBehaviour
{
    private GameObject hero;
    private PlayerMove playMove;
    public Dangban Dangban;
    public Wheel[] Wheels;
    public Vector3 TargetPosStop;
    public Vector3 TargetPosMissing;
    bool isStoping = false;
    bool isHappened = false;
    float timer = 0;
    // Use this for initialization
    void Start()
    {
        hero = GameObject.Find("Player");
        playMove = hero.GetComponent<PlayerMove>();
        Destroy(gameObject, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer < 1)
            Move1();
        else if (timer < 2)
            CarStop();
        else
            CarMove();
        if(transform.position.x>4)
        {
            playMove.btnRightPressed = false;
        }
    }
    void Move1()
    {
        transform.Translate(Vector3.right * 8 * Time.deltaTime);
    }
    void PlayAudio()
    {

        if (!isStoping)
        {
            gameObject.GetComponent<AudioSource>().Play();
            isStoping = true;
        }
    }
    void CarStop()
    {
        if (isHappened)
            return;
        PlayAudio();
        playMove.btnLeftPressed = true;
        foreach (Wheel wheel in Wheels)
        {
            wheel.CarStop();
        }

        hero.SetActive(true);
        Dangban.isStop = true;
    }
    void CarMove()
    {
        playMove.btnLeftPressed = false;
        playMove.btnRightPressed = true;
        Destroy(GameObject.Find("startCube"));
        transform.Translate(Vector3.right * 5 * Time.deltaTime);
        foreach (Wheel wheel in Wheels)
        {
            wheel.CarMove();
        }     
    }
}
