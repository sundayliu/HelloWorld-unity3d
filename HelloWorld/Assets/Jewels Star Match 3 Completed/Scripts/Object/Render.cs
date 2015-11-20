using UnityEngine;
using System.Collections;

public class Render : MonoBehaviour
{
    Animator Anim;
    void Start()
    {
        Anim = GetComponent<Animator>();
    }
    public void defautanimationstate(int state)
    {
        Anim.SetInteger("state", state);
    }
}
