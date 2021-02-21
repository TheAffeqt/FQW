// %BANNER_BEGIN%
// ---------------------------------------------------------------------
// %COPYRIGHT_BEGIN%
//
// Copyright (c) 2018-present, Magic Leap, Inc. All Rights Reserved.
// Use of this file is governed by the Creator Agreement, located
// here: https://id.magicleap.com/creator-terms
//
// %COPYRIGHT_END%
// ---------------------------------------------------------------------
// %BANNER_END%

using UnityEngine;
using UnityEngine.XR.MagicLeap;
using UnityEngine.Events;

namespace MagicLeap
{
    /// <summary>
    /// This class allows the user to cycle between various PlacementContent
    /// objects and see a visual representation of a valid or invalid location.
    /// Once a valid location has been determined the user may place the content.
    /// </summary>
    [RequireComponent(typeof(Placement))]
    public class PlacementExample : MonoBehaviour
    {
        #region Private Variables
        [SerializeField, Tooltip("The controller that is used in the scene to cycle and place objects.")]
        private ControllerConnectionHandler _controllerConnectionHandler = null;

        [SerializeField, Tooltip("The placement objects that are used in the scene.")]
        private GameObject[] _placementGameObjects = null;

        private Placement _placement = null;
        private PlacementObject _placementObject = null;
        private int _placementIndex = 0;

        public UnityEvent onDestroy;
        #endregion

        #region Unity Methods
        void Start()
        {
            if (_controllerConnectionHandler == null)
            {
                Debug.LogError("Error: PlacementExample._controllerConnectionHandler is not set, disabling script.");
                enabled = false;
                return;
            }

            _placement = GetComponent<Placement>();

            //MLInput.OnControllerButtonDown += HandleOnButtonDown;
            MLInput.OnTriggerDown += HandleOnTriggerDown;

            StartPlacement();
        }

        void Update()
        {
            // Update the preview location, inside of the validation area.
            if (_placementObject != null)
            {
                _placementObject.transform.position = _placement.AdjustedPosition - _placementObject.LocalBounds.center;
                _placementObject.transform.rotation = _placement.Rotation;
            }

            if(Input.GetKeyDown(KeyCode.N))
            {
                NextPlacementObject();
            }
        }

        void OnDestroy()
        {
            //MLInput.OnControllerButtonDown -= HandleOnButtonDown;
            MLInput.OnTriggerDown -= HandleOnTriggerDown;
            onDestroy.Invoke();
        }
        #endregion

        #region Event Handlers
        /// <summary>
        /// Handles the event for button down.
        /// </summary>
        /// <param name="controller_id">The id of the controller.</param>
        /// <param name="button">The button that is being pressed.</param>

        private void HandleOnTriggerDown(byte controllerId, float pressure)
        {
            _placement.Confirm();
        }

        private void HandlePlacementComplete(Vector3 position, Quaternion rotation)
        {
            if (_placementGameObjects != null && _placementGameObjects.Length > _placementIndex)
            {
                GameObject content = _placementGameObjects[_placementIndex];

                content.transform.position = _placement.AdjustedPosition - _placementObject.LocalBounds.center;
                content.transform.rotation = rotation;
                content.gameObject.SetActive(true);

                try
                {
                    DeployWrapper objToShift = content.GetComponent<DeployWrapper>();
                    objToShift.UnWrap();
                }
                catch
                {
                    print("DeployUnWrapError");
                }
                
                _placement.Resume();

                NextPlacementObject();
            }
        }
        #endregion

        #region Private Methods
        private PlacementObject CreatePlacementObject(int index = 0)
        {
            // Destroy previous preview instance
            if (_placementObject != null)
            {
                Destroy(_placementObject.gameObject);
            }

            // Create the next preview instance.
            if (_placementGameObjects != null && _placementGameObjects.Length > index)
            {
                GameObject previewObject = Instantiate(_placementGameObjects[index]);

                // Detect all children in the preview and set children to ignore raycast.
                Collider[] colliders = previewObject.GetComponents<Collider>();
                for (int i = 0; i < colliders.Length; ++i)
                {
                    colliders[i].enabled = false;
                }

                // Find the placement object.
                PlacementObject placementObject = previewObject.GetComponent<PlacementObject>();

                if (placementObject == null)
                {
                    Destroy(previewObject);
                    Debug.LogError("Error: PlacementExample.placementObject is not set, disabling script.");

                    enabled = false;
                }

                return placementObject;
            }

            return null;
        }

        private void StartPlacement()
        {
            _placementObject = CreatePlacementObject(_placementIndex);

            if (_placementObject != null)
            {
                _placement.Cancel();
                _placement.Place(_controllerConnectionHandler.transform, _placementObject.Volume, _placementObject.AllowHorizontal, _placementObject.AllowVertical, HandlePlacementComplete);
            }
        }

        public void NextPlacementObject()
        {
            if (_placementGameObjects != null)
            {
                _placementIndex++;
                if (_placementIndex >= _placementGameObjects.Length)
                {
                    if (_placementObject != null)
                    {
                        Destroy(_placementObject.gameObject);
                    }
                    Destroy(transform.parent.gameObject);
                }
            }

            StartPlacement();
        }
        #endregion
    }
}
