using UnityEngine;
using System.Collections;

public class Music : MonoBehaviour
{

    public static Music music;
    public static bool isMusic;

    void Awake()
    {
        if (music == null)
        {
            DontDestroyOnLoad(gameObject);
            music = this;
        }
        else if (music != this)
        {
            Destroy(gameObject);
        }
    }

    public void MusicON()
    {
        GetComponent<AudioSource>().mute = false;
    }
    public void MusicOFF()
    {
        GetComponent<AudioSource>().mute = true;
    }

}
