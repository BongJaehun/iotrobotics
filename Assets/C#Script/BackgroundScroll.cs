using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    Material mat;
    float currentXoffset = 0;
    public float speed;
    void Start()
    {
        mat = GetComponent<SpriteRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        currentXoffset += speed * Time.deltaTime;
        mat.mainTextureOffset = new Vector2(currentXoffset, 0);
    }
}
