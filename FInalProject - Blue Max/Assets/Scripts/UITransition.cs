using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITransition : MonoBehaviour
{
    public Image camfader;

    private void Start()
    {
        camfader.raycastTarget = false;
    }

    //Fade UI plane that covers camera view from one colour to another. works with alpha channel.
    public IEnumerator CameraColourFade(Color col1, Color col2, float fadeTime) 
    {
        float timer = 0.0f;
        camfader.color = col1;

        while (camfader.color != col2) //not equal to target alpha
        {
            camfader.raycastTarget = true;
            camfader.color = Color.Lerp(col1, col2, timer / fadeTime);
            timer += Time.deltaTime;
            yield return null;
        }
        camfader.raycastTarget = false;
    }
}
