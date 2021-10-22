using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCollision : MonoBehaviour
{

    void OnCollisionEnter(Collision col)
    {
        if (col.collider.CompareTag("Room Objects"))
        {
            Debug.Log("Hit");
        }
    }

}
