using UnityEngine;

namespace ARFoundationDemo
{
    [RequireComponent(typeof(ARFeatheredPlaneMeshVisualizer))]
    public class ARShadowPlaneMeshCaster : MonoBehaviour
    {
        ARFeatheredPlaneMeshVisualizer a_ARFeatheredPlaneMeshVisualizer;
        ARShadowVisualizer a_ARShadowVisualizer;

        private void Awake()
        {
            a_ARFeatheredPlaneMeshVisualizer = GetComponent<ARFeatheredPlaneMeshVisualizer>();
            a_ARShadowVisualizer = FindObjectOfType<ARShadowVisualizer>();
        }

        private void OnEnable()
        {
            a_ARFeatheredPlaneMeshVisualizer.meshChanged += OnMeshChanged;
        }

        private void OnDisable()
        {
            a_ARFeatheredPlaneMeshVisualizer.meshChanged += OnMeshChanged;
        }

        private void OnMeshChanged(Mesh mesh)
        {
            if (a_ARShadowVisualizer != null)
            {
                a_ARShadowVisualizer.OnUpdateMesh(mesh);
            }
        }
    }
}
