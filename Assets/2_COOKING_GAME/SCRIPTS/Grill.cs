/*
 * Author: Curtis Low
 * Date: 06/08/2024
 * Description: 
 * This class manages the state of the grill, including turning it on and off, 
 * adjusting the heat level, and updating the visual and audio feedback. 
 * It handles particle effects for different heat levels, updates the state 
 * text and color, and manages audio based on the presence of beef in the grill area.
 */using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class Grill : MonoBehaviour
{
    // Mask used to filter the raycast layers
    public LayerMask raycastLayerMask;

    // Enumeration to represent different grill states
    public enum GrillState { Off, Low, Medium, High }
    // Current state of the grill
    private GrillState currentState = GrillState.Off;
    // Indicates if the grill is turned on
    public bool isTurnedOn = false;
    // Event triggered when the grill state changes
    public event Action OnGrillStateChanged;

    // Particle systems for different fire effects
    public ParticleSystem tinyFireParticles;
    public ParticleSystem mediumFireParticles;
    public ParticleSystem bigFireParticles;
    public GameObject tinyFire;
    public GameObject mediumFire;
    public GameObject bigFire;

    private int beefCountInTrigger = 0;
    // Tags for beef objects
    public List<string> beefTags = new List<string> { "BeefKarubi", "BeefSirloin", "BeefRibeye" };

    // Audio source for sizzling sound
    public AudioSource sizzlingAudioSource;

    // Text UI element to display the grill state
    public TextMeshProUGUI grillStateText;

    // Define colors for each state
    public Color offColor = Color.gray;
    public Color lowColor = Color.green;
    public Color mediumColor = Color.yellow;
    public Color highColor = Color.red;

    // Audio source for button clicks
    public AudioSource buttonAudio;

    // Flag to prevent handling multiple inputs at the same time , used when debugging
    private bool isHandlingInput = false;

    // Initialize the grill state
    private void Start()
    {
        UpdateGrillState();

        sizzlingAudioSource.Stop(); // ensure the audio source is stopped at the start


    }
    private void OnEnable()
    {
        // Initialize particle systems and fire game objects
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
        // Increase the grill state if it's below the maximum (High)
        if (currentState < GrillState.High)
        {
            currentState++;
            Debug.Log("IncreaseGrillState called. New state: " + currentState);
            UpdateGrillState();
        }
        else
        {
            Debug.Log("IncreaseGrillState called, but state is already at High.");
        }
    }

    public void DecreaseGrillState()
    {
        // Decrease the grill state if it's above the minimum (Off)
        if (currentState > GrillState.Off)
        {   
            currentState--;
            Debug.Log("DecreaseGrillState called. New state: " + currentState);
            UpdateGrillState();
        }
        else
        {
            Debug.Log("DecreaseGrillState called, but state is already at Off.");
        }
    }
    private void UpdateGrillState()
    {
        // Turn the grill on if the state is Low, Medium, or High
        // Turn the grill off if the state is Off
        if (currentState == GrillState.Off)
        {
            // Turn off all particle effects
            isTurnedOn = false;
            DisableAllParticleSystems();


        }
        else
        {
            // Update particle effects based on the current state
            isTurnedOn = true;
            UpdateParticleSystems();

        }
        Debug.Log("Grill Turned On: " + isTurnedOn);

        // Manage audio playback
        UpdateSizzlingAudio();

        // Update the grill state text
        UpdateGrillStateText();

        // Trigger the OnGrillStateChanged event if there are subscribers
        OnGrillStateChanged?.Invoke();
    }

    private void UpdateParticleSystems()
    {
        // Disable all particle effects before enabling the relevant ones

        DisableAllParticleSystems(); 
        tinyFire.SetActive(true);
        mediumFire.SetActive(true);
        bigFire.SetActive(true);

        // Play the appropriate particle system based on the current grill state
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


    // Return the current state of the grill
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

    // Play or stop sizzling audio based on grill state and beef presence
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

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Mouse button down detected.");
            HandleInput(Input.mousePosition);
        }


    }
    private void HandleInput(Vector2 screenPosition)
    {
        if (isHandlingInput) return; // Prevent handling multiple inputs at the same time

        isHandlingInput = true;

        if (Camera.main == null)
        {
            Debug.LogError("Main camera is not found.");
            isHandlingInput = false;
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, raycastLayerMask))
        {
            GameObject hitObject = hit.transform.gameObject;
            Debug.Log("Raycast hit object: " + hitObject.name);
            // Check for button interactions
            if (hitObject.CompareTag("UpBtn"))
            {
                buttonAudio.Play();
                Debug.Log("clicked on grill toggle up btn");
                IncreaseGrillState();
            }

            if (hitObject.CompareTag("DownBtn"))
            {
                buttonAudio.Play();
                Debug.Log("clicked on grill toggle down btn");
                DecreaseGrillState();
            }
        }
        else
        {
            Debug.Log("Raycast did not hit any objects.");
        }

        // Reset input handling
        isHandlingInput = false;
    }

    // Update the text and color based on the current grill state
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