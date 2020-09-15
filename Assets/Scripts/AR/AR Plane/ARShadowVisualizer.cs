using UnityEngine;

namespace ARFoundationDemo
{
    public class ARShadowVisualizer : MonoBehaviour
    {
        MeshRenderer a_MeshRenderer;
        MeshFilter a_MeshFilter;

        private const string sortingLayer = "ShadowPlane";
        private const int sortingId = 1;

        private void Awake()
        {
            a_MeshFilter = GetComponent<MeshFilter>();
            a_MeshRenderer = GetComponent<MeshRenderer>();
        }

        private void Start()
        {
            if (a_MeshRenderer != null)
            {
                a_MeshRenderer.sortingLayerID = sortingId;
                a_MeshRenderer.sortingLayerName = sortingLayer;
            }
        }

        public void OnUpdateMesh(Mesh mesh)
        {
            if (a_MeshFilter != null)
            {
                a_MeshFilter.sharedMesh = mesh;
            }
        }
    }
}
