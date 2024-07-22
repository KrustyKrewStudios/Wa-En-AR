using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class Grill : MonoBehaviour
{

    public LayerMask raycastLayerMask; 

    public enum GrillState { Off, Low, Medium, High }
    private GrillState currentState = GrillState.Off;
    public bool isTurnedOn = false;
    public event Action OnGrillStateChanged;

    public ParticleSystem tinyFireParticles;
    public ParticleSystem mediumFireParticles;
    public ParticleSystem bigFireParticles;
    public GameObject tinyFire;
    public GameObject mediumFire;
    public GameObject bigFire;

    private int beefCountInTrigger = 0;
    public List<string> beefTags = new List<string> { "BeefKarubi", "BeefSirloin", "BeefRibeye" };

    public AudioSource sizzlingAudioSource;

    public TextMeshProUGUI grillStateText;

    // Define colors for each state
    public Color offColor = Color.gray;
    public Color lowColor = Color.green;
    public Color mediumColor = Color.yellow;
    public Color highColor = Color.red;



    private void Start()
    {
          UpdateGrillState();

        sizzlingAudioSource.Stop(); // Ensure the audio source is stopped at the start
        

    }
    private void OnEnable()
    {
        tinyFire.SetActive(false);
        mediumFire.SetActive(false);
        bigFire.SetActive(false);
        tinyFireParticles.Clear();
        mediumFireParticles.Clear();
        bigFireParticles.Clear();
        tinyFireParticles.Stop();
        mediumFireParticles.Stop();
        bigFireParticles.Stop();



    }

    public void IncreaseGrillState()
    {
        if (currentState < GrillState.High)
        {
            currentState++;
            UpdateGrillState();
            Debug.Log("Grill State Increased: " + currentState);
        }
    }

    public void DecreaseGrillState()
    {
        if (currentState > GrillState.Off)
        {
            currentState--;
            UpdateGrillState();
            Debug.Log("Grill State Decreased: " + currentState);
        }
    }

    private void UpdateGrillState()
    {
        // Turn the grill on if the state is Low, Medium, or High
        // Turn the grill off if the state is Off
        if (currentState == GrillState.Off)
        {
            isTurnedOn = false;
            DisableAllParticleSystems();

        }
        else
        {
            isTurnedOn = true;
            UpdateParticleSystems();

        }
        Debug.Log("Grill Turned On: " + isTurnedOn);

        // Manage audio playback
        UpdateSizzlingAudio();

        // Update the grill state text
        UpdateGrillStateText();

        // Invoke the OnGrillStateChanged event if there are any subscribers
        OnGrillStateChanged?.Invoke();
    }

    private void UpdateParticleSystems()
    {

        DisableAllParticleSystems(); // Disable all first
        tinyFire.SetActive(true);
        mediumFire.SetActive(true);
        bigFire.SetActive(true);    


        switch (currentState)
        {
            case GrillState.Low:
                tinyFireParticles.Play();
                break;
            case GrillState.Medium:
                mediumFireParticles.Play();
                break;
            case GrillState.High:
                bigFireParticles.Play();
                break;
        }
    }

    private void DisableAllParticleSystems()
    {
        tinyFireParticles.Stop();
        mediumFireParticles.Stop();
        bigFireParticles.Stop();
    }


    public GrillState GetCurrentGrillState()
    {
        return currentState;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (beefTags.Contains(other.tag))
        {
            beefCountInTrigger++;
            UpdateSizzlingAudio();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (beefTags.Contains(other.tag))
        {
            beefCountInTrigger = Mathf.Max(0, beefCountInTrigger - 1);
            UpdateSizzlingAudio();
        }
    }

    private void UpdateSizzlingAudio()
    {
        if (isTurnedOn && beefCountInTrigger > 0)
        {
            if (!sizzlingAudioSource.isPlaying)
            {
                sizzlingAudioSource.Play();
            }
        }
        else
        {
            sizzlingAudioSource.Stop();
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // For mouse click or tap on screen
        {
            Debug.Log("Mouse button down detected.");
            HandleInput(Input.mousePosition);
        }

        // Handle touch input
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Debug.Log("Touch detected.");
                HandleInput(touch.position);
            }
        }
    }

    private void HandleInput(Vector2 screenPosition)
    {
        Debug.Log("Handling input at screen position: " + screenPosition);

        if (Camera.main == null)
        {
            Debug.LogError("Main camera is not found. Ensure the camera has the 'MainCamera' tag.");
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, raycastLayerMask)) // Use the LayerMask in the Raycast
        {
            GameObject hitObject = hit.transform.gameObject;
            Debug.Log("Raycast hit object: " + hitObject.name);

            if (hitObject.CompareTag("UpBtn"))
            {
                Debug.Log("clicked on grill toggle up btn");
                IncreaseGrillState();

            }

            if (hitObject.CompareTag("DownBtn"))
            {
                Debug.Log("clicked on grill toggle down btn");
                DecreaseGrillState();
            }

        }
        else
        {
            Debug.Log("Raycast did not hit any objects.");
        }
    }

    private void UpdateGrillStateText()
    {
        if (grillStateText == null) return;

        switch (currentState)
        {
            case GrillState.Off:
                grillStateText.text = "Off";
                grillStateText.color = offColor;
                break;
            case GrillState.Low:
                grillStateText.text = "Low";
                grillStateText.color = lowColor;
                break;
            case GrillState.Medium:
                grillStateText.text = "Medium";
                grillStateText.color = mediumColor;
                break;
            case GrillState.High:
                grillStateText.text = "High";
                grillStateText.color = highColor;
                break;
        }
    }


}
