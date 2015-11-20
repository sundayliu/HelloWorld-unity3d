using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
/// <summary>
/// manager cell grid
/// </summary>
public class CellScript : MonoBehaviour
{
    static public int[,] map;
    public GameObject CellPrefab;
    public Vector2 Size;
    public Transform parent;
    public static GameObject[,] Cells;
    public GameObject[] CellEffect;
    string cellmap;
    public static bool isCellMove = false;
    public static bool movedone = true;


    // Use this for initialization
    void Start()
    {

        Cells = new GameObject[7, 9];
        map = new int[7, 9];
        map = mapReader(MapLoader.MapPlayer.Name);
        GribCreate(map);
    }

    void Update()
    {
        if (!movedone)
        {
            GribMove();
        }

    }
    /// <summary>
    /// create cell grid 
    /// </summary>
    /// <param name="map"></param>
    public void GribCreate(int[,] map)
    {
        if (MapLoader.Mode == 1)
        {
            MapLoader.CellNotEmpty = 0;
            for (int y = 0; y < Size.y; y++)
                for (int x = 0; x < Size.x; x++)
                    if (map[x, y] > 0)
                    {
                        if (map[x, y] < 10)
                            CellPrefab.GetComponent<SpriteRenderer>().sprite = CellPrefab.GetComponent<Cell>().cellsprite[map[x, y]];
                        else
                            CellPrefab.GetComponent<SpriteRenderer>().sprite = CellPrefab.GetComponent<Cell>().cellsprite[map[x, y] / 10];
                        GameObject Cell = Instantiate(CellPrefab) as GameObject;
                        Cell.transform.parent = parent;
                        Cell.transform.localPosition = new Vector3(x, y, -0.2f);
                        Cell.transform.localScale = new Vector3(1.98f, 1.945f, 1f);
                        Cell.GetComponent<Cell>().Type = map[x, y];
                        Cells[x, y] = Cell;
                        MapLoader.CellNotEmpty++;
                    }
        }
        else
        {
            for (int y = 0; y < 9; y++)
                for (int x = 0; x < 7; x++)
                {
                    CellPrefab.GetComponent<SpriteRenderer>().sprite = CellPrefab.GetComponent<Cell>().cellsprite[0];
                    GameObject Cell = Instantiate(CellPrefab) as GameObject;
                    Cell.transform.parent = parent;
                    Cell.transform.localPosition = new Vector3(x, y, -0.2f);
                    Cell.transform.localScale = new Vector3(1.98f, 1.945f, 1f);
                    Cell.GetComponent<Cell>().Type = map[x, y];
                    Cells[x, y] = Cell;
                }
        }
    }
    /// <summary>
    /// move grid to start game position
    /// </summary>
    void GribMove()
    {
        if (parent.transform.localPosition.x > -2.87f)
            parent.localPosition -= new Vector3(Time.smoothDeltaTime * 8, 0, 0);
        else
        {
            parent.localPosition = new Vector3(-2.87f, parent.localPosition.y, parent.localPosition.z);
            movedone = true;
        }
    }
    /// <summary>
    /// read map data from resource folder by name
    /// </summary>
    /// <param name="mapname"></param>
    /// <returns></returns>
    int[,] mapReader(string mapname)
    {
        int[,] tmpmap = new int[7, 9];
        string mapdata = "";
        string assetpath = @"Assets/Jewels Star Match 3 Completed/Resources/Maps/" + mapname + ".txt";
#if UNITY_EDITOR
        mapdata = UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset>(@assetpath).ToString();
#else

				TextAsset txtass = (TextAsset)Resources.Load ("Maps/" + mapname, typeof(TextAsset));
				mapdata = txtass.ToString ();
#endif
        string[] result = mapdata.Split(new char[] { '	', '\n' });
        int dem = 0;
        for (int y = 8; y >= 0; y--)
            for (int x = 0; x < 7; x++)
            {
                tmpmap[x, y] = int.Parse(result[dem]);
                dem++;
            }
        return tmpmap;
    }
}
