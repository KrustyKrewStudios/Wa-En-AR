/*
 * Author: Bhoomika Manot
 * Date: 15/6/2024
 * Description: Code for AR Game
                1. Spawning different types of meat
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CookingSpawner: MonoBehaviour
{
    //Declare variables
    public GameObject[] foodSpawn;
    private bool hasSpawned = false;
    public Transform spawningPoint;

    public void SpawnObject()
    {
        if (!hasSpawned && spawningPoint != null && foodSpawn.Length > 0)
        {
            // Randomly select an object from the foodSpawn array
            int randomIndex = Random.Range(0, foodSpawn.Length);
            GameObject selectedFood = foodSpawn[randomIndex];

            // Instantiate the selected object at the spawning point
            Instantiate(selectedFood, spawningPoint.position, spawningPoint.rotation);

            // Set hasSpawned to true to prevent multiple spawns
            hasSpawned = true;
        }
    }
}
