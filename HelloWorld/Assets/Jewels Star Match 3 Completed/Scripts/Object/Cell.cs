using UnityEngine;
using System.Collections;

public class Cell : MonoBehaviour
{

    public int Type;
    public Sprite[] cellsprite;
    public GameObject[] CellEffect;
    public GameObject star;
    public GameObject stareff;
    int x;
    int y;
    bool isEmpty = false;
    SpriteRenderer mSpriteRenderer;
    Animation anim;
    Transform mTranform;

    void Start()
    {
        mTranform = transform;
        mSpriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animation>();
        x = Mathf.RoundToInt(transform.localPosition.x);
        y = Mathf.RoundToInt(transform.localPosition.y);
        CellEffectAdd();
    }
    /// <summary>
    /// check cell type and show star condition
    /// </summary>
    public void playAnimation()
    {
        if (mTranform.childCount > 0)
            Destroy(mTranform.GetChild(0).gameObject);

        if (MapLoader.Mode == 1)
        {
            if (Type < 0)
                Type = 0;

            if (Type > 0)
            {
                Animationplayer();
                Type--;
            }
            if (Type <= 0 && !isEmpty)
            {
                isEmpty = true;
                MapLoader.CellNotEmpty--;
                if (MapLoader.CellNotEmpty <= 0 && !MapLoader.Starwin)
                {
                    MapLoader.Starwin = true;
                    Process.showstar = true;
                }
            }
        }
    }
    /// <summary>
    /// run animation change sprite 's cell by cell type
    /// </summary>
    void Animationplayer()
    {
        if (Type == 3)
            anim.Play("redtoblue");
        else if (Type == 2)
            anim.Play("bluetogray");
        else if (Type == 1)
            anim.Play("graytotranf");
    }

    /// <summary>
    /// remove cell effect
    /// </summary>
    public void RemoveCellEffect()
    {
        if (Type % 10 == 4 && !checkJewelInCell() && checkmap())
            JewelSpawn.QuaX[x]++;

        if (Type > 10)
        {
            int s = Type % 10;
            Type = Type / 10;

            CellScript.map[x, y] = Type;
            if (JewelSpawn.JewelList[x, y] != null)
                JewelSpawn.JewelList[x, y].GetComponent<Jewel>().JewelProcessing();
            transform.GetChild(0).GetComponent<Animator>().enabled = true;

            if (s == 4)
                Sound.sound.lockcash();
            else Sound.sound.icecash();

            Destroy(transform.GetChild(0).gameObject, 0.5f);
        }

    }
    /// <summary>
    /// add  cell effect
    /// </summary>
    void CellEffectAdd()
    {
        if (Type > 10)
        {
            int t = (Type % 10) - 4;
            GameObject tmp = Instantiate(CellEffect[t]) as GameObject;
            tmp.transform.parent = transform;
            tmp.transform.localPosition = new Vector3(0, 0, -1.2f);
            if (Type % 10 == 5)
                tmp.transform.localScale = new Vector3(0.4f, 0.41f, 1);

        }
    }

    bool checkJewelInCell()
    {
        return false;
    }
    /// <summary>
    /// check locked cell on column
    /// </summary>
    /// <returns></returns>
    bool checkmap()
    {
        for (int i = y + 1; i < 9; i++)
            if (CellScript.map[x, i] % 10 == 4)
                return false;
        return true;

    }
    /// <summary>
    /// change sprite 's cell
    /// </summary>
    /// <param name="idx"></param>
    void setSprite(int idx)
    {
        mSpriteRenderer.sprite = cellsprite[idx];
    }

}
