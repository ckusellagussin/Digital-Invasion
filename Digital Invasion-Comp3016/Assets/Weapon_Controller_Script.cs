using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Controller_Script : MonoBehaviour
{
    [SerializeField]
    GameObject HandBone;

    // Update is called once per frame
    void Update()
    {
        transform.position = HandBone.transform.position;
        transform.rotation = HandBone.transform.rotation;
    }
}
