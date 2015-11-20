using UnityEngine;
using System.Collections;

public class brScale : MonoBehaviour
{
    public float X = -1;

    // Update is called once per frame
    void Update()
    {
        if (X != -1 && X != transform.localScale.x)
            ScaleX(X);
    }
    void ScaleX(float x)
    {
        if (Mathf.Abs(x - transform.localScale.x) > 0.45)
        {
            if (transform.localScale.x > x)
                transform.localScale -= new Vector3(Time.smoothDeltaTime * 31.6f, 0, 0);
            else if (transform.localScale.x < x)
                transform.localScale += new Vector3(Time.smoothDeltaTime * 31.6f, 0, 0);
        }
        else
        {
            transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
            X = -1;
        }
    }
}
