using System;
using System.Collections;
using UnityEngine;

public class CoroutineHelper
{
    /**
     * Usage: StartCoroutine(CoroutineUtils.Chain(...))
     * For example:
     *     StartCoroutine(CoroutineUtils.Chain(
     *         CoroutineUtils.Do(() => Debug.Log("A")),
     *         CoroutineUtils.WaitForSeconds(2),
     *         CoroutineUtils.Do(() => Debug.Log("B"))));
     */

    public static IEnumerator Chain(params IEnumerator[] actions)
    {
        foreach (IEnumerator action in actions)
        {
            yield return action;
        }
    }

    /**
     * Usage: StartCoroutine(CoroutineUtils.WaitForSeconds(seconds))
    */
    public static IEnumerator DelaySeconds(Action action, float delay)
    {
        Debug.Log("aaaaaaaaaaaa");
        yield return new WaitForSeconds(delay);
        action();
    }

    public static IEnumerator WaitUntil(Func<bool> predicate)
    {
        yield return new WaitUntil(predicate);
    }
    public static IEnumerator WaitWhile(Func<bool> predicate)
    {
        yield return new WaitWhile(predicate);
    }

    public static IEnumerator WaitForSeconds(float time)
    {
        yield return new WaitForSeconds(time);
    }

    public static IEnumerator WaitForSecondsRealTime(float time)
    {
        yield return new WaitForSecondsRealtime(time);
    }

    public static IEnumerator WaitForEndOfFrame()
    {
        yield return new WaitForEndOfFrame();
    }

    public static IEnumerator WaitForFixedUpdate()
    {
        yield return new WaitForFixedUpdate();
    }

    public static IEnumerator Do(Action action)
    {
        action();
        yield return 0;
    }
}