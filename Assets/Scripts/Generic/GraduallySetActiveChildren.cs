using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraduallySetActiveChildren : MonoBehaviour
{
    [SerializeField] private float initialDelay = 0.1f;
    [SerializeField] private float delay = 0.1f;
    [SerializeField] private bool active = true;
    [SerializeField] private bool init = false;

    private void Awake() {
        if (init) {
            for (int i = 0; i < transform.childCount; i++) {
                transform.GetChild(i).gameObject.SetActive(!active);
            }
        }
    }

    IEnumerator Start() {
        yield return new WaitForSeconds(initialDelay);
        for (int i = 0; i < transform.childCount; i++) {
            transform.GetChild(i).gameObject.SetActive(active);
            yield return new WaitForSeconds(delay);
        }
    }
}
