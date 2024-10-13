using Photon.Pun;
using UnityEngine;

public class PlayerController : MonoBehaviourPunCallbacks, IPunObservable
{
    #region Private Fields

    [Tooltip("Follow camera work for this player")]
    [SerializeField] private CameraWork cameraWork;

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
        if (cameraWork != null)
        {
            cameraWork.OnStartFollowing();
        }
        else
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> CameraWork Component on playerPrefab.", this);
        }
    }

    private void Update()
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

        if (cameraWork.CameraTransform)
        {
            float vertical = Input.GetAxis("Vertical");
            float horizontal = Input.GetAxis("Horizontal");
            var force = cameraWork.CameraTransform.forward * vertical +
                        cameraWork.CameraTransform.right * horizontal;
            force.y = 0;
            rb.AddForce(force * speed);

            rb.velocity = new Vector3(
                Mathf.Clamp(rb.velocity.x, -maxSpeed, maxSpeed),
                rb.velocity.y,
                Mathf.Clamp(rb.velocity.z, -maxSpeed, maxSpeed)
            );
        }

        if(Input.GetMouseButton(1)){
            cameraWork.RotateAround(Input.GetAxis("Mouse X"));
        }
    }
    
    #endregion
    
    #region IPunObservable implementation

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
    }

    #endregion
}