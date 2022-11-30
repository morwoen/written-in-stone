using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Execute after the player movement for smooth following of the player
[DefaultExecutionOrder(1)]
public class CircleOfDeath : ActiveEffect
{
    [SerializeField] private CircleOfDeathMissile missilePrefab;
    [SerializeField] private ExperienceSO experience;

    private List<CircleOfDeathMissile> missiles;
    private Transform player;
    private AudioSource audioSource;

    public override void UpdateGameObject() {
    }

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    private IEnumerator Start() {
        player = FindObjectOfType<PlayerController>().transform;

        missiles = new();
        var count = 2 + experience.Level / 10;

        for (var i = 0; i < count; i++) {
            var missile = Instantiate(missilePrefab, (transform.position + transform.forward * 5).WithY(1.5f), Quaternion.identity, transform);
            missile.SetDamage(damage);
            missiles.Add(missile);

            yield return new WaitForSeconds(1);
        }

        yield return new WaitForSeconds(1);

        for (int i = 0; i < missiles.Count; i++) {
            var temp = missiles[i];
            int randomIndex = Random.Range(i, missiles.Count);
            missiles[i] = missiles[randomIndex];
            missiles[randomIndex] = temp;
        }

        while (missiles.Count > 0) {
            var missile = missiles[0];
            missiles.RemoveAt(0);

            if (missile) {
                missile.transform.parent = null;
                missile.enabled = true;
                audioSource.PlayOneShot(effectSound);
            }

            yield return new WaitForSeconds(0.5f);
        }

        Destroy(gameObject);
    }

    private void Update() {
        transform.position = Vector3.Lerp(transform.position, player.position, .8f);

        foreach (var missile in missiles) {
            if (missile) {
                missile.transform.RotateAround(transform.position, Vector3.up, 80 * Time.deltaTime);
                missile.transform.LookAt(transform.position.WithY(1.5f));
            }
        }
    }
}
