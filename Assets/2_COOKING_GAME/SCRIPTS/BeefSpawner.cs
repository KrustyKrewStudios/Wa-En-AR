using Imagine.WebAR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeefSpawner : MonoBehaviour
{
    public GameObject karubiPrefab; 
    public GameObject sirloinPrefab; 

    public Transform spawnPoint; // Assign the spawn point in the inspector

    public Transform arCameraTransform; // Public variable for AR camera transform


    private bool isBeefSpawned = false; // Flag to track if a beef is currently spawned

    public BoxCollider spawnCollider; // Reference to the BoxCollider component

    public Transform[] grillSpots; // Assign multiple grill spots in the inspector
    public float dropHeight = 2.0f; // Height from which the beef drops
    private int nextSpotIndex = 0; // Index to track the next available spot

    void Start()
    {
        // Get the BoxCollider component from the spawn point
        spawnCollider = spawnPoint.GetComponent<BoxCollider>();

        if (spawnCollider == null)
        {
            Debug.LogError("No BoxCollider found on the spawn point!");
        }

        if (grillSpots == null || grillSpots.Length == 0)
        {
            Debug.LogError("No grill spots assigned!");
        }


    }

    public void SpawnKarubi()
    {
        if (nextSpotIndex >= grillSpots.Length)
        {
            Debug.Log("All grill spots are occupied.");
            return;
        }

        Transform spawnSpot = grillSpots[nextSpotIndex];
        Vector3 spawnPosition = new Vector3(spawnSpot.position.x, spawnSpot.position.y + dropHeight, spawnSpot.position.z);

        GameObject newKarubi = Instantiate(karubiPrefab, spawnPosition, Quaternion.identity);
        StartCoroutine(DropToGrill(newKarubi, spawnSpot.position));

        nextSpotIndex++;
    }

    public void SpawnSirloin()
    {
        if (nextSpotIndex >= grillSpots.Length)
        {
            Debug.Log("All grill spots are occupied.");
            return;
        }

        Transform spawnSpot = grillSpots[nextSpotIndex];
        Vector3 spawnPosition = new Vector3(spawnSpot.position.x, spawnSpot.position.y + dropHeight, spawnSpot.position.z);

        GameObject newSirloin = Instantiate(sirloinPrefab, spawnPosition, Quaternion.identity);
        StartCoroutine(DropToGrill(newSirloin, spawnSpot.position));

        nextSpotIndex++;
    }

    private IEnumerator DropToGrill(GameObject beef, Vector3 targetPosition)
    {
        float duration = 1.0f; // Duration of the drop animation
        Vector3 startPosition = beef.transform.position;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            beef.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        beef.transform.position = targetPosition;
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
