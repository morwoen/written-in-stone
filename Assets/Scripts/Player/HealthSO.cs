using UnityEngine;

[CreateAssetMenu(fileName = "New Health", menuName = "ScriptableObjects/Health")]
public class HealthSO : ScriptableObject
{
    public delegate void HealthChange(int current, int max);
    public event HealthChange change;
    public delegate void PlayerDied();
    public event PlayerDied death;
    public delegate void PlayerRespawned();
    public event PlayerRespawned respawn;

    public int MaxHealth = 100;
    public int CurrentHealth { get; private set; } = 0;
    public int BonusHealth = 0;

    private int healthWhenIncreased = 0;

    private void OnEnable() {
        Respawn();
    }

    public bool Damage(int damage) {
        var newValue = CurrentHealth - damage;
        if (newValue < 0 && CurrentHealth > 1) {
            CurrentHealth = 1;
        } else {
            CurrentHealth = Mathf.Clamp(newValue, 0, MaxHealth + BonusHealth);
        }
        change?.Invoke(CurrentHealth, MaxHealth + BonusHealth);
        if (CurrentHealth <= 0) {
            death?.Invoke();
        }
        return CurrentHealth <= 0;
    }

    public void Heal(int damage) {
        CurrentHealth = Mathf.Clamp(CurrentHealth + damage, 0, MaxHealth + BonusHealth);
        change?.Invoke(CurrentHealth, MaxHealth + BonusHealth);
    }

    public void Respawn() {
        CurrentHealth = MaxHealth;
        BonusHealth = 0;
        healthWhenIncreased = 0;
        change?.Invoke(CurrentHealth, MaxHealth);
        respawn?.Invoke();
    }

    public void SetBonusHealth(int bonus) {
        var diff = bonus - BonusHealth;

        BonusHealth = bonus;
        if (diff > 0) {
            healthWhenIncreased = CurrentHealth;
            CurrentHealth = Mathf.Clamp(CurrentHealth + diff, 0, MaxHealth + BonusHealth);
        } else if (diff < 0) {
            if (healthWhenIncreased < CurrentHealth) {
                CurrentHealth = healthWhenIncreased;
                healthWhenIncreased = 0;
            }
            CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth + BonusHealth);
        }
        change?.Invoke(CurrentHealth, MaxHealth + BonusHealth);
    }
}
