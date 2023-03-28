using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testShadow : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    Vector2 startpos;

    private void Start()
    {
        startpos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 ppos;
        ppos = player.transform.position;
        Vector2 translation = new Vector2(ppos.x, startpos.y);
        transform.position = translation;
    }
}
