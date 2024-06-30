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

    private int currentOrderIndex = 0; // Index to keep track of the current order


    private void Start()
    {
        StartMinigame();
    }

    public void StartMinigame()
    {
        SetOrder1(); // Start with the first order
    }

    // Function to set order 1 (medium Karubi)
    public void SetOrder1()
    {
        currentOrderType = BeefType.Karubi;
        currentOrderState = BeefBase.BeefState.Medium;
        UpdateOrderText("Medium Karubi");
        Debug.Log("order 1 medium karubi");
    }

    // Function to set order 2 (well-done Sirloin)
    public void SetOrder2()
    {
        currentOrderType = BeefType.Sirloin;
        currentOrderState = BeefBase.BeefState.WellDone;
        UpdateOrderText("Well-done Sirloin");
        Debug.Log("order 2 well sirloin ");

    }

    // Function to set order 3 (rare Karubi)
    public void SetOrder3()
    {
        currentOrderType = BeefType.Karubi;
        currentOrderState = BeefBase.BeefState.Rare;
        UpdateOrderText("Rare Karubi");
        Debug.Log("order 3 rare karubi");

    }

    // Function to update the order text
    private void UpdateOrderText(string orderDescription)
    {
        orderText.text = "Current Order: " + orderDescription;
        Debug.Log("Current Order: " + orderDescription);
    }

    // Function to set the next order based on the current order index
    private void SetNextOrder()
    {
        currentOrderIndex = (currentOrderIndex + 1) % 3; // Cycle through 0, 1, 2

        switch (currentOrderIndex)
        {
            case 0:
                SetOrder1();
                break;
            case 1:
                SetOrder2();
                break;
            case 2:
                SetOrder3();
                break;
        }
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
                        SetNextOrder(); // Set the next order

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
    