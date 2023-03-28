using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrollscript : MonoBehaviour {

    public float speedX = 0.0f;
    public float speedY = 0.0f;
    Renderer rend;

    void Start () 
    {
        rend = GetComponent<Renderer>();
	}
	
	void FixedUpdate ()
    {
        rend.material.mainTextureOffset = new Vector2((Time.time * speedX) % 1, (Time.time * speedY) % 1);
    }
}
