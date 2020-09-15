using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.Interaction.Toolkit.AR;

namespace ARFoundationDemo
{
    public class ARSpawnInteractable : ARBaseGestureInteractable
    {
        /// <summary>
        /// Camera being used to render the passthrough camera image.
        /// </summary>
        public Camera ARCamera;

        /// <summary>
        /// A prefab to place when a raycast from a user touch hits a plane.
        /// </summary>
        public GameObject PlacementObjectPrefab;

        /// <summary>
        /// InteractablePrefab to attach placed objects to.
        /// </summary>
        public GameObject InteractablePrefab;

        private ARAnchorManager _arAnchorManager;

        protected ARAnchorManager ArAnchorManager
        {
            get
            {
                if (_arAnchorManager == null)
                {
                    _arAnchorManager = GetComponent<ARAnchorManager>();
                }

                return _arAnchorManager;
            }
        }

        private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

        #region PublicMethods

        private bool CheckDependantManagers()
        {
            if (ArAnchorManager == null)
            {
                return false;
            }

            return true;
        }

        protected override bool CanStartManipulationForGesture(TapGesture gesture)
        {
            if (gesture.TargetObject == null)
            {
                return true;
            }

            return false;
        }

        protected override void OnEndManipulation(TapGesture gesture)
        {
            if (gesture.WasCancelled)
            {
                return;
            }

            // If gesture is targeting an existing object we are done.
            if (gesture.TargetObject != null)
            {
                return;
            }

            if (GestureTransformationUtility.Raycast(gesture.StartPosition, hits, TrackableType.Planes))
            {
                var hit = hits[0];

                // Use hit pose and camera pose to check if hittest is from the
                // back of the plane, if it is, no need to create the anchor.
                if (Vector3.Dot(ARCamera.transform.position - hit.pose.position, hit.pose.rotation * Vector3.up) < 0)
                {
                    return;
                }

                // Spawn main object
                var spawnObject = Instantiate(PlacementObjectPrefab, hit.pose.position, hit.pose.rotation);

                // Spawn interactable for spawn object
                var interactableObject = Instantiate(InteractablePrefab, hit.pose.position, hit.pose.rotation);

                // Make game object a child of the interactable.
                spawnObject.transform.parent = interactableObject.transform;

                if (CheckDependantManagers())
                {
                    // Create an referencePoint to allow ARCore to track the hitpoint as understanding of
                    // the physical world evolves.
                    var anchorPoint = ArAnchorManager.AddAnchor(hit.pose);

                    //TODO: Save list of referencePoint and add,update or remove on ArAnchorManager's event anchorsChanged

                    // Make manipulator a child of the anchor.
                    interactableObject.transform.parent = anchorPoint.transform;
                }
            }
        }

        #endregion /PublicMethods

    }
}
