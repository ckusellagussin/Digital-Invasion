using System.Collections;
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

    public GameObject bulletPrefab;

    void Start()
    {
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
                if(results.Count == 0)
                {
                    if (changeable)
                    {
                        ShowButton(false);
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
                if (results.Count == 0)
                {
                    if (changeable)
                    {
                        ShowButton(false);
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

    public Vector3 Shoot(GameObject target, GameObject shooter)
    {
        AI_Follower_Script shooterUnit = shooter.GetComponent<AI_Follower_Script>();

        Vector3 newPos = new Vector3(0,0,0);

        for (float i = 0; i < 2; i += 0.3f)
        {
            StartCoroutine(DelayedSpawn(i, shooterUnit));
        }

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
                    if(Vector3.Distance(shooter.transform.position, h.transform.position) <= 1.5 && target != h.collider.gameObject)
                    {
                        RaycastHit hit;
                        shooter.transform.position += shooter.transform.right;
                        Chunk_Script avoidChunk = shooter.GetComponent<AI_Follower_Script>().GetChunkUnder();
                        if (avoidChunk.impassable == false)
                        {
                            if (Physics.Raycast(shooter.transform.position + up1, shooter.transform.forward, out hit, 1.5f))
                            {
                                if (hit.collider.tag == ("Tall Cover"))
                                {
                                    Debug.Log("Attempted and hit cover still");
                                }
                                else
                                {
                                    ShootRound(target, shooter, shooter.transform.position);
                                    newPos = shooter.transform.position;
                                    Debug.Log("Attempted and didn't hit cover");
                                    shooter.transform.position += -shooter.transform.right;
                                    break;
                                }
                            }
                            else
                            {
                                ShootRound(target, shooter, shooter.transform.position);
                                newPos = shooter.transform.position;
                                Debug.Log("Attempted and didn't hit cover");
                                shooter.transform.position += -shooter.transform.right;
                                break;
                            }
                        }

                        shooter.transform.position += -shooter.transform.right;
                        shooter.transform.position += -shooter.transform.right;
                        avoidChunk = shooter.GetComponent<AI_Follower_Script>().GetChunkUnder();
                        if (avoidChunk.impassable == false)
                        {
                            if (Physics.Raycast(shooter.transform.position + up1, shooter.transform.forward, out hit, 1.5f))
                            {
                                if (hit.collider.tag == ("Tall Cover"))
                                {
                                    Debug.Log("Attempted and hit cover still");
                                    Debug.Log("Hit Tall Cover");
                                    shot = false;
                                    StartCoroutine(DelayedDamage(1.0f, h.collider.gameObject));
                                    break;
                                }
                                else
                                {
                                    ShootRound(target, shooter, shooter.transform.position);
                                    newPos = shooter.transform.position;
                                    Debug.Log("Attempted and didn't hit cover");
                                    shooter.transform.position += shooter.transform.right;
                                    break;
                                }
                            }
                            else
                            {
                                ShootRound(target, shooter, shooter.transform.position);
                                newPos = shooter.transform.position;
                                Debug.Log("Attempted and didn't hit cover");
                                shooter.transform.position += shooter.transform.right;
                                break;
                            }
                        }
                    }
                    else
                    {
                        Debug.Log("Hit Tall Cover");
                        StartCoroutine(DelayedDamage(1.0f, h.collider.gameObject));
                        shot = false;
                        newPos = shooterUnit.transform.position;
                        break;
                    }
                }
                else if (h.collider.tag == "Low Cover")
                {
                    if (Vector3.Distance(target.transform.position, h.transform.position) <= 0.5)
                    {
                        Debug.Log("Hit Low Cover");
                        StartCoroutine(DelayedDamage(1.0f, h.collider.gameObject));
                        shot = false;
                        newPos = shooterUnit.transform.position;
                        break;
                    }
                    else if (Vector3.Distance(target.transform.position, h.transform.position) <= 1.5)
                    {
                        if (target.tag == "Good Guy" ||  target.tag == "Bad Guy")
                        {
                            Debug.Log("Hit Low Cover");
                            StartCoroutine(DelayedDamage(1.0f, h.collider.gameObject));
                            shot = false;
                            newPos = shooterUnit.transform.position;
                            break;
                        }
                        else if (distance - Vector3.Distance(shooter.transform.position, h.transform.position) <= 0.5 && target.tag == "Low Cover")
                        {
                            Debug.Log("Hit Low Cover");
                            StartCoroutine(DelayedDamage(1.0f, h.collider.gameObject));
                            shot = false;
                            newPos = shooterUnit.transform.position;
                            break;
                        }
                        else
                        {
                            Debug.Log("Went Over Low Cover");
                        }
                    }
                    else if(target.transform == h.collider.gameObject.transform)
                    {
                        Debug.Log("Hit Low Cover");
                        StartCoroutine(DelayedDamage(1.0f, h.collider.gameObject));
                        shot = false;
                        newPos = shooterUnit.transform.position;
                        break;
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
            newPos = shooterUnit.transform.position;
        }

        return newPos;
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
            hits = Physics.RaycastAll(newShooterTransform + up1, (target.transform.position - newShooterTransform), distance);
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
                        shot = false;
                        StartCoroutine(DelayedDamage(1.0f, h.collider.gameObject));
                        break;
                    }
                }
                else if (h.collider.tag == "Low Cover")
                {
                    if (distance - Vector3.Distance(shooter.transform.position, h.transform.position) <= 0.5)
                    {
                        Debug.Log("Hit Low Cover");
                        shot = false;
                        StartCoroutine(DelayedDamage(1.0f, h.collider.gameObject));
                        break;
                    }
                    else if (distance - Vector3.Distance(shooter.transform.position, h.transform.position) <= 1.5)
                    {
                        if (target.tag == "Good Guy" || target.tag == "Bad Guy")
                        {
                            Debug.Log("Hit Low Cover");
                            shot = false;
                            StartCoroutine(DelayedDamage(1.0f, h.collider.gameObject));
                            break;
                        }
                        else if (distance - Vector3.Distance(shooter.transform.position, h.transform.position) <= 0.5 && target.tag == "Low Cover")
                        {
                            Debug.Log("Hit Low Cover");
                            shot = false;
                            StartCoroutine(DelayedDamage(1.0f, h.collider.gameObject));
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


    IEnumerator DelayedSpawn(float delay, AI_Follower_Script shooterUnit)
    {
        yield return new WaitForSeconds(delay);
        GameObject bulletTemp = Instantiate(bulletPrefab, shooterUnit.transform);
        Quaternion random = new Quaternion(shooterUnit.transform.rotation.x + Random.Range(-0.05f,0.05f), shooterUnit.transform.rotation.y + Random.Range(-0.05f, 0.05f), shooterUnit.transform.rotation.z, shooterUnit.transform.rotation.w);
        bulletTemp.transform.position = shooterUnit.transform.position + (shooterUnit.transform.up);
        bulletTemp.transform.rotation = random;
        bulletTemp.GetComponent<Bullet_Mover_Script>().DestroyIn30();
    }


    IEnumerator DelayedDamage(float delay, GameObject damageReciever)
    {
        yield return new WaitForSeconds(delay);
        damageReciever.GetComponent<Cover_Item>().TakeDamage();
    }

}
