using UnityEngine;
using System.Collections;

public class WorldTouch : MonoBehaviour
{

    // Use this for initialization
    bool isHold = false;
    Vector3 previuosPosition;
    bool isMove = false;
    public GameObject worldparent;
    public static int WorldIndex = 0;
    public GameObject[] botButton;
    public Sprite[] botButtonSprite;
    public GameObject title;
    public Sprite[] titlesprite;


    void Start()
    {
#if UNITY_IPHONE
      Application.targetFrameRate = 60;
#endif
        WorldIndex = PlayerPrefs.GetInt("WORLD", 0);
        worldsetposistion(WorldIndex);
        ButtonBotSetting(WorldIndex);

    }
    // Update is called once per frame
    void Update()
    {
        TouchProcess();
    }

    void TouchProcess()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isHold = true;
            isMove = false;
            previuosPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isHold = false;
            if (!isMove && TouchChecker(Input.mousePosition) != null)
                ClickProcess(TouchChecker(Input.mousePosition));


        }

        if (isHold)
        {
            StartCoroutine(refreshPreMousePosition());
            if (Mathf.Abs(Input.mousePosition.x - previuosPosition.x) > 20)
            {
                isMove = true;
                if (Input.mousePosition.x - previuosPosition.x < 0 && WorldIndex < 2)
                {
                    worldparent.GetComponent<WorldMove>().X = -5 * (WorldIndex + 1);
                    ButtonBotSetting(WorldIndex + 1);
                    PlayerPrefs.SetInt("WORLD", WorldIndex + 1);
                }
                else if (Input.mousePosition.x - previuosPosition.x > 0 && WorldIndex > 0)
                {
                    worldparent.GetComponent<WorldMove>().X = -5 * (WorldIndex - 1);
                    ButtonBotSetting(WorldIndex - 1);
                    PlayerPrefs.SetInt("WORLD", WorldIndex - 1);
                }
            }

        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.LoadLevel(GlobalConsts.SCENE_HOME);
        }

    }

    void ClickProcess(GameObject obj)
    {
        string name = obj.name;
        switch (name)
        {
            case "World1":
                WorldData.world = 0;
                SelectMapTouch.world = 1;
                Application.LoadLevel(GlobalConsts.SCENE_MAP_SELECT);
                break;
            case "World2":
                WorldData.world = 1;
                SelectMapTouch.world = 2;
                Application.LoadLevel(GlobalConsts.SCENE_MAP_SELECT);
                break;
            case "World3":
                WorldData.world = 2;
                SelectMapTouch.world = 3;
                Application.LoadLevel(GlobalConsts.SCENE_MAP_SELECT);
                break;
            case "back":
                StartCoroutine(waitback(obj));
                break;
        }


    }
    IEnumerator waitback(GameObject obj)
    {
        obj.GetComponent<Animator>().enabled = true;
        yield return new WaitForSeconds(0.15f);
        Application.LoadLevel(GlobalConsts.SCENE_HOME);
    }

    IEnumerator refreshPreMousePosition()
    {
        yield return new WaitForSeconds(0.4f);
        previuosPosition = Input.mousePosition;
    }

    GameObject TouchChecker(Vector3 mouseposition)
    {
        Vector3 wp = Camera.main.ScreenToWorldPoint(mouseposition);
        Vector2 touchPos = new Vector2(wp.x, wp.y);
        if (Physics2D.OverlapPoint(touchPos))
        {
            return Physics2D.OverlapPoint(touchPos).gameObject;
        }
        return null;
    }

    void ButtonBotSetting(int index)
    {
        botButton[0].GetComponent<SpriteRenderer>().sprite = botButtonSprite[0];
        botButton[1].GetComponent<SpriteRenderer>().sprite = botButtonSprite[0];
        botButton[2].GetComponent<SpriteRenderer>().sprite = botButtonSprite[0];

        botButton[index].GetComponent<SpriteRenderer>().sprite = botButtonSprite[1];
        title.GetComponent<SpriteRenderer>().sprite = titlesprite[index];

    }
    void worldsetposistion(int index)
    {
        worldparent.transform.localPosition = new Vector3(-index * 5f, -0.8f, 0);
    }
}
