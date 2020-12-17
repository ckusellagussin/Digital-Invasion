using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turn_Script : MonoBehaviour
{
    public List<AI_Follower_Script> allList;
    public List<AI_Follower_Script> goodList;
    public List<AI_Follower_Script> badList;
    private bool actionsLeft = false;
    public AI_Script aiScript;

    public AI_Follower_Script newUnit;

    public int currentTeam = 0;

    void Start()
    {
        foreach (AI_Follower_Script af in allList)
        {
            af.NewTurn();
        }
    }

    public void DelayedCheckActions()
    {
        Invoke("CheckActions", 1.5f);
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

        aiScript.aiEntity.GetComponent<AI_Follower_Script>().weaponRange.SetActive(false);
        aiScript.aiEntity = GetNextUnit().gameObject;
        CheckVisibleEnemies();
        aiScript.DistanceTemplate();
        aiScript.cameraTrolley.transform.position = aiScript.aiEntity.transform.position;
        aiScript.aiEntity.GetComponent<AI_Follower_Script>().weaponRange.SetActive(true);
    }

    public AI_Follower_Script GetNextUnit()
    {
        newUnit = null;

        if (currentTeam == 0)
        {
            foreach (AI_Follower_Script fol in badList)
            {
                if (fol.GetActions() > 0)
                {
                    newUnit = fol;
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
                    newUnit = fol;
                    currentTeam = 0;
                    break;
                }
            }
        }

        if(newUnit == null)
        {
            if (goodList.Count > 0 && badList.Count > 0)
            {
                if (currentTeam == 1)
                {
                    foreach (AI_Follower_Script fol in badList)
                    {
                        if (fol.GetActions() > 0)
                        {
                            newUnit = fol;
                            currentTeam = 1;
                            break;
                        }
                    }

                }
                else if (currentTeam == 0)
                {
                    foreach (AI_Follower_Script fol in goodList)
                    {
                        if (fol.GetActions() > 0)
                        {
                            newUnit = fol;
                            currentTeam = 0;
                            break;
                        }
                    }
                }
            }
            else
            {
                Debug.Log("Somebody Won!");
            }
        }

        return newUnit;
    }

    public void CheckVisibleEnemies()
    {

        if (currentTeam == 0)
        {
            foreach (AI_Follower_Script bad in badList)
            {
                bad.visibleToEnemy = false;
                foreach (AI_Follower_Script fol in goodList)
                {
                    fol.gameObject.SetActive(true);
                    if (fol.crouching)
                    {
                        fol.animManager.Crouch(true);
                    }
                    if (Vector3.Distance(fol.transform.position, bad.transform.position) >= fol.viewDistance)
                    {
                        if (bad.visibleToEnemy == false)
                        {
                            bad.gameObject.SetActive(false);
                        }
                    }
                    else
                    {
                        bad.visibleToEnemy = true;
                        bad.gameObject.SetActive(true);
                        if (bad.crouching)
                        {
                            bad.animManager.Crouch(true);
                        }
                    }
                }
            }

        }
        else if (currentTeam == 1)
        {
            foreach (AI_Follower_Script bad in goodList)
            {
                bad.visibleToEnemy = false;
                foreach (AI_Follower_Script fol in badList)
                {
                    fol.gameObject.SetActive(true);
                    if (fol.crouching)
                    {
                        fol.animManager.Crouch(true);
                    }
                    if (Vector3.Distance(fol.transform.position, bad.transform.position) >= fol.viewDistance)
                    {
                        if (bad.visibleToEnemy == false)
                        {
                            bad.gameObject.SetActive(false);
                        }
                    }
                    else
                    {
                        bad.visibleToEnemy = true;
                        bad.gameObject.SetActive(true);
                        if (bad.crouching)
                        {
                            bad.animManager.Crouch(true);
                        }
                    }
                }
            }
        }
    }
}
