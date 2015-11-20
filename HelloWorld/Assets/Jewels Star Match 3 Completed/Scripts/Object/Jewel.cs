using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Jewel : MonoBehaviour
{
    public Vector2 PosMap;
    public List<Vector2> listX;
    public List<Vector2> listY;
    public int indexinlist;
    public int type;
    public float X = -1;
    public float Y = -1;
    public float baseY = -1;
    public bool isDrop = false;
    public bool isDestroy;
    public int PowerUp = 0;
    public Vector2 PowerUpPosition;
    public bool isProcess = false;
    public bool isMove = false;
    public int dtype = -1;
    public bool iswaitdes = false;  //destroy flag
    int x;
    int y;
    float droptime = 0.45f;
    float spamtime = 0.4f;
    float BonusTime = 22f;
    public GameObject[] effect;
    public GameObject Number;
    public Sprite[] NumberSprite;
    public List<int> lowList = new List<int>();
    public List<int> lowpos = new List<int>();
    public List<GameObject> column = new List<GameObject>();
    public bool isSound = false;

    Transform mtransform;
    BoxCollider2D mBoxCollider2D;

    void Start()
    {
        mtransform = transform;
        mBoxCollider2D = GetComponent<BoxCollider2D>();
    }

    void Update()
    {

        if (baseY != -1)
            MoveToStart();
        if (X != -1 && X != mtransform.localPosition.x)
            MoveToX(X);
        if (Y != -1 && Y != mtransform.localPosition.y)
            MoveToY(Y);

        if (iswaitdes)
        {
            iswaitdes = false;
            Destroying();
        }

    }

    void MoveToX(float x)
    {
        isMove = true;
        if (Mathf.Abs(x - mtransform.localPosition.x) > 0.15)
        {
            mBoxCollider2D.enabled = false;
            if (mtransform.localPosition.x > x)
                mtransform.localPosition -= new Vector3(Time.smoothDeltaTime * 8f, 0, 0);
            else if (mtransform.localPosition.x < x)
                mtransform.localPosition += new Vector3(Time.smoothDeltaTime * 8f, 0, 0);
        }
        else
        {
            mtransform.localPosition = new Vector3(x, mtransform.localPosition.y, mtransform.localPosition.z);
            X = -1;
            isMove = false;
            mBoxCollider2D.enabled = true;
            if (type == 99)
                if (WinChecker())
                {
                    GameObject.Find("top").GetComponent<Menu>().Win();
                }
        }
    }

    void MoveToY(float y)
    {
        isMove = true;
        if (Mathf.Abs(y - mtransform.localPosition.y) > 0.15)
        {
            mBoxCollider2D.enabled = false;
            if (mtransform.localPosition.y > y)
                mtransform.localPosition -= new Vector3(0, Time.smoothDeltaTime * 8f, 0);
            else if (mtransform.localPosition.y < y)
                mtransform.localPosition += new Vector3(0, Time.smoothDeltaTime * 8f, 0);
        }
        else
        {
            mtransform.localPosition = new Vector3(mtransform.localPosition.x, y, mtransform.localPosition.z);
            Y = -1;
            mBoxCollider2D.enabled = true;
            isMove = false;
            if (type == 99)
                if (WinChecker())
                {
                    GameObject.Find("top").GetComponent<Menu>().Win();
                }
        }

    }

    public void MoveToStart()
    {
        isMove = true;
        Y = -1;
        Touch.supportTime = 3f;
        Touch.supportTimeRp = 1.5f;
        if (Mathf.Abs(baseY - mtransform.localPosition.y) > 0.15 && baseY < mtransform.localPosition.y)
        {
            mBoxCollider2D.enabled = false;
            mtransform.localPosition -= new Vector3(0, Time.smoothDeltaTime * 10f, 0);
        }
        else
        {
            mtransform.localPosition = new Vector3(mtransform.localPosition.x, baseY, mtransform.localPosition.z);
            baseY = -1;
            isDrop = false;
            isMove = false;

            x = Mathf.RoundToInt(mtransform.localPosition.x);
            PowerUpPosition = new Vector2(-1, -1);

            if (CellScript.map[(int)PosMap.x, (int)PosMap.y] % 10 != 4)
                JewelProcessing();
            if (type == 99)
                if (WinChecker())
                {
                    GameObject.Find("top").GetComponent<Menu>().Win();
                }
            mtransform.Find("Render").GetComponent<Animator>().SetInteger("state", 103);
            mBoxCollider2D.enabled = true;
        }
    }

    public void JewelProcessing()
    {
        StartCoroutine(JewelProcess());
    }

    IEnumerator JewelProcess()
    {
        isProcess = true;
        yield return null;
        x = (int)PosMap.x;
        y = (int)PosMap.y;

        listX.Clear();
        listY.Clear();
        listX = JewelController.RowChecker(type, y, x);
        listY = JewelController.ColumnChecker(type, x, y);
        if (listX.Count + listY.Count == 3)
            CallDestroy(new Vector2(-1, -1), 0);
        else if (listX.Count + listY.Count == 4)
        {
            Vector2 tmp = Editor.powerUp1(listX, listY);
            if (x != (int)tmp.x || y != (int)tmp.y)
                CallDestroy(tmp, 0);
            else if (PowerUp == 0)
            {
                CallDestroy(tmp, 1);
                PowerUp = 1;
                Effect.SpawnEnchan(effect[0], gameObject);
            }
            else
            {
                CallDestroy(tmp, 0);
                Destroying();
            }
        }
        else if (listX.Count + listY.Count >= 5)
        {
            Vector2 tmp = Editor.PowerUpType(listX, listY);
            if (x != (int)tmp.x || y != (int)tmp.y)
                Destroying();
            else
            {
                CallDestroy(tmp, 0);
                mtransform.Find("Render").GetComponent<SpriteRenderer>().enabled = false;
                type = 9;
                PowerUp = 9;
                Effect.SpawnType9(effect[1], gameObject);
                Editor.LightingRandomPoint();
            }

        }
        isProcess = false;
    }

    public void JewelDrop()
    {
        drop();
    }
    /// <summary>
    /// find and move jewel to new position
    /// </summary>
    void drop()
    {
        indexinlist = PosChecker((int)PosMap.x, (int)PosMap.y);
        if (indexinlist != -1 && indexinlist != PosMap.y)
        {
            isMove = true;
            JewelSpawn.JewelList[(int)PosMap.x, (int)PosMap.y] = null;
            JewelSpawn.JewelList[(int)PosMap.x, indexinlist] = gameObject;
            PosMap = new Vector2((int)PosMap.x, indexinlist);
            baseY = (int)PosMap.y;
        }
    }

    /// <summary>
    /// processing jewel destroy
    /// </summary>
    /// <returns></returns>
    IEnumerator destroy()
    {
        Touch.supportTime = 3f;
        Touch.supportTimeRp = 1.5f;
        if (type < 99)
        {
            GetComponent<BoxCollider2D>().enabled = false;
            Process.DropTimer = droptime;
            if (Process.SpawnWaitTime > 0)
                Process.SpawnWaitTime = spamtime;

            JewelSpawn.JewelList[(int)PosMap.x, (int)PosMap.y] = null;
            isDestroy = true;

            try
            {
                CellScript.Cells[(int)PosMap.x, (int)PosMap.y].GetComponent<Cell>().playAnimation();
            }
            catch { }

            yield return new WaitForSeconds(0.2f);

            mtransform.Find("Render").GetComponent<Animator>().SetInteger("state", type);
            Editor.cellprocess((int)PosMap.x, (int)PosMap.y);
            PowerProcess(PowerUp);
            Effect.SpawnNumber(new Vector2(mtransform.position.x, mtransform.position.y), Number, NumberSprite, 0.5f);

            if (isSound)
                Sound.sound.jewelclr();

            Destroy(gameObject, 0.5f);
        }
        yield return null;

    }
    /// <summary>
    /// process if jewel power > 0
    /// </summary>
    /// <param name="power"></param>
    /// <returns></returns>
    bool PowerProcess(int power)
    {
        switch (power)
        {
            case 0:
                return false;
            case 1:
                //boom
                Effect.SpawnBoom(new Vector2(mtransform.position.x, mtransform.position.y), effect[2], 0.5f);
                Editor.DestroyAround(PosMap);
                return false;
            case 2:
                //row lighting
                Effect.RowLighting(mtransform.position.y, effect[3]);
                Editor.RowLighting(PosMap);
                return false;
            case 3:
                //column lighting
                Effect.ColumnLighting(mtransform.position.x, effect[4]);
                Editor.ColumnLighting(PosMap);

                return true;
            case 4://time 
                GameObject.Find("top").GetComponent<Menu>().timeinc(BonusTime);
                return false;
        }
        return false;

    }

    public void Destroying()
    {
        StartCoroutine(destroy());
    }
    /// <summary>
    /// destroy all jewel same jewel type around by row and column
    /// </summary>
    /// <param name="UnDestroyPosition"></param>
    /// <param name="pow"></param>
    public void CallDestroy(Vector2 UnDestroyPosition, int pow)
    {
        List<Vector2> DestroyList = new List<Vector2>();
        foreach (Vector2 v in listX)
        {
            DestroyList.Add(v);
        }

        DestroyList.Add(new Vector2(x, y));
        foreach (Vector2 v in listY)
        {
            DestroyList.Add(v);
        }

        foreach (Vector2 v in DestroyList)
        {
            if (v != UnDestroyPosition)
            {
                GameObject tmp = JewelSpawn.JewelList[(int)v.x, (int)v.y];

                if (v == DestroyList[0] && tmp != null && tmp.GetComponent<Jewel>().PowerUp == 0)
                    tmp.GetComponent<Jewel>().isSound = true;

                if (tmp != null && !tmp.GetComponent<Jewel>().isDestroy && tmp.GetComponent<Jewel>().type == type)
                    if (tmp.GetComponent<Jewel>().listX.Count <= 0 || tmp.GetComponent<Jewel>().listY.Count <= 0)
                    {
                        tmp.GetComponent<Jewel>().Destroying();
                    }
            }
            else
            {
                GameObject tmp = JewelSpawn.JewelList[(int)v.x, (int)v.y];
                if (tmp != null && !tmp.GetComponent<Jewel>().isDestroy && tmp.GetComponent<Jewel>().type == type)
                    tmp.GetComponent<Jewel>().DoPowerUp(1);
            }
        }
    }

    public void DoPowerUp(int pow)
    {
        if (PowerUp == 0 && pow == 1)
        {
            PowerUp = 1;
            Effect.SpawnEnchan(effect[0], gameObject);
            Editor.LightingRandomPoint();
            try
            {
                CellScript.Cells[(int)PosMap.x, (int)PosMap.y].GetComponent<Cell>().playAnimation();
            }
            catch { }
        }

    }

    int PosChecker(int x, int y)
    {
        int indx = -1;
        lowList.Clear();

        int newpos = y;
        if (CellScript.map[x, y] % 10 == 4)
            return indx;
        for (int i = y - 1; i >= 0; i--)
        {
            if (CellScript.map[x, i] % 10 == 4)
            {
                return newpos;
            }
            if (JewelSpawn.JewelList[x, i] == null && CellScript.map[x, i] > 0)
            {
                newpos = i;
            }

        }
        indx = newpos;
        return indx;

    }

    public void PlayMoveAnimation(Vector2 posmap2)
    {
        StartCoroutine(MoveAnimation(posmap2));
    }
    IEnumerator MoveAnimation(Vector2 posmap2)
    {
        int state = 100;
        if (PosMap.x < posmap2.x)
            state = 13;
        else if (PosMap.x > posmap2.x)
            state = 12;
        else if (PosMap.y < posmap2.y)
            state = 10;
        else if (PosMap.y > posmap2.y)
            state = 11;



        mtransform.Find("Render").GetComponent<Animator>().SetInteger("state", state);
        yield return new WaitForSeconds(0.2f);
        mtransform.Find("Render").GetComponent<Animator>().SetInteger("state", 100);
    }
    bool WinChecker()
    {
        if (Mathf.RoundToInt(mtransform.localPosition.y) == Editor.MinCell((int)PosMap.x))
        {
            return true;
        }
        return false;

    }
    void playSound()
    {
        if (PowerUp == 1)
            Sound.sound.boom();
        else if (PowerUp == 2 || PowerUp == 3)
            Sound.sound.elec();
    }

}
