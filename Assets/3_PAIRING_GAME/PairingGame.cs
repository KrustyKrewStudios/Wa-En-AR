using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PairingGame : MonoBehaviour
{
    public GameObject wheel;
    public float spinDuration = 5f; 
    private bool isSpinning = false;
    private float currentSpeed;
    private float spinTimer;
    public TMP_Text resultText;

    void Update()
    {
        if (isSpinning)
        {
            // Rotate the wheel
            wheel.transform.Rotate(Vector3.up, currentSpeed * Time.deltaTime);

            // Decrement the spin timer
            spinTimer -= Time.deltaTime;

            // Check if spin duration has passed
            if (spinTimer <= 0f)
            {
                isSpinning = false;
                currentSpeed = 0f;
                ShowResult();
            }
            else
            {
                // Deceleration
                currentSpeed -= 100f * Time.deltaTime;
                if (currentSpeed <= 0f)
                {
                    isSpinning = false;
                    currentSpeed = 0f;
                    ShowResult();
                }
            }
        }
    }

    public void SpinWheel()
    {
        if (!isSpinning)
        {
            isSpinning = true;
            currentSpeed = 1000f;
            spinTimer = spinDuration; 
        }
    }

    private void ShowResult()
    {
        float angle = wheel.transform.eulerAngles.y;
        int result = GetResultFromAngle(angle);
        resultText.text = "Result: " + result.ToString();
    }

    private int GetResultFromAngle(float angle)
    {
        if (angle >= 22f && angle < -50f)
            return 3;
        else if (angle >= -50f && angle < -122f)
            return 2;
        else if (angle >= -122f && angle < -194f)
            return 1;
        else if (angle >= -194f && angle < -264f)
            return 5;
        else if (angle >= -264f && angle < 22f)
            return 4;
        else
            return 4; 
    }
}
