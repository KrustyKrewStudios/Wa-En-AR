using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PairingGame : MonoBehaviour
{
    //Declaration of values
    public GameObject wheel;

    //Spin timer
    public float spinDuration = 5f;
    private bool isSpinning = false;
    private float currentSpeed;
    private float spinTimer;

    //Results
    private string resultToShow = "Result: "; 
    public TMP_Text resultText;

    void Update()
    {
        if (isSpinning)
        {
            //Rotate the wheel
            wheel.transform.Rotate(Vector3.up, currentSpeed * Time.deltaTime);

            //Decrement the spin timer
            spinTimer -= Time.deltaTime;

            //Check if spin duration has passed
            if (spinTimer <= 0f)
            {
                isSpinning = false;
                currentSpeed = 0f;
                //Start coroutine to delay showing result
                StartCoroutine(DelayedShowResult(1f));
            }
            else
            {
                //Deceleration
                currentSpeed -= 100f * Time.deltaTime;
                if (currentSpeed <= 0f)
                {
                    isSpinning = false;
                    currentSpeed = 0f;
                    //Start coroutine to delay showing result
                    StartCoroutine(DelayedShowResult(1f));
                }
            }
        }
    }

    IEnumerator DelayedShowResult(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        //Show the stored result after delay
        resultText.text = resultToShow;
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

    void OnTriggerEnter(Collider other)
    {
        //Only update resultToShow if the wheel is still spinning
        if (isSpinning) 
        {
            if (other.CompareTag("Mesh4"))
            {
                resultToShow = "Result: 4";
            }
            else if (other.CompareTag("Mesh3"))
            {
                resultToShow = "Result: 3";
            }
            else
            {
                resultToShow = "Result: Nil";
            }
        }
    }
}
