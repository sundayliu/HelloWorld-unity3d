using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class DataLoader : MonoBehaviour
{

    // Use this for initialization
    public GameObject[] worlds;
    public static List<Player> DataPlayer;
    void Start()
    {
        DataPlayer = new List<Player>();
        //PlayerPrefs.DeleteAll ();
        if (bool.Parse(PlayerPrefs.GetString("FIRSTTIME", "True")))
            DataDefaultLoader();
        SaveDataToList();


    }

    void setWorldUnlock()
    {
        if (DataPlayer[0].UnLocked)
        {
            worlds[0].transform.GetChild(0).gameObject.SetActive(false);
            worlds[0].transform.GetChild(1).GetComponent<TextMesh>().text = UnlockCount(DataPlayer, 0).ToString() + "/99";

        }
        else
        {
            worlds[0].GetComponent<BoxCollider2D>().enabled = false;
            worlds[0].transform.GetChild(1).gameObject.SetActive(false);
        }

        if (DataPlayer[99].UnLocked)
        {
            worlds[1].transform.GetChild(0).gameObject.SetActive(false);
            worlds[1].transform.GetChild(1).GetComponent<TextMesh>().text = UnlockCount(DataPlayer, 1).ToString() + "/99";

        }
        else
        {
            worlds[1].GetComponent<BoxCollider2D>().enabled = false;
            worlds[1].transform.GetChild(1).gameObject.SetActive(false);
        }

        if (DataPlayer[198].UnLocked)
        {
            worlds[2].transform.GetChild(0).gameObject.SetActive(false);
            worlds[2].transform.GetChild(1).GetComponent<TextMesh>().text = UnlockCount(DataPlayer, 2).ToString() + "/99";
        }
        else
        {
            worlds[2].GetComponent<BoxCollider2D>().enabled = false;
            worlds[2].transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    int UnlockCount(List<Player> l, int worldindex)
    {
        int tmp = 0;
        for (int i = worldindex * 99; i < (worldindex + 1) * 99; i++)
        {
            if (l[i].UnLocked) tmp++;
        }
        return tmp;
    }

    void SaveDataToList()
    {
        List<Player> tmp = new List<Player>();
        PlayerPrefsSerializer mpp = new PlayerPrefsSerializer();
        tmp = mpp.LoadPref();
        for (int i = 0; i < 297; i++)
        {
            DataPlayer.Add(tmp[i]);
        }
        setWorldUnlock();
    }

    // Update is called once per frame
    void DataDefaultLoader()
    {
        string AssetFileName = "WorldData";
        string AssetFilePath;
#if UNITY_IPHONE
    AssetFilePath = @"Assets/Jewels Star Match 3 Completed/Resources/" + AssetFileName + ".txt";
#else
        AssetFilePath = @"Assets/Jewels Star Match 3 Completed/Resources/" + AssetFileName + ".xml";
#endif

        string XmlString = "";
#if UNITY_EDITOR
        XmlString = UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset>(AssetFilePath).ToString();
#else
				XmlString = ((TextAsset)Resources.Load (AssetFileName, typeof(TextAsset))).ToString ();
#endif
        PlayerPrefs.SetString("DATA", XmlString);
        PlayerPrefs.SetString("FIRSTTIME", "False");
    }

    byte[] Base64StrToByteArray(string str)
    {
        return System.Text.Encoding.UTF8.GetBytes(str);
    }
}
