using UnityEngine;

// Blade: the not-so-mystical thing that follows your mouse/touch and slashes fruit.
// Think of it as a polite ghost hand with excellent aim and terrible manners.
public class Blade : MonoBehaviour
{
    public float sliceForce = 5f;
    public float minSliceVelocity = 0.01f;
    private Camera mainCamera;
    private Collider bladeCollider;
    private TrailRenderer trail;
    public Vector3 direction { get; private set; }
    public bool slicing;
    private void Awake()
    {
        // Awake: Unity calls this first. We wake up our references so the blade can
        // immediately start being dramatic in the scene.
        mainCamera = Camera.main;
        bladeCollider = GetComponent<Collider>();
        trail = GetComponentInChildren<TrailRenderer>();
    }
    private void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        if(Input.GetMouseButtonDown(0))
            StartSlice();
        
        else if(Input.GetMouseButtonUp(0))
            EndSlice();
        
        else if(slicing)
            ContinueSlice(Input.mousePosition);
#elif UNITY_IOS || UNITY_ANDROID
        if(Input.touchCount>0)
        {
            Touch touch=Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    StartSlice(touch.position);
                    break;
                case TouchPhase.Moved:
                    if (slicing) ContinueSlice(touch.position);
                    break;
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    EndSlice();
                    break;
            }
        }
#endif
    }

    private void StartSlice(Vector3 screenPosition)
    {
        // StartSlice: user pressed/touched — begin the elegant art of fruit mutilation.
        // Convert screen coords to world coords and teleport the blade there like a ninja.
        Vector3 position = mainCamera.ScreenToWorldPoint(screenPosition);
        position.z = 0f; // keep it 2D-friendly
        transform.position = position;
        slicing = true;
        if (bladeCollider != null) bladeCollider.enabled = true;
        if (trail != null) { trail.Clear(); trail.enabled = true; }
    }
    private void EndSlice()
    {
        // EndSlice: user released input. The blade sulks and hides its collider and trail.
        slicing = false;
        if (bladeCollider != null) bladeCollider.enabled = false;
        if (trail != null) trail.enabled = false;
    }
    private void ContinueSlice(Vector3 screenPosition)
    {
        // ContinueSlice: move the blade smoothly and decide whether it should be
        // 'active' based on how fast the user is swiping. Slow pokes are harmless.
        Vector3 newPosition = mainCamera.ScreenToWorldPoint(screenPosition);
        newPosition.z = 0f;
        direction = newPosition - transform.position;
        float velocity = direction.magnitude / Time.deltaTime;
        if (bladeCollider != null) bladeCollider.enabled = velocity > minSliceVelocity;
        transform.position = newPosition;
    }
    private void StartSlice() => StartSlice(Input.mousePosition);
    private void ContinueSlice() => ContinueSlice(Input.mousePosition);

}
