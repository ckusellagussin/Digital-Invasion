using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Mover_Script : MonoBehaviour
{
    public float speed;
    float time;

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * speed;
        Physics.IgnoreLayerCollision(9,10);
    }

    public void DestroyIn30()
    {
        if (time == 0)
        {
            float time = Time.time;
        }

        if(Time.time <= time + 3)
        {
            Destroy(gameObject);
        }
        else
        {
            Invoke("DestroyIn30", 1.0f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
