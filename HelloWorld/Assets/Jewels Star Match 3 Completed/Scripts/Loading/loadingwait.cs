using UnityEngine;
using System.Collections;

public class loadingwait : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        StartCoroutine(LoadLevelByFade());
    }

    IEnumerator LoadLevelByFade()
    {
        // Go to the Home Scene after 3.6 second
        yield return new WaitForSeconds(3.6f);
        // animation transfer scene
        float fadeTime = GameObject.Find("Screen").GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        Application.LoadLevel(GlobalConsts.SCENE_HOME);
    }

}
