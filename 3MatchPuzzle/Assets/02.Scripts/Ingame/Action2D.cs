using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Action2D
{
    public static IEnumerator MoveTo(Transform target, Vector3 to, float duration = 0.15f, bool IsSwap = false)
    {
        FindMatches.MovingDot.Enqueue(target);
        Vector2 startPos = target.transform.position;

        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            if (target == null)
            {
                FindMatches.MovingDot.Dequeue();
                yield break;
            }

            elapsed += Time.smoothDeltaTime;
            target.transform.position = Vector2.Lerp(startPos, to, elapsed / duration);

            yield return null;
        }

        if(target.GetComponent<Dot>() == true)
        {
            Dot target_Dot = target.GetComponent<Dot>();

            if (IsSwap)
            {
                Board.SetChangeDotArray(target_Dot, to);
            }
            target_Dot.column = (int)to.x;
            target_Dot.row = (int)to.y;

        }

        target.transform.position = to;
        FindMatches.MovingDot.Dequeue();
        yield return null;
    }

    
}
