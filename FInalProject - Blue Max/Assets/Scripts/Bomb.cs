using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private GameObject bombSplash;
    private float bombTimer = 0.5f;

    // Update is called once per frame
    void Update()
    {
        bombTimer -= Time.deltaTime;
        if (bombTimer < 0)
        {
            GameObject bs = Instantiate(bombSplash, transform.position, Quaternion.identity);
            bs.GetComponent<Rigidbody>().AddForce(new Vector2(-280.0f, -210.0f));
            AudioCreator.CreateAudioSource(AudioCreator.explosionSFXsArray[Random.Range(0, AudioCreator.explosionSFXsArray.Length)], 0.4f, transform);
            Destroy(gameObject);
            Destroy(bs, 2.0f);
        }
    }
}
