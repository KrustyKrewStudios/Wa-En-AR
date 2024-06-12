using UnityEngine;

public class BeefCooking : MonoBehaviour
{
    // Define the cooking states
    public enum CookingState
    {
        Raw,
        Rare,
        MediumRare,
        MediumWell,
        WellDone,
        Burnt
    }

    public CookingState currentState = CookingState.Raw;
    public float transitionTime = 5.0f; // Time in seconds for each state transition

    private float timer;
    private Renderer beefRenderer;
    private bool isOnGrill = false;

    // Assign the materials in the Unity Editor
    public Material rawMaterial;
    public Material rareMaterial;
    public Material mediumRareMaterial;
    public Material mediumWellMaterial;
    public Material wellDoneMaterial;
    public Material burntMaterial;

    void Start()
    {
        beefRenderer = GetComponent<Renderer>();
        timer = 0.0f;
        UpdateBeefAppearance();
    }

    void Update()
    {
        if (isOnGrill)
        {
            timer += Time.deltaTime;

            if (timer >= transitionTime)
            {
                timer = 0.0f;
                TransitionToNextState();
            }
        }
    }

    void TransitionToNextState()
    {
        if (currentState < CookingState.Burnt)
        {
            currentState++;
            UpdateBeefAppearance();
        }
    }

    void UpdateBeefAppearance()
    {
        switch (currentState)
        {
            case CookingState.Raw:
                beefRenderer.material = rawMaterial;
                break;
            case CookingState.Rare:
                beefRenderer.material = rareMaterial;
                break;
            case CookingState.MediumRare:
                beefRenderer.material = mediumRareMaterial;
                break;
            case CookingState.MediumWell:
                beefRenderer.material = mediumWellMaterial;
                break;
            case CookingState.WellDone:
                beefRenderer.material = wellDoneMaterial;
                break;
            case CookingState.Burnt:
                beefRenderer.material = burntMaterial;
                break;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Grill"))
        {
            isOnGrill = true;
            Debug.Log("Beef is on the grill.");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Grill"))
        {
            isOnGrill = false;
            Debug.Log("Beef is off the grill.");
        }
    }
}
