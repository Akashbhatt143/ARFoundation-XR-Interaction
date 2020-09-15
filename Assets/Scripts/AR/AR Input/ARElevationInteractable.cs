using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.AR;

namespace ARFoundationDemo
{
    [RequireComponent(typeof(ARSelectionInteractable))]
    public class ARElevationInteractable : ARBaseGestureInteractable
    {
        /// <summary>
        /// The visualization game object that will become active when the object is Elevate.
        /// </summary>        
        [SerializeField, Tooltip("The GameObject that will become active when the object is Elevate.")]
        LineRenderer m_ElevationVisualization;
        public LineRenderer elevationVisualization { get { return m_ElevationVisualization; } set { m_ElevationVisualization = value; } }

        private Vector3 m_Origin;
        private ARSelectionInteractable arSelectionInteractable;

        private void Start()
        {
            if (arSelectionInteractable == null)
            {
                arSelectionInteractable = GetComponent<ARSelectionInteractable>();
            }
        }

        private bool IsSelected
        {
            get
            {
                if (arSelectionInteractable == null)
                {
                    arSelectionInteractable = GetComponent<ARSelectionInteractable>();
                }

                return arSelectionInteractable.IsSelectableBy(ARGestureInteractor.Instance);
            }
        }

        protected override bool CanStartManipulationForGesture(TwoFingerDragGesture gesture)
        {
            if (!IsSelected)
            {
                Debug.Log("Not selected for Elevation");
                return false;
            }

            if (gesture.TargetObject != null)
            {
                Debug.Log("Elevation object target not null");
                return false;
            }

            if (transform.parent.up != Vector3.up && transform.parent.up != Vector3.down)
            {
                // Don't allow elevation on vertical planes.
                Debug.Log("Don't allow elevation on vertical planes.");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Starts the elevation.
        /// </summary>
        /// <param name="gesture">The current gesture.</param>
        protected override void OnStartManipulation(TwoFingerDragGesture gesture)
        {
            Debug.Log("Elevation Manupulation Start");
            m_Origin = transform.localPosition;
            m_Origin.y = transform.InverseTransformPoint(transform.parent.position).y;
            m_Origin = transform.TransformPoint(m_Origin);
            OnStartElevationVisualization(m_Origin, transform.position);
        }

        /// <summary>
        /// Continues the elevation.
        /// </summary>
        /// <param name="gesture">The current gesture.</param>
        protected override void OnContinueManipulation(TwoFingerDragGesture gesture)
        {
            float elevationScale = 0.25f;

            Quaternion cameraRotation = Camera.main.transform.rotation;
            Vector3 rotatedDelta = cameraRotation * gesture.Delta;

            float elevationAmount = (rotatedDelta.y / Screen.dpi) * elevationScale;
            transform.Translate(0.0f, elevationAmount, 0.0f);

            // We cannot move it below the original position.
            if (transform.localPosition.y < transform.parent.InverseTransformPoint(m_Origin).y)
            {
                transform.position = transform.parent.TransformPoint(
                    new Vector3(
                        transform.localPosition.x,
                        transform.parent.InverseTransformPoint(m_Origin).y,
                        transform.localPosition.z));
            }

            arSelectionInteractable?.OnElevationChangedScaled(Mathf.Abs(transform.position.y - m_Origin.y));
            OnContinueElevationVisualization(transform.position);
        }

        /// <summary>
        /// Finishes the elevation.
        /// </summary>
        /// <param name="gesture">The current gesture.</param>
        protected override void OnEndManipulation(TwoFingerDragGesture gesture)
        {
            OnEndElevationVisualization();
        }


        /// <summary>
        /// Called when an ElevationManipulator manipulation is started.
        /// </summary>
        /// <param name="startPosition">The start position of the object.</param>
        /// <param name="currentPosition">The current position of the object.</param>
        private void OnStartElevationVisualization(Vector3 startPosition, Vector3 currentPosition)
        {
            if (elevationVisualization != null)
            {
                elevationVisualization.SetPosition(0, startPosition);
                elevationVisualization.SetPosition(1, currentPosition);
                elevationVisualization.enabled = true;
            }
        }

        /// <summary>
        /// Called when an ElevationManipulator manipulation is continued to a new position.
        /// </summary>
        /// <param name="currentPosition">The current position of the object.</param>
        private void OnContinueElevationVisualization(Vector3 currentPosition)
        {
            if (elevationVisualization != null)
            {
                elevationVisualization.SetPosition(1, currentPosition);
            }
        }

        /// <summary>
        /// Called when an ElevationManipulator manipulation is ended.
        /// </summary>
        private void OnEndElevationVisualization()
        {
            if (elevationVisualization != null)
            {
                elevationVisualization.enabled = false;
            }
        }
    }
}
