using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeResolutionEvent : CTEvent
{
    public int resolution;
}

public class GameEventManager : MonoBehaviour
{
    void Awake()
    {
        CTEventManager.AddListener<ChangeResolutionEvent>(test);
    }

    void OnDestroy()
    {
        CTEventManager.AddListener<ChangeResolutionEvent>(test);
    }

    void test(ChangeResolutionEvent eventData)
    {
        //CTEventManager.FireEvent(new ChangeResolutionEvent() { resolution = 20 });
    }
}
