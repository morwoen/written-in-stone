using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthBar : MonoBehaviour
{
    private float desiredValue = 1;
    private float currentValue = 1;

    private Material mat;
    private Camera cam;

    private void Awake() {
        mat = GetComponent<Renderer>().sharedMaterial;
        cam = Camera.main;
    }

    private void Update() {
        currentValue = Mathf.Lerp(currentValue, desiredValue, .5f);
        mat.SetFloat("_Percentage", currentValue);
    }

    private void LateUpdate() {
        transform.rotation = Quaternion.LookRotation(-cam.transform.up);
    }

    public void SetValue(float value) {
        desiredValue = value;
    }
}
