using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CooldownManagement;

[CreateAssetMenu(fileName = "New Dash", menuName = "ScriptableObjects/Dash")]
public class DashSO : ScriptableObject
{
    public delegate void DashUsed(Cooldown cooldown);
    public event DashUsed used;

    public delegate void MaxDashChargesChanged(int charges);
    public event MaxDashChargesChanged maxChargesChanged;

    [SerializeField] private float cooldown = 1f;
    [SerializeField] private int maxCharges = 1;
    public int MaxCharges { get; private set; } = 1;
    public int Charges { get; private set; }


    private void OnEnable() {
        Restart();
    }

    public void Restart() {
        MaxCharges = maxCharges;
        Charges = MaxCharges;
    }

    public void AddCharge() {
        MaxCharges += 1;
        Charges += 1;
        maxChargesChanged?.Invoke(MaxCharges);
    }

    public void RemoveCharge() {
        MaxCharges -= 1;
        Charges -= 1;
        maxChargesChanged?.Invoke(MaxCharges);
    }

    public bool Dash() {
        if (Charges < 1) return false;

        Charges -= 1;
        used?.Invoke(Cooldown.Wait(cooldown)
            .OnComplete(() => {
                Charges += 1;
            }));
        return true;
    }
}
