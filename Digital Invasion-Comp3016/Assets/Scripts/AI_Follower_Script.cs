using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AI_Follower_Script : MonoBehaviour
{
    public Chunk_Script currentChunk;
    public Chunk_Script targetChunk;
    public GameObject manager;
    public GameObject healthBar;
    public Turn_Script turnScript;
   
    public float speed;
    public float maxDistance;
    public float maxRange;
    public float currentHealth;
    public float maxHealth;
    public float damage;


    public Slider slider;
    

    [SerializeField]
    private List<Chunk_Script> path;
    private List<Chunk_Script> donePath;
    [SerializeField]
    private Chunk_Script endChunk;

    [SerializeField]
    private MeshRenderer[] pips;

    [SerializeField]
    private int actions;

    private Turn_Script[] turnS;

    public float calculateHealthValue()
    {

        return currentHealth / maxHealth;


    }


    private void Start()
    {
        turnScript = manager.gameObject.GetComponent<Turn_Script>();

        currentHealth = maxHealth;
        slider.value = calculateHealthValue();
       

        
    }

    // Update is called once per frame
    void Update()
    {

        slider.value = calculateHealthValue();

        if (currentHealth < maxHealth)
        {
            healthBar.SetActive(true);

        }



        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;


        }



        if (targetChunk != null)
        {
            if (currentChunk != endChunk)
            {
                if (this.transform.position.x <= targetChunk.positionX + 0.1f && this.transform.position.x > targetChunk.positionX - 0.1f && this.transform.position.z <= targetChunk.positionZ + 0.1f && this.transform.position.z > targetChunk.positionZ - 0.1f)
                {
                    // Change target chunk to next chunk in path list
                    int currentIndex = path.IndexOf(targetChunk);
                    if (currentIndex + 1 < path.Count)
                    {
                        targetChunk = path[currentIndex + 1];
                    }
                }
                else if (this.transform.position.x <= targetChunk.positionX + 0.1f && this.transform.position.x > targetChunk.positionX - 0.1f)
                {
                    // Move towards z centre of target chunk
                    transform.position = Vector3.MoveTowards(this.transform.position, targetChunk.transform.position, speed * Time.deltaTime);

                    Vector3 direction = (targetChunk.transform.position - transform.position).normalized;
                    Quaternion lookRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, speed * Time.deltaTime);
                }
                else if (this.transform.position.z <= targetChunk.positionZ + 0.1f && this.transform.position.z > targetChunk.positionZ - 0.1f)
                {
                    // Move towards x centre of target chunk
                    transform.position = Vector3.MoveTowards(this.transform.position, targetChunk.transform.position, speed * Time.deltaTime);

                    Vector3 direction = (targetChunk.transform.position - transform.position).normalized;
                    Quaternion lookRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, speed * Time.deltaTime);
                }
                else
                {
                    // Move towards centre of target chunk
                    transform.position = Vector3.MoveTowards(this.transform.position, targetChunk.transform.position, speed * Time.deltaTime);

                    Vector3 direction = (targetChunk.transform.position - transform.position).normalized;
                    Quaternion lookRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, speed * Time.deltaTime);
                }
            }
            else if (this.transform.position.x != endChunk.positionX || this.transform.position.z != endChunk.positionZ)
            {
                //Move towards endChunk centre
                transform.position = Vector3.MoveTowards(this.transform.position, targetChunk.transform.position, speed * Time.deltaTime);

                Vector3 direction = (targetChunk.transform.position - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, speed * Time.deltaTime);
            }
            //Do Nothing - turn off calculations?
            else
            {
                targetChunk = null;
                if (GetActions() == 0)
                {
                    turnScript.aiScript.aiEntity = turnScript.GetNextUnit().gameObject;
                    turnScript.aiScript.cameraTrolley.transform.position = turnScript.aiScript.aiEntity.transform.position;
                }
            }
        }
    }

    void FixedUpdate()
    {
        Ray ray = new Ray(this.transform.position, -this.transform.up);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("Chunk"))
            {
                currentChunk = hit.collider.gameObject.GetComponent<Chunk_Script>();
            }
        }

    }



    public void SetPath(List<Chunk_Script> newPath)
    {
        if (newPath != path) 
        {
            path = newPath;
            targetChunk = path[0];
            endChunk = newPath[newPath.Count - 1];
        }
    }

    public void DealDamage(AI_Follower_Script target)
    {
        
        slider.value = calculateHealthValue();

        if (currentHealth <= 0)
        {
            Destroy(target.gameObject);
            turnScript.badList.Remove(target);

        }
     
        
        if(target.gameObject.tag == "Good Guy")
       {
            currentHealth =- damage;
            slider.value = calculateHealthValue();

        }
       
       else if (target.gameObject.tag == "Bad Guy")
        {
            currentHealth =- damage;
            slider.value = calculateHealthValue();

        }

      //  turnScript.allList.Remove(target);
      //  if(target.gameObject.tag == "Good Guy")
      //  {
            

       //    turnScript.badList.Remove(target);
      //  } 
      // else if(target.gameObject.tag == "Bad Guy")
      //  {
           
       //      turnScript.goodList.Remove(target);
      //  }
      //  Destroy(target.gameObject);

    }

    

    public int GetActions()
    {
        return actions;
    }

    public void TakeAction(int actionUsed)
    {
        actions -= actionUsed;
        if (turnScript == null)
        {
            turnScript = manager.GetComponent<Turn_Script>();
        }
        if (actions == 1)
        {
            pips[0].enabled = false;
            turnScript.CheckActions();
        }
        else if (actions <= 0)
        {
            actions = 0;
            pips[0].enabled = false;
            pips[1].enabled = false;
            turnScript.CheckActions();
        }
    }

    public void NewTurn()
    {
        actions = 2;
        pips[0].enabled = true;
        pips[1].enabled = true;
    }

    


}

