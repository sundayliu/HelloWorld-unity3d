using UnityEngine;
using System.Collections;

public class Win : MonoBehaviour
{

    public static bool win = false;
    void Awake()
    {
        win = false;
    }

    public void checkWin(int x, int y)
    {
        if (y == min(x))
        {
            win = true;
        }
    }

    int min(int x)
    {
        int tmp = 0;
        for (int i = 0; i < 9; i++)
            if (CellScript.map[x, i] > 0)
            {
                tmp = i;
                break;
            }
        return tmp;
    }
}
