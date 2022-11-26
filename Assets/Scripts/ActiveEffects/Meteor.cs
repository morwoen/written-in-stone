using UnityEngine;

public class Meteor : ActiveEffect
{
    [SerializeField]
    int spawnRange = 5;
    [SerializeField]
    int spawnHeight = 30;
    [SerializeField]
    int velocity = 1;
    [SerializeField]
    float explosionDuration = 1f;
    [SerializeField]
    float markerDuration = 1f;
    [SerializeField]
    int explosionScale = 4;
    [SerializeField]
    float rangeMultiplier = 2f;

    [SerializeField]
    GameObject explosion;
    [SerializeField]
    Transform marker;
    [SerializeField] private LayerMask enemiesLayerMask;

    Vector3 direction;

    public override void UpdateGameObject() {
        transform.localScale *= areaMultiplier;
    }

    void Explode() {
        GameObject explosionInstance = Instantiate(explosion, transform.position, Quaternion.identity);
        explosionInstance.transform.localScale *= explosionScale * areaMultiplier;
        Destroy(explosionInstance, explosionDuration);
        Destroy(gameObject);
    }

    void Start() {
        var randomPoint = Random.insideUnitCircle;

        var enemies = Physics.OverlapSphere(transform.position, spawnRange, enemiesLayerMask);
        if (enemies.Length == 0) {
            transform.position = new Vector3(
                transform.position.x + randomPoint.x * spawnRange,
                spawnHeight,
                transform.position.z + randomPoint.y * spawnRange
            );
        } else {
            var target = enemies[Random.Range(0, enemies.Length)];
            transform.position = target.transform.position.WithY(spawnHeight);
        }

        direction = transform.position;
        direction.y = 0;
        Transform markerInstance = Instantiate(marker, direction, Quaternion.identity);
        markerInstance.localScale = transform.localScale;
        Destroy(markerInstance.gameObject, markerDuration);
    }

    void Update() {
        transform.Translate(Vector3.down * velocity * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other) {
        LayerMask mask = LayerMask.GetMask("Enemy");
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, transform.localScale.x * rangeMultiplier, mask);

        foreach (Collider hitCollider in hitColliders) {
            hitCollider.GetComponent<Enemy>().Damage(damage);
        }

        Explode();
    }
}
