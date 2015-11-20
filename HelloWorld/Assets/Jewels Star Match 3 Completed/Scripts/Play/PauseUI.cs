using UnityEngine;
using System.Collections;

public class PauseUI : MonoBehaviour
{

    public GameObject[] Button;
    public Sprite[] ButtonSprite;
    public GameObject pauseUI;
    public GameObject PlayingUI;
    public GameObject br;
    bool isHold = false;
    bool isSelect = false;
    string _name;
    // Use this for initialization
    void Start()
    {
        Editor.down = true;
        Time.timeScale = 0;
        Menu.isRun = false;
        if (MapLoader.Mode == 0)
        {
            Button[2].SetActive(false);
            Button[0].transform.localPosition = new Vector3(0.6f, 0, 0);
            Button[1].transform.localPosition = new Vector3(-0.6f, 0, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            isHold = true;

        if (Input.GetMouseButtonUp(0))
        {
            isHold = false;
            isSelect = false;
            if (TouchChecker(Input.mousePosition) != null && TouchChecker(Input.mousePosition).name == _name)
                ButtonAction(_name);
            unSelect();
            _name = "null";
        }


        if (isHold && !isSelect)
        {

            if (!isSelect && TouchChecker(Input.mousePosition) != null)
            {
                holdeffect(TouchChecker(Input.mousePosition).name);
                _name = TouchChecker(Input.mousePosition).name;
            }
            isSelect = true;
        }

        if (Input.GetKeyDown(KeyCode.Escape) && Touch.isPause)
        {
            Sound.sound.click();
            Editor.down = false;
            Touch.isPause = false;
            Menu.isRun = true;
            Time.timeScale = 1;
            pauseUI.SetActive(false);
            br.transform.localPosition = new Vector3(0, 0, 0);
        }
    }
    void holdeffect(string nameobj)
    {

        switch (nameobj)
        {
            case "menu":
                Button[0].GetComponent<SpriteRenderer>().sprite = ButtonSprite[1];
                break;
            case "resume":
                Button[1].GetComponent<SpriteRenderer>().sprite = ButtonSprite[3];
                break;
            case "restart":
                Button[2].GetComponent<SpriteRenderer>().sprite = ButtonSprite[5];
                break;
        }
    }
    void unSelect()
    {
        Button[0].GetComponent<SpriteRenderer>().sprite = ButtonSprite[0];
        Button[1].GetComponent<SpriteRenderer>().sprite = ButtonSprite[2];
        Button[2].GetComponent<SpriteRenderer>().sprite = ButtonSprite[4];
    }
    void ButtonAction(string buttonname)
    {
        Sound.sound.click();
        switch (buttonname)
        {
            case "menu":
                Time.timeScale = 1;
                Sound.sound.click();
                StartCoroutine(waittodo(0, 1, 0));
                if (MapLoader.Mode == 1)
                {
                    WorldData.world = (MapLoader.MapPlayer.Level - 1) / 99;
                    Application.LoadLevel(GlobalConsts.SCENE_MAP_SELECT);
                }
                else
                    Application.LoadLevel(GlobalConsts.SCENE_HOME);

                break;
            case "resume":
                Sound.sound.click();
                Editor.down = false;
                Touch.isPause = false;
                Menu.isRun = true;
                Time.timeScale = 1;
                pauseUI.SetActive(false);

                break;
            case "restart":
                Time.timeScale = 1;
                Sound.sound.click();
                StartCoroutine(waittodo(2, 5, 4));
                Application.LoadLevel(GlobalConsts.SCENE_PLAY);
                break;
        }
    }

    IEnumerator waittodo(int render, int sprite, int sprite1)
    {
        Button[render].GetComponent<SpriteRenderer>().sprite = ButtonSprite[sprite];
        yield return new WaitForSeconds(0.15f);
        Button[render].GetComponent<SpriteRenderer>().sprite = ButtonSprite[sprite1];
    }
    IEnumerator waittoresume(int render, int sprite, int sprite1)
    {
        Button[render].GetComponent<SpriteRenderer>().sprite = ButtonSprite[sprite];
        yield return new WaitForSeconds(0.15f);
        Button[render].GetComponent<SpriteRenderer>().sprite = ButtonSprite[sprite1];
        br.transform.localPosition = new Vector3(0, 0, 0);
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
}
