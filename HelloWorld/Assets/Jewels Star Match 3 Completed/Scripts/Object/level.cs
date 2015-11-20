using UnityEngine;
using System.Collections;

public class level : MonoBehaviour
{

    public Player map = new Player();
    public GameObject star;
    public Sprite[] mapsprite;
    public Sprite[] starSprite;
    int lvshow;

    void Start()
    {
        if (!map.UnLocked)
        {
            GetComponent<SpriteRenderer>().sprite = mapsprite[1];
            star.SetActive(false);
            GetComponent<BoxCollider2D>().enabled = false;
            transform.GetChild(1).GetComponent<TextMesh>().text = "";
        }
        else
        {
            star.GetComponent<SpriteRenderer>().sprite = starSprite[map.Stars];
            if (map.Level <= 99)
                lvshow = map.Level;
            else if (map.Level <= 198)
                lvshow = map.Level - 99;
            else
                lvshow = map.Level - 198;
            transform.GetChild(1).GetComponent<TextMesh>().text = (lvshow).ToString();
        }
    }
}
