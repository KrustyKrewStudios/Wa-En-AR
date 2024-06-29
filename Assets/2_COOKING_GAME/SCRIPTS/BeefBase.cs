using Imagine.WebAR;
using UnityEngine;
using UnityEngine.UI;


public abstract class BeefBase : MonoBehaviour
{
    public enum BeefState { Raw, Rare, Medium, WellDone, Burnt }
    protected BeefState currentState = BeefState.Raw;

    protected OrderManager orderManager; // Reference to the OrderManager

    protected bool isCooking = false;
    protected bool isOnGrill = false;
    protected float cookingProgress = 0f;

    public Material[] stateMaterials; // Assign different materials for each state
    public float baseCookingTime = 10f; // Base time in seconds to transition to the next state on low heat

    protected Grill grill; // Reference to the Grill object

    public GameObject progressBarUI;
    public Image progressBar;
    public Transform progressBarTransform;
    public ARCamera ARCamera;

    protected virtual void Start()
    {
        orderManager = FindObjectOfType<OrderManager>();
        if (orderManager == null)
        {
            Debug.LogError("OrderManager not found in the scene.");
        }

        grill = FindObjectOfType<Grill>();
        if (grill != null)
        {
            grill.OnGrillStateChanged += HandleGrillStateChanged;
        }

        UpdateMaterial();
        Debug.Log("Initial Beef State: " + currentState);
        progressBarUI.SetActive(false);
    }

    protected virtual void Update()
    {
        if (isCooking && isOnGrill && grill != null && grill.isTurnedOn && currentState != BeefState.Burnt)
        {
            Grill.GrillState currentGrillState = grill.GetCurrentGrillState();
            float multiplier = GetGrillMultiplier(currentGrillState);
            float timeToNextState = baseCookingTime / multiplier;

            cookingProgress += Time.deltaTime / timeToNextState;
            cookingProgress = Mathf.Clamp01(cookingProgress);

            if (cookingProgress >= 1f)
            {
                AdvanceCookingState();
                cookingProgress = 0f;
            }

            UpdateProgressBar(cookingProgress, 1f);
        }

        FaceCamera();
    }

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

    protected virtual void AdvanceCookingState()
    {
        if (currentState < BeefState.Burnt)
        {
            currentState++;
            UpdateMaterial();
            Debug.Log("Beef State Changed: " + currentState);

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

    protected virtual void UpdateMaterial()
    {
        GetComponent<Renderer>().material = stateMaterials[(int)currentState];
    }

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

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Grill"))
        {
            isOnGrill = false;
            StopCooking();
            Debug.Log("Beef is off the grill.");
        }
    }

    public virtual void StartCooking()
    {
        isCooking = true;
        Debug.Log("Cooking Started");
        progressBarUI.SetActive(true);
    }

    public virtual void StopCooking()
    {
        isCooking = false;
        Debug.Log("Cooking Stopped");
    }

    protected virtual void UpdateProgressBar(float progress, float maxProgress)
    {
        float fillAmount = progress / maxProgress;
        progressBar.fillAmount = fillAmount;
    }

    protected virtual void FaceCamera()
    {
        if (progressBarTransform != null && ARCamera != null)
        {
            progressBarTransform.LookAt(progressBarTransform.position + ARCamera.transform.rotation * Vector3.forward,
                                        ARCamera.transform.rotation * Vector3.up);
        }
        else
        {

            if (ARCamera == null)
            {
                Debug.LogWarning("ARCamera is null");
            }
        }
    }

    public abstract BeefState GetCurrentState();

    protected virtual void OnMouseDown()
    {
        orderManager.SelectBeef(gameObject);
    }
}