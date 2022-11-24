using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        var collectable = other.GetComponent<ICollectable>();
        collectable?.Collect(transform.parent);
    }
}
