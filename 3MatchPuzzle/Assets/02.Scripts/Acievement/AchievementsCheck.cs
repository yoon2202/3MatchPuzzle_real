using UnityEngine;

public abstract class AchievementsCheck : MonoBehaviour
{
    protected int MaxCount;
    protected int CurrentCount;

    public abstract void DotCheck(GameObject gameObject);
}
