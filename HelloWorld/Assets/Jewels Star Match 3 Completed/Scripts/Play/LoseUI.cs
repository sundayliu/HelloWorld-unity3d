using UnityEngine;
using System.Collections;

public class LoseUI : MonoBehaviour
{

    public GameObject[] Button;
    public Sprite[] ButtonSprite;
    public TextMesh[] text;

    void Start()
    {
        Editor.down = true;
        text[0].text = MapLoader.score.ToString();
        if (MapLoader.Mode == 1)
            text[1].text = MapLoader.MapPlayer.HightScore.ToString();
        else
        {
            if (MapLoader.score > long.Parse(PlayerPrefs.GetString("ClassicHightScore", "0")))
                PlayerPrefs.SetString("ClassicHightScore", MapLoader.score.ToString());
            text[1].text = PlayerPrefs.GetString("ClassicHightScore", "0");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            if (TouchChecker(Input.mousePosition) != null)
                ButtonAction(TouchChecker(Input.mousePosition).name);
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
                if (MapLoader.Mode == 1)
                    Application.LoadLevel(GlobalConsts.SCENE_PLAY);
                else
                    classicmode();
                break;
        }
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
