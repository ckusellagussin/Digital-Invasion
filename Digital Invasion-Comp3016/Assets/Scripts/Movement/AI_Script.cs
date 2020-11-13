using System.Collections.Generic;
using UnityEngine;

public class AI_Script : MonoBehaviour
{
    private const int M_Straight_Cost = 10;
    private const int M_Diagonal_Cost = 14;

    public List<Chunk_Script> openPath;
    public List<Chunk_Script> closedPath;
    public Chunk_Script startChunk;
    public Chunk_Script endChunk;

    public List<Chunk_Script> path;
    public GameObject aiEntity;

    public Pathfinding_Grid_Script grid;

    public GameObject cameraTrolley;
    public Camera mainCamera;

    private GameObject selectedObject;
    [SerializeField]
    private Material_Container_Script materialContainer;
    [SerializeField]
    private Shooting_Script shooter;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (selectedObject != null)
            {
                selectedObject.GetComponent<MeshRenderer>().material = materialContainer.FindMaterial(selectedObject.tag);
                selectedObject = null;
            }

            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Chunk"))
                {
                    SetGrid();
                    endChunk = hit.collider.gameObject.GetComponent<Chunk_Script>();

                    if (!endChunk.impassable && !endChunk.lowCover)
                    {
                        path = FindPath(endChunk.gameObject);
                        if (path != null)
                        {
                            path = CalculatePath(endChunk);
                            MoveUnit(path);
                        }
                    } 

                }
                if (hit.collider.CompareTag("Tall Cover") || hit.collider.CompareTag("Low Cover"))
                {
                    cameraTrolley.transform.position = hit.collider.gameObject.transform.position;
                    if (selectedObject == null)
                    {
                        selectedObject = hit.collider.gameObject;
                        selectedObject.GetComponent<MeshRenderer>().material = materialContainer.FindMaterial("Selected");
                    }
                    else
                    {
                        selectedObject.GetComponent<MeshRenderer>().material = materialContainer.FindMaterial(selectedObject.tag);
                        selectedObject = hit.collider.gameObject;
                        selectedObject.GetComponent<MeshRenderer>().material = materialContainer.FindMaterial("Selected");
                    }
                    shooter.ShowButton(true);
                }
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Unit"))
                {
                    aiEntity = hit.collider.gameObject;
                }
            }
        }
    }

    public void DistanceHighlight(GameObject newEndChunk)
    {
        SetGrid();
        path = FindPath(newEndChunk);
        if (path != null)
        {
            newEndChunk.GetComponentInChildren<MeshRenderer>().enabled = true;
        }
    }

    private List<Chunk_Script> FindPath(GameObject endChunk)
    {
        Vector3 raycastPoint = new Vector3(0, 0.2f, 0);
        Ray ray = new Ray(aiEntity.transform.position + raycastPoint, -aiEntity.transform.up);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("Chunk"))
            {
                startChunk = hit.collider.gameObject.GetComponent<Chunk_Script>();
            }
        }

        openPath = new List<Chunk_Script>();
        closedPath = new List<Chunk_Script>();

        openPath.Add(startChunk);

        startChunk.gCost = 0;
        startChunk.hCost = CalculateDistanceCost(startChunk, endChunk.GetComponent<Chunk_Script>());
        startChunk.CalcFCost();

        while(openPath.Count > 0)
        {
            Chunk_Script currentChunk = GetLowestFCost(openPath);
            if (currentChunk == endChunk.GetComponent<Chunk_Script>())
            {
                return closedPath;
            }

            openPath.Remove(currentChunk);
            closedPath.Add(currentChunk);

            foreach(Chunk_Script neighbour in GetNeighbourList(currentChunk))
            {
                if (closedPath.Contains(neighbour)) continue;

                if (neighbour != null)
                {
                    int tentativeGCost = currentChunk.gCost + CalculateDistanceCost(currentChunk, neighbour);
                    if (tentativeGCost < neighbour.gCost && tentativeGCost <= aiEntity.GetComponent<AI_Follower_Script>().maxDistance)
                    {
                        if (!neighbour.impassable)
                        {
                            neighbour.fromChunk = currentChunk;
                            neighbour.gCost = tentativeGCost;
                            neighbour.hCost = CalculateDistanceCost(neighbour, endChunk.GetComponent<Chunk_Script>());
                            neighbour.CalcFCost();

                            if (!openPath.Contains(neighbour))
                            {
                                openPath.Add(neighbour);
                            }
                        }
                    }
                }
            }
        }

        return null;
    }

    private List<Chunk_Script> GetNeighbourList(Chunk_Script currentChunk)
    {
        List<Chunk_Script> neightbourList = new List<Chunk_Script>();

        if(currentChunk.positionX - 1 >= -grid.width / 2)
        {
            neightbourList.Add(GetChunk(currentChunk.positionX - 1, currentChunk.positionZ));
            if (currentChunk.positionZ - 1 >= -grid.height / 2)
            {
                neightbourList.Add(GetChunk(currentChunk.positionX - 1, currentChunk.positionZ - 1));
            }
            if (currentChunk.positionZ + 1 < grid.height / 2)
            {
                neightbourList.Add(GetChunk(currentChunk.positionX - 1, currentChunk.positionZ + 1));
            }
        }

        if (currentChunk.positionX + 1 >= -grid.width / 2)
        {
            neightbourList.Add(GetChunk(currentChunk.positionX + 1, currentChunk.positionZ));
            if (currentChunk.positionZ - 1 >= -grid.height / 2)
            {
                neightbourList.Add(GetChunk(currentChunk.positionX + 1, currentChunk.positionZ - 1));
            }
            if (currentChunk.positionZ + 1 < grid.height / 2)
            {
                neightbourList.Add(GetChunk(currentChunk.positionX + 1, currentChunk.positionZ + 1));
            }
        }
        
        if (currentChunk.positionZ - 1 >= -grid.height / 2)
        {
            neightbourList.Add(GetChunk(currentChunk.positionX, currentChunk.positionZ - 1));
        }
        if (currentChunk.positionZ + 1 < grid.height / 2)
        {
            neightbourList.Add(GetChunk(currentChunk.positionX, currentChunk.positionZ + 1));
        }

        if (neightbourList.Count > 0)
        {
            return neightbourList;
        }
        else
        {
            return null;
        }
    }

    private List<Chunk_Script> CalculatePath(Chunk_Script endChunk)
    {
        List<Chunk_Script> path = new List<Chunk_Script>();
        path.Add(endChunk);
        Chunk_Script currentChunk = endChunk;
        while(currentChunk.fromChunk != null)
        {
            path.Add(currentChunk.fromChunk);
            currentChunk = currentChunk.fromChunk;
        }

        path.Reverse();
        return path;
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

    private Chunk_Script GetChunk(int x, int z)
    {
        return grid.GetChunk(x, z);
    }

    private int CalculateDistanceCost(Chunk_Script a, Chunk_Script b)
    {
        int cost = 0;

        if (a == null || b == null)
        {
            Debug.Log("Null a or b");
        } 

        int xDistance = Mathf.Abs(a.positionX - b.positionX);
        int zDistance = Mathf.Abs(a.positionZ - b.positionZ);
        int remaining = Mathf.Abs(xDistance - zDistance);

        cost = M_Diagonal_Cost * Mathf.Min(xDistance, zDistance) + M_Straight_Cost * remaining;

        return cost;
    }

    private Chunk_Script GetLowestFCost(List<Chunk_Script> chunkList)
    {
        Chunk_Script lowestFCostChunk = chunkList[0];

        foreach(Chunk_Script cs in chunkList)
        {
            if (cs.fCost < lowestFCostChunk.fCost)
            {
                lowestFCostChunk = cs;
            }
        }

        return lowestFCostChunk;
    }

    private void MoveUnit(List<Chunk_Script> path)
    {
        aiEntity.GetComponent<AI_Follower_Script>().SetPath(path);
    }

    public void ConfirmShot()
    {
        shooter.Shoot(selectedObject, aiEntity);
    }
}
