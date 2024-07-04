using UnityEngine;
using TMPro;
using static BeefBase;
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
            orderText.text = "No beef on the plate to check.";
        }
    }

    private void CheckOrder(GameObject beefOnPlate)
    {
        if (beefOnPlate != null)
        {
            // Check if the beef matches the current order type
            string beefTag = beefOnPlate.tag;

            if ((beefTag == "Karubi" && currentOrderType == BeefType.Karubi) ||
                (beefTag == "Sirloin" && currentOrderType == BeefType.Sirloin))
            {
                BeefBase beefComponent = beefOnPlate.GetComponent<BeefBase>();
                if (beefComponent != null)
                {
                    BeefBase.BeefState selectedState = beefComponent.GetCurrentState();

                    if (selectedState == currentOrderState)
                    {
                        Debug.Log("Order checked: Correct!");
                        orderText.text = "Order Checked: Correct!";
                        StartCoroutine(RemoveBeefAfterDelay(beefOnPlate)); // Start coroutine to remove beef
                    }
                    else
                    {
                        Debug.Log("Order checked: Incorrect temperature!");
                        orderText.text = "Order Checked: Incorrect temperature!";
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
            }
        }
        else
        {
            Debug.Log("No beef selected to check order.");
            orderText.text = "No beef selected to check order.";
        }
    }

    private IEnumerator RemoveBeefAfterDelay(GameObject beef)
    {
        yield return new WaitForSeconds(1.0f); // Wait for 1 second before removing the beef

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
        SetNextOrder(); // Set the next order after the beef is removed
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
