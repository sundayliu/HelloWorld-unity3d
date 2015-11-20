using UnityEngine;
using System.Collections;

public class SelectMapTouch : MonoBehaviour
{

    bool isHold = false;
    Vector3 previuosPosition;
    bool isMove = false;
    public GameObject worldparent;
    public static int GroupIndex = 0;
    public GameObject[] botButton;
    public Sprite[] botButtonSprite;
    public static int world;

    void Start()
    {
#if UNITY_IPHONE
      Application.targetFrameRate = 60;
#endif
        world = WorldData.world + 1;
        GroupIndex = PlayerPrefs.GetInt("WORLD" + world + "GROUPMAP", 0);
        worldsetposistion(GroupIndex);
        ButtonBotSetting(GroupIndex);

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
                if (Input.mousePosition.x - previuosPosition.x < 0 && GroupIndex < 4)
                {
                    worldparent.GetComponent<GroupMove>().X = -5.5f * (GroupIndex + 1);
                    ButtonBotSetting(GroupIndex + 1);
                    PlayerPrefs.SetInt("WORLD" + world + "GROUPMAP", (GroupIndex + 1));
                }
                else if (Input.mousePosition.x - previuosPosition.x > 0 && GroupIndex > 0)
                {
                    worldparent.GetComponent<GroupMove>().X = -5.5f * (GroupIndex - 1);
                    ButtonBotSetting(GroupIndex - 1);
                    PlayerPrefs.SetInt("WORLD" + world + "GROUPMAP", (GroupIndex - 1));
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.LoadLevel(GlobalConsts.SCENE_WORD_SELECT);
        }
    }

    void ClickProcess(GameObject obj)
    {
        string name = obj.name;
        if(name.Contains("level1")){
            StartCoroutine(waitToloadlevel(obj));
        }
        else if (name.Contains("btn_back"))
        {
            StartCoroutine(waitback(obj));
        }
    }
    IEnumerator waitToloadlevel(GameObject obj)
    {
        obj.GetComponent<Animator>().enabled = true;
        yield return new WaitForSeconds(0.15f);
        MapLoader.MapPlayer = obj.GetComponent<level>().map;
        MapLoader.Mode = 1;
        Application.LoadLevel(GlobalConsts.SCENE_PLAY);
    }

    IEnumerator waitback(GameObject obj)
    {
        obj.GetComponent<Animator>().enabled = true;
        yield return new WaitForSeconds(0.2f);
        Application.LoadLevel(GlobalConsts.SCENE_WORD_SELECT);
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
        botButton[3].GetComponent<SpriteRenderer>().sprite = botButtonSprite[0];
        botButton[4].GetComponent<SpriteRenderer>().sprite = botButtonSprite[0];

        botButton[index].GetComponent<SpriteRenderer>().sprite = botButtonSprite[1];

    }
    void worldsetposistion(int index)
    {
        worldparent.transform.localPosition = new Vector3(-index * 5.5f, 0.09f, -1);
    }
}
