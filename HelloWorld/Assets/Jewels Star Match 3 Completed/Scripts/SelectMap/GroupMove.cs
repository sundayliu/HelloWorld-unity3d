using UnityEngine;
using System.Collections;

public class GroupMove : MonoBehaviour
{

    public float X = -100;
    bool left;
    // Update is called once per frame
    void Update()
    {
        if (X != -100 && X != transform.localPosition.x)
            MoveToX(X);
    }
    void MoveToX(float x)
    {
        if (Mathf.Abs(x - transform.localPosition.x) > 0.15)
        {
            if (transform.localPosition.x > x)
            {
                transform.localPosition -= new Vector3(Time.smoothDeltaTime * 10f, 0, 0);
                left = true;
            }
            else if (transform.localPosition.x < x)
            {
                transform.localPosition += new Vector3(Time.smoothDeltaTime * 10f, 0, 0);
                left = false;
            }
        }
        else
        {
            transform.localPosition = new Vector3(x, transform.localPosition.y, transform.localPosition.z);
            X = -100;
            if (left && SelectMapTouch.GroupIndex < 5)
                SelectMapTouch.GroupIndex++;
            if (!left && SelectMapTouch.GroupIndex > 0)
                SelectMapTouch.GroupIndex--;
        }
    }
}
