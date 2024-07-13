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

    private int currentOrderIndex = 0; // Index to keep track of the current order

    public BeefSpawner beefSpawner; // Reference to the BeefSpawner script

    private void Start()
    {
        StartMinigame();
    }

    public void StartMinigame()
    {
        SetNextOrder(); // Start with the first order
    }

    // Function to set order 1 (medium Karubi)
    private void SetNextOrder()
    {
        currentOrderType = GetRandomBeefType();
        currentOrderState = GetRandomBeefState();
        UpdateOrderText($"{currentOrderState} {currentOrderType}");
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
            StartCoroutine(ShowMessageAndRevert("No beef on the plate to check."));
        }
    }

    private void CheckOrder(GameObject beefOnPlate)
    {
        if (beefOnPlate != null)
        {
            // Check if the beef matches the current order type
            string beefTag = beefOnPlate.tag;

            if ((beefTag == "Karubi" && currentOrderType == BeefType.Karubi) ||
                (beefTag == "Sirloin" && currentOrderType == BeefType.Sirloin) ||
                (beefTag == "Chuck" && currentOrderType == BeefType.Chuck) ||
                (beefTag == "Ribeye" && currentOrderType == BeefType.Ribeye) ||
                (beefTag == "Tongue" && currentOrderType == BeefType.Tongue))
            {
                BeefBase beefComponent = beefOnPlate.GetComponent<BeefBase>();
                if (beefComponent != null)
                {
                    BeefBase.BeefState selectedState = beefComponent.GetCurrentState();

                    if (selectedState == currentOrderState)
                    {
                        Debug.Log("Order checked: Correct!");
                        orderText.text = "Order Checked: Correct!";
                        StartCoroutine(HandleCorrectOrder(beefOnPlate)); // Start coroutine to handle correct order
                    }
                    else
                    {
                        Debug.Log("Incorrect temperature!");
                        StartCoroutine(ShowMessageAndRevert("Order Checked: Incorrect temperature!"));
                    }
                }
                else
                {
                    Debug.Log("Selected object is not a valid beef.");
                    StartCoroutine(ShowMessageAndRevert("Selected object is not a valid beef."));
                }
            }
            else
            {
                Debug.Log("Incorrect beef type!");
                StartCoroutine(ShowMessageAndRevert("Order Checked: Incorrect type!"));
            }
        }
        else
        {
            Debug.Log("No beef selected to check order.");
            StartCoroutine(ShowMessageAndRevert("No beef selected to check order."));
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
        SetNextOrder(); // Set the next order after the beef is removed
    }

    private IEnumerator ShowMessageAndRevert(string message)
    {
        orderText.text = message;
        yield return new WaitForSeconds(3.0f); // Wait for 3 seconds
        UpdateOrderText(GetCurrentOrderDescription()); // Revert to showing the current order
    }

    private string GetCurrentOrderDescription()
    {
        switch (currentOrderIndex)
        {
            case 0:
                return "Medium Karubi";
            case 1:
                return "Well-done Sirloin";
            case 2:
                return "Rare Karubi";
            default:
                return "";
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
