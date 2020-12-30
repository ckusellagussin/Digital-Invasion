using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Script : MonoBehaviour
{
    private const int M_Straight_Cost = 10;
    private const int M_Diagonal_Cost = 14;

    public List<Chunk_Script> openPath;
    public List<Chunk_Script> closedPath;
    public List<Chunk_Script> visibleTemplate;
    public Chunk_Script startChunk;
    public Chunk_Script endChunk;
    public LineRenderer renderer;

    public List<Chunk_Script> path;
    public GameObject aiEntity;

    public Pathfinding_Grid_Script grid;

    public GameObject cameraTrolley;
    public Camera mainCamera;
    public Camera unitCamera;

    private GameObject selectedObject;
    [SerializeField]
    private Material_Container_Script materialContainer;
    [SerializeField]
    private Shooting_Script shooter;

    public Turn_Script turnScript;
    public AI_Behaviour_Script behaviourScript;

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
                if (selectedObject.GetComponent<MeshRenderer>() != null)
                {
                    selectedObject.GetComponent<MeshRenderer>().material = materialContainer.FindMaterial(selectedObject.tag);
                }
                selectedObject = null;
            }

            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (aiEntity.GetComponent<AI_Follower_Script>().GetActions() > 0)
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
                                cameraTrolley.transform.position = endChunk.transform.position;
                                aiEntity.GetComponent<AI_Follower_Script>().TakeAction(1);
                                if (aiEntity.GetComponent<AI_Follower_Script>().animManager.anim.GetBool("Running") == false)
                                {
                                    aiEntity.GetComponent<AI_Follower_Script>().animManager.Run(true);
                                }
                                if (selectedObject != null)
                                {
                                    selectedObject.GetComponent<MeshRenderer>().material = materialContainer.FindMaterial(selectedObject.tag);
                                }
                                foreach(Chunk_Script cs in GetNeighbourList(endChunk))
                                {
                                    if(cs.lowCover)
                                    {
                                        aiEntity.GetComponent<AI_Follower_Script>().animManager.Crouch(true);
                                        aiEntity.GetComponent<AI_Follower_Script>().crouching = true;
                                        break;
                                    }
                                    else
                                    {
                                        aiEntity.GetComponent<AI_Follower_Script>().crouching = false;
                                        aiEntity.GetComponent<AI_Follower_Script>().animManager.Crouch(false);
                                    }
                                }
                                renderer.enabled = false;
                            }
                        }
                    }
                    else if (hit.collider.CompareTag("Tall Cover") || hit.collider.CompareTag("Low Cover"))
                    {
                        float distance = Vector3.Distance(hit.collider.gameObject.transform.position, aiEntity.transform.position);
                        if (distance < aiEntity.GetComponent<AI_Follower_Script>().maxRange)
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
                            Vector3[] positions = new Vector3[] {
                                    aiEntity.transform.position,
                                    selectedObject.transform.position
                                };
                            renderer.SetPositions(positions);
                            renderer.enabled = true;
                            shooter.ShowButton(true);
                        }
                    }
                    else if(hit.collider.CompareTag("Good Guy"))
                    {
                        if (aiEntity.GetComponent<AI_Follower_Script>().turnScript.currentTeam == 1)
                        {
                            float distance = Vector3.Distance(hit.collider.gameObject.transform.position, aiEntity.transform.position);
                            if (distance < aiEntity.GetComponent<AI_Follower_Script>().maxRange)
                            {
                                cameraTrolley.transform.position = hit.collider.gameObject.transform.position;
                                if (selectedObject == null)
                                {
                                    selectedObject = hit.collider.gameObject;
                                }
                                else
                                {
                                    selectedObject = hit.collider.gameObject;
                                }
                                Vector3[] positions = new Vector3[] {
                                    aiEntity.transform.position,
                                    selectedObject.transform.position
                                };
                                renderer.SetPositions(positions);
                                renderer.enabled = true;
                                shooter.ShowButton(true);
                            }
                        }
                    }
                    else if (hit.collider.CompareTag("Bad Guy"))
                    {
                        if (aiEntity.GetComponent<AI_Follower_Script>().turnScript.currentTeam == 0)
                        {
                            float distance = Vector3.Distance(hit.collider.gameObject.transform.position, aiEntity.transform.position);
                            if (distance < aiEntity.GetComponent<AI_Follower_Script>().maxRange)
                            {
                                cameraTrolley.transform.position = hit.collider.gameObject.transform.position;
                                if (selectedObject == null)
                                {
                                    selectedObject = hit.collider.gameObject;
                                }
                                else
                                {
                                    selectedObject = hit.collider.gameObject;
                                }
                                Vector3[] positions = new Vector3[] {
                                    aiEntity.transform.position,
                                    selectedObject.transform.position
                                };
                                renderer.SetPositions(positions);
                                renderer.enabled = true;
                                shooter.ShowButton(true);
                            }
                        }
                    }
                    else
                    {
                        selectedObject.GetComponent<MeshRenderer>().material = materialContainer.FindMaterial(selectedObject.tag);
                        renderer.enabled = false;
                    }
                }
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Good Guy"))
                {
                    if (turnScript.currentTeam == 0 && aiEntity.GetComponent<AI_Follower_Script>().GetActions() != 1)
                    {
                        aiEntity.GetComponent<AI_Follower_Script>().weaponRange.SetActive(false);
                        aiEntity = hit.collider.gameObject;
                        turnScript.CheckVisibleEnemies();
                        cameraTrolley.transform.position = aiEntity.transform.position;
                        DistanceTemplate();
                        aiEntity.GetComponent<AI_Follower_Script>().weaponRange.SetActive(true);
                    } 
                    else
                    {
                        cameraTrolley.transform.position = aiEntity.transform.position;
                        aiEntity.GetComponent<AI_Follower_Script>().weaponRange.SetActive(true);
                    }
                }
                else if (hit.collider.CompareTag("Bad Guy"))
                {
                    if (turnScript.currentTeam == 1 && aiEntity.GetComponent<AI_Follower_Script>().GetActions() != 1)
                    {
                        aiEntity.GetComponent<AI_Follower_Script>().weaponRange.SetActive(false);
                        aiEntity = hit.collider.gameObject;
                        turnScript.CheckVisibleEnemies();
                        cameraTrolley.transform.position = aiEntity.transform.position;
                        DistanceTemplate();
                        aiEntity.GetComponent<AI_Follower_Script>().weaponRange.SetActive(true);
                    }
                    else
                    {
                        cameraTrolley.transform.position = aiEntity.transform.position;
                        aiEntity.GetComponent<AI_Follower_Script>().weaponRange.SetActive(true);
                    }
                }
                else
                {
                    if (selectedObject != null && (selectedObject.tag != "Bad Guy" || selectedObject.tag != "Good Guy"))
                    {
                        selectedObject.GetComponent<MeshRenderer>().material = materialContainer.FindMaterial(selectedObject.tag);
                    }
                    renderer.enabled = false;
                }
            }
        }
    }

    //public void DistanceHighlight(GameObject newEndChunk)
    //{
    //    SetGrid();
    //    path = FindPath(newEndChunk);
    //    if (path != null)
    //    {
    //        newEndChunk.GetComponentInChildren<MeshRenderer>().enabled = true;
    //    }
    //}

    public void DistanceTemplate()
    {
        SetGrid();
        FindTemplate();
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

    public List<Chunk_Script> FindPathAI(GameObject endChunk, GameObject unit)
    {
        Vector3 raycastPoint = new Vector3(0, 0.2f, 0);
        Ray ray = new Ray(unit.transform.position + raycastPoint, -unit.transform.up);
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

        while (openPath.Count > 0)
        {
            Chunk_Script currentChunk = GetLowestFCost(openPath);
            if (currentChunk == endChunk.GetComponent<Chunk_Script>())
            {
                return closedPath;
            }
            //else if(currentChunk.gCost >= unit.GetComponent<AI_Follower_Script>().maxDistance)
            //{
            //    closedPath.RemoveAt(closedPath.Count - 1);
            //    return closedPath;
            //}

            openPath.Remove(currentChunk);
            closedPath.Add(currentChunk);

            foreach (Chunk_Script neighbour in GetNeighbourList(currentChunk))
            {
                if (closedPath.Contains(neighbour)) continue;

                if (neighbour != null)
                {
                    int tentativeGCost = currentChunk.gCost + CalculateDistanceCost(currentChunk, neighbour);
                    if (tentativeGCost <= neighbour.gCost)
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

    private List<Chunk_Script> FindTemplate()
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

        foreach (Chunk_Script cs in visibleTemplate)
        {
            cs.gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;
        }

        openPath = new List<Chunk_Script>();
        closedPath = new List<Chunk_Script>();
        visibleTemplate = new List<Chunk_Script>();

        openPath.Add(startChunk);

        startChunk.gCost = 0;
        startChunk.CalcFCost();

        while (openPath.Count > 0)
        {
            Chunk_Script currentChunk = GetLowestFCost(openPath);

            openPath.Remove(currentChunk);
            closedPath.Add(currentChunk);

            foreach (Chunk_Script neighbour in GetNeighbourList(currentChunk))
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
                            neighbour.CalcFCost();

                            if (!openPath.Contains(neighbour))
                            {
                                openPath.Add(neighbour);
                                visibleTemplate.Add(neighbour);
                                neighbour.GetComponentInChildren<MeshRenderer>().enabled = true;
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

    public List<Chunk_Script> CalculatePath(Chunk_Script endChunk)
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

    public void SetGrid()
    {
        foreach(Chunk_Script cs in grid.chunksL)
        { 
            cs.gCost = int.MaxValue;
            cs.CalcFCost();
            cs.fromChunk = null;
        }
    }

    public Chunk_Script GetChunk(int x, int z)
    {
        return grid.GetChunk(x, z);
    }

    public int CalculateDistanceCost(Chunk_Script a, Chunk_Script b)
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
        aiEntity.GetComponent<AI_Follower_Script>().TakeAction(2);
        mainCamera.enabled = false;
        unitCamera.enabled = true;
        renderer.enabled = false;
        aiEntity.GetComponent<AI_Follower_Script>().animManager.Shoot(true);
        unitCamera.transform.rotation = aiEntity.transform.rotation;
        unitCamera.transform.position = aiEntity.transform.position + (aiEntity.transform.right / 2) + (aiEntity.transform.up * 1.7f) + (-aiEntity.transform.forward);
        StartCoroutine(DelayedUnitSwitch(2.5f));
    }
    public void ConfirmShot(GameObject unit, GameObject target)
    {
        shooter.Shoot(target, unit);
        aiEntity.GetComponent<AI_Follower_Script>().TakeAction(2);
        mainCamera.enabled = false;
        unitCamera.enabled = true;
        renderer.enabled = false;
        aiEntity.GetComponent<AI_Follower_Script>().animManager.Shoot(true);
        unitCamera.transform.rotation = aiEntity.transform.rotation;
        unitCamera.transform.position = aiEntity.transform.position + (aiEntity.transform.right / 2) + (aiEntity.transform.up * 1.7f) + (-aiEntity.transform.forward);
        StartCoroutine(DelayedUnitSwitch(2.5f, "AI"));
    }

    IEnumerator DelayedUnitSwitch(float delay)
    {
        yield return new WaitForSeconds(delay);
        aiEntity.GetComponent<AI_Follower_Script>().animManager.Shoot(false);

        if (aiEntity.GetComponent<AI_Follower_Script>().GetActions() == 0)
        {
            aiEntity.GetComponent<AI_Follower_Script>().weaponRange.SetActive(false);
            aiEntity = turnScript.GetNextUnit().gameObject;
        }
        unitCamera.enabled = false;
        mainCamera.enabled = true;
        if (turnScript.currentTeam == 0)
        {
            cameraTrolley.transform.position = aiEntity.transform.position;
            DistanceTemplate();
            aiEntity.GetComponent<AI_Follower_Script>().weaponRange.SetActive(true);
            turnScript.CheckVisibleEnemies();
        }
        else
        {
            behaviourScript.TakeAction(aiEntity.GetComponent<AI_Follower_Script>());
        }
    }

    IEnumerator DelayedUnitSwitch(float delay, string forAI)
    {
        yield return new WaitForSeconds(delay);
        aiEntity.GetComponent<AI_Follower_Script>().animManager.Shoot(false);

        if (aiEntity.GetComponent<AI_Follower_Script>().GetActions() == 0)
        {
            aiEntity.GetComponent<AI_Follower_Script>().weaponRange.SetActive(false);
            aiEntity = turnScript.GetNextUnit().gameObject;
        }
        unitCamera.enabled = false;
        mainCamera.enabled = true;
        if (turnScript.currentTeam == 0)
        {
            cameraTrolley.transform.position = aiEntity.transform.position;
            DistanceTemplate();
            aiEntity.GetComponent<AI_Follower_Script>().weaponRange.SetActive(true);
            turnScript.CheckVisibleEnemies();
        }
        else
        {
            behaviourScript.TakeAction(aiEntity.GetComponent<AI_Follower_Script>());
        }
    }
}
