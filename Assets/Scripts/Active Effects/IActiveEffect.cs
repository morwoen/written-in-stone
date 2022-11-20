using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IActiveEffect
{
    float EffectScale { get; set; }

    void SetParameters(float effectScale);

    void UpdateGameObject();
}
