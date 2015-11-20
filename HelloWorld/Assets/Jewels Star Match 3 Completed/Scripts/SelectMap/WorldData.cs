using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldData : MonoBehaviour
{

    public static List<Player> Maps;
    public static int world;
    public GameObject levelprefeb;
    public Transform[] groupparent;
    int dem = 0;
    // Use this for initialization
    void Start()
    {
        Maps = new List<Player>();
        WorldMapdataloader();
        MapDraw();
    }

    // Update is called once per frame
    void MapDraw()
    {
        dem = 0;
        for (int i = 0; i <= 4; i++)
            LoadLevelToGroup(i);

    }

    void LoadLevelToGroup(int i)
    {
        float y = -1.15f;
        for (int j = 0; j < 5; j++)
            for (int k = -1; k <= 2; k++)
            {
                InsLevel(dem, i, k, j * y);
                dem++;
                if (dem == 99)
                    break;
            }
    }
    void InsLevel(int lv, int groupindex, float x, float y)
    {

        GameObject tmp = Instantiate(levelprefeb) as GameObject;
        tmp.GetComponent<level>().map = Maps[lv];

        tmp.transform.parent = groupparent[groupindex].transform;
        tmp.transform.localPosition = new Vector3(x, y, 0);

    }
    void WorldMapdataloader()
    {
        for (int i = world * 99; i < (world + 1) * 99; i++)
        {
            Maps.Add(DataLoader.DataPlayer[i]);
        }
    }
}
