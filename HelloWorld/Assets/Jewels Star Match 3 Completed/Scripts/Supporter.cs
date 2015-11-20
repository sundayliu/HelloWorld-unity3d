using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Supporter : MonoBehaviour
{

    int[,] virtualjewel = new int[7, 9];
    public Vector2[] Result = new Vector2[2];

    public void SetVirtualJewel()
    {
        for (int x = 0; x < 7; x++)
            for (int y = 0; y < 9; y++)
            {
                GameObject tmp = JewelSpawn.JewelList[x, y];
                if (tmp != null && CellScript.map[x, y] < 10)
                {
                    virtualjewel[x, y] = tmp.GetComponent<Jewel>().type;
                }
                else virtualjewel[x, y] = -1;
            }
    }

    public GameObject[] MoveSupportGameObject()
    {

        SetVirtualJewel();
        Vector2[] tmp = new Vector2[2];
        GameObject[] Objtmp = new GameObject[2];
        for (int i = 0; i < 7; i++)
            for (int j = 0; j < 9; j++)
            {
                if (virtualjewel[i, j] >= 0)
                {
                    tmp = positionChecker(i, j);
                    if (tmp[0].x != -1)
                    {
                        Result = tmp;
                        goto mgoto;
                    }
                }
            }
        Result = tmp;
    mgoto:
        if (Result[0].x != -1)
        {
            Objtmp = ObjFinder(Result);
        }

        return Objtmp;

    }
    GameObject[] ObjFinder(Vector2[] v)
    {
        GameObject[] tmp = new GameObject[2];
        tmp[0] = JewelSpawn.JewelList[(int)v[0].x, (int)v[0].y];
        tmp[1] = JewelSpawn.JewelList[(int)v[1].x, (int)v[1].y];
        return tmp;

    }

    void setDefaut()
    {
        for (int i = 0; i < 7; i++)
            for (int j = 0; j < 9; j++)
                virtualjewel[i, j] = -1;
    }

    Vector2[] positionChecker(int x, int y)
    {
        List<Vector2> sameType = new List<Vector2>();
        Vector2[] tmp = new Vector2[2];
        tmp[0] = new Vector2(-1, -1);
        tmp[1] = new Vector2(-1, -1);
        sameType = same(x, y);

        if (sameType.Count < 0)
            return tmp;
        else
        {
            for (int i = 0; i < sameType.Count; i++)
            {
                tmp = YChecker(sameType[i], x, y);
                if (tmp[0].x != -1)
                    return tmp;
                else
                {
                    tmp = XChecker(sameType[i], x, y);
                    if (tmp[0].x != -1)
                        return tmp;
                }
            }

            tmp = JumpChecker(x, y);
            if (tmp[0].x != -1)
            {
                return tmp;
            }
        }

        return tmp;
    }

    Vector2[] XChecker(Vector2 v, int x, int y)
    {

        Vector2[] tmp = new Vector2[2];
        tmp[0] = new Vector2(-1, -1);
        tmp[1] = new Vector2(-1, -1);

        if (virtualjewel[x, y] == 9)
        {
            if (y + 1 < 9 && virtualjewel[x, y + 1] >= 0)
            {
                tmp[0] = new Vector2(x, y);
                tmp[1] = new Vector2(x, y + 1);
                return tmp;
            }
            else if (y - 1 >= 0 && virtualjewel[x, y - 1] >= 0)
            {
                tmp[0] = new Vector2(x, y);
                tmp[1] = new Vector2(x, y - 1);
                return tmp;
            }
        }


        if ((int)v.y > y && y + 2 < 9 && virtualjewel[x, y + 2] >= 0)
        {
            if (x - 1 >= 0 && virtualjewel[x - 1, y + 2] == virtualjewel[x, y])
            {
                tmp[0] = new Vector2(x, y + 2);
                tmp[1] = new Vector2(x - 1, y + 2);
                return tmp;
            }
            else if (x + 1 <= 6 && virtualjewel[x + 1, y + 2] == virtualjewel[x, y])
            {
                tmp[0] = new Vector2(x, y + 2);
                tmp[1] = new Vector2(x + 1, y + 2);
                return tmp;
            }
            else if (y + 3 <= 8 && virtualjewel[x, y + 3] == virtualjewel[x, y])
            {
                tmp[0] = new Vector2(x, y + 2);
                tmp[1] = new Vector2(x, y + 3);
                return tmp;
            }
        }

        else if ((int)v.y < y && y - 2 >= 0 && virtualjewel[x, y - 2] >= 0)
        {
            if (x - 1 >= 0 && virtualjewel[x - 1, y - 2] == virtualjewel[x, y])
            {
                tmp[0] = new Vector2(x, y - 2);
                tmp[1] = new Vector2(x - 1, y - 2);
                return tmp;
            }
            else if (x + 1 <= 6 && virtualjewel[x + 1, y - 2] == virtualjewel[x, y])
            {
                tmp[0] = new Vector2(x, y - 2);
                tmp[1] = new Vector2(x + 1, y - 2);
                return tmp;
            }
            else if (y - 3 >= 0 && virtualjewel[x, y - 3] == virtualjewel[x, y])
            {
                tmp[0] = new Vector2(x, y - 2);
                tmp[1] = new Vector2(x, y - 3);
                return tmp;
            }
        }

        return tmp;

    }

    Vector2[] YChecker(Vector2 v, int x, int y)
    {
        Vector2[] tmp = new Vector2[2];
        tmp[0] = new Vector2(-1, -1);
        tmp[1] = new Vector2(-1, -1);

        if (virtualjewel[x, y] == 9)
        {
            if (x + 1 < 7 && virtualjewel[x + 1, y] >= 0)
            {
                tmp[0] = new Vector2(x, y);
                tmp[1] = new Vector2(x + 1, y);
                return tmp;
            }
            else if (x - 1 >= 0 && virtualjewel[x - 1, y] >= 0)
            {
                tmp[0] = new Vector2(x, y);
                tmp[1] = new Vector2(x - 1, y);
                return tmp;
            }
        }

        if ((int)v.x > x && x + 2 < 7 && virtualjewel[x + 2, y] >= 0)
        {
            if (y - 1 >= 0 && virtualjewel[x + 2, y - 1] == virtualjewel[x, y])
            {
                tmp[0] = new Vector2(x + 2, y);
                tmp[1] = new Vector2(x + 2, y - 1);
                return tmp;
            }
            else if (y + 1 <= 8 && virtualjewel[x + 2, y + 1] == virtualjewel[x, y])
            {
                tmp[0] = new Vector2(x + 2, y);
                tmp[1] = new Vector2(x + 2, y + 1);
                return tmp;
            }
            else if (x + 3 <= 6 && virtualjewel[x + 3, y] == virtualjewel[x, y])
            {
                tmp[0] = new Vector2(x + 2, y);
                tmp[1] = new Vector2(x + 3, y);
                return tmp;
            }
        }
        else if ((int)v.x < x && x - 2 >= 0 && virtualjewel[x - 2, y] >= 0)
        {
            if (y - 1 >= 0 && virtualjewel[x - 2, y - 1] == virtualjewel[x, y])
            {
                tmp[0] = new Vector2(x - 2, y);
                tmp[1] = new Vector2(x - 2, y - 1);
                return tmp;
            }
            else if (y + 1 <= 8 && virtualjewel[x - 2, y + 1] == virtualjewel[x, y])
            {
                tmp[0] = new Vector2(x - 2, y);
                tmp[1] = new Vector2(x - 2, y + 1);
                return tmp;
            }
            else if (x - 3 >= 0 && virtualjewel[x - 3, y] == virtualjewel[x, y])
            {
                tmp[0] = new Vector2(x - 2, y);
                tmp[1] = new Vector2(x - 3, y);
                return tmp;
            }
        }
        return tmp;
    }

    List<Vector2> same(int x, int y)
    {
        List<Vector2> lsttmp = new List<Vector2>();
        Vector2[] tmp = new Vector2[4];
        tmp[0] = new Vector2(x - 1, y);
        tmp[1] = new Vector2(x + 1, y);
        tmp[2] = new Vector2(x, y - 1);
        tmp[3] = new Vector2(x, y + 1);

        if (virtualjewel[x, y] == 9)
        {
            for (int i = 0; i < 4; i++)
                if ((int)tmp[i].x >= 0 && (int)tmp[i].y >= 0)
                    if ((int)tmp[i].x < 7 && (int)tmp[i].y < 9)
                        lsttmp.Add(tmp[i]);
            return lsttmp;
        }


        for (int i = 0; i < 4; i++)
            if ((int)tmp[i].x >= 0 && (int)tmp[i].y >= 0)
                if ((int)tmp[i].x < 7 && (int)tmp[i].y < 9)
                    if (virtualjewel[(int)tmp[i].x, (int)tmp[i].y] == virtualjewel[x, y])
                        lsttmp.Add(tmp[i]);
        return lsttmp;

    }

    List<Vector2> sameJump(int x, int y)
    {
        List<Vector2> lsttmp = new List<Vector2>();
        Vector2[] tmp = new Vector2[4];
        tmp[0] = new Vector2(x - 1, y - 1);
        tmp[1] = new Vector2(x + 1, y + 1);
        tmp[2] = new Vector2(x + 1, y - 1);
        tmp[3] = new Vector2(x - 1, y + 1);

        for (int i = 0; i < 4; i++)
            if ((int)tmp[i].x >= 0 && (int)tmp[i].y >= 0)
                if ((int)tmp[i].x < 7 && (int)tmp[i].y < 9)
                    if (virtualjewel[(int)tmp[i].x, (int)tmp[i].y] == virtualjewel[x, y])
                        lsttmp.Add(tmp[i]);

        return lsttmp;

    }

    Vector2[] JumpChecker(int x, int y)
    {
        List<Vector2> sameType = new List<Vector2>();
        Vector2[] tmp = new Vector2[2];
        tmp[0] = new Vector2(-1, -1);
        tmp[1] = new Vector2(-1, -1);
        sameType = sameJump(x, y);


        if (sameType.Count < 2)
            return tmp;
        else
        {
            for (int i = 0; i < sameType.Count; i++)
                for (int j = i + 1; j < sameType.Count; j++)
                    if ((sameType[i].x + sameType[j].x) / 2 != x || (sameType[i].y + sameType[j].y) / 2 != y)
                    {
                        int tmpx = (int)(sameType[i].x + sameType[j].x) / 2;
                        int tmpy = (int)(sameType[i].y + sameType[j].y) / 2;
                        if (virtualjewel[tmpx, tmpy] >= 0)
                        {
                            tmp[0] = new Vector2(x, y);
                            tmp[1] = new Vector2(tmpx, tmpy);
                            return tmp;
                        }
                    }
        }
        return tmp;
    }
}
