public class RatData
{
    public RatData(float currentHealth, float maxHealth, string name)
    {
        CurrentHealth = currentHealth;
        MaxHealth = maxHealth;
        Name = name;
    }
    public RatData()
        : this(DEFAULT_NEW_RAT_HEALTH, DEFAULT_NEW_RAT_HEALTH, "Joe") {}
    public RatData(string name)
        : this(DEFAULT_NEW_RAT_HEALTH, DEFAULT_NEW_RAT_HEALTH, name) {}

    const float DEFAULT_NEW_RAT_HEALTH = 100f;
    public float CurrentHealth;
    public float MaxHealth;
    public string Name;
}
