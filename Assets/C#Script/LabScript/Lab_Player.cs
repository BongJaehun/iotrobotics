using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lab_Player : MonoBehaviour
{
    Rigidbody2D rigid;
    public float speed;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            rigid.velocity = Vector2.zero;
        }


        if (Input.GetMouseButton(0))
        {
            rigid.gravityScale = 0;
            rigid.AddForce(Vector2.up * speed);
        }
        else
        {
            rigid.gravityScale = 1;
        }

        if (rigid.velocity.y != 0)
        {
            print(rigid.velocity.y);
        }
    }
}
