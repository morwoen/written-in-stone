using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CooldownManagement;

public class ExperiencePickupCollectable : MonoBehaviour, ICollectable
{
    [SerializeField] private ExperienceSO experience;
    [SerializeField] private float animationDuration;

    public void Collect(Transform player) {
        var pos = transform.position;

        GetComponent<Collider>().enabled = false;

        Cooldown.Wait(animationDuration)
            .OnProgress((current, total) => {
                transform.position = Vector3.Lerp(pos, player.position, current / total);
            })
            .OnComplete(() => {
                experience.AddExperience(10);
                Destroy(gameObject);
            });
    }
}
