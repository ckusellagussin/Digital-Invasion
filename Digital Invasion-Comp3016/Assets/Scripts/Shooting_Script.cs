using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shooting_Script : MonoBehaviour
{
    public Button shootButton;

    public void ShowButton(bool setting)
    {
        shootButton.gameObject.SetActive(setting);
    }

    public void Shoot(GameObject target, GameObject shooter)
    {
        AI_Follower_Script shooterUnit = shooter.GetComponent<AI_Follower_Script>();

        Vector3 up1 = new Vector3(0, 0.5f);
        RaycastHit[] hits;

        float distance = Vector3.Distance(shooter.transform.position, target.transform.position);

        if (distance < shooterUnit.maxRange)
        {
            hits = Physics.RaycastAll(shooter.transform.position + up1, (target.transform.position - shooter.transform.position), distance);
            foreach (RaycastHit h in hits)
            {
                if (h.collider.tag == "Tall Cover")
                {
                    Debug.Log("Hit Tall Cover");
                    h.collider.gameObject.GetComponent<Cover_Item>().TakeDamage();
                    break;
                }
                if (h.collider.tag == "Low Cover")
                {
                    if(distance - Vector3.Distance(shooter.transform.position, h.transform.position) <= 1.5)
                    {
                        if (target.tag == "Unit")
                        {
                            Debug.Log("Hit Low Cover");
                            h.collider.gameObject.GetComponent<Cover_Item>().TakeDamage();
                            break;
                        }
                        else if (distance - Vector3.Distance(shooter.transform.position, h.transform.position) <= 0.5 && target.tag == "Low Cover")
                        {
                            Debug.Log("Hit Low Cover");
                            h.collider.gameObject.GetComponent<Cover_Item>().TakeDamage();
                            break;
                        }
                        else
                        {
                            Debug.Log("Went Over Low Cover");
                        }
                    } 
                    else if(distance - Vector3.Distance(shooter.transform.position, h.transform.position) <= 0.5)
                    {
                        Debug.Log("Hit Low Cover");
                        h.collider.gameObject.GetComponent<Cover_Item>().TakeDamage();
                        break;
                    }
                    else
                    {
                        Debug.Log("Went Over Low Cover");
                    }
                }
                if (h.collider.tag == "Unit")
                {
                    shooterUnit.DealDamage(h.collider.gameObject.GetComponent<AI_Follower_Script>());
                    Debug.Log("Hit Unit");
                }
            }
        }
    }
}
