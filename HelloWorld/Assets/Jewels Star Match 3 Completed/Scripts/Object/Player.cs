using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

public class Player
{

    [XmlAttribute("Level")]
    public int Level;
    [XmlAttribute("Name")]
    public string Name;
    [XmlAttribute("Locked")]
    public bool UnLocked;
    [XmlAttribute("Stars")]
    public int Stars;
    [XmlAttribute("HightScore")]
    public long HightScore;
}

[XmlRoot("WorldMap")]
public class PlayerUtils
{
    [XmlArrayItem("Map")]
    public List<Player> Maps = new List<Player>();
    public string path = "";

    public void Save()
    {

        var serializer = new XmlSerializer(typeof(PlayerUtils));
        using (var stream = new FileStream(path, FileMode.Create))
        {
            serializer.Serialize(stream, this);
        }

    }

    public List<Player> load()
    {
        var serializer = new XmlSerializer(typeof(PlayerUtils));
        using (var stream = new FileStream(path, FileMode.Open))
        {
            return (serializer.Deserialize(stream) as PlayerUtils).Maps;
        }

    }

    public void Update(Player map)
    {
        for (int i = 0; i < Maps.Count; i++)
            if (Maps[i].Level == map.Level)
            {
                Maps[i] = map;
                break;
            }
        Save();
    }
}

[XmlRoot("WorldMap")]
public class PlayerPrefsSerializer
{
    [XmlArrayItem("Map")]
    public List<Player> Maps = new List<Player>();
    public string key = "DATA";

    public void SavePref()
    {

        var mySeriData = new XmlSerializer(typeof(PlayerPrefsSerializer));
        StringWriter Writer = new StringWriter();
        mySeriData.Serialize(Writer, this);
        string tmp = Writer.ToString();
        PlayerPrefs.SetString(key, tmp);
        Writer.Close();
    }

    public List<Player> LoadPref()
    {
        string tmp = PlayerPrefs.GetString(key);
        StringReader Reader = new StringReader(tmp); ;
        XmlSerializer xmlseri = new XmlSerializer(typeof(PlayerPrefsSerializer));
        return (xmlseri.Deserialize(Reader) as PlayerPrefsSerializer).Maps;
        //Reader.Close();
    }
    public void Update(int lv, Player map)
    {
        Maps[lv - 1] = map;
        SavePref();
    }

}