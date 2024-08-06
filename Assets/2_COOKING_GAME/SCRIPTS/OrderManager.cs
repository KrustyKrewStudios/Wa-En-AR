/*
 * Author: Curtis Low
 * Date: 06/08/2024
 * Description:
 * This class manages the order and serving of beef items in the game.
 * It handles setting orders, checking if the served beef matches the order,
 * updating UI elements for orders and order status, and managing game states.
 */
using UnityEngine;
using TMPro;
using System.Collections;

public class OrderManager : MonoBehaviour
{
    public Transform servingPlateTransform; // Transform for the serving plate
    public TMP_Text orderText; 

    private GameObject selectedBeef; // Store the selected beef

    private BeefType currentOrderType = BeefType.Karubi; // Default to Karubi for the first order
    private BeefBase.BeefState currentOrderState = BeefBase.BeefState.Medium; // Default to Medium for Karubi

    // order tracker UI
    public TMP_Text orderTrackerText;

    private int correctOrdersServed = 0; // Counter for the correct orders served
    private int totalOrdersToServe = 5; // Total orders to serve to win the game


    public GameObject endScreenPanel; // Reference to the End Screen Panel UI
    private bool isEndlessMode = false; // Flag to track if the game is in endless mode


    public BeefSpawner beefSpawner; // Reference to the BeefSpawner script

    public LayerMask raycastLayerMask; // LayerMask for Raycasting

    private AudioSource audioSource;
    public AudioSource wrongAudio;
    public AudioSource winAudio;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        // Initialize the minigame
        StartMinigame();
    }

    public void StartMinigame()
    {
        correctOrdersServed = 0; // Reset the counter at the start of the game
        isEndlessMode = false; // Ensure we are not in endless mode initially
        endScreenPanel.SetActive(false); // Hide end screen panel
        SetNextOrder(); // Start with the first order
    }

    // Function to set the next order
    private void SetNextOrder()
    {
        if (!isEndlessMode)
        {
            // Randomly select a new beef type and state
            currentOrderType = GetRandomBeefType();
            currentOrderState = GetRandomBeefState();
            UpdateOrderText($"{currentOrderState} {currentOrderType}");
            UpdateOrderTrackerText();
            Debug.Log($"New Order: {currentOrderState} {currentOrderType}");
        }
        // Randomly select a new beef type and state
        else
        {
            orderTrackerText.gameObject.SetActive(false); // Hide order tracker in endless mode
            currentOrderType = GetRandomBeefType();
            currentOrderState = GetRandomBeefState();
            UpdateOrderText($"{currentOrderState} {currentOrderType}");
        }
    }

    // Function to get a random beef type
    private BeefType GetRandomBeefType()
    {
        BeefType[] beefTypes = { BeefType.Karubi, BeefType.Sirloin, BeefType.Chuck, BeefType.Ribeye, BeefType.Tongue };
        return beefTypes[Random.Range(0, beefTypes.Length)];
    }

    // Function to get a random beef state
    private BeefBase.BeefState GetRandomBeefState()
    {
        BeefBase.BeefState[] beefStates = { BeefBase.BeefState.Rare, BeefBase.BeefState.Medium, BeefBase.BeefState.WellDone };
        return beefStates[Random.Range(0, beefStates.Length)];
    }


    // Function to update the order text on UI
    private void UpdateOrderText(string orderDescription)
    {
        // Replace "WellDone" with "Well Done" if present
        if (orderDescription.Contains("WellDone"))
        {
            orderDescription = orderDescription.Replace("WellDone", "Well Done");
        }

        // Capitalize each word in the order description
        orderText.text = "Current Order: " + CapitalizeWords(orderDescription);
        Debug.Log("Current Order: " + orderText.text);
    }

    // Method to capitalize each word in a string
    private string CapitalizeWords(string phrase)
    {
        var words = phrase.Split(' ');
        for (int i = 0; i < words.Length; i++)
        {
            var word = words[i];
            if (word.Length > 0)
            {
                words[i] = char.ToUpper(word[0]) + word.Substring(1).ToLower();
            }
        }
        return string.Join(' ', words);
    }

    // Function to update the order tracker on the UI
    private void UpdateOrderTrackerText()
    {
        if (!isEndlessMode)
        {
            orderTrackerText.text = $"Order {correctOrdersServed + 1}/{totalOrdersToServe}";
            Debug.Log($"Order {correctOrdersServed + 1}/{totalOrdersToServe}");
        }
    }

    // Function to select and move beef to the serving plate
    public void SelectBeef(GameObject beefObject)
    {
        // Move the beef to the serving plate
        beefObject.transform.position = servingPlateTransform.position;
        Debug.Log("Beef placed on plate: " + beefObject.name);
    }

    // Function to serve the beef when the serve button is clicked
    public void ServeBeef()
    {
        audioSource.Play();

        GameObject beefOnPlate = beefSpawner.GetBeefOnPlate();

        if (beefOnPlate != null)
        {
            CheckOrder(beefOnPlate);
        }
        else
        {
            Debug.Log("No beef on the plate to check.");
        }
    }

    // Function to check if the served beef matches the current order
    private void CheckOrder(GameObject beefOnPlate)
    {
        if (beefOnPlate != null)
        {
            string beefTag = beefOnPlate.tag;
            bool typeCorrect = beefTag == currentOrderType.ToString();
            BeefBase beefComponent = beefOnPlate.GetComponent<BeefBase>();

            if (beefComponent != null)
            {
                BeefBase.BeefState selectedState = beefComponent.GetCurrentState();
                bool stateCorrect = selectedState == currentOrderState;

                if (typeCorrect && stateCorrect)
                {
                    Debug.Log("Order checked: Correct!");
                    orderText.text = "Order Served!";
                    // Handle the correct order
                    StartCoroutine(HandleCorrectOrder(beefOnPlate));
                }
                else
                {
                    string feedback = "Order Incorrect: ";
                    if (!typeCorrect) feedback += "Wrong Beef Type ";
                    if (!stateCorrect) feedback += "Wrong Temperature ";
                    orderText.text = feedback.Trim();
                    // Play incorrect order feedback audio
                    wrongAudio.Play();

                    StartCoroutine(ResetOrderTextAfterDelay(3f, $"{currentOrderState} {currentOrderType}"));
                }
            }
            else
            {
                Debug.Log("Selected object is not a valid beef.");
            }
        }
        else
        {
            Debug.Log("No beef selected to check order.");
        }
    }

    // Coroutine to handle the correct order by scaling down the beef and removing it
    private IEnumerator HandleCorrectOrder(GameObject beef)
    {
        yield return new WaitForSeconds(1.5f); 

        float duration = 0.5f; 
        Vector3 startScale = beef.transform.localScale;
        Vector3 endScale = Vector3.zero; // Target scale is zero to make the beef disappear
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            beef.transform.localScale = Vector3.Lerp(startScale, endScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        beef.transform.localScale = endScale;
        Destroy(beef); // Remove the beef object from the scene
        beefSpawner.plateOccupied = false;

        correctOrdersServed++;

        if (correctOrdersServed >= totalOrdersToServe)
        {
            EndGame(); // End the game if the required number of orders is served
        }
        else
        {
            SetNextOrder(); // Set the next order after the beef is removed
        } 
    }

    // Function to handle the end of the game
    public void EndGame()
        {
            orderText.text = "Congratulations! You served all orders correctly!";
            Debug.Log("good job dumbass");
            winAudio.Play();
            endScreenPanel.SetActive(true);

    }

    // Function to continue in endless mode after winning the game
    public void ContinueInEndlessMode()
    {
        isEndlessMode = true;
        endScreenPanel.SetActive(false);
        correctOrdersServed = 0; // Reset the orders count for endless mode
        SetNextOrder(); // Set the next order
    }


    // Coroutine to reset the order text after a delay
    private IEnumerator ResetOrderTextAfterDelay(float delay, string newOrder)
    {
        yield return new WaitForSeconds(delay);
        UpdateOrderText(newOrder);
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // For mouse click or tap on screen
        {
            Debug.Log("Mouse button down detected.");
            HandleInput(Input.mousePosition);
        }


    }

    // Function to handle input and raycasting
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

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, raycastLayerMask)) 
        {
            GameObject hitObject = hit.transform.gameObject;
            Debug.Log("Raycast hit object: " + hitObject.name);

            if (hitObject.CompareTag("Bell"))
            {
                Debug.Log("clicked bell");
                audioSource.Play();

                ServeBeef(); // Call ServeBeef function

            }


        }
        else
        {
            Debug.Log("Raycast did not hit any objects.");
        }
    }

}



public enum BeefType
{
    Karubi,
    Sirloin,
    Chuck,
    Ribeye,
    Tongue
}

public enum BeefState
{
    Raw,
    Rare,
    Medium,
    WellDone,
    Burnt
}
