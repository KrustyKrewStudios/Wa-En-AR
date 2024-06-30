using Imagine.WebAR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeefSpawner : MonoBehaviour
{
    public GameObject karubiPrefab; 
    public GameObject sirloinPrefab; 



    public Transform[] grillSpots; // Assign multiple grill spots in the inspector
    public Transform[] servingPlateSpots; // Assign multiple serving plate spots in the inspector

    public float dropHeight = 2.0f; // Height from which the beef drops
    private int nextGrillSpotIndex = 0; // Index to track the next available grill spot
    private int nextServingSpotIndex = 0; // Index to track the next available serving spot

    private GameObject selectedBeef; // Currently selected beef


    void Start()
    {

        if (grillSpots == null || grillSpots.Length == 0)
        {
            Debug.LogError("No grill spots assigned!");
        }

        if (servingPlateSpots == null || servingPlateSpots.Length == 0)
        {
            Debug.LogError("No serving plate spots assigned!");
        }

    }

    public void SpawnKarubi()
    {
        if (nextGrillSpotIndex >= grillSpots.Length)
        {
            Debug.Log("All grill spots are occupied.");
            return;
        }

        Transform spawnSpot = grillSpots[nextGrillSpotIndex];
        Vector3 spawnPosition = new Vector3(spawnSpot.position.x, spawnSpot.position.y + dropHeight, spawnSpot.position.z);

        GameObject newKarubi = Instantiate(karubiPrefab, spawnPosition, Quaternion.identity);
        StartCoroutine(DropToGrill(newKarubi, spawnSpot.position));

        nextGrillSpotIndex++;
    }

    public void SpawnSirloin()
    {
        if (nextGrillSpotIndex >= grillSpots.Length)
        {
            Debug.Log("All grill spots are occupied.");
            return;
        }

        Transform spawnSpot = grillSpots[nextGrillSpotIndex];
        Vector3 spawnPosition = new Vector3(spawnSpot.position.x, spawnSpot.position.y + dropHeight, spawnSpot.position.z);

        GameObject newSirloin = Instantiate(sirloinPrefab, spawnPosition, Quaternion.identity);
        StartCoroutine(DropToGrill(newSirloin, spawnSpot.position));

        nextGrillSpotIndex++;
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
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // For mouse click or tap on screen
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                GameObject hitObject = hit.transform.gameObject;
                if (hitObject.CompareTag("Karubi") || hitObject.CompareTag("Sirloin"))
                {
                    selectedBeef = hitObject;
                    Debug.Log("Selected beef: " + selectedBeef.name);
                }
            }
        }

        if (selectedBeef != null && Input.GetKeyDown(KeyCode.Space)) // Space key to move beef (for testing)
        {
            MoveSelectedBeef();
        }
    }

    public void MoveSelectedBeef()
    {
        if (selectedBeef == null) return;

        if (selectedBeef.transform.parent == null || selectedBeef.transform.parent.CompareTag("Grill"))
        {
            if (nextServingSpotIndex >= servingPlateSpots.Length)
            {
                Debug.Log("All serving spots are occupied.");
                return;
            }

            Transform servingSpot = servingPlateSpots[nextServingSpotIndex];
            StartCoroutine(MoveBeef(selectedBeef, servingSpot.position));
            nextServingSpotIndex++;
        }
        else if (selectedBeef.transform.parent.CompareTag("ServingPlate"))
        {
            if (nextGrillSpotIndex >= grillSpots.Length)
            {
                Debug.Log("All grill spots are occupied.");
                return;
            }

            Transform grillSpot = grillSpots[nextGrillSpotIndex];
            StartCoroutine(MoveBeef(selectedBeef, grillSpot.position));
            nextGrillSpotIndex++;
        }

        selectedBeef = null; // Deselect the beef after moving
    }

    private IEnumerator MoveBeef(GameObject beef, Vector3 targetPosition)
    {
        float duration = 0.5f; // Duration of the move animation
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


}
