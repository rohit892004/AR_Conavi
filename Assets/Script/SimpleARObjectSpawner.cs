using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems;

public class SimpleARObjectSpawner : MonoBehaviour
{
    public ARRaycastManager raycastManager;
    public GameObject objectPrefab;

    private GameObject spawnedObject;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    // 🔒 LOCK after spawn
    private bool placementLocked = false;

    void Update()
    {
        // ❌ once placed, no more movement
        if (placementLocked)
            return;

        if (Input.touchCount == 0)
            return;

        Touch touch = Input.GetTouch(0);

        if (touch.phase != TouchPhase.Began)
            return;

        // 🚫 Ignore UI touches (buttons, panels)
        if (EventSystem.current != null &&
            EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            return;

        if (raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = hits[0].pose;

            spawnedObject = Instantiate(objectPrefab, hitPose.position, hitPose.rotation);

            // 🔐 LOCK placement
            placementLocked = true;
        }
    }
}
