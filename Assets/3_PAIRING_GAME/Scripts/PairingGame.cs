using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PairingGame : MonoBehaviour
{
    //Declaration of values
    public GameObject wheel;
    public GameObject chuck;
    public GameObject karubi;
    public GameObject ribeye;
    public GameObject sirloin;
    public GameObject tongue;
    //public GameObject drink1;
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
        chuck.SetActive(false);
        karubi.SetActive(false);
        ribeye.SetActive(false);
        sirloin.SetActive(false);
        tongue.SetActive(false);
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
        //Debug.Log("Updating");

        if (isSpinning == true)
        {
            //drink1.transform.Translate(Vector3.one * currentSpeed * Time.deltaTime);

            //Rotate the wheel
            wheel.transform.Rotate(Vector3.up, currentSpeed * Time.deltaTime);

            //Decrement the spin timer
            spinTimer -= Time.deltaTime;

            //Debug.Log("Rotating");
            //Debug.Log(spinDuration);
            //Debug.Log(spinTimer);

            //Check if spin duration has passed
            if (spinTimer <= 0f)
            {
                isSpinning = false;
                currentSpeed = 0f;
                //Start coroutine to delay showing result
                StartCoroutine(DelayedShowResult(1f));
                //Debug.Log("Start coroutine");
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
                    //Debug.Log("Start coroutine");
                }
            }
        }
    }

    IEnumerator DelayedShowResult(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        //Show the stored result after delay
        resultText.text = resultToShow;
        Debug.Log(resultToShow);

        //Set active objects according to result
        if (resultToShow == "Ribeye")
        {
            ribeye.SetActive(true);
        }
        if (resultToShow == "Chuck")
        {
            chuck.SetActive(true);
        }
        if (resultToShow == "Sirloin")
        {
            sirloin.SetActive(true);
        }
        if (resultToShow == "Karubi")
        {
            karubi.SetActive(true);
        }
        if (resultToShow == "Beef Tongue")
        {
            tongue.SetActive(true);
        }
    }

    public void SpinWheel()
    {
        if (!isSpinning)
        {
            //Start spinning
            isSpinning = true;
            currentSpeed = 1000f;
            spinTimer = spinDuration;
            //Debug.Log("Spinning");

            //Reset 
            resultText.text = " ";
            chuck.SetActive(false);
            karubi.SetActive(false);
            ribeye.SetActive(false);
            sirloin.SetActive(false);
            tongue.SetActive(false);
        }


    }

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("OnTrigger");

        //Only update resultToShow if the wheel is still spinning
        if (isSpinning) 
        {
            //Debug.Log("OnTrigger - Unchecked");

            if (other.CompareTag("Mesh3"))
            {
                resultToShow = "Ribeye";
            }
            else if (other.CompareTag("Mesh4"))
            {
                resultToShow = "Sirloin";
            }
            else if (other.CompareTag("Mesh5"))
            {
                resultToShow = "Chuck";
            }
            else if (other.CompareTag("Mesh1"))
            {
                resultToShow = "Karubi";
            }
            else if (other.CompareTag("Mesh2"))
            {
                resultToShow = "Beef Tongue";
            }
            else
            {
                resultToShow = "Result: Nil";
                //Debug.Log("Nil");
            }
        }
    }
}
