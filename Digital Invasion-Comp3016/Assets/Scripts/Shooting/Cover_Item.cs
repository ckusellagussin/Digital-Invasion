using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cover_Item : MonoBehaviour
{
    public Chunk_Script chunkUnder;
    public GameObject lowCoverPrefab;
    public float yoffset;

    public void SetChunkUnder(Chunk_Script chunk)
    {
        chunkUnder = chunk;
    }


    public void TakeDamage()
    {
        if(gameObject.CompareTag("Tall Cover"))
        {
            GameObject replacement = Instantiate(lowCoverPrefab, transform.position, transform.rotation);
            gameObject.GetComponent<Collider>().enabled = false;
            replacement.transform.position = this.transform.position;
            replacement.transform.parent = transform.parent;
            replacement.GetComponentInChildren<Cover_Item>().SetChunkUnder(chunkUnder);
            Destroy(gameObject);
            replacement.GetComponentInChildren<Cover_Item>().chunkUnder.CheckOnTop();
        }
        if (gameObject.CompareTag("Low Cover"))
        {
            gameObject.GetComponent<Collider>().enabled = false;

            Destroy(gameObject);
        }
    }
}
