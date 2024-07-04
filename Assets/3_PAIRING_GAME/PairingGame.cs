using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PairingGame : MonoBehaviour
{
    //Declaration of values
    public GameObject wheel;
    //public GameObject beef1;
    //public string[] scriptNamesToDisable;

    //Spin timer
    public float spinDuration = 5f;
    private bool isSpinning = false;
    private float currentSpeed;
    private float spinTimer;

    //Results
    private string resultToShow = " "; 
    public TMP_Text resultText;

    void Start()
    {
        //DisableScripts(beef1, scriptNamesToDisable);
    }

    /*void DisableScripts(GameObject gameObject, string[] scriptNames)
    {
        foreach (string scriptName in scriptNames)
        {
            gameObject.SendMessage("SetScriptDisabled", SendMessageOptions.DontRequireReceiver);
        }
    }

    void EnableScripts(GameObject gameObject, string[] scriptNames)
    {
        foreach (string scriptName in scriptNames)
        {
            gameObject.SendMessage("SetScriptEnabled", SendMessageOptions.DontRequireReceiver);
        }
    }*/

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
            resultText.text = " ";
        }


    }

    void OnTriggerEnter(Collider other)
    {
        //Only update resultToShow if the wheel is still spinning
        if (isSpinning) 
        {
            if (other.CompareTag("Mesh3"))
            {
                resultToShow = "Result: Beef";
            }
            else if (other.CompareTag("Mesh4"))
            {
                resultToShow = "Result: 4";
            }
            else if (other.CompareTag("Mesh5"))
            {
                resultToShow = "Result: 5";
            }
            else if (other.CompareTag("Mesh1"))
            {
                resultToShow = "Result: 1";
            }
            else if (other.CompareTag("Mesh2"))
            {
                resultToShow = "Result: 2";
            }
            else
            {
                resultToShow = "Result: Nil";
            }
        }
    }
}
