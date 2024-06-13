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
        }
        else
        {
            isTurnedOn = true;
        }
        Debug.Log("Grill Turned On: " + isTurnedOn);
    }
        

    public GrillState GetCurrentGrillState()
    {
        return currentState;
    }


}
