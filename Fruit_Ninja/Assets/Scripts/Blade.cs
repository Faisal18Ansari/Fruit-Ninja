using UnityEngine;

public class Blade : MonoBehaviour
{
    public float sliceForce = 5f;
    public float minSliceVelocity = 0.01f;
    private Camera camera;
    private Collider collider;
    private TrailRenderer trail;
    public Vector3 direction { get; private set; }
    public bool slicing { get; private set; }
    private void Awake()
    {
        camera = Camera.main;
        collider = GetComponent<Collider>();
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
                case TouchPhase.Begun:
                    StartSLice();
                    Break;
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    EndSlice();
                    Break;
            }
        }
#endif
    }

    private void StartSlice(Vector3 screenPosition)
    {
        Vector3 position = camera.ScreenToWorldPoint(screenPosition);
        position.z = 0f;
        transform.position = position;
        slicing = true;
        collider.enabled = true;
        trail.Clear();
        trail.enabled = true;
    }
    private void EndSlice()
    {
        slicing = false;
        collider.enabled = false;
        trail.enabled = false;
    }
    private void ContinueSlice(Vector3 screenPosition)
    {
        Vector3 newPosition = camera.ScreenToWorldPoint(screenPosition);
        newPosition.z = 0f;
        direction = newPosition - transform.position;
        float velocity = direction.magnitude / Time.deltaTime;
        collider.enabled = velocity > minSliceVelocity;
        transform.position = newPosition;
    }
    private void StartSlice() => StartSlice(Input.mousePosition);
    private void ContinueSlice() => ContinueSlice(Input.mousePosition);

}
