using System.Collections;
using UnityEngine;

// Spawner: quietly and reliably spawns fruits and the occasional bomb.
// It's the invisible carnival operator who keeps the show going.
[RequireComponent(typeof(Collider))]
public class Spawner : MonoBehaviour
{
    private Collider spawnArea; // the box or area where spawn positions are chosen

    [SerializeField] private GameObject[] fruitPrefabs;
    [SerializeField] private GameObject bombPrefab;
    [Range(0f, 1f)]
    [SerializeField] private float bombChance = 0.05f;
    [SerializeField] private float minSpawnDelay = 0.25f;
    [SerializeField] private float maxSpawnDelay = 1f;

    [SerializeField] private float minAngle = -15f;
    [SerializeField] private float maxAngle = 15f;

    [SerializeField] private float minForce = 18f;
    [SerializeField] private float maxForce = 22f;

    [SerializeField] private float maxLifetime = 5f;

    private void Awake()
    {
        // Awake: cache our spawn area so we know where to fling fruit from.
        spawnArea = GetComponent<Collider>();
    }

    private void OnEnable()
    {
        // Start the main spawn coroutine. It's the heart that pumps fruit into the scene.
        StartCoroutine(Spawn());
    }

    private void OnDisable()
    {
        // Stop all coroutines if this thing gets turned off. No more fruit today.
        StopAllCoroutines();
    }

    private IEnumerator Spawn()
    {
        // Give the player a moment to prepare, then begin random chaos.
        yield return new WaitForSeconds(2f);

        while (enabled)
        {
            // Pick a fruit at random (or a bomb, if fate is unkind).
            GameObject prefab = fruitPrefabs[Random.Range(0, fruitPrefabs.Length)];

            if (Random.value < bombChance) {
                prefab = bombPrefab; // curveball!
            }

            // Pick a random position inside the spawn area's bounds.
            Vector3 position = new Vector3
            {
                x = Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x),
                y = Random.Range(spawnArea.bounds.min.y, spawnArea.bounds.max.y),
                z = Random.Range(spawnArea.bounds.min.z, spawnArea.bounds.max.z)
            };

            Quaternion rotation = Quaternion.Euler(0f, 0f, Random.Range(minAngle, maxAngle));

            GameObject fruit = Instantiate(prefab, position, rotation);
            Destroy(fruit, maxLifetime); // clean up when it's old and tired

            float force = Random.Range(minForce, maxForce);
            fruit.GetComponent<Rigidbody>().AddForce(fruit.transform.up * force, ForceMode.Impulse);

            // Wait a random time before spawning the next fruity tragedy.
            yield return new WaitForSeconds(Random.Range(minSpawnDelay, maxSpawnDelay));
        }
    }

}