using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchingResults : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Beef1") && other.CompareTag("Drink1"))
        {
            Debug.Log("Table items matched");
        }
        if (other.CompareTag("Ribeye") && other.CompareTag("Drink1"))
        {
            Debug.Log("Table items matched - ribeye");
        }
    }
}
