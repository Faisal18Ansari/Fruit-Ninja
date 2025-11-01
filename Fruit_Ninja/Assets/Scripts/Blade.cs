using UnityEngine;

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
        Vector3 position = mainCamera.ScreenToWorldPoint(screenPosition);
        position.z = 0f;
        transform.position = position;
        slicing = true;
        if (bladeCollider != null) bladeCollider.enabled = true;
        if (trail != null) { trail.Clear(); trail.enabled = true; }
    }
    private void EndSlice()
    {
        slicing = false;
        if (bladeCollider != null) bladeCollider.enabled = false;
        if (trail != null) trail.enabled = false;
    }
    private void ContinueSlice(Vector3 screenPosition)
    {
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
