using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField]
    int duration = 1;

    void Awake() {
        Destroy(gameObject, duration);
    }
}
