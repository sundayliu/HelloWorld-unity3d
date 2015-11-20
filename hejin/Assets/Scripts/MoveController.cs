using UnityEngine;
using System.Collections;

public class MoveController : MonoBehaviour {

    void OnEnable()
    {
        EasyJoystick.On_JoystickMove += OnJoystickMove;
        EasyJoystick.On_JoystickMoveEnd += OnJoystickMoveEnd;
    }

    void OnDisable()
    {
        EasyJoystick.On_JoystickMove -= OnJoystickMove;
        EasyJoystick.On_JoystickMoveEnd -= OnJoystickMoveEnd;
    }

    void OnDestroy()
    {
        EasyJoystick.On_JoystickMove -= OnJoystickMove;
        EasyJoystick.On_JoystickMoveEnd -= OnJoystickMoveEnd;
    }


    void OnJoystickMoveEnd(MovingJoystick move)
    {
        if (move.joystickName == "MoveJoystick")
        {
            gameObject.GetComponent<PlayerMove>().btnLeftPressed = false;
            gameObject.GetComponent<PlayerMove>().btnRightPressed = false;
            gameObject.GetComponent<PlayerMove>().btnDownPressed = false;
            gameObject.GetComponent<PlayerMove>().btnUpPressed = false;
            //animation.CrossFade("idle");
        }
    }
    void OnJoystickMove(MovingJoystick move)
    {
        if (move.joystickName != "MoveJoystick")
        {
            return;
        }


        float joyPositionX = move.joystickAxis.x;
        float joyPositionY = move.joystickAxis.y;

        if (joyPositionY != 0 || joyPositionX != 0)
        {
            //设置角色的朝向（朝向当前坐标+摇杆偏移量）
            //transform.LookAt(new Vector3(transform.position.x + joyPositionX, transform.position.y, transform.position.z + joyPositionY));
            //移动玩家的位置（按朝向位置移动）
            //transform.Translate(Vector3.forward * Time.deltaTime * 5);
            //播放奔跑动画
            //animation.CrossFade("run");
            //if (joyPositionX > 0)
            //{
            //    gameObject.GetComponent<PlayerMove>().btnLeftPressed = false;
            //    gameObject.GetComponent<PlayerMove>().btnRightPressed = true;
            //    gameObject.GetComponent<PlayerMove>().isRightDir = true;
            //    //transform.Translate(Vector3.right * Time.deltaTime * 1);
            //}
            //else
            //{
            //    gameObject.GetComponent<PlayerMove>().btnRightPressed = false;
            //    gameObject.GetComponent<PlayerMove>().btnLeftPressed = true;
            //    gameObject.GetComponent<PlayerMove>().isRightDir = false;
            //    //transform.Translate(Vector3.left * Time.deltaTime * 1);
            //}
            //if (joyPositionY > 0.4)
            //{
            //    //print("Y:" + joyPositionY);
            //    gameObject.GetComponent<PlayerMove>().btnDownPressed = false;
            //    gameObject.GetComponent<PlayerMove>().btnUpPressed = true;
            //}
            //else
            //{
            //    if (joyPositionY < -0.4)
            //    {
            //        gameObject.GetComponent<PlayerMove>().btnUpPressed = false;
            //        gameObject.GetComponent<PlayerMove>().btnDownPressed = true;
            //    }
            //    else
            //    {
            //        gameObject.GetComponent<PlayerMove>().btnDownPressed = false;
            //        gameObject.GetComponent<PlayerMove>().btnUpPressed = false;
            //    }
            //}
            ////xin
            if (Mathf.Abs(joyPositionY) < 0.4)
            {
                gameObject.GetComponent<PlayerMove>().btnDownPressed = false;
                gameObject.GetComponent<PlayerMove>().btnUpPressed = false;
                if (joyPositionX > 0)
                {
                    gameObject.GetComponent<PlayerMove>().btnLeftPressed = false;
                    gameObject.GetComponent<PlayerMove>().btnRightPressed = true;
                    gameObject.GetComponent<PlayerMove>().isRightDir = true;
                }
                else
                {
                    gameObject.GetComponent<PlayerMove>().btnRightPressed = false;
                    gameObject.GetComponent<PlayerMove>().btnLeftPressed = true;
                    gameObject.GetComponent<PlayerMove>().isRightDir = false;
                }
            }
            else
            {
                if (joyPositionY > 0.4)
                {
                    gameObject.GetComponent<PlayerMove>().btnDownPressed = false;
                    gameObject.GetComponent<PlayerMove>().btnUpPressed = true;
                    if (joyPositionX > 0.2)
                    {
                        gameObject.GetComponent<PlayerMove>().btnLeftPressed = false;
                        gameObject.GetComponent<PlayerMove>().btnRightPressed = true;
                        gameObject.GetComponent<PlayerMove>().isRightDir = true;
                    }
                    else
                        if (joyPositionX < -0.2)
                        {
                            gameObject.GetComponent<PlayerMove>().btnRightPressed = false;
                            gameObject.GetComponent<PlayerMove>().btnLeftPressed = true;
                            gameObject.GetComponent<PlayerMove>().isRightDir = false;
                        }
                        else
                        {
                            gameObject.GetComponent<PlayerMove>().btnRightPressed = false;
                            gameObject.GetComponent<PlayerMove>().btnLeftPressed = false;
                        }
                }
                else
                {
                    if (joyPositionY < -0.4)
                    {
                        gameObject.GetComponent<PlayerMove>().btnUpPressed = false;
                        gameObject.GetComponent<PlayerMove>().btnDownPressed = true;
                        if (joyPositionX > 0.2)
                        {
                            gameObject.GetComponent<PlayerMove>().btnLeftPressed = false;
                            gameObject.GetComponent<PlayerMove>().btnRightPressed = true;
                            gameObject.GetComponent<PlayerMove>().isRightDir = true;
                        }
                        else
                            if (joyPositionX < -0.2)
                            {
                                gameObject.GetComponent<PlayerMove>().btnRightPressed = false;
                                gameObject.GetComponent<PlayerMove>().btnLeftPressed = true;
                                gameObject.GetComponent<PlayerMove>().isRightDir = false;
                            }
                            else
                            {
                                gameObject.GetComponent<PlayerMove>().btnRightPressed = false;
                                gameObject.GetComponent<PlayerMove>().btnLeftPressed = false;
                            }
                    }
                }
            }

        }
    }
}
