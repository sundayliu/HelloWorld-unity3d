using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Touch : MonoBehaviour
{
    public static int BackCount;
    GameObject Pointer;
    GameObject ObjPointer;
    public GameObject JewelSelected;
    public GameObject JewelMatch;
    GameObject[] SupportObj = new GameObject[2];
    float postmp = -1;
    float postmp1 = -1;
    float postmp3 = -1;
    float postmp4 = -1;
    bool isMove = false;
    bool ishold = false;
    bool isCheck = false;
    bool isSP = false;
    public static float supportTime = 3f;
    public static float supportTimeRp = 1.5f;
    public GameObject pausebutton;
    public Sprite[] pausesprite;
    public GameObject PlayingUI;
    public GameObject PauseUI;
    public GameObject LoseUI;

    public GameObject br;
    public static bool isPause = false;
    public GameObject nomove;
    Supporter sp;

    public GameObject vienxanh;
    float clicktime = 0.2f;
    void Start()
    {

#if UNITY_IPHONE
      Application.targetFrameRate = 60;
#endif

        sp = new Supporter();
        Editor.down = false;
        supportTimeRp = 5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMove)
        {
            if (clicktime > 0)
                clicktime -= Time.deltaTime;
            if (clicktime <= 0)
            {
                isMove = false;
                clicktime = 0.2f;
            }
        }

        if (!Editor.down)
        {
            if (!isMove)
            {
                if (Input.GetMouseButtonDown(0) && !ishold && JewelSelected == null)
                {
                    Pointer = CheckTouch(Input.mousePosition);
                    if (Pointer != null && Pointer.name.Contains("Jewel"))
                    {

                        if (!Pointer.GetComponent<Jewel>().isMove && !Pointer.GetComponent<Jewel>().isDestroy)
                        {
                            JewelSelected = Pointer;
                            addSelectEffect(JewelSelected);
                            ishold = true;
                            Pointer = null;
                        }
                    }
                    isCheck = false;
                }

                if (ishold)
                {
                    supportTime = 3f;
                    removeSupportEffect(SupportObj);
                    ObjPointer = CheckTouch(Input.mousePosition);
                    if (ObjPointer != null && ObjPointer.name.Contains("Jewel"))
                        if (JewelSelected != ObjPointer && !isCheck)
                        {
                            isCheck = true;
                            JewelMatch = ObjPointer;
                            if (DistanceCheck(JewelSelected, JewelMatch))
                            {
                                WaitToBack(JewelSelected, JewelMatch);
                            }
                            else
                            {
                                RemoveSelectEffect(JewelSelected);
                                JewelSelected = null;
                                JewelMatch = null;
                                isCheck = false;
                                ishold = false;
                            }
                            ishold = false;
                        }
                }
            }
        }

        if (Input.GetMouseButtonDown(0))
            if (CheckTouch(Input.mousePosition) != null && CheckTouch(Input.mousePosition).name.CompareTo("pause") == 0)
            {
                if (!isPause && !Menu.IsLose && !Menu.IsWin)
                {
                    isPause = true;
                    StartCoroutine(waittodopause(1, 0));
                }
            }
        if (Input.GetKeyDown(KeyCode.Escape) && !isPause && !Menu.IsWin && !Menu.IsLose)
        {
            isPause = true;
            PauseUI.SetActive(true);
            Time.timeScale = 0;
        }
        SupposterTimeCountdown();
    }

    void SupposterTimeCountdown()
    {
        if (Menu.isRun && supportTimeRp <= 0 && JewelSpawn.spawnStart)
            Supposter();
        else if (Menu.isRun)
            supportTimeRp -= Time.deltaTime;

        if (supportTime > 0 && Menu.isRun)
        {
            isSP = false;
            supportTime -= Time.deltaTime;
        }
        else if (!isSP && Menu.isRun)
        {
            isSP = true;
            try
            {
                SupportObj[0].transform.FindChild("Render").GetComponent<Animator>().SetInteger("state", 101);
                SupportObj[1].transform.FindChild("Render").GetComponent<Animator>().SetInteger("state", 101);
            }
            catch
            {
            }
        }
    }

    void Supposter()
    {

        if (SupportObj[0] == null)
        {
            sp.SetVirtualJewel();
            SupportObj = sp.MoveSupportGameObject();
            if (SupportObj[0] == null)
            {
                if (MapLoader.Mode == 1)
                {
                    StartCoroutine(waitnomove());
                    supportTimeRp = 5f;
                }
                else
                {
                    Menu.IsLose = true;
                    LoseUI.SetActive(true);
                    PlayingUI.SetActive(false);
                    Sound.sound.fail();
                }

            }
        }
    }

    void autoplay()
    {
        if (SupportObj[0] != null && SupportObj[1] != null)
        {
            if (SupportObj[0].GetComponent<Jewel>().type == 9)
            {
                StartCoroutine(Editor.DestroyAllType(SupportObj[1]));
                JewelSwap(SupportObj[0], SupportObj[1]);
                SupportObj[0].GetComponent<Jewel>().Destroying();
                SupportObj[1].GetComponent<Jewel>().Destroying();
            }

            ListSwap(SupportObj[0], SupportObj[1]);
            SupportObj[0].GetComponent<Jewel>().JewelProcessing();
            SupportObj[1].GetComponent<Jewel>().JewelProcessing();
            JewelSwap(SupportObj[0], SupportObj[1]);
        }
    }
    IEnumerator waittodopause(int sprite, int sprite1)
    {
        Sound.sound.click();
        pausebutton.GetComponent<SpriteRenderer>().sprite = pausesprite[sprite];
        yield return new WaitForSeconds(0.15f);
        pausebutton.GetComponent<SpriteRenderer>().sprite = pausesprite[sprite1];
        PauseUI.SetActive(true);
        Time.timeScale = 0;
    }

    void removeSupportEffect(GameObject[] obj)
    {
        if (obj[0] != null)
        {
            obj[0].transform.FindChild("Render").GetComponent<Animator>().SetInteger("state", 100);
            obj[0] = null;
        }
        if (obj[1] != null)
        {
            obj[1].transform.FindChild("Render").GetComponent<Animator>().SetInteger("state", 100);
            obj[1] = null;
        }
    }

    void addSelectEffect(GameObject obj)
    {
        Vector2 pos;
        if (obj != null)
        {
            pos = obj.GetComponent<Jewel>().PosMap;
            GameObject tmp = Instantiate(vienxanh) as GameObject;
            tmp.name = vienxanh.name;
            tmp.transform.parent = CellScript.Cells[(int)pos.x, (int)pos.y].transform;
            tmp.transform.localPosition = new Vector3(0, 0, -1.1f);
        }
    }
    void RemoveSelectEffect(GameObject obj)
    {
        Vector2 pos;
        if (obj != null)
        {
            GameObject tmp = null;
            pos = obj.GetComponent<Jewel>().PosMap;
            if (CellScript.Cells[(int)pos.x, (int)pos.y].transform.childCount > 0)
                tmp = CellScript.Cells[(int)pos.x, (int)pos.y].transform.GetChild(0).gameObject;
            if (tmp != null)
                Destroy(tmp);
        }
    }

    GameObject CheckTouch(Vector3 mouseposition)
    {
        Vector3 wp = Camera.main.ScreenToWorldPoint(mouseposition);
        Vector2 touchPos = new Vector2(wp.x, wp.y);
        if (Physics2D.OverlapPoint(touchPos))
        {
            return Physics2D.OverlapPoint(touchPos).gameObject;
        }
        return null;
    }

    void JewelSwap(GameObject obj1, GameObject obj2)
    {
        postmp = (int)obj1.transform.localPosition.x;
        postmp1 = (int)obj2.transform.localPosition.x;
        postmp3 = (int)obj1.transform.localPosition.y;
        postmp4 = (int)obj2.transform.localPosition.y;
        obj1.GetComponent<Jewel>().X = postmp1;
        obj2.GetComponent<Jewel>().X = postmp;
        obj1.GetComponent<Jewel>().Y = postmp4;
        obj2.GetComponent<Jewel>().Y = postmp3;
    }

    void ListSwap(GameObject obj1, GameObject obj2)
    {
        Vector2 vObj1Tmp = obj1.GetComponent<Jewel>().PosMap;
        Vector2 vObj2Tmp = obj2.GetComponent<Jewel>().PosMap;

        JewelSpawn.JewelList[(int)vObj1Tmp.x, (int)vObj1Tmp.y] = null;
        JewelSpawn.JewelList[(int)vObj2Tmp.x, (int)vObj2Tmp.y] = null;
        JewelSpawn.JewelList[(int)vObj1Tmp.x, (int)vObj1Tmp.y] = obj2;
        JewelSpawn.JewelList[(int)vObj2Tmp.x, (int)vObj2Tmp.y] = obj1;

        obj1.GetComponent<Jewel>().PosMap = vObj2Tmp;
        obj2.GetComponent<Jewel>().PosMap = vObj1Tmp;

    }

    bool DistanceCheck(GameObject obj1, GameObject obj2)
    {
        if (obj1 != null && obj2 != null)
        {
            float x = Mathf.Abs(obj1.transform.localPosition.x - obj2.transform.localPosition.x);
            float y = Mathf.Abs(obj1.transform.localPosition.y - obj2.transform.localPosition.y);
            if (x < 2 && y < 2)
                if ((x - y) != 0)
                    return true;
                else
                {
                    return false;
                }
            else
            {
                return false;
            }
        }
        return false;
    }

    void WaitToBack(GameObject obj1, GameObject obj2)
    {
        RemoveSelectEffect(obj1);
        Touch.supportTimeRp = 1.5f;
        isMove = true;

        if (obj1 != null && obj2 != null && obj1.GetComponent<Jewel>().type == 9 && obj2.GetComponent<Jewel>().type != 99)
        {
            StartCoroutine(Editor.DestroyAllType(obj2));
            JewelSwap(obj1, obj2);
            obj1.GetComponent<Jewel>().Destroying();
            obj2.GetComponent<Jewel>().Destroying();
        }
        else if (obj2 != null && obj1 != null && obj2.GetComponent<Jewel>().type == 9 && obj1.GetComponent<Jewel>().type != 99)
        {
            StartCoroutine(Editor.DestroyAllType(obj1));
            JewelSwap(obj1, obj2);
            obj1.GetComponent<Jewel>().Destroying();
            obj2.GetComponent<Jewel>().Destroying();
        }
        else
        {
            bool check1 = BackChecker(obj1, obj2);
            bool check2 = BackChecker(obj2, obj1);
            if (check1 || check2)
            {
                ListSwap(obj1, obj2);
                obj1.GetComponent<Jewel>().JewelProcessing();
                obj2.GetComponent<Jewel>().JewelProcessing();
                JewelSwap(obj1, obj2);
            }
            else
            {
                obj1.GetComponent<Jewel>().PlayMoveAnimation(obj2.GetComponent<Jewel>().PosMap);
                obj2.GetComponent<Jewel>().PlayMoveAnimation(obj1.GetComponent<Jewel>().PosMap);
                Sound.sound.unSwap();
            }


        }
        ishold = false;
        JewelSelected = null;
        JewelMatch = null;
    }

    bool BackChecker(GameObject obj1, GameObject obj2)
    {
        Vector2 vtmp1 = obj1.GetComponent<Jewel>().PosMap;
        Vector2 vtmp2 = obj2.GetComponent<Jewel>().PosMap;
        int type1 = obj1.GetComponent<Jewel>().type;

        List<Vector2> r = JewelController.RowChecker(type1, (int)vtmp2.y, (int)vtmp2.x);
        List<Vector2> c = JewelController.ColumnChecker(type1, (int)vtmp2.x, (int)vtmp2.y);
        r.Remove(vtmp2);
        c.Remove(vtmp2);
        r.Remove(vtmp1);
        c.Remove(vtmp1);
        if (r.Count == 2 && !d_cheker(r))
            r.Clear();
        if (c.Count == 2 && !d_cheker(c))
            c.Clear();

        if (r.Count >= 2 || c.Count >= 2)
        {
            return true;
        }

        return false;
    }

    bool d_cheker(List<Vector2> r)
    {
        if (Mathf.Abs(r[0].x - r[1].x) > 2 || Mathf.Abs(r[0].y - r[1].y) > 2)
        {
            return false;
        }
        return true;
    }

    IEnumerator waitnomove()
    {
        RemoveSelectEffect(JewelSelected);
        if (MapLoader.Mode == 1)
        {
            Editor.down = true;
            Menu.isRun = false;
            Destroy(Instantiate(nomove), 2.5f);
            yield return new WaitForSeconds(2.5f);
            JewelSpawn.isRespawn = true;
            yield return new WaitForSeconds(1f);
            Menu.isRun = true;
            Editor.down = false;
        }
        else
        {
            JewelSpawn.spawnStart = false;
        }
    }
    void classiclose()
    {
        if (MapLoader.score > long.Parse(PlayerPrefs.GetString("ClassicHightScore", "0")))
            PlayerPrefs.SetString("ClassicHightScore", MapLoader.score.ToString());
        LoseUI.SetActive(true);
        PlayingUI.SetActive(false);
        Sound.sound.fail();
    }

}
