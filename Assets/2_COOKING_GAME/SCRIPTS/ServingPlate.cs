using System.Collections.Generic;
using UnityEngine;

public class ServingPlate : MonoBehaviour
{
    private GameObject beefOnPlate; // Store the beef currently on the serving plate
    private OrderManager orderManager;

    // Detect when beef enters the trigger collider on the serving plate
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Karubi") || other.CompareTag("Sirloin"))
        {
            beefOnPlate = other.gameObject;
            Debug.Log("Beef moved onto plate: " + beefOnPlate.name);
        }
    }

    // Detect when beef exits the trigger collider on the serving plate
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == beefOnPlate)
        {
            beefOnPlate = null;
            Debug.Log("Beef removed from plate.");
        }
    }

    // Method to get the beef currently on the plate
    public GameObject GetBeefOnPlate()
    {
        return beefOnPlate;
    }
}
