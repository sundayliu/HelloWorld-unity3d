using UnityEngine;
using System.Collections;

public class EffectTimer : MonoBehaviour
{

    float ComboResetTime = 1.2f;
    public static int Combo = 0;
    public static bool isResetCombo = true;

    void Start()
    {
        Combo = 0;
        StartCoroutine(ComboResetTicker());
    }

    IEnumerator ComboResetTicker()
    {
        while (true)
        {
            yield return new WaitForSeconds(ComboResetTime);
            if (isResetCombo)
            {
                Combo = 0;
            }
            isResetCombo = true;
        }
    }
}
