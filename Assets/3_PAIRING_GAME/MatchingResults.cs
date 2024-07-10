using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MatchingResults : MonoBehaviour
{
    private HashSet<string> detectedItems = new HashSet<string>();

    public string[] beefItems = { "Chuck", "Sirloin", "Ribeye", "ShortRib", "BeefTongue" };
    public string[] drinkItems = { "Campfire", "Martini", "Sake", "Rose" };

    // Dictionary to store ratings for each combination
    private Dictionary<string, int> comboRatings = new Dictionary<string, int>();

    public TMP_Text ratingText;

    void Start()
    {
        //Initialize comboRatings with your ratings
        comboRatings.Add("Chuck_Campfire", 8);
        comboRatings.Add("Chuck_Martini", 6);
        comboRatings.Add("Chuck_Sake", 8);
        comboRatings.Add("Chuck_Rose", 6);
        comboRatings.Add("Sirloin_Campfire", 8);
        comboRatings.Add("Sirloin_Martini", 9);
        comboRatings.Add("Sirloin_Sake", 7);
        comboRatings.Add("Sirloin_Rose", 7);
        comboRatings.Add("Ribeye_Campfire", 10);
        comboRatings.Add("Ribeye_Martini", 7);
        comboRatings.Add("Ribeye_Sake", 6);
        comboRatings.Add("Ribeye_Rose", 8);
        comboRatings.Add("ShortRib_Campfire", 6);
        comboRatings.Add("ShortRib_Martini", 5);
        comboRatings.Add("ShortRib_Sake", 7);
        comboRatings.Add("ShortRib_Rose", 9);
        comboRatings.Add("BeefTongue_Campfire", 5);
        comboRatings.Add("BeefTongue_Martini", 5);
        comboRatings.Add("BeefTongue_Sake", 5);
        comboRatings.Add("BeefTongue_Rose", 5);

        Debug.Log("Combo Initialized");
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger script working");

        string tag = other.tag;
        detectedItems.Add(tag);

        CheckForCombinations();
    }

    /*void OnTriggerExit(Collider other)
    {
        string tag = other.tag;
        detectedItems.Remove(tag);
    }*/

    private void CheckForCombinations()
    {
        foreach (string beef in beefItems)
        {
            foreach (string drink in drinkItems)
            {
                if (detectedItems.Contains(beef) && detectedItems.Contains(drink))
                {
                    Debug.Log("Rated!");
                    RatePairing(beef, drink);
                    // Optionally reset the state after rating
                    //detectedItems.Remove(beef);
                    //detectedItems.Remove(drink);
                }
            }
        }
    }

    private void RatePairing(string beef, string drink)
    {
        string comboKey = $"{beef}_{drink}";
        if (comboRatings.TryGetValue(comboKey, out int rating))
        {
            Debug.Log("Giving Rating");
            string ratingString = $"Rating for pairing {beef} with {drink}: {rating}";
            Debug.Log($"Rating for pairing {beef} with {drink}: {rating}");

            if (ratingText != null)
            {
                ratingText.text = ratingString;
            }
        }
        else
        {
            Debug.Log($"No rating found for pairing {beef} with {drink}");
        }
    }
}
