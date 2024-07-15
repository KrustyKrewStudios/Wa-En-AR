using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MatchingResults : MonoBehaviour
{
    //Declarations
    public GameObject drinkTest;

    public TMP_Text ratingText;

    private HashSet<string> detectedItems = new HashSet<string>();

    //List of Items
    private string[] beefItems = { "Chuck", "Sirloin", "Ribeye", "Karubi", "BeefTongue" };
    private string[] drinkItems = { "Campfire", "Martini", "Sake", "OldFashioned" };

    //Dictionary to store ratings for each combination
    private Dictionary<string, int> comboRatings = new Dictionary<string, int>();

    public void ActiveSet()
    {
        drinkTest.gameObject.SetActive(true);
    }

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
            //Debug.Log("Checking beef: " + beef);

            //Check for which drink
            foreach (string drink in drinkItems)
            {
                //Debug.Log("Checking drink: " + drink);
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

        //Check for which beef
        foreach (string beef in beefItems)
        {
            //Check for which drink
            foreach (string drink in drinkItems)
            {
                if (detectedItems.Contains(beef) && detectedItems.Contains(drink))
                {
                    //Set inactive
                    itemsToDeactivate.Add(beef);
                    itemsToDeactivate.Add(drink);
                }
            }
        }

        //Remove from list of detected items
        foreach (string item in itemsToDeactivate)
        {
            GameObject[] itemObjects = GameObject.FindGameObjectsWithTag(item);
            foreach (GameObject itemObject in itemObjects)
            {
                itemObject.SetActive(false);
            }
            detectedItems.Remove(item);
        }
    }

    private void RatePairing(string beef, string drink)
    {
        //Add to string to check for rating given (in Start())
        string comboKey = $"{beef}_{drink}";
        if (comboRatings.TryGetValue(comboKey, out int rating))
        {

            //Convert to string 
            string ratingString = $"Rating for pairing {beef} with {drink}: {rating}";

            //Debug.Log($"Rating for pairing {beef} with {drink}: {rating}");

            if (ratingText != null)
            {
                //Add to TMP_text
                ratingText.text = ratingString;
            }
        }
        else
        {
            string ratingString = $"No rating found for pairing {beef} with {drink}";

            if (ratingText != null)
            {
                ratingText.text = ratingString;
            }
        }
    }
}
