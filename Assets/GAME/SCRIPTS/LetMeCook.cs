using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode()]
public class LetMeCook : MonoBehaviour
{
    public enum BeefState { Raw, Rare, Medium, WellDone, Burnt }
    private BeefState currentState = BeefState.Raw;

    private bool isCooking = false;
    private bool isOnGrill = false; // Add this flag to track if the beef is on the grill
    private float cookingProgress = 0f;

    public Material[] stateMaterials; // Assign different materials for each state
    public float baseCookingTime = 10f; // Base time in seconds to transition to the next state on low heat

    private Grill grill; // Reference to the Grill object

    public int maximumProgress;
    public int currentProgress;
    public Image progressBar;

    private void Start()
    {
        grill = FindObjectOfType<Grill>(); // Find the Grill object in the scene
        if (grill != null)
        {
            grill.OnGrillStateChanged += HandleGrillStateChanged;
        }
        UpdateMaterial(); // Ensure the initial material is set
        Debug.Log("Initial Beef State: " + currentState);
    }


    private void Update()
    {
        if (isCooking && isOnGrill && grill != null && grill.isTurnedOn)
        {
            Grill.GrillState currentGrillState = grill.GetCurrentGrillState();
            float multiplier = GetGrillMultiplier(currentGrillState);
            float timeToNextState = baseCookingTime / multiplier;

            cookingProgress += Time.deltaTime;

            Debug.Log("Grill State: " + currentGrillState + ", Cooking Progress: " + cookingProgress + "/" + timeToNextState);

            if (cookingProgress >= timeToNextState)
            {
                AdvanceCookingState();
                cookingProgress = 0f; // Reset progress for the next state
            }

            UpdateProgressBar(cookingProgress, timeToNextState);
        }
    }

    private float GetGrillMultiplier(Grill.GrillState grillState)
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

    private void AdvanceCookingState()
    {
        if (currentState < BeefState.Burnt)
        {
            currentState++;
            UpdateMaterial();
            Debug.Log("Beef State Changed: " + currentState);
        }
        else
        {
            isCooking = false; // Stop cooking when burnt
            Debug.Log("Beef is burnt. Cooking stopped.");
        }

        // Reset progress bar when state changes
        currentProgress = 0;
        progressBar.fillAmount = 0;
    }

    private void UpdateMaterial()
    {
        GetComponent<Renderer>().material = stateMaterials[(int)currentState];
    }

    private void HandleGrillStateChanged()
    {
        if (isOnGrill && grill != null)
        {
            if (grill.isTurnedOn)
            {
                StartCooking();
            }
            // No need to handle the case when grill turns off, as StopCooking() is already called in OnTriggerExit.
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Grill"))
        {
            isOnGrill = true;
            StartCooking(); // Always start cooking when beef is on the grill
            Debug.Log("Beef is on the grill.");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Grill"))
        {
            isOnGrill = false;
            StopCooking();
            Debug.Log("Beef is off the grill.");
        }
    }

    public void StartCooking()
    {
        isCooking = true;
        Debug.Log("Cooking Started");
    }

    public void StopCooking()
    {
        isCooking = false;
        Debug.Log("Cooking Stopped");
    }

    void UpdateProgressBar(float progress, float maxProgress)
    {
        float fillAmount = progress / maxProgress;
        progressBar.fillAmount = fillAmount;
    }
}
