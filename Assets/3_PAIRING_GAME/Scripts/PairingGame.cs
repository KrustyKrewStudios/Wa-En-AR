

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
    public AudioSource wheelAudio;
    public GameObject resultPanel;
    public GameObject invalidPanel;

    //Spin timer
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
        invalidPanel.SetActive(false);
    }

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

            //Play bgm
            wheelAudio.Play();

            //Debug.Log("Rotating");
            //Debug.Log(spinDuration);
            //Debug.Log(spinTimer);

            //Check if spin duration has passed
            if (spinTimer <= 0f)
            {
                isSpinning = false;
                currentSpeed = 0f;
                wheelAudio.Stop();

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
                    wheelAudio.Stop();

                    //Start coroutine to delay showing result
                    StartCoroutine(DelayedShowResult(1f));
                    //Debug.Log("Start coroutine");
                }
            }
        }
    }

    //Delay showing results 
    IEnumerator DelayedShowResult(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        //Show the stored result after delay
        resultPanel.SetActive(true);
        resultText.text = "You got " + resultToShow;
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

    //Function to spin wheel
    public void SpinWheel()
    {
        //Check if any beef items are active
        if (chuck.activeInHierarchy || karubi.activeInHierarchy || ribeye.activeInHierarchy || sirloin.activeInHierarchy || tongue.activeInHierarchy)
        {
            //Show the invalid panel
            invalidPanel.SetActive(true); 
            return; 
        }

        if (!isSpinning)
        {
            //Start spinning
            isSpinning = true;
            currentSpeed = 1000f;

            //Set random time for spinning
            spinTimer = Random.Range(2.5f, 5f);
            Debug.Log(spinTimer);

            //Reset
            resultPanel.SetActive(false);
            resultText.text = " ";
            chuck.SetActive(false);
            karubi.SetActive(false);
            ribeye.SetActive(false);
            sirloin.SetActive(false);
            tongue.SetActive(false);

            //Hide the invalid panel if previously shown
            invalidPanel.SetActive(false); 
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
