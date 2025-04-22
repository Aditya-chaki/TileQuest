using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantControl : MonoBehaviour
{
    void OnCollisionExit(Collision other)
    {
        ContactPoint contact = other.contacts[0];
         Vector3 pos = contact.point;
         Debug.Log(pos);
    }
}
