using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Grill : MonoBehaviour
{
    public enum GrillState { Off, Low, Medium, High }
    private GrillState currentState = GrillState.Off;
    public bool isTurnedOn = false;
    public event Action OnGrillStateChanged;

    public ParticleSystem tinyFireParticles;
    public ParticleSystem mediumFireParticles;
    public ParticleSystem bigFireParticles;
    // public Material[] stateMaterials; // Ensure this array has 4 materials for Off, Low, Medium, High
    //private Renderer grillRenderer;

    private void Start()
    {
      //  grillRenderer = GetComponent<Renderer>();
     //   if (grillRenderer == null)
      //  {
      //      Debug.LogError("Renderer component not found on the Grill GameObject.");
      //      return;
      //  }
      UpdateGrillState(); 
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

        // Update the grill material based on the current state
 //       if (stateMaterials != null && stateMaterials.Length == Enum.GetNames(typeof(GrillState)).Length)
 //       {
  //          grillRenderer.material = stateMaterials[(int)currentState];
  //      }
  //     else
   ///        Debug.LogError("State materials array is not properly set up.");
    //    }

        // Invoke the OnGrillStateChanged event if there are any subscribers
        OnGrillStateChanged?.Invoke();
    }

    private void UpdateParticleSystems()
    {
        DisableAllParticleSystems(); // Disable all first

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
}
