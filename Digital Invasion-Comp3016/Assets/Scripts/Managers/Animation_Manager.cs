using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation_Manager : MonoBehaviour
{
    public Animator anim;

    public void Run(bool running)
    {
        anim.SetBool("Running", running);
    }

    public void Die()
    {
        anim.SetBool("Die", true);
    }
}
