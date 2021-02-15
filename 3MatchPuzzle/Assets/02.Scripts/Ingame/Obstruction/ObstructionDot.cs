using UnityEngine;
using UnityEngine.UI;

public class ObstructionDot : MonoBehaviour
{
    [Header("체력 표시 UI")]
    public Text Healthbar;
    public int Health;

    virtual public void TakeDamage(int Damage) { }
}
