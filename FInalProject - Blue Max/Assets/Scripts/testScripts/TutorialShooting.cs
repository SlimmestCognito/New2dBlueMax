using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialShooting : MonoBehaviour
{
    RectTransform rt;
    public List<Vector2> posList = new List<Vector2>();
    bool CanStartCoroutine = true;

    // Start is called before the first frame update
    void Start()
    {
        rt = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (CanStartCoroutine)
        {
            StartCoroutine(MoveDirection());
        }
    }

    float s = 0.2f;
    private IEnumerator MoveDirection()
    {
        CanStartCoroutine = false;
        float timeElapse = 0;

        //moveto target
        while (timeElapse < s)
        {
            rt.anchoredPosition = Vector2.Lerp(posList[0], posList[1], timeElapse / s);
            timeElapse += Time.deltaTime;
            yield return null;
        }
        rt.anchoredPosition = new(500, 500); //offscreen
        yield return new WaitForSeconds(0.2f);

        CanStartCoroutine = true;
    }

    private void OnDisable()
    {
        StopCoroutine("MoveDirection");
        CanStartCoroutine = true;
    }

}
