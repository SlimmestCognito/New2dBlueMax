using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialMovement : MonoBehaviour
{
    RectTransform rt;

    Vector2 StartPos = new(0, 0);
    public List<Vector2> posList = new List<Vector2>();

    bool CanStartCoroutine = true;

    [SerializeField]
    AnimationCurve movementCurve;

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
            StartCoroutine(MoveDirection(posList[Random.Range(0, posList.Count)]));
        }
    }

    float s = 1.0f;

    private IEnumerator MoveDirection(Vector2 movetoPos) 
    {
        CanStartCoroutine = false;
        float timeElapse = 0;

        //moveto target
        while (timeElapse < s)
        {
            rt.anchoredPosition = Vector2.Lerp(StartPos, movetoPos, movementCurve.Evaluate(timeElapse));
            timeElapse += Time.deltaTime;
            yield return null;
        }

        //move back to start
        timeElapse = 0;
        yield return new WaitForSeconds(0.2f);

        //moveto target
        while (timeElapse < s)
        {
            rt.anchoredPosition = Vector2.Lerp(movetoPos, StartPos, movementCurve.Evaluate(timeElapse));
            timeElapse += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);
        CanStartCoroutine = true;
    }

    private void OnDisable()
    {
        StopCoroutine("MoveDirection");
        CanStartCoroutine = true;
    }
}
