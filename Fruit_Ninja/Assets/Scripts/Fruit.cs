using UnityEngine;

// Fruit: the poor unsuspecting protagonist of this game. It dreams of sunlight
// and a peaceful life, but instead meets physics and your swashbuckling finger.
public class Fruit : MonoBehaviour
{
    public GameObject whole;   // the intact fruit model
    public GameObject sliced;  // the post-mortem pieces
    private Rigidbody rb;
    private Collider fruitCollider;
    private ParticleSystem juiceEffect; // the dramatic splatter
    public int point = 1; // how much the game values this fruit
    private void Awake()
    {
        // Cache components so we stop asking the Fruit to look for its keys every frame.
        rb = GetComponent<Rigidbody>();
        fruitCollider = GetComponent<Collider>();
        juiceEffect = GetComponentInChildren<ParticleSystem>();
    }

    private void Sliced(Vector3 direction, Vector3 position, float force)
    {   
        // Sliced: handle the glorious aftermath. Swap models, play particles,
        // fling the slices like a tiny salad being thrown across the room.
        FindAnyObjectByType<GameManager>().IncreaseScore(point);
    whole.SetActive(false);
    sliced.SetActive(true);
    if (fruitCollider != null) fruitCollider.enabled = false;
    if (juiceEffect != null) juiceEffect.Play();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        sliced.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        Rigidbody[] slices = sliced.GetComponentsInChildren<Rigidbody>();
        foreach(Rigidbody slice in slices)
        {
            // Give each slice the original fruit's velocity and an extra push
            slice.velocity = rb.velocity;
            slice.AddForceAtPosition(direction * force, position, ForceMode.Impulse);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        // If the blade touches the fruit, tell the fruit to accept its fate.
        if (other.CompareTag("Player"))
        {   
            Blade blade=other.GetComponent<Blade>();
            // Defensive: make sure blade isn't null (just in case a friendly ghost bumped it)
            if (blade != null)
                Sliced(blade.direction, blade.transform.position, blade.sliceForce);
        }
    }
}
