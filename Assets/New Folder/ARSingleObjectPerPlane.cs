using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems;

public class ARSingleObjectPlacer : MonoBehaviour
{
    [Header("AR")]
    public ARRaycastManager arRaycastManager;

    [Header("Prefab")]
    public GameObject objectPrefab;

    private static GameObject spawnedObject;
    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void Update()
    {
        if (Input.touchCount == 0)
            return;

        Touch touch = Input.GetTouch(0);

        if (touch.phase != TouchPhase.Began)
            return;

        // 🚫 Ignore UI touches (VERY IMPORTANT)
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            return;

        if (arRaycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = hits[0].pose;

            PlaceOrMove(hitPose.position, hitPose.rotation);
        }
    }

    void PlaceOrMove(Vector3 position, Quaternion rotation)
    {
        // ✅ FIRST TIME → Spawn
        if (spawnedObject == null)
        {
            spawnedObject = Instantiate(objectPrefab, position, rotation);
        }
        // ✅ NEXT TIMES → MOVE ONLY (NO DELETE)
        else
        {
            spawnedObject.transform.SetPositionAndRotation(position, rotation);
        }
    }
}
