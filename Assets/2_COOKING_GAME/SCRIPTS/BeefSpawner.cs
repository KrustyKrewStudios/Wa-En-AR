using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BeefSpawner : MonoBehaviour
{
    public GameObject karubiPrefab;
    public GameObject sirloinPrefab;
    public GameObject chuckPrefab;
    public GameObject ribeyePrefab;
    public GameObject tonguePrefab;

    public OrderManager orderManager;

    public Transform beefParent; // Parent transform for the spawned beef objects


    public Transform[] grillSpots; // Assign multiple grill spots in the inspector
    public Transform servingPlateSpot; // Single serving plate spot

    public float dropHeight = 2.0f; // Height from which the beef drops

    private GameObject selectedBeef; // Currently selected beef

    private bool[] grillSpotOccupied;
    public bool plateOccupied = false; // Track if serving plate spot is occupied
    private GameObject plateBeef; // Store the reference to the beef object

    private Dictionary<GameObject, int> beefSpotIndexMap; // Maps beef objects to their spot indices

    public LayerMask raycastLayerMask; // Add a LayerMask for Raycast

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
        Quaternion spawnRotation = Quaternion.Euler(90f, 90f, 0f); // Rotate 90 degrees on the X and Y axes

        GameObject newKarubi = Instantiate(karubiPrefab, spawnPosition, spawnRotation, beefParent); // Set the parent
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
        Quaternion spawnRotation = Quaternion.Euler(0f, 180f, 0f); // Rotate 90 degrees on the X-axis

        GameObject newSirloin = Instantiate(sirloinPrefab, spawnPosition, spawnRotation, beefParent); // Set the parent
        StartCoroutine(DropToGrill(newSirloin, spawnSpot.position));

        grillSpotOccupied[nextGrillSpotIndex] = true;
        beefSpotIndexMap[newSirloin] = nextGrillSpotIndex; // Map beef to grill spot index
    }

    public void SpawnRibeye()
    {
        int nextGrillSpotIndex = GetNextAvailableSpot(grillSpotOccupied);

        if (nextGrillSpotIndex == -1)
        {
            Debug.Log("All grill spots are occupied.");
            return;
        }

        Transform spawnSpot = grillSpots[nextGrillSpotIndex];
        Vector3 spawnPosition = new Vector3(spawnSpot.position.x, spawnSpot.position.y + dropHeight, spawnSpot.position.z);
        Quaternion spawnRotation = Quaternion.Euler(90f, 90f, 0f); // Rotate 90 degrees on the Y-axis

        GameObject newRibeye = Instantiate(ribeyePrefab, spawnPosition, spawnRotation, beefParent); // Set the parent
        StartCoroutine(DropToGrill(newRibeye, spawnSpot.position));

        grillSpotOccupied[nextGrillSpotIndex] = true;
        beefSpotIndexMap[newRibeye] = nextGrillSpotIndex; // Map beef to grill spot index
    }


    public void SpawnChuck()
    {
        int nextGrillSpotIndex = GetNextAvailableSpot(grillSpotOccupied);

        if (nextGrillSpotIndex == -1)
        {
            Debug.Log("All grill spots are occupied.");
            return;
        }

        Transform spawnSpot = grillSpots[nextGrillSpotIndex];
        Vector3 spawnPosition = new Vector3(spawnSpot.position.x, spawnSpot.position.y + dropHeight, spawnSpot.position.z);
        Quaternion spawnRotation = Quaternion.Euler(90f, 90f, 0f); // Rotate 90 degrees on the X-axis

        GameObject newChuck = Instantiate(chuckPrefab, spawnPosition, spawnRotation, beefParent); // Set the parent
        StartCoroutine(DropToGrill(newChuck, spawnSpot.position));

        grillSpotOccupied[nextGrillSpotIndex] = true;
        beefSpotIndexMap[newChuck] = nextGrillSpotIndex; // Map beef to grill spot index
    }

    public void SpawnTongue()
    {
        int nextGrillSpotIndex = GetNextAvailableSpot(grillSpotOccupied);

        if (nextGrillSpotIndex == -1)
        {
            Debug.Log("All grill spots are occupied.");
            return;
        }

        Transform spawnSpot = grillSpots[nextGrillSpotIndex];
        Vector3 spawnPosition = new Vector3(spawnSpot.position.x, spawnSpot.position.y + dropHeight, spawnSpot.position.z);
        Quaternion spawnRotation = Quaternion.Euler(0f, 180f, 0f); // Rotate 180 degrees on the X-axis

        GameObject newTongue = Instantiate(tonguePrefab, spawnPosition, spawnRotation, beefParent); // Set the parent
        StartCoroutine(DropToGrill(newTongue, spawnSpot.position));

        grillSpotOccupied[nextGrillSpotIndex] = true;
        beefSpotIndexMap[newTongue] = nextGrillSpotIndex; // Map beef to grill spot index
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

        GameObject[] ribeyeObjects = GameObject.FindGameObjectsWithTag("Ribeye");
        foreach (GameObject ribeye in ribeyeObjects)
        {
            Destroy(ribeye);
        }

        GameObject[] chuckObjects = GameObject.FindGameObjectsWithTag("Chuck");
        foreach (GameObject chuck in chuckObjects)
        {
            Destroy(chuck);
        }

        GameObject[] tongueObject = GameObject.FindGameObjectsWithTag("Tongue");
        foreach (GameObject tongue in tongueObject)
        {
            Destroy(tongue);
        }




        Debug.Log("All spawned beef (Karubi and Sirloin) has been cleared from the scene.");

        // Reset the indices after clearing the beef
        for (int i = 0; i < grillSpotOccupied.Length; i++)
        {
            grillSpotOccupied[i] = false;
        }

        plateOccupied = false; // Reset serving spot occupied status
        beefSpotIndexMap.Clear(); // Clear the map
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // For mouse click or tap on screen
        {
            Debug.Log("Mouse button down detected.");
            HandleInput(Input.mousePosition);
        }

    }

    private void HandleInput(Vector2 screenPosition)
    {
        Debug.Log("Handling input at screen position: " + screenPosition);

        if (Camera.main == null)
        {
            Debug.LogError("Main camera is not found. Ensure the camera has the 'MainCamera' tag.");
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, raycastLayerMask)) // Use the LayerMask in the Raycast
        {
            GameObject hitObject = hit.transform.gameObject;
            Debug.Log("Raycast hit object: " + hitObject.name);

            if (hitObject.CompareTag("Karubi") || hitObject.CompareTag("Sirloin") || hitObject.CompareTag("Chuck") || hitObject.CompareTag("Ribeye") || hitObject.CompareTag("Tongue"))
            {
                selectedBeef = hitObject;
                OutlineSelection.Instance.OutlineBeef(selectedBeef);
                Debug.Log("Selected beef: " + selectedBeef.name);
            }
            else if (hitObject.CompareTag("ServingPlate"))
            {
                Debug.Log("Serving plate tapped.");
                if (selectedBeef != null)
                {
                    MoveSelectedBeef();
                }
                else
                {
                    Debug.LogWarning("No beef selected to move to the serving plate.");
                }
            }
        }
        else
        {
            Debug.Log("Raycast did not hit any objects.");
        }
    }

    public void MoveSelectedBeef()
    {
        if (selectedBeef == null) return;

        if (beefSpotIndexMap.TryGetValue(selectedBeef, out int currentSpotIndex))
        {
            if (selectedBeef.transform.parent == beefParent || selectedBeef.transform.parent == null) // Ensure parent is beefParent
            {
                if (!plateOccupied)
                {
                    Transform servingSpot = servingPlateSpot;
                    StartCoroutine(MoveBeef(selectedBeef, servingSpot.position));

                    plateOccupied = true; // Set serving spot to occupied
                    grillSpotOccupied[currentSpotIndex] = false; // Clear grill spot occupied
                    beefSpotIndexMap[selectedBeef] = -1; // Update map to indicate it's on the serving plate
                }
                else
                {
                    Debug.Log("Serving plate spot is occupied.");
                    return;
                }
            }
            else if (selectedBeef.transform.parent == beefParent) // Ensure parent is beefParent
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
                plateOccupied = false; // Clear serving spot occupied
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
        if (other.CompareTag("Karubi") || other.CompareTag("Sirloin") || other.CompareTag("Chuck") || other.CompareTag("Ribeye") || other.CompareTag("Tongue"))
        {
            // Set serving spot occupied status
            plateOccupied = true;

            // Store the reference to the beef object for further interaction.
            plateBeef = other.gameObject;
            Debug.Log("Beef moved onto plate: " + plateBeef.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == plateBeef)
        {
            plateBeef = null;
            Debug.Log("Beef removed from plate.");
            plateOccupied = false;
        }
    }

    public GameObject GetBeefOnPlate()
    {
        return plateBeef;
    }

    public void ClearServingPlate()
    {
        if (plateOccupied)
        {
            if (plateBeef != null)
            {
                // Destroy the beef object if it exists
                Destroy(plateBeef);
                plateBeef = null; // Clear the reference after destroying
            }

            // Reset serving spot occupied status
            plateOccupied = false;
        }
        else
        {
            Debug.Log("Serving plate is already empty.");
        }
    }
}
