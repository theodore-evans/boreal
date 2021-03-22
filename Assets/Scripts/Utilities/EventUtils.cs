using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public static class EventUtils
{
    public static IEnumerator DelayedAction(UnityAction action)
    {
        yield return new WaitForEndOfFrame(); // Wait for the next frame
        action.Invoke(); // execute a delegate
    }
}
