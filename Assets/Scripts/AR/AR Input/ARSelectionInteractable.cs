using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.AR;

namespace ARFoundationDemo
{
    public class ARSelectionInteractable : ARBaseGestureInteractable
    {
        bool m_GestureSelected;

        private float m_ScaledElevation;

        /// <summary>
        /// The visualization game object that will become active when the object is selected.
        /// </summary>        
        [SerializeField, Tooltip("The GameObject that will become active when the object is selected.")]
        GameObject m_SelectionVisualization;
        public GameObject selectionVisualization { get { return m_SelectionVisualization; } set { m_SelectionVisualization = value; } }


        /// <summary>
        /// The Unity Update() method.
        /// </summary>
        void Update()
        {

        }

        /// <summary>
        /// Determines if this interactable can be selected by a given interactor.
        /// </summary>
        /// <param name="interactor">Interactor to check for a valid selection with.</param>
        /// <returns>True if selection is valid this frame, False if not.</returns>
        public override bool IsSelectableBy(XRBaseInteractor interactor)
        {
            if (!(interactor is ARGestureInteractor))
                return false;

            return m_GestureSelected;
        }

        /// <summary>
        /// Returns true if the manipulation can be started for the given gesture.
        /// </summary>
        /// <param name="gesture">The current gesture.</param>
        /// <returns>True if the manipulation can be started.</returns>
        protected override bool CanStartManipulationForGesture(TapGesture gesture)
        {
            return true;
        }

        /// <summary>
        /// Function called when the manipulation is ended.
        /// </summary>
        /// <param name="gesture">The current gesture.</param>
        protected override void OnEndManipulation(TapGesture gesture)
        {
            if (gesture.WasCancelled)
                return;
            if (gestureInteractor == null)
                return;

            if (gesture.TargetObject == gameObject)
            {
                // Toggle selection
                m_GestureSelected = !m_GestureSelected;
            }
            else
                m_GestureSelected = false;
        }

        /// <summary>This method is called by the interaction manager 
        /// when the interactor first initiates selection of an interactable.</summary>
        /// <param name="interactor">Interactor that is initiating the selection.</param>
        protected override void OnSelectEnter(XRBaseInteractor interactor)
        {
            base.OnSelectEnter(interactor);

            if (m_SelectionVisualization != null)
                m_SelectionVisualization.SetActive(true);
        }

        /// <summary>This method is called by the interaction manager 
        /// when the interactor ends selection of an interactable.</summary>
        /// <param name="interactor">Interactor that is ending the selection.</param>
        protected override void OnSelectExit(XRBaseInteractor interactor)
        {
            base.OnSelectExit(interactor);

            if (m_SelectionVisualization != null)
                m_SelectionVisualization.SetActive(false);
        }

        /// <summary>
        /// Should be called when the object elevation changes, to make sure that the Selection
        /// Visualization remains always at the plane level. This is the elevation that the object
        /// has multiplied by the local scale in the y coordinate.
        /// </summary>
        /// <param name="scaledElevation">The current object's elevation scaled with the local y
        /// scale.</param>
        public void OnElevationChangedScaled(float scaledElevation)
        {
            m_ScaledElevation = scaledElevation;
            selectionVisualization.transform.localPosition =
                new Vector3(0, -scaledElevation / transform.localScale.y, 0);
        }

        /// <summary>
        /// Should be called when the object elevation changes, to make sure that the Selection
        /// Visualization remains always at the plane level. This is the elevation that the object
        /// has, independently of the scale.
        /// </summary>
        /// <param name="elevation">The current object's elevation.</param>
        public void OnElevationChanged(float elevation)
        {
            m_ScaledElevation = elevation * transform.localScale.y;
            selectionVisualization.transform.localPosition = new Vector3(0, -elevation, 0);
        }
    }
}
