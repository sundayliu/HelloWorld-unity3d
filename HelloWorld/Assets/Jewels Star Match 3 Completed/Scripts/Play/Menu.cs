using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour
{

    public GameObject[] TimeBar;
    float value;
    public static bool isRun = false;
    public float time;
    public GameObject WinUI;
    public GameObject LoseUI;
    public GameObject PlayingUI;
    public static bool IsWin;
    public static bool IsLose;
    public static long scorebar = 0;

    bool onetime = false;

    void Start()
    {
        onetime = false;
        if (MapLoader.Mode == 1)
        {
            value = 28.2f / MapLoader.time;
        }
        else
        {
            TimeBar[1].transform.localScale = new Vector3(0, 1, 0.2f);
            value = 28.2f / 5000f;
            scorebar = 0;
        }
        StartCoroutine(wait());
    }


    // Update is called once per frame
    void Update()
    {
        if (isRun && MapLoader.Mode == 1)
            timeCountdown();
        else if (isRun && MapLoader.Mode == 0)
            ScoreInc();
    }
    void timeCountdown()
    {
        TimeBar[1].transform.localScale = new Vector3(value * MapLoader.time, 1, 0.2f);
        MapLoader.time -= Time.deltaTime;
        time = MapLoader.time;
        if (MapLoader.time <= 0 && !IsWin)
        {
            isRun = false;
            LoseUI.SetActive(true);
            IsLose = true;
            Sound.sound.fail();
        }
    }

    void ScoreInc()
    {
        if (scorebar <= 5000)
        {
            TimeBar[1].transform.localScale = new Vector3(value * scorebar, 1, 0.2f);
        }
        else if (!onetime)
        {
            onetime = true;
            StartCoroutine(ClassicUplevel());
        }
    }

    public void Win()
    {
        if (!IsLose)
        {
            Sound.sound.pass();
            isRun = false;
            WinUI.SetActive(true);
            IsWin = true;
        }
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(5f);
        isRun = true;
    }

    IEnumerator ClassicUplevel()
    {
        JewelSpawn.spawnStart = false;
        Touch.supportTimeRp = 5f;
        //destroy all
        for (int x = 0; x < 7; x++)
            for (int y = 0; y < 9; y++)
                if (JewelSpawn.JewelList[x, y] != null)
                    JewelSpawn.JewelList[x, y].GetComponent<Jewel>().Destroying();
        yield return new WaitForSeconds(1.5f);
        Player p = new Player();
        p.HightScore = long.Parse(PlayerPrefs.GetString("ClassicHightScore", "0"));
        p.Level = MapLoader.MapPlayer.Level + 1;
        p.Name = "classic";
        p.Stars = 0;
        p.UnLocked = true;

        MapLoader.MapPlayer = p;
        MapLoader.Mode = 0;
        Application.LoadLevel(GlobalConsts.SCENE_PLAY);
    }

    public void timeinc(float _time)
    {
        if (!Menu.IsLose && !Menu.IsLose)
            StartCoroutine(inctimepersecond(_time));
    }
    IEnumerator inctimepersecond(float _time)
    {
        float tmptime = _time;
        float d = 0.4f;
        while (tmptime > 0)
        {
            yield return new WaitForSeconds(0.01f);
            if (MapLoader.time < MapLoader.TIMEPLAYER)
                MapLoader.time += d;
            tmptime -= d;
        }

    }

    public void scoreinc(long _score)
    {
        StartCoroutine(incscorepersecond(_score));
    }
    IEnumerator incscorepersecond(long _score)
    {
        long tmpscore = _score;
        int d = 1;
        while (tmpscore > 0)
        {
            yield return new WaitForSeconds(0.02f);
            if (scorebar <= 5000)
                scorebar += d;
            tmpscore -= d;
        }

    }

}
