using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Serialization;

enum EaseType
{
    easeIn,
    easeOut,
    easeInOut
}

public class Movement : MonoBehaviour
{
    public GameObject cube;
    public GameObject cube1;
    public GameObject cube2;
    public GameObject cube3;
    public GameObject cube4;

    void Start()
    {
        var endPos = new Vector3(20, 0, 0);
        move(cube, cube.transform.position, cube.transform.position + endPos, 3);
        move(cube1, cube1.transform.position, cube1.transform.position + endPos, 3, true);
        EaseIn(cube2, cube2.transform.position, cube2.transform.position + endPos, 3, true);
        EaseOut(cube3, cube3.transform.position, cube3.transform.position + endPos, 3, true);
        EaseInOut(cube4, cube4.transform.position, cube4.transform.position + endPos, 3, true);
    }

    void EasePingpong(GameObject gameObject, Vector3 begin, Vector3 end, float time, EaseType easeType)
    {
        StartCoroutine(updateEasePingpong(gameObject, begin, end, time, easeType));
    }

    void EaseInOut(GameObject gameObject, Vector3 begin, Vector3 end, float time, bool pingpong = false)
    {
        if (pingpong)
        {
            EasePingpong(gameObject, begin, end, time, EaseType.easeInOut);
        }
        else
        {
            StartCoroutine(updateEaseInOut(gameObject, begin, end, time));
        }
    }

    void EaseOut(GameObject gameObject, Vector3 begin, Vector3 end, float time, bool pingpong = false)
    {
        if (pingpong)
        {
            EasePingpong(gameObject, begin, end, time, EaseType.easeOut);
        }
        else
        {
            StartCoroutine(updateEaseOut(gameObject, begin, end, time));
        }
    }

    void EaseIn(GameObject gameObject, Vector3 begin, Vector3 end, float time, bool pingpong = false)
    {
        if (pingpong)
        {
            EasePingpong(gameObject, begin, end, time, EaseType.easeIn);
        }
        else
        {
            StartCoroutine(updateEaseIn(gameObject, begin, end, time));
        }
    }

    IEnumerator updateEaseInOut(GameObject gameObject, Vector3 begin, Vector3 end, float time)
    {
        var path = end - begin;
        var startTime = Time.time;
        while (true)
        {
            float ratio = 1 / (1 + Mathf.Pow(2.7f, -10 * ((Time.time - startTime) / time - 0.5f))) + 0.006920939f;
            gameObject.transform.position = begin + path * ratio;
            if (Vector3.SqrMagnitude(gameObject.transform.position - begin) >= path.magnitude * path.magnitude)
            {
                gameObject.transform.position = end;
                break;
            }
            yield return 0;
        }
    }

    IEnumerator updateEaseIn(GameObject gameObject, Vector3 begin, Vector3 end, float time)
    {
        var path = end - begin;
        var startTime = Time.time;
        while (true)
        {
            float ratio = Mathf.Pow((Time.time - startTime) / time, 4);
            gameObject.transform.position = begin + path * ratio;
            if (Vector3.SqrMagnitude(gameObject.transform.position - begin) >= path.magnitude * path.magnitude)
            {
                gameObject.transform.position = end;
                break;
            }
            yield return 0;
        }
    }

    IEnumerator updateEaseOut(GameObject gameObject, Vector3 begin, Vector3 end, float time)
    {
        var path = end - begin;
        var startTime = Time.time;
        while (true)
        {
            float ratio = Mathf.Pow(Mathf.Log((Time.time - startTime) / time + 1, 10), 0.2f) + 0.213457934f;
            gameObject.transform.position = begin + path * ratio;
            if (Vector3.SqrMagnitude(gameObject.transform.position - begin) >= path.magnitude * path.magnitude)
            {
                gameObject.transform.position = end;
                break;
            }
            yield return 0;
        }
    }

    // IEnumerator EaseOut(GameObject gameObject, Vector3 begin, Vector3 end, float time)
    // {
    //     var startSlowTimeRatio = Mathf.Log(0, 10) + 1;
    //     var path = end - begin;
    //     var path1 = path * startSlowTimeRatio;
    //     var startTime = Time.time;
    //     while (true)
    //     {
    //         float ratio = Mathf.Log((Time.time - startTime) / time, 10) + 1;
    //         if (ratio >= 0)
    //         {
    //             gameObject.transform.position = begin + path * ratio;
    //         }
    //         else
    //         {
    //             ratio = Time.deltaTime / (startSlowTimeRatio * time);
    //             gameObject.transform.position += path1 * ratio;
    //         }
    //         if (Vector3.SqrMagnitude(end - gameObject.transform.position) < 0.001f)
    //         {
    //             gameObject.transform.position = end;
    //             break;
    //         }
    //         yield return 0;
    //     }
    // }

    void move(GameObject gameObject, Vector3 begin, Vector3 end, float time, bool pingpong = false)
    {
        if (pingpong)
        {
            StartCoroutine(updateMovePingpong(gameObject, begin, end, time));
        }
        else
        {
            StartCoroutine(updateMove(gameObject, begin, end, time));
        }
    }

    IEnumerator updateMove(GameObject gameObject, Vector3 begin, Vector3 end, float time)
    {
        Vector3 path = end - begin;
        while (true)
        {
            float ratio = Time.deltaTime / time;
            gameObject.transform.position += path * ratio;
            if (Vector3.SqrMagnitude(end - gameObject.transform.position) < 0.001f)
            {
                gameObject.transform.position = end;
                break;
            }
            yield return 0;
        }
    }

    IEnumerator updateMovePingpong(GameObject gameObject, Vector3 begin, Vector3 end, float time)
    {
        var targetPos = end;
        StartCoroutine(updateMove(gameObject, begin, end, time));
        while (true)
        {
            if (Vector3.SqrMagnitude(targetPos - gameObject.transform.position) < 0.001f)
            {
                targetPos = targetPos == end ? begin : end;
                StartCoroutine(updateMove(gameObject, gameObject.transform.position, targetPos, time));
            }
            yield return 0;
        }
    }

    IEnumerator updateEasePingpong(GameObject gameObject, Vector3 begin, Vector3 end, float time, EaseType easeType)
    {
        var targetPos = end;
        var originPos = begin;
        var path = end - begin;
        SelectEaseUpdate(gameObject, begin, end, time, easeType);
        while (true)
        {
            if (Vector3.SqrMagnitude(gameObject.transform.position - originPos) >= path.magnitude * path.magnitude)
            {
                targetPos = targetPos == end ? begin : end;
                originPos = originPos == begin ? end : begin;
                SelectEaseUpdate(gameObject, originPos, targetPos, time, easeType);
            }
            yield return 0;
        }
    }

    void SelectEaseUpdate(GameObject gameObject, Vector3 begin, Vector3 end, float time, EaseType easeType)
    {
        switch (easeType)
        {
            case EaseType.easeIn:
                StartCoroutine(updateEaseIn(gameObject, begin, end, time));
                break;
            case EaseType.easeOut:
                StartCoroutine(updateEaseOut(gameObject, begin, end, time));
                break;
            case EaseType.easeInOut:
                StartCoroutine(updateEaseInOut(gameObject, begin, end, time));
                break;
        }
    }
}