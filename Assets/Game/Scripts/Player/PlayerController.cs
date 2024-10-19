using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.Animations;
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
    
    [Header("Player Info")]
    [SerializeField] private TMP_Text nicknameText;
    
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
        UpdateNickname();
        
        if (!photonView.IsMine) return;
        cameraWorkComponent.OnStartFollowing();
    }

    private void UpdateNickname()
    {
        if (!nicknameText) return;
        nicknameText.text = photonView.Owner.NickName;
        if (Camera.main == null) return;
        nicknameText.transform.rotation = Quaternion.Euler(0, 180, 0);
        nicknameText.canvas.worldCamera = Camera.main;
        nicknameText.canvas.GetComponent<LookAtConstraint>().AddSource(new ConstraintSource
        {
            sourceTransform = Camera.main.transform,
            weight = 1
        });
        nicknameText.canvas.GetComponent<LookAtConstraint>().constraintActive = true;
    }

    private void FixedUpdate()
    {
        ProcessInputs();
        nicknameText.canvas.transform.position = transform.position + Vector3.up;
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


    // Called by PUN, when this game object has been network instantiated with PhotonNetwork.Instantiate
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }

    #endregion
}