using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour
{
    private Color cBlack = new Vector4(0, 0, 0, 1);
    private Color cTransparent = new Vector4(0, 0, 0, 0);

    private bool allowInput;

    // Start is called before the first frame update
    void Start()
    {
        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = 120;

        allowInput = false;
        StartCoroutine("SplashScreenStart");

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey && allowInput)
        {
            StopAllCoroutines();
            StartCoroutine("MoveToMainMenu");
        }
    }

    IEnumerator SplashScreenStart()
    {
        StartCoroutine(Camera.main.GetComponent<UITransition>().CameraColourFade(cBlack, cTransparent, 1.0f));
        yield return new WaitForSeconds(1);
        allowInput = true;
        yield return new WaitForSeconds(3);
        StartCoroutine("MoveToMainMenu");
    }

    IEnumerator MoveToMainMenu() 
    {
        allowInput = false;
        StartCoroutine(Camera.main.GetComponent<UITransition>().CameraColourFade(cTransparent, cBlack, 1.0f));
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("MainMenu");
    }
}
