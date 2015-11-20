using UnityEngine;
using System.Collections;
/// <summary>
/// 开始菜单按钮等切换控制
/// </summary>
public class GameMenu : MonoBehaviour {

    public GameObject menu;
    public GameObject opration;
    public GameObject diffcult;
    public GameObject charatorselect;
    public Color oprationColor;

    public void Start()
    {
        Screen.orientation = ScreenOrientation.AutoRotation;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
    }
	// Use this for initialization

    public void OnMenuPlay()
    {
        menu.SetActive(false);
        opration.SetActive(true);
    }
    public void OnOprationPlay()
    {
        GameObject.Find("OprationButton").SetActive(false);
        opration.GetComponent<UISprite>().color = oprationColor;
        diffcult.SetActive(true);
    }
    public void OnDiffcultPlay()
    {
        PlayGame();
        diffcult.SetActive(false);
        opration.SetActive(false);
     
        //charatorselect.SetActive(true);
    }
    public void PlayGame()
    {
        charatorselect.SetActive(false);
        Application.LoadLevel(1);
    }
}
