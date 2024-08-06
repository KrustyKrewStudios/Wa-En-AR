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
        //Check if InstructionsPanel is active
        if (!instructionsPanel.activeInHierarchy)
        {
            //Activate InstructionsPanel
            instructionsPanel.SetActive(true);

            //Set visible InstructionsPanel
            wheelInstructions.SetActive(true);
            drinkInstructions.SetActive(false);
            menuInstructions.SetActive(false);
            rateInstructions.SetActive(false);
        }
    }

}
