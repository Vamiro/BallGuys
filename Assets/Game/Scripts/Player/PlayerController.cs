using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;

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
        if (cameraWorkComponent != null)
        {
            cameraWorkComponent.OnStartFollowing();
        }
        else
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> CameraWork Component on playerPrefab.", this);
        }
    }

    private void FixedUpdate()
    {
        ProcessInputs();
    }

    #endregion

    #region Private Methods
    
    private void ProcessInputs()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.Instance.LeaveRoom();
        }

        if (cameraWorkComponent.CameraTransform)
        {
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
        }

        if(Input.GetMouseButton(1)){
            cameraWorkComponent.RotateAround(Input.GetAxis("Mouse X"));
        }
    }
    
    #endregion
    
    #region IPunObservable implementation

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
    }

    #endregion
}