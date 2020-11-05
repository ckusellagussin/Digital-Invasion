using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk_Script : MonoBehaviour
{
    public int positionX;
    public int positionZ;

    public int gCost;
    public int hCost;
    public int fCost;

    public bool impassable = false;

    public Chunk_Script fromChunk;

    // Start is called before the first frame update
    void Start()
    {
        Ray ray = new Ray(this.transform.position, this.transform.up);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("Tall Cover"))
            {
                impassable = true;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CalcFCost()
    {
        fCost = gCost + hCost;
        if (impassable)
        {
            fCost = int.MaxValue;
        }
    }

    public void SetPositions(int x, int z)
    {
        positionX = x;
        positionZ = z;
    }
}
