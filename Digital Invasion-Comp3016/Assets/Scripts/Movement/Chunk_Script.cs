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

    public Chunk_Script fromChunk;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CalcFCost()
    {
        fCost = gCost + hCost;
    }

    public void SetPositions(int x, int z)
    {
        positionX = x;
        positionZ = z;
    }
}
