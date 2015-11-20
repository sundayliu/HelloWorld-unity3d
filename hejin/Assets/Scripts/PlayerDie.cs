using UnityEngine;
using System.Collections;
/// <summary>
/// 主角死亡状态
/// </summary>
public class PlayerDie : MonoBehaviour {
    public SpriteRenderer srDie;
    public Sprite[] spDie;
    public float animSpeed = 20;
    private float animInterval = 0;
    private int spLength = 0;
    private int spIndex = 0;
    private float animTimer = 0;
    bool isDie = false;
	// Use this for initialization
	void Start () {
        animInterval = 1 / animSpeed;
        spLength = spDie.Length;
	}
	
	// Update is called once per frame
    void Update()
    {
        if (!isDie)
        {
            animTimer += Time.deltaTime;
            if (animTimer > animInterval)
            {
                animTimer -= animInterval;
                spIndex++;
                spIndex %= spLength;
                srDie.sprite = spDie[spIndex];
                if (spIndex == spLength - 1)
                {
                    isDie = true;
                    Destroy(gameObject,1f);
                }
            }
        }
        else
        {
            dieControl();
        }
    }
    float dieControlTimer = 0;
    bool isHide = false;
    private void dieControl()
    {
        dieControlTimer += Time.deltaTime;
        if (dieControlTimer>0.1f)
        {
            dieControlTimer -= 0.1f;
            if (isHide)
                srDie.sprite = null;
            else
                srDie.sprite = spDie[spLength - 1];
            isHide = !isHide;
        }
    }
}
