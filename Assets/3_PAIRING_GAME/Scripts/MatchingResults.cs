using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MatchingResults : MonoBehaviour
{
    //Declarations
    public TMP_Text ratingText;
    public GameObject ratingPanel;

    //Drinks declraration
    public Transform spawnPoint;
    public Transform oldFashionedSpawnPoint;
    public GameObject campfire;
    public GameObject martini;
    public GameObject sake;
    public GameObject oldFashioned;

    //Counter for button clicks
    private int clickCounter = 0;
    public GameObject endPagePanel;

    private HashSet<string> detectedItems = new HashSet<string>();

    //List of Items
    private string[] beefItems = { "Chuck", "Sirloin", "Ribeye", "Karubi", "BeefTongue" };
    private string[] drinkItems = { "Campfire", "Martini", "Sake", "OldFashioned" };

    //Dictionary to store ratings for each combination
    private Dictionary<string, int> comboRatings = new Dictionary<string, int>();

    void Start()
    {
        //Initialize Combo ratings
        comboRatings.Add("Chuck_Campfire", 8);
        comboRatings.Add("Chuck_Martini", 6);
        comboRatings.Add("Chuck_Sake", 8);
        comboRatings.Add("Chuck_OldFashioned", 6);
        comboRatings.Add("Sirloin_Campfire", 8);
        comboRatings.Add("Sirloin_Martini", 9);
        comboRatings.Add("Sirloin_Sake", 7);
        comboRatings.Add("Sirloin_OldFashioned", 7);
        comboRatings.Add("Ribeye_Campfire", 10);
        comboRatings.Add("Ribeye_Martini", 7);
        comboRatings.Add("Ribeye_Sake", 6);
        comboRatings.Add("Ribeye_OldFashioned", 8);
        comboRatings.Add("Karubi_Campfire", 6);
        comboRatings.Add("Karubi_Martini", 5);
        comboRatings.Add("Karubi_Sake", 7);
        comboRatings.Add("Karubi_OldFashioned", 9);
        comboRatings.Add("BeefTongue_Campfire", 5);
        comboRatings.Add("BeefTongue_Martini", 5);
        comboRatings.Add("BeefTongue_Sake", 5);
        comboRatings.Add("BeefTongue_OldFashioned", 5);

        //Debug.Log("Combo Initialied");
    }

    public void SpawnCampfire()
    {
        if (campfire != null && spawnPoint != null)
        {
            Instantiate(campfire, spawnPoint.position, spawnPoint.rotation);
        }
    }

    public void SpawnSake()
    {
        if (sake != null && spawnPoint != null)
        {
            Instantiate(sake, spawnPoint.position, spawnPoint.rotation);
        }
    }

    public void SpawnMartini()
    {
        if (martini != null && spawnPoint != null)
        {
            Instantiate(martini, spawnPoint.position, spawnPoint.rotation);
        }
    }

    public void SpawnOldFashioned()
    {
        if (oldFashioned != null && oldFashionedSpawnPoint != null)
        {
            Instantiate(oldFashioned, oldFashionedSpawnPoint.position, oldFashionedSpawnPoint.rotation);
        }
    }

    //Detect items via OnTrigger & Tags
    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Trigger script working");

        string tag = other.tag;
        Debug.Log($"Tag detected: {tag}");
        detectedItems.Add(tag);
    }

    /*void OnTriggerExit(Collider other)
    {
        string tag = other.tag;
        //Debug.Log($"Tag exited: {tag}");
        detectedItems.Remove(tag);
        other.gameObject.SetActive(false);
    }*/

    public void CheckForCombinations()
    {
        //Debug.Log("Detected items: " + string.Join(", ", detectedItems));

        //Check for which beef
        foreach (string beef in beefItems)
        {
            //Check for which drink
            foreach (string drink in drinkItems)
            {
                if (detectedItems.Contains(beef) && detectedItems.Contains(drink))
                {
                    //Call for func for rating items 
                    RatePairing(beef, drink);
                }
            }
        }
    }

    public void DeactivateRatedItems()
    {
        List<string> itemsToDeactivate = new();

        foreach (string beef in beefItems)
        {
            foreach (string drink in drinkItems)
            {
                if (detectedItems.Contains(beef) && detectedItems.Contains(drink))
                {
                    itemsToDeactivate.Add(beef);
                    itemsToDeactivate.Add(drink);
                }
            }
        }

        foreach (string item in itemsToDeactivate)
        {
            GameObject[] itemObjects = GameObject.FindGameObjectsWithTag(item);
            foreach (GameObject itemObject in itemObjects)
            {
                itemObject.SetActive(false);
            }
        }
    }

    private void RatePairing(string beef, string drink)
    {
        //Add to string to check for rating given (in Start())
        string comboKey = $"{beef}_{drink}";
        if (comboRatings.TryGetValue(comboKey, out int rating))
        {

            //Convert to string 
            string ratingString = $"Rating for pairing is {rating}";

            //Debug.Log($"Rating for pairing {beef} with {drink}: {rating}");

            if (ratingText != null)
            {
                ratingPanel.SetActive(true);

                //Add to TMP_text
                ratingText.text = ratingString;

                ClickCounter();
            }
        }
        else
        {
            string ratingString = $"No rating found for pairing {beef} with {drink}";

            if (ratingText != null)
            {
                ratingPanel.SetActive(true);
                ratingText.text = ratingString;
            }
        }
    }

    public void ClickCounter()
    {
        clickCounter++;
        if (clickCounter == 5)
        {
            endPagePanel.gameObject.SetActive(true);
            clickCounter = 0;
        }
    }

}
