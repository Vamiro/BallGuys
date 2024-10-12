using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWork : MonoBehaviour
{
    #region Private Fields

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

        // [Tooltip("The Smoothing for the camera to follow the target")]
        // [SerializeField]
        // private float smoothSpeed = 0.125f;

        Transform cameraTransform;
        bool isFollowing;

        // Cache for camera offset
        Vector3 cameraOffset = Vector3.zero;
        Vector3 initialCameraOffset = Vector3.zero;

        #endregion

        #region Public Fields
        
        public Transform CameraTransform
        {
            get { return cameraTransform; }
        }

        #endregion

        #region MonoBehaviour Callbacks

        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during initialization phase
        /// </summary>
        void Start()
        {
            // Start following the target if wanted.
            if (followOnStart)
            {
                OnStartFollowing();
            }
        }


        void LateUpdate()
        {
            // The transform target may not destroy on level load,
            // so we need to cover corner cases where the Main Camera is different everytime we load a new scene, and reconnect when that happens
            if (cameraTransform == null && isFollowing)
            {
                OnStartFollowing();
            }

            // only follow is explicitly declared
            if (isFollowing) {
                Follow ();
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Raises the start following event.
        /// Use this when you don't know at the time of editing what to follow, typically instances managed by the photon network.
        /// </summary>
        public void OnStartFollowing()
        {
            cameraTransform = Camera.main.transform;
            isFollowing = true;
            Cut();
        }
        
        /// <summary>
        /// Rotate the camera around the target based on mouse movement in the X direction
        /// </summary>
        /// <param name="mouseX">Mouse movement in the X direction</param>
        public void RotateAround(float mouseX)
        {
            if (cameraTransform)
            {
                cameraTransform.RotateAround(this.transform.position, Vector3.up, mouseX);
                cameraOffset = Quaternion.Euler(0, mouseX, 0) * cameraOffset;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Follow the target smoothly
        /// </summary>
        void Follow()
        {
            if (cameraOffset == Vector3.zero)
            {
                cameraOffset.z = -distance;
                cameraOffset.y = height;
            }

            //cameraTransform.position = Vector3.Lerp(cameraTransform.position, this.transform.position + cameraOffset, smoothSpeed * Time.deltaTime);
            cameraTransform.position = this.transform.position + cameraOffset;
            cameraTransform.LookAt(this.transform.position + centerOffset);
        }


        void Cut()
        {
            cameraOffset.z = -distance;
            cameraOffset.y = height;

            cameraTransform.position = this.transform.position + cameraOffset;
            cameraTransform.LookAt(this.transform.position + centerOffset);
        }
        #endregion
}
