using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Material_Container_Script : MonoBehaviour
{
    public List<string> tagList;
    public List<Material> materialList;

    public Material FindMaterial(string tag)
    {
        if (tagList.Contains(tag))
        {
            return materialList[tagList.IndexOf(tag)];
        } 
        else
        {
            return materialList[0];
        }
    }
}
