﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Shooting_Script : MonoBehaviour
{
    public Button shootButton;
    public AI_Follower_Script damagedUnit;
    public GraphicRaycaster gRaycaster;
    public bool changeable = true;

    private PointerEventData pData;
    private EventSystem events;

    void Start()
    {
        //Fetch the Raycaster from the GameObject (the Canvas)
        gRaycaster = GetComponent<GraphicRaycaster>();
        //Fetch the Event System from the Scene
        events = GetComponent<EventSystem>();
    }

    private void Update()
    {
        if (shootButton.gameObject.activeSelf)
        {
            if (Input.GetMouseButtonDown(0))
            {
                pData = new PointerEventData(events);
                pData.position = Input.mousePosition;

                List<RaycastResult> results = new List<RaycastResult>();

                gRaycaster.Raycast(pData, results);

                foreach(RaycastResult rr in results)
                {
                    if(rr.gameObject.tag != "ShootButton")
                    {
                        if (changeable)
                        {
                            ShowButton(false);
                        }
                    } else
                    {
                        Debug.Log("Shoot Button");
                    }
                }
            }
            if (Input.GetMouseButtonDown(1))
            {
                pData = new PointerEventData(events);
                pData.position = Input.mousePosition;

                List<RaycastResult> results = new List<RaycastResult>();

                gRaycaster.Raycast(pData, results);

                foreach (RaycastResult rr in results)
                {
                    if (rr.gameObject.tag != "ShootButton")
                    {
                        if (changeable)
                        {
                            ShowButton(false);
                        }
                    }
                    else
                    {
                        Debug.Log("Shoot Button");
                    }
                }
            }
        }
    }

    public void ShowButton(bool setting)
    {
        shootButton.gameObject.SetActive(setting);
        changeable = false;
        Invoke("ChangeChangeable", 0.5f);
    }

    public void ChangeChangeable()
    {
        changeable = true;
    }

    public void Shoot(GameObject target, GameObject shooter)
    {
        AI_Follower_Script shooterUnit = shooter.GetComponent<AI_Follower_Script>();

        Vector3 up1 = new Vector3(0, 0.5f);
        RaycastHit[] hits;

        float distance = Vector3.Distance(shooter.transform.position, target.transform.position);
        bool shot = false;

        if (distance < shooterUnit.maxRange)
        {
            hits = Physics.RaycastAll(shooter.transform.position + up1, (target.transform.position - shooter.transform.position), distance);
            Vector3 lookDirection = target.transform.position;
            lookDirection.y = 0;
            shooter.transform.LookAt(lookDirection);
            foreach (RaycastHit h in hits)
            {
                if (h.collider.tag == "Tall Cover")
                {
                    if(Vector3.Distance(shooter.transform.position, h.transform.position) <= 2 && target != h.collider.gameObject)
                    {
                        RaycastHit hit;

                        if (Physics.Raycast(shooter.transform.position + up1 + new Vector3(1, 0, 0), shooter.transform.forward, out hit, 1.5f))
                        {
                            if (hit.collider.tag == ("Tall Cover"))
                            {
                                Debug.Log("Attempted and hit cover still");
                            }
                            else
                            {
                                ShootRound(target, shooter, shooter.transform.position + new Vector3(1, 0, 0));
                                Debug.Log("Attempted and didn't hit cover");
                                break;
                            }
                        }
                        else
                        {
                            ShootRound(target, shooter, shooter.transform.position + new Vector3(1, 0, 0));
                            Debug.Log("Attempted and didn't hit cover");
                            break;
                        }

                        if (Physics.Raycast(shooter.transform.position + up1 + new Vector3(-1, 0, 0), shooter.transform.forward, out hit, 1.5f))
                        {
                            if (hit.collider.tag == ("Tall Cover"))
                            {
                                Debug.Log("Attempted and hit cover still");
                                Debug.Log("Hit Tall Cover");
                                h.collider.gameObject.GetComponent<Cover_Item>().TakeDamage();
                                break;
                            }
                            else
                            {
                                ShootRound(target, shooter, shooter.transform.position + new Vector3(1, 0, 0));
                                Debug.Log("Attempted and didn't hit cover");
                                break;
                            }
                        }
                        else
                        {
                            ShootRound(target, shooter, shooter.transform.position + new Vector3(-1, 0, 0));
                            Debug.Log("Attempted and didn't hit cover");
                            break;
                        }
                    }
                    else
                    {
                        Debug.Log("Hit Tall Cover");
                        h.collider.gameObject.GetComponent<Cover_Item>().TakeDamage();
                        break;
                    }
                }
                else if (h.collider.tag == "Low Cover")
                {
                    if (distance - Vector3.Distance(shooter.transform.position, h.transform.position) <= 0.5)
                    {
                        Debug.Log("Hit Low Cover");
                        h.collider.gameObject.GetComponent<Cover_Item>().TakeDamage();
                        break;
                    }
                    else if (distance - Vector3.Distance(shooter.transform.position, h.transform.position) <= 1.5)
                    {
                        if (target.tag == "Good Guy" ||  target.tag == "Bad Guy")
                        {
                            Debug.Log("Hit Low Cover");
                            h.collider.gameObject.GetComponent<Cover_Item>().TakeDamage();
                            shot = false;
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
                    else
                    {
                        Debug.Log("Went Over Low Cover");
                    }
                }
                else if (h.collider.tag == "Good Guy")
                {
                    if (shooter.tag == "Bad Guy")
                    {
                        if (target == h.collider.gameObject)
                        {
                            shot = true;
                            damagedUnit = h.collider.gameObject.GetComponent<AI_Follower_Script>();
                        }
                    }
                }
                else if (h.collider.tag == "Bad Guy")
                {
                    if (shooter.tag == "Good Guy")
                    {
                        if (target == h.collider.gameObject)
                        {
                            shot = true;
                            damagedUnit = h.collider.gameObject.GetComponent<AI_Follower_Script>();
                        }
                    }
                }
            }
        }
        if (shot == true)
        {
            shooterUnit.DealDamage(damagedUnit);
            Debug.Log("Hit Unit");
        }
    }

    public void ShootRound(GameObject target, GameObject shooter, Vector3 newShooterTransform)
    {
        AI_Follower_Script shooterUnit = shooter.GetComponent<AI_Follower_Script>();

        Vector3 up1 = new Vector3(0, 0.5f);
        RaycastHit[] hits;
        bool shot = false;

        float distance = Vector3.Distance(newShooterTransform, target.transform.position);

        if (distance < shooterUnit.maxRange)
        {
            hits = Physics.RaycastAll(shooter.transform.position + up1, (target.transform.position - shooter.transform.position), distance);
            Vector3 lookDirection = target.transform.position;
            lookDirection.y = 0;
            shooter.transform.LookAt(lookDirection);
            foreach (RaycastHit h in hits)
            {
                if (h.collider.tag == "Tall Cover")
                {
                    if (Vector3.Distance(shooter.transform.position, h.transform.position) <= 2 && target != h.collider.gameObject)
                    {

                    }
                    else
                    {
                        Debug.Log("Hit Tall Cover");
                        h.collider.gameObject.GetComponent<Cover_Item>().TakeDamage();
                        break;
                    }
                }
                else if (h.collider.tag == "Low Cover")
                {
                    if (distance - Vector3.Distance(shooter.transform.position, h.transform.position) <= 0.5)
                    {
                        Debug.Log("Hit Low Cover");
                        h.collider.gameObject.GetComponent<Cover_Item>().TakeDamage();
                        break;
                    }
                    else if (distance - Vector3.Distance(shooter.transform.position, h.transform.position) <= 1.5)
                    {
                        if (target.tag == "Good Guy" || target.tag == "Bad Guy")
                        {
                            Debug.Log("Hit Low Cover");
                            shot = false;
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
                    else
                    {
                        Debug.Log("Went Over Low Cover");
                    }
                }
                else if (h.collider.tag == "Good Guy")
                {
                    if (shooter.tag == "Bad Guy")
                    {
                        if (target == h.collider.gameObject)
                        {
                            shot = true;
                            damagedUnit = h.collider.gameObject.GetComponent<AI_Follower_Script>();
                        }
                    }
                }
                else if (h.collider.tag == "Bad Guy")
                {
                    if (shooter.tag == "Good Guy")
                    {
                        if (target == h.collider.gameObject)
                        {
                            shot = true;
                            damagedUnit = h.collider.gameObject.GetComponent<AI_Follower_Script>();
                        }
                    }
                }
            }
        }

        if (shot == true)
        {
            shooterUnit.DealDamage(damagedUnit);
            Debug.Log("Hit Unit");
        }
    }
}