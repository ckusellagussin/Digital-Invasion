﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turn_Script : MonoBehaviour
{
    public List<AI_Follower_Script> allList;
    public List<AI_Follower_Script> goodList;
    public List<AI_Follower_Script> badList;
    private bool actionsLeft = false;
    public AI_Script aiScript;

    public int currentTeam = 0;

    void Start()
    {
        foreach (AI_Follower_Script af in allList)
        {
            af.NewTurn();
        }
    }

    public void CheckActions()
    {
        foreach(AI_Follower_Script af in allList)
        {
            if (af.GetActions() > 0)
            {
                actionsLeft = true;
                break;
            }
            else
            {
                actionsLeft = false;
            }
        }
        if (!actionsLeft)
        {
            NewTurn();
            Debug.Log("New Turn");
        }
    }

    public void NewTurn()
    {
        foreach (AI_Follower_Script af in allList)
        {
            af.NewTurn();
        }

        aiScript.aiEntity = GetNextUnit().gameObject;
        aiScript.DistanceTemplate();
        aiScript.cameraTrolley.transform.position = aiScript.aiEntity.transform.position;
    }

    public AI_Follower_Script GetNextUnit()
    {
        AI_Follower_Script newAI = null;

        if (currentTeam == 0)
        {
            foreach (AI_Follower_Script fol in badList)
            {
                if (fol.GetActions() > 0)
                {
                    newAI = fol;
                    currentTeam = 1;
                    break;
                }
            }

        }
        else if (currentTeam == 1)
        {
            foreach (AI_Follower_Script fol in goodList)
            {
                if (fol.GetActions() > 0)
                {
                    newAI = fol;
                    currentTeam = 0;
                    break;
                }
            }
        }

        if(newAI == null)
        {
            if (goodList.Count > 0 && badList.Count > 0)
            {
                NewTurn();
            }
            else
            {
                Debug.Log("Somebody Won!");
            }
        }

        return newAI;
    }
}
