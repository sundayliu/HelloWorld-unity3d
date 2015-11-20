using UnityEngine;
using System.Collections;

public class WinUI : MonoBehaviour
{

    public GameObject[] Button;
    public Sprite[] ButtonSprite;
    public TextMesh[] text;
    public GameObject[] star;
    public Sprite[] StarSprite;
    public GameObject goldStar;
    public GameObject StarParent;
    string _name;
    bool isHold = false;
    bool isSelect = false;
    long score = 0;
    // Use this for initialization
    void Start()
    {
        Editor.down = true;
        score = MapLoader.score + (int)MapLoader.time * 500;
        text[0].text = score.ToString();
        if (MapLoader.MapPlayer.HightScore < score)
            text[1].text = score.ToString();
        else
            text[1].text = MapLoader.MapPlayer.HightScore.ToString();

        text[2].text = ((int)MapLoader.time * 500).ToString();
        setStar(score);

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

    }
    void holdeffect(string nameobj)
    {
        switch (nameobj)
        {
            case "menu":
                Sound.sound.click();
                Button[0].GetComponent<SpriteRenderer>().sprite = ButtonSprite[1];
                break;
            case "restart":
                Sound.sound.click();
                Button[1].GetComponent<SpriteRenderer>().sprite = ButtonSprite[3];
                break;
            case "next":
                Sound.sound.click();
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
        switch (buttonname)
        {
            case "menu":
                StartCoroutine(waittodo(0, 1, 0));
                if (MapLoader.Mode == 1)
                {
                    WorldData.world = (MapLoader.MapPlayer.Level - 1) / 99;
                    Application.LoadLevel(GlobalConsts.SCENE_MAP_SELECT);
                }
                else
                    Application.LoadLevel(GlobalConsts.SCENE_HOME);
                break;
            case "restart":
                StartCoroutine(waittodo(1, 3, 2));
                Application.LoadLevel(GlobalConsts.SCENE_PLAY);
                break;
            case "next":
                StartCoroutine(waittodo(2, 5, 4));
                SavePlayer();
                break;
        }
    }
    void SavePlayer()
    {
        if (MapLoader.MapPlayer.Level != 297)
        {
            if (MapLoader.MapPlayer.HightScore < score)
                MapLoader.MapPlayer.HightScore = score;
            int s = setStar(score);
            if (MapLoader.MapPlayer.Stars < s)
                MapLoader.MapPlayer.Stars = s;
            DataLoader.DataPlayer[MapLoader.MapPlayer.Level - 1] = MapLoader.MapPlayer;
            PlayerPrefsSerializer p = new PlayerPrefsSerializer();
            p.Maps = p.LoadPref();
            p.Update(MapLoader.MapPlayer.Level, MapLoader.MapPlayer);
            Player m = new Player();
            m = p.Maps[MapLoader.MapPlayer.Level];
            m.UnLocked = true;
            if (MapLoader.MapPlayer.Level < 297)
            {
                p.Update(MapLoader.MapPlayer.Level + 1, m);
                DataLoader.DataPlayer[MapLoader.MapPlayer.Level].UnLocked = true;
            }
            MapLoader.MapPlayer = m;
            Application.LoadLevel(GlobalConsts.SCENE_PLAY);
        }
        else Application.LoadLevel(GlobalConsts.SCENE_HOME);

    }

    int setStar(long _score)
    {
        int tmp = 1;
        if (_score > 80000)
        {
            tmp = 3;
            spawnStar(star[0].transform.localPosition);
            spawnStar(star[1].transform.localPosition);
            spawnStar(star[2].transform.localPosition);
        }
        else if (_score > 60000)
        {
            tmp = 2;
            spawnStar(star[0].transform.localPosition);
            spawnStar(star[1].transform.localPosition);
        }
        else
        {
            tmp = 1;
            spawnStar(star[0].transform.localPosition);
        }
        return tmp;
    }

    IEnumerator waittodo(int render, int sprite, int sprite1)
    {
        Button[render].GetComponent<SpriteRenderer>().sprite = ButtonSprite[sprite];
        yield return new WaitForSeconds(0.15f);
        Button[render].GetComponent<SpriteRenderer>().sprite = ButtonSprite[sprite1];
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

    void spawnStar(Vector3 position)
    {
        GameObject tmp = Instantiate(goldStar) as GameObject;
        tmp.transform.parent = StarParent.transform;
        tmp.transform.localPosition = new Vector3(position.x, position.y, -0.2f);
        tmp.transform.localScale = new Vector3(1, 1, 1);

    }
}