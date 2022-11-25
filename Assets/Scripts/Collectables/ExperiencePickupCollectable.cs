using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperiencePickupCollectable : MonoBehaviour, ICollectable
{
    [SerializeField] private ExperienceSO experience;

    public void Collect(Transform player) {
        experience.AddExperience(10);
        // TODO: Animate?
        Destroy(gameObject);
    }
}
