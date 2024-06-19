using Imagine.WebAR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeefSpawner : MonoBehaviour
{
    public GameObject beefPrefab; // Assign the beef prefab in the inspector
    public Transform spawnPoint; // Assign the spawn point in the inspector

    public Transform arCameraTransform; // Public variable for AR camera transform

    public ARCamera ARCamera;

    private bool isBeefSpawned = false; // Flag to track if a beef is currently spawned

    public BoxCollider spawnCollider; // Reference to the BoxCollider component

    void Start()
    {
        // Get the BoxCollider component from the spawn point
        spawnCollider = spawnPoint.GetComponent<BoxCollider>();

        if (spawnCollider == null)
        {
            Debug.LogError("No BoxCollider found on the spawn point!");
        }
    }

    public void SpawnBeef()
    {
        // Check for colliders within the spawn collider
        Collider[] colliders = Physics.OverlapBox(spawnCollider.bounds.center, spawnCollider.bounds.extents, Quaternion.identity);

        // Filter colliders to check for "Beef"
        bool foundBeef = false;
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Beef"))
            {
                foundBeef = true;
                break;
            }
        }

        // If there are beef colliders inside the spawn collider, do not spawn a new beef
        if (foundBeef)
        {
            Debug.Log("Cannot spawn beef: There are beef objects within the spawn area.");
            return;
        }

        // If no beef colliders are detected, instantiate the beef prefab at the spawn point position and rotation
        GameObject newBeef = Instantiate(beefPrefab, spawnPoint.position, spawnPoint.rotation);

        // Get the TwoFingerPan component from the instantiated beef prefab
        TwoFingerPan panScript = newBeef.GetComponent<TwoFingerPan>();
        if (panScript != null)
        {
            // Assign the AR camera's transform to the cam variable in TwoFingerPan
            panScript.cam = arCameraTransform;
        }

        // Get the LetMeCook component from the child of the instantiated beef prefab
        LetMeCook beefScript = newBeef.GetComponentInChildren<LetMeCook>();

        if (beefScript != null)
        {
            // Log that the beefScript component was successfully obtained
            Debug.Log("Successfully obtained LetMeCook component from newBeef.");

            // Assign the ARCamera to the beefScript
            beefScript.ARCamera = ARCamera;

            // Log the ARCamera assignment
            if (ARCamera != null)
            {
                Debug.Log($"ARCamera successfully assigned to beefScript: {ARCamera.name}");
            }
            else
            {
                Debug.LogWarning("ARCamera is null, cannot assign to beefScript.");
            }
        }
        else
        {
            // Log a warning if the LetMeCook component could not be found
            Debug.LogWarning("Could not find LetMeCook component on newBeef.");
        }


        Debug.Log("Beef spawned at: " + spawnPoint.position);
    }

    // Visualize the spawn area in the Scene view for debugging
    private void OnDrawGizmos()
    {
        if (spawnCollider != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(spawnCollider.bounds.center, spawnCollider.bounds.size);
        }
    }
}
