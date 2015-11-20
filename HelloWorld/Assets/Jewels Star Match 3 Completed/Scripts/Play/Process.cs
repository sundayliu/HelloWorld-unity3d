using UnityEngine;
using System.Collections;

public class Process : MonoBehaviour
{

    public static float DropTimer = 0;
    public static float SpawnWaitTime = 0;
    bool isdroped = true;
    bool isspawn = true;
    public GameObject stareff;
    public GameObject star;
    public static bool showstar;
    bool one = false;
    void Start()
    {
        isdroped = true;
    }

    void Update()
    {
        DropTimerCd();
        SpawnTimerCd();
    }


    void DropTimerCd()
    {
        if (DropTimer > 0)
        {
            isdroped = false;
            DropTimer -= Time.smoothDeltaTime;
        }
        else if (!isdroped)
        {
            isdroped = true;
            Editor.DropAll();
            GetComponent<JewelSpawn>().SpawnJe();
            ShowStar();
        }
    }
    public void ShowStar()
    {
        if (showstar && !one)
        {
            one = true;
            showstar = false;
            StartCoroutine(AddStarWin());
        }
    }

    IEnumerator AddStarWin()
    {
        int rowRandom = -1;
        Editor.down = true;
        while (rowRandom == -1)
        {
            rowRandom = Random.Range(0, 7);
            if (ColumnNull(rowRandom))
                rowRandom = -1;
        }
        int y = MaxCell(rowRandom);

        GameObject tmp = null;

        yield return null;
        tmp = JewelSpawn.JewelList[rowRandom, y];
        try
        {
            tmp.GetComponent<Jewel>().type = 99;
            tmp.GetComponent<Jewel>().PowerUp = 99;
            Effect.SpawnStarWin(tmp, star, false);
            tmp.transform.Find("Render").GetComponent<SpriteRenderer>().enabled = false;
            MapLoader.starwin = tmp;
            Instantiate(stareff);
            Editor.down = false;
        }
        catch { StartCoroutine(AddStarWin()); }

    }

    int MaxCell(int x)
    {
        int y = 8;
        while (true)
        {
            if (CellScript.map[x, y] > 0)
                return y;
            else if (y < 0)
                return y;

            y--;
        }
    }

    bool ColumnNull(int x)
    {
        for (int i = 0; i < 9; i++)
            if (CellScript.map[x, i] > 0)
                return false;
        return true;
    }

    void SpawnTimerCd()
    {
        if (SpawnWaitTime > 0)
        {
            isspawn = false;
            SpawnWaitTime -= Time.smoothDeltaTime;
        }
        else if (!isspawn)
        {
            isspawn = true;
        }
    }
}
