using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Behaviour_Script : MonoBehaviour
{
    public AI_Script aiScript;
    public Turn_Script turnScript;
    public Shooting_Script shootingScript;
    public List<Chunk_Script> path;
    public List<Chunk_Script> truePath;

    public void TakeAction(AI_Follower_Script unit)
    {
        float distance = 0;

        AI_Follower_Script closestEnemy = null;

        foreach (AI_Follower_Script fol in turnScript.goodList)
        {
            float tempDist = Vector3.Distance(unit.gameObject.transform.position, fol.gameObject.transform.position);

            if (distance == 0)
            {
                distance = tempDist;
                closestEnemy = fol;
            }
            else if(tempDist < distance)
            {
                distance = tempDist;
                closestEnemy = fol;
            }
        }

        if(distance > unit.maxRange)
        {
            aiScript.SetGrid();
            MoveUnit(unit, closestEnemy);
            unit.TakeAction(1);
        } 
        else
        {
            aiScript.ConfirmShot(unit.gameObject, closestEnemy.gameObject);
        }

    }

    public void MoveUnit(AI_Follower_Script unit, AI_Follower_Script closestEnemy)
    {
        if (closestEnemy != null)
        {
            path = aiScript.FindPathAI(closestEnemy.GetChunkUnder().gameObject, unit.gameObject);
            
            truePath = new List<Chunk_Script>();

            if (path != null)
            {
                int currentGCost = 0;
                int currentID = -1;



                path = aiScript.CalculatePath(closestEnemy.GetChunkUnder());
                foreach (Chunk_Script chunk in path)
                {
                    currentID += 1;
                    int tentativeGCost = currentGCost + aiScript.CalculateDistanceCost(path[currentID], path[currentID + 1]);
                    if (tentativeGCost > unit.GetComponent<AI_Follower_Script>().maxDistance)
                    {
                        break;
                    }
                    else
                    {
                        currentGCost = tentativeGCost;
                        truePath.Add(chunk);
                    }
                }
                unit.GetComponent<AI_Follower_Script>().SetPath(truePath);
            }
            else
            {
                Debug.Log("Couldn't Find Path");
            }
        }
        else
        {
            Debug.Log("Something went wrong with finding the closest unit");
        }
    }

    IEnumerator DelayedForSeconds(float delay, AI_Follower_Script unit)
    {
        yield return new WaitForSeconds(delay);
        TakeAction(unit);
        Debug.Log("Delay Complete");
    }
}
