using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnInvisible : MonoBehaviour
{
    //Destroy when Offscreen
    void OnBecameInvisible()
    {
        Destroy(gameObject);      
    }
}
