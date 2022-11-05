using UnityEngine;

[CreateAssetMenu(fileName = "New Health", menuName = "ScriptableObjects/Health")]
public class HealthSO : ScriptableObject
{
    public delegate void HealthChange(int current, int max);
    public event HealthChange change;

    public int MaxHealth = 100;
    public int CurrentHealth { get; private set; } = 0;

    private void OnEnable() {
        Respawn();
    }

    public bool Damage(int damage) {
        var newValue = CurrentHealth - damage;
        if (newValue < 0 && CurrentHealth > 1) {
            CurrentHealth = 1;
        } else {
            CurrentHealth = Mathf.Clamp(newValue, 0, MaxHealth);
        }
        change?.Invoke(CurrentHealth, MaxHealth);
        return CurrentHealth <= 0;
    }

    public void Heal(int damage) {
        CurrentHealth = Mathf.Clamp(CurrentHealth + damage, 0, MaxHealth);
        change?.Invoke(CurrentHealth, MaxHealth);
    }

    public void Respawn() {
        CurrentHealth = MaxHealth;
        change?.Invoke(CurrentHealth, MaxHealth);
    }
}
