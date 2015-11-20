using UnityEngine;
using System.Collections;

public class WebManager : MonoBehaviour {

    string m_info = "Nothing";

    public Texture2D m_uploadImage;

    protected Texture2D m_downloadTexture;

    protected AudioClip m_downloadClip;

	// Use this for initialization
	void Start () {

        StartCoroutine(DownloadSound());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        GUI.BeginGroup(new Rect(Screen.width * 0.5f - 100, Screen.height * 0.5f - 100, 500, 200), "");

       
        GUI.Label(new Rect(10, 10, 400, 30), m_info);

        if ( m_downloadTexture!=null )
            GUI.DrawTexture(new Rect(0, 0, m_downloadTexture.width, m_downloadTexture.height), m_downloadTexture);

        if (GUI.Button(new Rect(10, 50, 150, 30), "Get Data"))
        {
            StartCoroutine(IGetData());
        }

        if (GUI.Button(new Rect(10, 100, 150, 30), "Post Data"))
        {
            StartCoroutine(IPostData());
        }

        if (GUI.Button(new Rect(10, 150, 150, 30), "Request Image"))
        {
            StartCoroutine(IRequestPNG());
        }


        GUI.EndGroup();
    }

    IEnumerator IGetData()
    {
        WWW www = new WWW("http://127.0.0.1/Test.php?username=get&password=12345");

        yield return www;

        if (www.error != null)
        {
            m_info = www.error;
            yield return null;
        }

        m_info = www.text;
    }

    IEnumerator IPostData()
    {
        System.Collections.Hashtable headers = new System.Collections.Hashtable();
        headers.Add("Content-Type", "application/x-www-form-urlencoded");

        string data = "username=post&password=6789";
        byte[] bs = System.Text.UTF8Encoding.UTF8.GetBytes(data);

        WWW www = new WWW("http://127.0.0.1/Test.php", bs, headers);

        yield return www;

        if (www.error != null)
        {
            m_info = www.error;
            yield return null;
        }

        m_info = www.text;
    }

    IEnumerator IRequestPNG()
    {
        byte[] bs = m_uploadImage.EncodeToPNG();

        WWWForm form = new WWWForm();
        form.AddBinaryData("picture", bs, "screenshot", "image/png");

        WWW www = new WWW("http://127.0.0.1/Test.php", form);

        yield return www;

        if (www.error != null)
        {
            m_info = www.error;
            yield return null;
        }

        m_downloadTexture = www.texture;

    }

    IEnumerator DownloadSound()
    {

        WWW www = new WWW("http://127.0.0.1/music.wav");

        yield return www;

        if (www.error != null)
        {
            m_info = www.error;
            yield return null;
        }

        m_downloadClip = www.GetAudioClip(false);

        audio.PlayOneShot(m_downloadClip);

    }


}
