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

    private GameObject selectedBeef; // Currently selected beef

    private bool[] grillSpotOccupied;
    private bool[] servingSpotOccupied;

    private Dictionary<GameObject, int> beefSpotIndexMap; // Maps beef objects to their spot indices

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

        grillSpotOccupied = new bool[grillSpots.Length];
        servingSpotOccupied = new bool[servingPlateSpots.Length];

        beefSpotIndexMap = new Dictionary<GameObject, int>();
    }

    public void SpawnKarubi()
    {
        int nextGrillSpotIndex = GetNextAvailableSpot(grillSpotOccupied);

        if (nextGrillSpotIndex == -1)
        {
            Debug.Log("All grill spots are occupied.");
            return;
        }

        Transform spawnSpot = grillSpots[nextGrillSpotIndex];
        Vector3 spawnPosition = new Vector3(spawnSpot.position.x, spawnSpot.position.y + dropHeight, spawnSpot.position.z);

        GameObject newKarubi = Instantiate(karubiPrefab, spawnPosition, Quaternion.identity);
        StartCoroutine(DropToGrill(newKarubi, spawnSpot.position));

        grillSpotOccupied[nextGrillSpotIndex] = true;
        beefSpotIndexMap[newKarubi] = nextGrillSpotIndex; // Map beef to grill spot index
    }

    public void SpawnSirloin()
    {
        int nextGrillSpotIndex = GetNextAvailableSpot(grillSpotOccupied);

        if (nextGrillSpotIndex == -1)
        {
            Debug.Log("All grill spots are occupied.");
            return;
        }

        Transform spawnSpot = grillSpots[nextGrillSpotIndex];
        Vector3 spawnPosition = new Vector3(spawnSpot.position.x, spawnSpot.position.y + dropHeight, spawnSpot.position.z);

        GameObject newSirloin = Instantiate(sirloinPrefab, spawnPosition, Quaternion.identity);
        StartCoroutine(DropToGrill(newSirloin, spawnSpot.position));

        grillSpotOccupied[nextGrillSpotIndex] = true;
        beefSpotIndexMap[newSirloin] = nextGrillSpotIndex; // Map beef to grill spot index
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

        // Reset the indices after clearing the beef
        for (int i = 0; i < grillSpotOccupied.Length; i++)
        {
            grillSpotOccupied[i] = false;
        }

        for (int i = 0; i < servingSpotOccupied.Length; i++)
        {
            servingSpotOccupied[i] = false;
        }

        beefSpotIndexMap.Clear(); // Clear the map
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

        if (beefSpotIndexMap.TryGetValue(selectedBeef, out int currentSpotIndex))
        {
            if (selectedBeef.transform.parent == null || selectedBeef.transform.parent.CompareTag("Grill"))
            {
                int nextServingSpotIndex = GetNextAvailableSpot(servingSpotOccupied);

                if (nextServingSpotIndex == -1)
                {
                    Debug.Log("All serving spots are occupied.");
                    return;
                }

                Transform servingSpot = servingPlateSpots[nextServingSpotIndex];
                StartCoroutine(MoveBeef(selectedBeef, servingSpot.position));

                servingSpotOccupied[nextServingSpotIndex] = true;
                grillSpotOccupied[currentSpotIndex] = false;
                beefSpotIndexMap[selectedBeef] = nextServingSpotIndex; // Update the map to serving spot index
            }
            else if (selectedBeef.transform.parent.CompareTag("ServingPlate"))
            {
                int nextGrillSpotIndex = GetNextAvailableSpot(grillSpotOccupied);

                if (nextGrillSpotIndex == -1)
                {
                    Debug.Log("All grill spots are occupied.");
                    return;
                }

                Transform grillSpot = grillSpots[nextGrillSpotIndex];
                StartCoroutine(MoveBeef(selectedBeef, grillSpot.position));

                grillSpotOccupied[nextGrillSpotIndex] = true;
                servingSpotOccupied[currentSpotIndex] = false;
                beefSpotIndexMap[selectedBeef] = nextGrillSpotIndex; // Update the map to grill spot index
            }

            selectedBeef = null; // Deselect the beef after moving
        }
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

    private int GetNextAvailableSpot(bool[] spotOccupiedArray)
    {
        for (int i = 0; i < spotOccupiedArray.Length; i++)
        {
            if (!spotOccupiedArray[i])
            {
                return i;
            }
        }
        return -1; // No available spots
    }
}
