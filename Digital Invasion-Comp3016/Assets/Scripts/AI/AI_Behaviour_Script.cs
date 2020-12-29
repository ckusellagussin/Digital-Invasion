using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Behaviour_Script : MonoBehaviour
{
    public AI_Script aiScript;
    public Turn_Script turnScript;
    public List<Chunk_Script> path;
    public List<Chunk_Script> truePath;

    public void TakeAction(AI_Follower_Script unit)
    {
        float distance = 0;

        AI_Follower_Script closestEnemy = null;

        foreach (AI_Follower_Script fol in turnScript.goodList)
        {
            float tempDist = Vector3.Distance(unit.gameObject.transform.position, fol.gameObject.transform.position);

            if (tempDist > distance)
            {
                distance = tempDist;
                closestEnemy = fol;
            }
        }

        if(distance > 8)
        {
            aiScript.SetGrid();
            MoveUnit(unit, closestEnemy);
            unit.TakeAction(1);
        } else
        {
            Debug.Log("Unit should shoot!");
        }

        if(unit.GetActions() == 1)
        {
            TakeAction(unit);
        }
    }

    public void MoveUnit(AI_Follower_Script unit, AI_Follower_Script closestEnemy)
    {
        if (closestEnemy != null)
        {
            path = aiScript.FindPathAI(closestEnemy.GetChunkUnder().gameObject, unit.gameObject);

            //List<Chunk_Script> path2 = aiScript.FindPathAI(path[path.Count - 1].gameObject, unit.gameObject);

            //truePath = new List<Chunk_Script>();

            if (path != null)
            {
                //int currentGCost = 0;
                //int currentID = -1;

                //foreach (Chunk_Script chunk in path2)
                //{
                //    currentID += 1;
                //    int tentativeGCost = currentGCost + aiScript.CalculateDistanceCost(path[currentID], path[currentID + 1]);
                //    if (tentativeGCost > unit.GetComponent<AI_Follower_Script>().maxDistance)
                //    {
                //        break;
                //    }
                //    else
                //    {
                //        currentGCost = tentativeGCost;
                //        truePath.Add(chunk);
                //    }
                //}


                path = aiScript.CalculatePath(closestEnemy.GetChunkUnder());
                unit.GetComponent<AI_Follower_Script>().SetPath(path);
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
}
