/*
 *Author: Curtis Low
 * Date: 06/08/2024
 * Description: This abstract class represents a base for different types of beef objects
 * that can be cooked on a grill. It handles state transitions based on
 * cooking progress, updates the visual representation of the beef, and
 * manages interaction with the grill and progress UI.
 */

using Imagine.WebAR;
using UnityEngine;
using UnityEngine.UI;


public abstract class BeefBase : MonoBehaviour
{
    // Enum to represent different cooking states of the beef
    public enum BeefState { Raw, Rare, Medium, WellDone, Burnt }
    protected BeefState currentState = BeefState.Raw; // Current state of the beef

    protected OrderManager orderManager; // Reference to the OrderManager

    // Indicates if the beef is currently cooking   
    protected bool isCooking = false;

    // Indicates if the beef is currently on the grill
    protected bool isOnGrill = false;

    // Progress of the cooking process (0 to 1)
    protected float cookingProgress = 0f;

    // Materials to represent different beef states
    public Material[] stateMaterials;
    public float baseCookingTime = 10f; // Base time in seconds to transition to the next state on low heat

    // Reference to the Grill object
    protected Grill grill;

    // UI element to show the cooking progress
    public GameObject progressBarUI;
    // Image component to visually represent the cooking progress
    public Image progressBar;


    // Initialization
    protected virtual void Start()
    {
        orderManager = FindObjectOfType<OrderManager>();
        if (orderManager == null)
        {
            Debug.LogError("OrderManager not found in the scene.");
        }

        // Find and reference the Grill object in the scene and subscribe to its state change events
        grill = FindObjectOfType<Grill>();
        if (grill != null)
        {
            grill.OnGrillStateChanged += HandleGrillStateChanged;
        }
        // Set the initial material based on the current beef state
        UpdateMaterial();
        Debug.Log("Initial Beef State: " + currentState);
        progressBarUI.SetActive(false);
    }

    protected virtual void Update()
    {
        // Only update progress if cooking is active, beef is on the grill, and the grill is turned on
        if (isCooking && isOnGrill && grill != null && grill.isTurnedOn && currentState != BeefState.Burnt)
        {
            // Get the current grill state
            Grill.GrillState currentGrillState = grill.GetCurrentGrillState();

            // Determine time multiplier based on grill state
            float multiplier = GetGrillMultiplier(currentGrillState);

            // Calculate time needed to transition to the next state
            float timeToNextState = baseCookingTime / multiplier;

            // Update cooking progress
            cookingProgress += Time.deltaTime / timeToNextState;
            cookingProgress = Mathf.Clamp01(cookingProgress);


            // If progress reaches 100%, advance to the next cooking state
            if (cookingProgress >= 1f)
            {
                AdvanceCookingState();
                cookingProgress = 0f; // Reset progress for the next state
            }

            UpdateProgressBar(cookingProgress, 1f);// Update the progress bar UI
        }

    }

    // Get the multiplier for cooking time based on the grill's current state
    protected virtual float GetGrillMultiplier(Grill.GrillState grillState)
    {
        switch (grillState)
        {
            case Grill.GrillState.Low:
                return 1f;
            case Grill.GrillState.Medium:
                return 1.25f;
            case Grill.GrillState.High:
                return 1.5f;
            default:
                return 0f; // Grill is off
        }
    }

    // Advance to the next cooking state and update the material and UI
    protected virtual void AdvanceCookingState()
    {
        if (currentState < BeefState.Burnt)
        {
            currentState++;
            UpdateMaterial();
            Debug.Log("Beef State Changed: " + currentState);

            // If beef is burnt, stop cooking and hide the progress bar
            if (currentState == BeefState.Burnt)
            {
                isCooking = false;
                progressBarUI.SetActive(false);
                Debug.Log("Beef is burnt. Cooking stopped and progress bar hidden.");
            }
        }

        if (currentState != BeefState.Burnt)
        {
            progressBar.fillAmount = 0;
        }
    }

    // Update the material of the beef to reflect its current state
    protected virtual void UpdateMaterial()
    {
        GetComponent<Renderer>().material = stateMaterials[(int)currentState];
    }

    // Handle changes in the grill's state
    protected virtual void HandleGrillStateChanged()
    {
        if (isOnGrill && grill != null)
        {
            if (grill.isTurnedOn)
            {
                if (!isCooking)
                {
                    StartCooking();
                }
            }
            else
            {
                if (isCooking)
                {
                    StopCooking();
                }
            }
        }
    }

    // Triggered when the beef enters a collider with grill
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Grill"))
        {
            isOnGrill = true;
            Debug.Log("Beef is on the grill.");
            if (grill != null && grill.isTurnedOn)
            {
                StartCooking();
            }
        }
    }
    // Triggered when the beef exits a collider with grill
    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Grill"))
        {
            isOnGrill = false;
            StopCooking();
            Debug.Log("Beef is off the grill.");
        }
    }

    // Start the cooking process and show the progress bar UI
    public virtual void StartCooking()
    {
        isCooking = true;
        Debug.Log("Cooking Started");
        progressBarUI.SetActive(true);
    }

    // Stop the cooking process and hide the progress bar UI
    public virtual void StopCooking()
    {
        isCooking = false;
        Debug.Log("Cooking Stopped");
    }

    // Update the progress bar to reflect the current cooking progress
    protected virtual void UpdateProgressBar(float progress, float maxProgress)
    {
        float fillAmount = progress / maxProgress;
        progressBar.fillAmount = fillAmount;
    }

    // Abstract method to be implemented by derived classes to get the current beef state
    public abstract BeefState GetCurrentState();
}

