using UnityEngine;

public class CameraWorkComponent : MonoBehaviour
{
    [Tooltip("The distance in the local x-z plane to the target")]
    [SerializeField]
    private float distance = 7.0f;

    [Tooltip("The height we want the camera to be above the target")]
    [SerializeField]
    private float height = 3.0f;

    [Tooltip("Allow the camera to be offseted vertically from the target, for example giving more view of the sceneray and less ground.")]
    [SerializeField]
    private Vector3 centerOffset = Vector3.zero;

    [Tooltip("Set this as false if a component of a prefab being instanciated by Photon Network, and manually call OnStartFollowing() when and if needed.")]
    [SerializeField]
    private bool followOnStart = false;

    private Transform _cameraTransform;
    private Vector3 _cameraOffset = Vector3.zero;
    private bool _isFollowing;

    public Transform CameraTransform => _cameraTransform;
    
    void Start()
    {
        if (followOnStart)
        {
            OnStartFollowing();
        }
    }
    
    void LateUpdate()
    {
        if (_cameraTransform == null && _isFollowing)
        {
            OnStartFollowing();
        }

        if (_isFollowing) {
            Follow ();
        }
    }

    public void OnStartFollowing()
    {
        _cameraTransform = Camera.main.transform;
        _isFollowing = true;
        Follow();
    }
        
    /// <summary>
    /// Rotate the camera around the target based on mouse movement in the X direction
    /// </summary>
    /// <param name="mouseX">Mouse movement in the X direction</param>
    public void RotateAround(float mouseX)
    {
        if (_cameraTransform)
        {
            _cameraTransform.RotateAround(this.transform.position, Vector3.up, mouseX);
            _cameraOffset = Quaternion.Euler(0, mouseX, 0) * _cameraOffset;
        }
    }

    /// <summary>
    /// Follow the target
    /// </summary>
    void Follow()
    {
        if (_cameraOffset == Vector3.zero)
        {
            _cameraOffset.z = -distance;
            _cameraOffset.y = height;
        }

        _cameraTransform.position = this.transform.position + _cameraOffset;
        _cameraTransform.LookAt(this.transform.position + centerOffset);
    }
}
