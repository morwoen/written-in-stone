using UnityEngine;

public class TriggerSignal : MonoBehaviour
{
    private bool somethinIn = false;

    private void OnTriggerEnter(Collider other) {
        transform.parent.SendMessageUpwards("OnTriggerEnter");
        somethinIn = true;
    }

    private void OnTriggerExit(Collider other) {
        transform.parent.SendMessageUpwards("OnTriggerExit");
        somethinIn = false;
    }

    // TODO: Hack for the jam, triggers don't call OnTriggerExit after a disable
    private void OnDisable() {
        if (somethinIn)
            transform.parent.SendMessageUpwards("OnTriggerExit");
    }
}
