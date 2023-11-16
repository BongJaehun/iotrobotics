using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lab_Player : MonoBehaviour
{
    public float Y;
    void Update()
    {
        Y += Input.GetAxis("Mouse ScrollWheel") * 10;
        if (Mathf.Abs(Y) >= 10)
        {
            if (Y > 0)
            {
                Y = 10;
            }
            else
            {
                Y = -10;
            }
        }
        Debug.Log(Y);
    }
}
