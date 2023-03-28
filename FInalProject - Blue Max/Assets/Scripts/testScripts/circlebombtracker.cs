using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class circlebombtracker : MonoBehaviour
{
    public GameObject startCircle;

    public GameObject targetCircle;
    public GameObject trackingCircle;

    SpriteRenderer circleSR; 

    // Start is called before the first frame update
    void Start()
    {
        circleSR = startCircle.GetComponent<SpriteRenderer>();
        startCircle.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!UIMenuManager.isPaused)
        {

            if (Input.GetKey(KeyCode.R) && UIManager.playerBombs > 0 || Input.GetButton("Fire2") && UIManager.playerBombs > 0)
            {
                startCircle.SetActive(true);

                GameObject test = Instantiate(trackingCircle, startCircle.transform.position, Quaternion.identity);
                test.GetComponent<Rigidbody>().AddForce(new Vector2(-280.0f, -210.0f));
                //AudioCreator.CreateAudioSource(AudioCreator.explosionSFXsArray[Random.Range(0, AudioCreator.explosionSFXsArray.Length)], 0.4f, transform);
            }
            else 
            {
                startCircle.SetActive(false);
            }
        }
    }       
}
