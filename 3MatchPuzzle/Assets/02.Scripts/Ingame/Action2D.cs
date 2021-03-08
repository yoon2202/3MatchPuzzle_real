using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Action2D
{
    public static IEnumerator MoveTo(Dot target, Vector3 to, float duration = 0.15f, bool IsSwap = false)
    {
        FindMatches.MovingDot.Enqueue(target);
        Vector2 startPos = target.transform.position;
        //Vector2 endPos = to.transform.position;

        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            elapsed += Time.smoothDeltaTime;
            target.transform.position = Vector2.Lerp(startPos, to, elapsed / duration);

            yield return null;
        }
        if (IsSwap)
        {
            Board.SetChangeDotArray(target, to);
        }
        target.transform.position = to;
        target.column = (int)to.x;
        target.row = (int)to.y;
        FindMatches.MovingDot.Dequeue();
        yield break;
    }

    
}
