
public class Basic : ObstructionDot
{
    void Start()
    {
        Healthbar.text = Health.ToString();
    }
    void Update()
    {
        if (Health <= 0)
            Destroy(this.gameObject);
    }

    public override void TakeDamage(int Damage)
    {
        Health -= Damage;
        Healthbar.text = Health.ToString();
    }
}
