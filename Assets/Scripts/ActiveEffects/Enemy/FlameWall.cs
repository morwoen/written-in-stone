using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameWall : MonoBehaviour
{
    [SerializeField] private Transform markers;
    [SerializeField] private Transform fires;

    private IEnumerator Start() {
        for (int i = 0; i < markers.childCount; i++) {
            markers.GetChild(i).gameObject.SetActive(false);
        }
        for (int i = 0; i < fires.childCount; i++) {
            fires.GetChild(i).gameObject.SetActive(false);
        }

        StartCoroutine(ActivateIndicators());

        yield return new WaitForSeconds(1.3f);

        StartCoroutine(ActivateFires());

        yield return new WaitForSeconds(1.1f);

        StartCoroutine(RemoveFireColliders());
    }

    private IEnumerator ActivateIndicators() {
        for (int i = 0; i < markers.childCount; i++) {
            markers.GetChild(i).gameObject.SetActive(true);
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator ActivateFires() {
        for (int i = 0; i < fires.childCount; i++) {
            fires.GetChild(i).gameObject.SetActive(true);
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator RemoveFireColliders() {
        for (int i = 0; i < fires.childCount; i++) {
            var obj = fires.GetChild(i).gameObject;
            obj.GetComponent<Collider>().enabled = false;
            // Disable in order to send trigger exits if necessary
            obj.GetComponent<TriggerSignal>().enabled = false;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
