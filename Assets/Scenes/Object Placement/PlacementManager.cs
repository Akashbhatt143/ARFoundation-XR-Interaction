using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager), typeof(ARAnchorManager))]
public class PlacementManager : MonoBehaviour
{
    public GameObject placedPrefab;
    private ARRaycastManager arRaycastManager;
    private ARAnchorManager arAnchorManager;

    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void Awake()
    {
        arRaycastManager = GetComponent<ARRaycastManager>();
        arAnchorManager = GetComponent<ARAnchorManager>();
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Ended)
            {
                if (arRaycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
                {
                    var hitPose = hits[0].pose;
                    var anchorPoint = arAnchorManager.AddAnchor(hitPose);
                    var spawnObject = Instantiate(placedPrefab, hitPose.position, hitPose.rotation);
                    spawnObject.transform.parent = anchorPoint.transform;
                }
            }
        }
    }
}
