using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActiveEffect : MonoBehaviour
{
    protected int damage = 1;
    protected float areaMultiplier = 1;

    public void SetParameters(int damage, float areaMultiplier) {
        this.damage = damage;
        this.areaMultiplier = areaMultiplier;

        UpdateGameObject();
    }

    public abstract void UpdateGameObject();
}
