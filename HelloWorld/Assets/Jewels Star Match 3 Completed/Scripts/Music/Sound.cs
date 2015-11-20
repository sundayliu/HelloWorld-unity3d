using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sound : MonoBehaviour
{

    public static Sound sound;
    public static bool isSound;
    public AudioClip[] aclip;

    void Awake()
    {
        if (sound == null)
        {
            DontDestroyOnLoad(gameObject);
            sound = this;
        }
        else if (sound != this)
        {
            Destroy(gameObject);
        }
    }

    public void jewelclr()
    {
        if (isSound)
        {
            GetComponent<AudioSource>().PlayOneShot(aclip[0]);
        }
    }
    public void boom()
    {
        if (isSound)
        {
            GetComponent<AudioSource>().PlayOneShot(aclip[1]);
        }
    }
    public void bliz()
    {
        if (isSound)
        {
            GetComponent<AudioSource>().PlayOneShot(aclip[2]);
        }
    }
    public void elec()
    {
        if (isSound)
        {
            GetComponent<AudioSource>().PlayOneShot(aclip[3]);
        }
    }
    public void unSwap()
    {
        if (isSound)
        {
            GetComponent<AudioSource>().PlayOneShot(aclip[4]);
        }
    }
    public void icecash()
    {
        if (isSound)
        {
            GetComponent<AudioSource>().PlayOneShot(aclip[5]);
        }
    }
    public void lockcash()
    {
        if (isSound)
        {
            GetComponent<AudioSource>().PlayOneShot(aclip[6]);
        }
    }
    public void click()
    {
        if (isSound)
        {
            GetComponent<AudioSource>().PlayOneShot(aclip[7]);
        }
    }
    public void pass()
    {
        if (isSound)
        {
            GetComponent<AudioSource>().PlayOneShot(aclip[8]);
        }
    }
    public void fail()
    {
        if (isSound)
        {
            GetComponent<AudioSource>().PlayOneShot(aclip[9]);
        }
    }
}
