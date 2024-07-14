using UnityEngine;
using TMPro;
using System.Collections;

public class OrderManager : MonoBehaviour
{
    public Transform servingPlateTransform; // Transform for the serving plate
    public TMP_Text orderText; // Reference to the TextMeshProUGUI for displaying order status

    private GameObject selectedBeef; // Store the selected beef

    private BeefType currentOrderType = BeefType.Karubi; // Default to Karubi for the first order
    private BeefBase.BeefState currentOrderState = BeefBase.BeefState.Medium; // Default to Medium for Karubi


    public TMP_Text orderTrackerText; // Reference to the TextMeshProUGUI for displaying the order tracker (e.g., "Order 1/5")

    private int correctOrdersServed = 0; // Counter for the correct orders served
    private int totalOrdersToServe = 5; // Total orders to serve to win the game


    public BeefSpawner beefSpawner; // Reference to the BeefSpawner script

    private void Start()
    {
        StartMinigame();
    }

    public void StartMinigame()
    {
        correctOrdersServed = 0; // Reset the counter at the start of the game
        SetNextOrder(); // Start with the first order
    }

    private void SetNextOrder()
    {
        currentOrderType = GetRandomBeefType();
        currentOrderState = GetRandomBeefState();
        UpdateOrderText($"{currentOrderState} {currentOrderType}");
        UpdateOrderTrackerText();
        Debug.Log($"New Order: {currentOrderState} {currentOrderType}");
    }

    private BeefType GetRandomBeefType()
    {
        BeefType[] beefTypes = { BeefType.Karubi, BeefType.Sirloin, BeefType.Chuck, BeefType.Ribeye, BeefType.Tongue };
        return beefTypes[Random.Range(0, beefTypes.Length)];
    }

    private BeefBase.BeefState GetRandomBeefState()
    {
        BeefBase.BeefState[] beefStates = { BeefBase.BeefState.Rare, BeefBase.BeefState.Medium, BeefBase.BeefState.WellDone };
        return beefStates[Random.Range(0, beefStates.Length)];
    }


    // Function to update the order text
    private void UpdateOrderText(string orderDescription)
    {
        orderText.text = "Current Order: " + orderDescription;
        Debug.Log("Current Order: " + orderDescription);
    }

    // Function to update the order tracker text
    private void UpdateOrderTrackerText()
    {
        orderTrackerText.text = $"Order {correctOrdersServed + 1}/{totalOrdersToServe}";
        Debug.Log($"Order {correctOrdersServed + 1}/{totalOrdersToServe}");
    }


    public void SelectBeef(GameObject beefObject)
    {
        // Move the beef to the serving plate
        beefObject.transform.position = servingPlateTransform.position;
        Debug.Log("Beef placed on plate: " + beefObject.name);
    }

    // Function to check the order when the serve button is clicked
    public void ServeBeef()
    {
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
                    StartCoroutine(HandleCorrectOrder(beefOnPlate));
                }
                else
                {
                    string feedback = "Order Incorrect: ";
                    if (!typeCorrect) feedback += "Wrong Beef Type ";
                    if (!stateCorrect) feedback += "Wrong Temperature ";
                    orderText.text = feedback.Trim();

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


    private IEnumerator HandleCorrectOrder(GameObject beef)
    {
        yield return new WaitForSeconds(1.5f); // Wait for 1.5 seconds before proceeding

        float duration = 0.5f; // Duration of the scale-down animation
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

    public void EndGame()
        {
            orderText.text = "Congratulations! You served all orders correctly!";
            Debug.Log("good job dumbass");
        }
    private IEnumerator ResetOrderTextAfterDelay(float delay, string newOrder)
    {
        yield return new WaitForSeconds(delay);
        UpdateOrderText(newOrder);
    }


}



public enum BeefType
{
    Karubi,
    Sirloin,
    Chuck,
    Ribeye,
    Tongue
    // Add more beef types as needed
}

public enum BeefState
{
    Raw,
    Rare,
    Medium,
    WellDone,
    Burnt
}
