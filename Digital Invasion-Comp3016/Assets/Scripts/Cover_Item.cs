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
            replacement.GetComponent<Cover_Item>().SetChunkUnder(chunkUnder);
            replacement.GetComponent<Cover_Item>().chunkUnder.CheckOnTop();
            replacement.transform.position += new Vector3(0, replacement.GetComponent<Cover_Item>().yoffset * 2, 0);
            replacement.transform.parent = transform.parent;
            Destroy(gameObject);
        }
        if (gameObject.CompareTag("Low Cover"))
        {
            gameObject.GetComponent<Collider>().enabled = false;
            chunkUnder.CheckOnTop();
            Destroy(gameObject);
        }
    }
}
