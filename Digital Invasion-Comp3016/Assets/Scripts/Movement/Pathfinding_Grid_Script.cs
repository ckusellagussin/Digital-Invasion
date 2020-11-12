using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding_Grid_Script : MonoBehaviour
{
    public int width;
    public int height;

    public GameObject prefabChunk;
    public AI_Script aiPathfinder;
    public List<Chunk_Script> chunksL;

    // Start is called before the first frame update
    void Start()
    {
        chunksL = new List<Chunk_Script>();
        for(int i = -width + (width/2); i <= width/2; i++)
        {
            for (int j = -height + (height / 2); j <= height / 2; j++)
            {
                GameObject temp = Instantiate(prefabChunk, transform);
                temp.transform.position = new Vector3(i, 0, j);
                temp.GetComponent<Chunk_Script>().SetPositions(i, j, aiPathfinder);
                chunksL.Add(temp.GetComponent<Chunk_Script>());
            }
        }
    }

    public Chunk_Script GetChunk(int x, int z)
    {
        foreach(Chunk_Script cs in chunksL)
        {
            if (cs.positionX == x && cs.positionZ == z)
            {
                return cs;
            }
        }

        return null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
