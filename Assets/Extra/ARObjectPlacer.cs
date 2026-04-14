using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARObjectPlacer : MonoBehaviour
{
    [Header("AR Components")]
    [SerializeField] private ARRaycastManager arRaycastManager;
    [SerializeField] private ARPlaneManager arPlaneManager;
    [SerializeField] private Camera arCamera;
    
    [Header("Object Settings")]
    [SerializeField] private GameObject[] objectPrefabs;
    [SerializeField] private bool cycleThroughObjects = true;
    [SerializeField] private float placementCooldown = 0.5f;
    
    // Private fields
    private List<ARRaycastHit> raycastHits = new List<ARRaycastHit>();
    private List<GameObject> placedObjects = new List<GameObject>();
    private Camera playerCamera;
    private float lastPlacementTime;
    private bool isPlacementReady = false;
    private int currentObjectIndex = 0;
    
    
}