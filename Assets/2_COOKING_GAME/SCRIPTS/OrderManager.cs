using UnityEngine;
using TMPro;
using static BeefBase;

public class OrderManager : MonoBehaviour
{
    public Transform servingPlateTransform; // Transform for the serving plate
    public TMP_Text orderText; // Reference to the TextMeshProUGUI for displaying order status

    private GameObject selectedBeef; // Store the selected beef

    private BeefType currentOrderType = BeefType.Karubi; // Default to Karubi for the first order
    private BeefBase.BeefState currentOrderState = BeefBase.BeefState.Medium; // Default to Medium for Karubi

    public void StartMinigame()
    {
        // Set the current order state for Medium (can be adjusted based on your game logic)
        currentOrderType = BeefType.Karubi;
        currentOrderState = BeefBase.BeefState.Medium;
        Debug.Log("Current Order: Medium");

        // Update UI text to display the current order
        orderText.text = "Current Order: Medium Karubi";
    }

    private void Start()
    {
        StartMinigame();
    }

    public void SelectBeef(GameObject beefObject)
    {
        selectedBeef = beefObject;
        Debug.Log("Beef selected: " + selectedBeef.name);

        // Example: Teleport the selected beef to the serving plate
        if (selectedBeef != null)
        {
            selectedBeef.transform.position = servingPlateTransform.position;
            Debug.Log("Beef teleported to plate: " + selectedBeef.name);

            // Check if the beef matches the current order
            CheckOrder();
        }
        else
        {
            Debug.Log("No beef selected.");
        }
    }

    private void CheckOrder()
    {
        if (selectedBeef != null)
        {
            // Check if the selected beef has a tag
            string beefTag = selectedBeef.tag;

            // Compare the tag with the current order type
            if ((beefTag == "Karubi" && currentOrderType == BeefType.Karubi) ||
                (beefTag == "Sirloin" && currentOrderType == BeefType.Sirloin))
            {
                // Check if the selected beef component exists
                BeefBase beefComponent = selectedBeef.GetComponent<BeefBase>();
                if (beefComponent != null)
                {
                    BeefBase.BeefState selectedState = beefComponent.GetCurrentState();

                    // Compare with the current order state
                    if (selectedState == currentOrderState)
                    {
                        Debug.Log("Order checked: Correct!");
                        orderText.text = "Order Checked: Correct!";
                        // Handle correct order checked logic here
                    }
                    else
                    {
                        Debug.Log("Order checked: Incorrect temperature!");
                        orderText.text = "Order Checked: Incorrect temperature!";
                        // Handle incorrect temperature logic here
                    }
                }
                else
                {
                    Debug.Log("Selected object is not a valid beef.");
                    orderText.text = "Selected object is not a valid beef.";
                }
            }
            else
            {
                Debug.Log("Order checked: Incorrect type!");
                orderText.text = "Order Checked: Incorrect type!";
                // Handle incorrect type logic here
            }
        }
        else
        {
            Debug.Log("No beef selected to check order.");
            orderText.text = "No beef selected to check order.";
        }
    }
}

public enum BeefType
{
    Karubi,
    Sirloin,
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
    