using Imagine.WebAR;
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

    public Transform progressBarTransform; // Transform of the progress bar UI
    public ARCamera ARCamera;

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

            // Update the cooking progress based on the normalized delta time
            cookingProgress += Time.deltaTime / timeToNextState;

            // Ensure cooking progress doesn't exceed 1
            cookingProgress = Mathf.Clamp01(cookingProgress);

            Debug.Log("Grill State: " + currentGrillState + ", Cooking Progress: " + cookingProgress);

            // Check if we've reached or exceeded the required progress for the next state
            if (cookingProgress >= 1f)
            {
                AdvanceCookingState();
                cookingProgress = 0f; // Reset progress for the next state
            }

            // Update the progress bar
            UpdateProgressBar(cookingProgress, 1f); // Using 1f as max progress since it's normalized
        }

        FaceCamera();
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

    private void FaceCamera()
    {
        // Make the progress bar face the camera
        if (progressBarTransform != null && ARCamera != null)
        {
            progressBarTransform.LookAt(progressBarTransform.position + ARCamera.transform.rotation * Vector3.forward,
                                        ARCamera.transform.rotation * Vector3.up);
        }
        else
        {
            // Log if either progressBarTransform or ARCamera is null
            if (progressBarTransform == null)
            {
                Debug.LogWarning("progressBarTransform is null");
            }

            if (ARCamera == null)
            {
                Debug.LogWarning("ARCamera is null");
            }
        }

    }


}
