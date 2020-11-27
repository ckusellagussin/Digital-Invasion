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
    public bool lowCover = false;
    public bool mouseOver = false;

    public Chunk_Script fromChunk;
    public AI_Script aiPathfinder;

    // Start is called before the first frame update
    void Start()
    {
        Ray ray = new Ray(this.transform.position, this.transform.up);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 5))
        {
            if (hit.collider.CompareTag("Tall Cover"))
            {
                impassable = true;
                hit.collider.gameObject.GetComponent<Cover_Item>().SetChunkUnder(this);
            }
            if (hit.collider.CompareTag("Low Cover"))
            {
                lowCover = true;
                hit.collider.gameObject.GetComponent<Cover_Item>().SetChunkUnder(this);
            }
        }
    }

    // Check On Top of the chunk to see if cover has changed
    public void CheckOnTop()
    {
        Ray ray = new Ray(this.transform.position, this.transform.up);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("Tall Cover"))
            {
                impassable = true;
            } 
            else
            {
                impassable = false;
            }
            if (hit.collider.CompareTag("Low Cover"))
            {
                lowCover = true;
            }
            else
            {
                lowCover = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //private void OnMouseOver()
    //{
    //    if (mouseOver == false)
    //    {
    //        mouseOver = true;
    //        Invoke("WaitTimer", 0.1f);
    //    }
    //}

    //private void OnMouseExit()
    //{
    //    mouseOver = false;
    //    CancelInvoke("WaitTimer");
    //    gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;
    //}

    //private void WaitTimer()
    //{
    //    aiPathfinder.DistanceHighlight(gameObject);
    //}

    public void CalcFCost()
    {
        fCost = gCost + hCost;
        if (impassable)
        {
            fCost = int.MaxValue;
        }
        if (lowCover)
        {
            fCost += 15;
        }
    }

    public void SetPositions(int x, int z, AI_Script pathfinder)
    {
        positionX = x;
        positionZ = z;
        aiPathfinder = pathfinder;
    }
}
