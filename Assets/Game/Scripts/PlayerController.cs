using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Scripts
{
    public class PlayerController : MonoBehaviourPunCallbacks, IPunObservable
    {
        #region Private Fields

        [Tooltip("Follow camera work for this player")]
        [SerializeField]
        private CameraWork cameraWork;

        [Header("Movement Settings")]
        public Rigidbody rb;
        public float speed = 2;
        public float maxSpeed = 10;
        
        #endregion

        #region Public Fields

        [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
        public static GameObject LocalPlayerInstance;

        #endregion
        
        #region MonoBehaviour Callbacks

        private void Awake()
        {
            // #Important
            // used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
            if (photonView.IsMine)
            {
                PlayerController.LocalPlayerInstance = this.gameObject;
            }
            // #Critical
            // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
            DontDestroyOnLoad(this.gameObject);
        }

        private void Start()
        {
            if (cameraWork != null)
            {
                if (photonView.IsMine)
                {
                    cameraWork.OnStartFollowing();
                }
            }
            else
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> CameraWork Component on playerPrefab.", this);
            }
        }

        private void Update()
        {
            if (photonView.IsMine)
            {
                ProcessInputs();
            }
            
        }

        #endregion

        #region Private Methods
        
        void ProcessInputs()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameManager.Instance.LeaveRoom();
            }
            
            float vertical = Input.GetAxis("Vertical");
            float horizontal = Input.GetAxis("Horizontal");
            var force = cameraWork.CameraTransform.forward * vertical + cameraWork.CameraTransform.right * horizontal;
            force.y = 0;
            rb.AddForce(force);
            
            rb.velocity = new Vector3(
                Mathf.Clamp(rb.velocity.x, -maxSpeed, maxSpeed),
                rb.velocity.y,
                Mathf.Clamp(rb.velocity.z, -maxSpeed, maxSpeed)
            );
            
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
}