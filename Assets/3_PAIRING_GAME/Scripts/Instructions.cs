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
        instructionsPanel.gameObject.SetActive(true);

        wheelInstructions.gameObject.SetActive(true);

        drinkInstructions.gameObject.SetActive(false);
        menuInstructions.gameObject.SetActive(false);
        rateInstructions.gameObject.SetActive(false);
    }

}
