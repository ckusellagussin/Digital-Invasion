using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Script : MonoBehaviour
{
    public List<Chunk_Script> openPath;
    public List<Chunk_Script> closedPath;
    public Chunk_Script currentChunk;

    public Pathfinding_Grid_Script grid;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private List<Chunk_Script> FindPath(GameObject endChunk)
    {
        openPath = new List<Chunk_Script>();
        closedPath = new List<Chunk_Script>();

        openPath.Add(currentChunk);

        currentChunk.gCost = 0;

        return closedPath;
    }

    private void SetGrid()
    {
        foreach(Chunk_Script cs in grid.chunksL)
        {
            cs.gCost = int.MaxValue;
            cs.CalcFCost();
            cs.fromChunk = null;
        }
    }

    private int CalculateDistanceCost(Chunk_Script a, Chunk_Script b)
    {
        int cost = 0;

        int xDistance = Mathf.Abs(a.positionX - b.positionX);
        int zDistance = Mathf.Abs(a.positionZ - b.positionZ);

        return cost;
    }
}
