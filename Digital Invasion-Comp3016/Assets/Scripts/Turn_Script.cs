using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turn_Script : MonoBehaviour
{
    public List<AI_Follower_Script> unitList;
    private bool actionsLeft = false;

    void Start()
    {
        foreach (AI_Follower_Script af in unitList)
        {
            af.NewTurn();
        }
    }

    public void CheckActions()
    {
        foreach(AI_Follower_Script af in unitList)
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
            foreach (AI_Follower_Script af in unitList)
            {
                af.NewTurn();
            }
            Debug.Log("New Turn");
        }
    }

    public void NewTurn()
    {
        foreach (AI_Follower_Script af in unitList)
        {
            af.NewTurn();
        }
    }
}
