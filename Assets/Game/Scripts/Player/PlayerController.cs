using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody), typeof(CameraWorkComponent))]
public class PlayerController : MonoBehaviourPunCallbacks, IPunObservable
{
    #region Private Fields

    [FormerlySerializedAs("cameraWork")]
    [Tooltip("Follow camera work for this player")]
    [SerializeField] private CameraWorkComponent cameraWorkComponent;

    [Header("Movement Settings")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed = 2;
    [SerializeField] private float maxSpeed = 10;
    
    [Header("Gameplay Settings")]
    [SerializeField] private Vector3 spawnPoint;
    
    #endregion

    #region Public Fields

    [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
    public static GameObject LocalPlayerInstance;

    #endregion
    
    #region MonoBehaviour Callbacks

    private void Awake()
    {
        if (photonView.IsMine)
        {
            LocalPlayerInstance = gameObject;
            LocalPlayerInstance.gameObject.name = "LocalPlayer";
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (!photonView.IsMine) return;
        cameraWorkComponent.OnStartFollowing();

    }

    private void FixedUpdate()
    {
        ProcessInputs();

        if (transform.position.y is < -100 or > 100)
        {
            Respawn();
        }
    }

    #endregion

    #region Private Methods
    
    private void ProcessInputs()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.Instance.LeaveRoom();
        }

        if (!cameraWorkComponent.CameraTransform) return;

        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");
        var force = cameraWorkComponent.CameraTransform.forward * vertical +
                    cameraWorkComponent.CameraTransform.right * horizontal;
        force.y = 0;
        rb.AddForce(force * speed);
        rb.velocity = new Vector3(
            Mathf.Clamp(rb.velocity.x, -maxSpeed, maxSpeed),
            rb.velocity.y,
            Mathf.Clamp(rb.velocity.z, -maxSpeed, maxSpeed)
        );
        
        if(Input.GetMouseButton(1)){
            cameraWorkComponent.RotateAround(Input.GetAxis("Mouse X"));
        }
    }
    
    #endregion

    #region Public Methods

    public void Respawn()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = spawnPoint;
    }

    public void SetSpawnPoint(Vector3 point)
    {
        spawnPoint = point;
    }

    #endregion

    #region IPunObservable implementation

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
    }

    #endregion
}