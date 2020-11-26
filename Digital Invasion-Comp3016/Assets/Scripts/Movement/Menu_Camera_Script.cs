using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu_Camera_Script : MonoBehaviour
{
    public float speed;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0.05f * speed, 0);
    }
}
