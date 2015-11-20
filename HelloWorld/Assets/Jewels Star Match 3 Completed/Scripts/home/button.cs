using UnityEngine;
using System.Collections;

public class button : MonoBehaviour
{

    public float X = -1;

    // Update is called once per frame
    void Update()
    {
        if (X != -1 && X != transform.localPosition.x)
            MoveToX(X);
    }
    void MoveToX(float x)
    {
        if (Mathf.Abs(x - transform.localPosition.x) > 0.15)
        {
            if (transform.localPosition.x > x)
                transform.localPosition -= new Vector3(Time.smoothDeltaTime * 8f, 0, 0);
            else if (transform.localPosition.x < x)
                transform.localPosition += new Vector3(Time.smoothDeltaTime * 8f, 0, 0);
        }
        else
        {
            transform.localPosition = new Vector3(x, transform.localPosition.y, transform.localPosition.z);
            X = -1;
        }
    }
}
