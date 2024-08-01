using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Instructions : MonoBehaviour
{
    //Declaration of values
    public GameObject wheelInstructions;
    public GameObject drinkInstructions;
    public GameObject menuInstructions;
    public GameObject rateInstructions; 
    public GameObject instructionsPanel;

    public void InstuctionsReset()
    {
        if (!instructionsPanel.activeInHierarchy)
        {
            // Activate instructionsPanel
            instructionsPanel.SetActive(true);

            // Set the visibility of other instruction panels
            wheelInstructions.SetActive(true);
            drinkInstructions.SetActive(false);
            menuInstructions.SetActive(false);
            rateInstructions.SetActive(false);
        }
    }

}
