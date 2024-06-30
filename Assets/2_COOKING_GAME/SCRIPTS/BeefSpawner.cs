using Imagine.WebAR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeefSpawner : MonoBehaviour
{
    public GameObject karubiPrefab; // Assign the Karubi beef prefab in the inspector
    public GameObject sirloinPrefab; // Assign the Sirloin beef prefab in the inspector


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

    public void SpawnKarubi()
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
            Debug.Log("Cannot spawn Karubi: There are beef objects within the spawn area.");
            return;
        }

        // Instantiate the Karubi prefab at the spawn point position and rotation
        GameObject newKarubi = Instantiate(karubiPrefab, spawnPoint.position, spawnPoint.rotation);

        // Get the LetMeCook component from the child of the instantiated Karubi prefab
        KarubiBeef karubiScript = newKarubi.GetComponent<KarubiBeef>();

        if (karubiScript != null)
        {
            // Log that the karubiScript component was successfully obtained
            Debug.Log("Successfully obtained LetMeCook component from newKarubi.");

            // Assign the ARCamera to the karubiScript


        }
        else
        {
            // Log a warning if the LetMeCook component could not be found
            Debug.LogWarning("Could not find LetMeCook component on newKarubi.");
        }

        Debug.Log($"Karubi spawned at: {spawnPoint.position}");
    }

    public void SpawnSirloin()
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
            Debug.Log("Cannot spawn Sirloin: There are beef objects within the spawn area.");
            return;
        }

        // Instantiate the Sirloin prefab at the spawn point position and rotation
        GameObject newSirloin = Instantiate(sirloinPrefab, spawnPoint.position, spawnPoint.rotation);


        // Get the LetMeCook component from the child of the instantiated Sirloin prefab
        SirloinBeef sirloinScript = newSirloin.GetComponent<SirloinBeef>();

        if (sirloinScript != null)
        {
            // Log that the sirloinScript component was successfully obtained
            Debug.Log("Successfully obtained LetMeCook component from newSirloin.");


        }
        else
        {
            // Log a warning if the LetMeCook component could not be found
            Debug.LogWarning("Could not find LetMeCook component on newSirloin.");
        }

        Debug.Log($"Sirloin spawned at: {spawnPoint.position}");
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

    // Method to clear all spawned beef from the scene
    public void ClearBeef()
    {
        GameObject[] karubiObjects = GameObject.FindGameObjectsWithTag("Karubi");
        foreach (GameObject karubi in karubiObjects)
        {
            Destroy(karubi);
        }

        GameObject[] sirloinObjects = GameObject.FindGameObjectsWithTag("Sirloin");
        foreach (GameObject sirloin in sirloinObjects)
        {
            Destroy(sirloin);
        }

        Debug.Log("All spawned beef (Karubi and Sirloin) has been cleared from the scene.");
    }



}
