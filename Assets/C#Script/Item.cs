using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int itemID;

    void Start()
    {
        itemID = Random.Range(1, 4);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Destroyer"  || collision.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);
        }
    }
}
