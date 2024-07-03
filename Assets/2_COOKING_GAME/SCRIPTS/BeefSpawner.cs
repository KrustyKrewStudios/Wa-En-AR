using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeefSpawner : MonoBehaviour
{
    public GameObject karubiPrefab;
    public GameObject sirloinPrefab;

    public Transform[] grillSpots; // Assign multiple grill spots in the inspector
    public Transform servingPlateSpot; // Single serving plate spot

    public float dropHeight = 2.0f; // Height from which the beef drops

    private GameObject selectedBeef; // Currently selected beef

    private bool[] grillSpotOccupied;
    private bool servingSpotOccupied = false; // Track if serving plate spot is occupied
    private GameObject plateBeef; // Store the reference to the beef object

    private Dictionary<GameObject, int> beefSpotIndexMap; // Maps beef objects to their spot indices

    void Start()
    {
        if (grillSpots == null || grillSpots.Length == 0)
        {
            Debug.LogError("No grill spots assigned!");
        }

        grillSpotOccupied = new bool[grillSpots.Length];
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
        Quaternion spawnRotation = Quaternion.Euler(0f, 90f, 0f); // Rotate 90 degrees on the Y-axis

        GameObject newKarubi = Instantiate(karubiPrefab, spawnPosition, spawnRotation);
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
        Quaternion spawnRotation = Quaternion.Euler(0f, 90f, 0f); // Rotate 90 degrees on the Y-axis

        GameObject newSirloin = Instantiate(sirloinPrefab, spawnPosition, spawnRotation);
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

        servingSpotOccupied = false; // Reset serving spot occupied status
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
                if (!servingSpotOccupied)
                {
                    Transform servingSpot = servingPlateSpot;
                    StartCoroutine(MoveBeef(selectedBeef, servingSpot.position));

                    servingSpotOccupied = true; // Set serving spot to occupied
                    grillSpotOccupied[currentSpotIndex] = false; // Clear grill spot occupied
                    // No need to update beefSpotIndexMap since it's still on the serving plate
                }
                else
                {
                    Debug.Log("Serving plate spot is occupied.");
                    return;
                }
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
                beefSpotIndexMap[selectedBeef] = nextGrillSpotIndex; // Update the map to grill spot index
                servingSpotOccupied = false; // Clear serving spot occupied
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Karubi") || other.CompareTag("Sirloin"))
        {
            // Set serving spot occupied status
            servingSpotOccupied = true;

            // Optionally, you can store the reference to the beef object for further interaction.
            plateBeef = other.gameObject;
        }
    }

    public void ClearServingPlate()
    {
        if (servingSpotOccupied)
        {
            if (plateBeef != null)
            {
                // Destroy the beef object if it exists
                Destroy(plateBeef);
                plateBeef = null; // Clear the reference after destroying
            }

            // Reset serving spot occupied status
            servingSpotOccupied = false;
        }
        else
        {
            Debug.Log("Serving plate spot is already clear.");
        }
    }

}
