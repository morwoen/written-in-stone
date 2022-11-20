using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActiveEffect : MonoBehaviour
{
    protected int damage = 1;

    public void SetParameters(int damage) {
        this.damage = damage;
        UpdateGameObject();
    }

    public abstract void UpdateGameObject();
}
