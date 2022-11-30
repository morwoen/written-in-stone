using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEarthBend : ActiveEffect
{
    [SerializeField] private Animator wall;
    [SerializeField] private ExperienceSO experience;
    [SerializeField] private Vector2 distanceMinMax;
    [SerializeField] private float duration;

    public override void UpdateGameObject() {
    }

    private IEnumerator Start() {
        List<Animator> animators = new();

        var count = 1 + experience.Level / 10;
        for (int i = 0; i < count; i++) {
            var pos = distanceMinMax.y.RandomPointInCircle(distanceMinMax.x, 0);
            var anim = Instantiate(wall, transform.position + pos, Quaternion.Euler(0, Random.Range(0, 360), 0), transform);

            animators.Add(anim);
        }

        yield return new WaitForSeconds(duration);

        foreach (var anim in animators) {
            anim.SetTrigger("Hide");
        }

        Destroy(gameObject, 1);
    }
}
