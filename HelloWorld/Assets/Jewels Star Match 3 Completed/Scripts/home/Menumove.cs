using UnityEngine;
using System.Collections;

public class Menumove : MonoBehaviour
{

    public Sprite[] ButtonSprite;
    public GameObject[] Movebutton;
    public GameObject parentrender;
    public GameObject bg;
    public GameObject bg1;
    public GameObject help;
    public bool isMove = false;
    public bool isHold = false;
    bool isSelect = false;
    string _name;
    public GameObject main;

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

    void Start()
    {
        isHold = false;
        isSelect = false;
        _name = null;
        OnStartMusic();
        OnStartSound();
    }

    void ButtonAction(string buttonname)
    {
        isHold = false;
        switch (buttonname)
        {
            case "Arcade":
                Sound.sound.click();
                StartCoroutine(waittodo(4, 9, 8));
                Application.LoadLevel(GlobalConsts.SCENE_WORD_SELECT);
                break;
            case "classic":
                Sound.sound.click();
                classicmode();
                break;
            case "music":
                Sound.sound.click();
                if (PlayerPrefs.GetString("music", "ON").CompareTo("ON") == 0)
                {
                    PlayerPrefs.SetString("music", "OFF");
                    Movebutton[2].GetComponent<SpriteRenderer>().sprite = ButtonSprite[5];
                    Music.music.MusicOFF();
                }
                else
                {
                    PlayerPrefs.SetString("music", "ON");
                    Movebutton[2].GetComponent<SpriteRenderer>().sprite = ButtonSprite[4];
                    Music.music.MusicON();
                }
                break;

            case "sound":
                Sound.sound.click();
                if (PlayerPrefs.GetString("sound", "ON").CompareTo("ON") == 0)
                {
                    PlayerPrefs.SetString("sound", "OFF");
                    Movebutton[0].GetComponent<SpriteRenderer>().sprite = ButtonSprite[1];
                    Sound.isSound = false;
                }
                else
                {
                    PlayerPrefs.SetString("sound", "ON");
                    Movebutton[0].GetComponent<SpriteRenderer>().sprite = ButtonSprite[0];
                    Sound.isSound = true;
                }
                break;
        }
    }

    void holdeffect(string nameobj)
    {
        switch (nameobj)
        {
            case "Arcade":
                Movebutton[4].GetComponent<SpriteRenderer>().sprite = ButtonSprite[9];
                break;
            case "classic":
                Movebutton[5].GetComponent<SpriteRenderer>().sprite = ButtonSprite[11];
                break;
            case "music":
                if (PlayerPrefs.GetString("music", "ON").CompareTo("ON") == 0)
                    Movebutton[2].GetComponent<SpriteRenderer>().sprite = ButtonSprite[5];
                else
                    Movebutton[2].GetComponent<SpriteRenderer>().sprite = ButtonSprite[4];
                break;
            case "sound":
                if (PlayerPrefs.GetString("sound", "ON").CompareTo("ON") == 0)
                    Movebutton[0].GetComponent<SpriteRenderer>().sprite = ButtonSprite[1];
                else
                    Movebutton[0].GetComponent<SpriteRenderer>().sprite = ButtonSprite[0];
                break;
        }
    }

    void unSelect()
    {
        if (PlayerPrefs.GetString("music", "ON").CompareTo("ON") == 0)
            Movebutton[2].GetComponent<SpriteRenderer>().sprite = ButtonSprite[4];
        else
            Movebutton[2].GetComponent<SpriteRenderer>().sprite = ButtonSprite[5];

        if (PlayerPrefs.GetString("sound", "ON").CompareTo("ON") == 0)
            Movebutton[0].GetComponent<SpriteRenderer>().sprite = ButtonSprite[0];
        else
            Movebutton[0].GetComponent<SpriteRenderer>().sprite = ButtonSprite[1];

        Movebutton[1].GetComponent<SpriteRenderer>().sprite = ButtonSprite[2];
        Movebutton[4].GetComponent<SpriteRenderer>().sprite = ButtonSprite[8];
        Movebutton[5].GetComponent<SpriteRenderer>().sprite = ButtonSprite[10];
        Movebutton[6].GetComponent<SpriteRenderer>().sprite = ButtonSprite[12];
    }

    void OnStartMusic()
    {
        if (PlayerPrefs.GetString("music", "ON").CompareTo("ON") == 0)
        {
            Movebutton[2].GetComponent<SpriteRenderer>().sprite = ButtonSprite[4];
            Music.music.MusicON();
        }
        else
        {
            Movebutton[2].GetComponent<SpriteRenderer>().sprite = ButtonSprite[5];
            Music.music.MusicOFF();
        }

    }

    void OnStartSound()
    {
        if (PlayerPrefs.GetString("sound", "ON").CompareTo("ON") == 0)
        {
            Movebutton[0].GetComponent<SpriteRenderer>().sprite = ButtonSprite[0];
            Sound.isSound = true;
        }
        else
        {
            Movebutton[0].GetComponent<SpriteRenderer>().sprite = ButtonSprite[1];
            Sound.isSound = false;
        }

    }

    IEnumerator waittodo(int render, int sprite, int sprite1)
    {
        Movebutton[render].GetComponent<SpriteRenderer>().sprite = ButtonSprite[sprite];
        yield return new WaitForSeconds(0.15f);
        Movebutton[render].GetComponent<SpriteRenderer>().sprite = ButtonSprite[sprite1];
    }

    void AnimationActive(GameObject render, bool ismove)
    {
        if (ismove)
            render.GetComponent<Animator>().SetInteger("state", 2);
        else
            render.GetComponent<Animator>().SetInteger("state", 1);

    }

    IEnumerator move()
    {
        if (!isMove)
        {

            bg1.GetComponent<brScale>().X = 20f;
            bg.GetComponent<button>().X = 5.3f;

            Movebutton[3].GetComponent<button>().X = 4.8f;
            yield return new WaitForSeconds(0);
            Movebutton[2].GetComponent<button>().X = 3.6f;
            yield return new WaitForSeconds(0);
            Movebutton[1].GetComponent<button>().X = 2.4f;
            yield return new WaitForSeconds(0);
            Movebutton[0].GetComponent<button>().X = 1.2f;
        }
        else
        {

            bg.GetComponent<button>().X = 0.28f;
            bg1.GetComponent<brScale>().X = 0.2f;

            Movebutton[3].GetComponent<button>().X = 0;
            yield return new WaitForSeconds(0);
            Movebutton[2].GetComponent<button>().X = 0;
            yield return new WaitForSeconds(0);
            Movebutton[1].GetComponent<button>().X = 0;
            yield return new WaitForSeconds(0);
            Movebutton[0].GetComponent<button>().X = 0;
        }

    }

    GameObject TouchChecker(Vector3 pos)
    {

        Vector3 wp = Camera.main.ScreenToWorldPoint(pos);
        Vector2 touchPos = new Vector2(wp.x, wp.y);
        GameObject ObjPointer = null;
        if (Physics2D.OverlapPoint(touchPos))
            ObjPointer = Physics2D.OverlapPoint(touchPos).gameObject;

        return ObjPointer;

    }
    void classicmode()
    {
        Player p = new Player();
        p.HightScore = long.Parse(PlayerPrefs.GetString("ClassicHightScore", "0"));
        p.Level = 1;
        p.Name = "classic";
        p.Stars = 0;
        p.UnLocked = true;

        MapLoader.MapPlayer = p;
        MapLoader.Mode = 0;
        MapLoader.score = 0;
        Application.LoadLevel(GlobalConsts.SCENE_PLAY);
    }

}
