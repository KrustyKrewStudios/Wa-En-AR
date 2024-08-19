/*
 * Name: Bhoomika Manot
 * Date: 1 July 2024
 * Description: Code for spawning drinks, detecting combos & counting the number of times players have played to spawn panel for reward
 */

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
    public GameObject invalidPanel;
    public ParticleSystem cheerEffect;
    public GameObject cheerFX;
    public AudioSource cheerAudio;

    //Drinks declaration
    public Transform spawnPoint;
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
    private Dictionary<string, (int rating, string description)> comboRatings = new Dictionary<string, (int, string)>();

    void Start()
    {
        //Initialize Combo ratings
        comboRatings.Add("Chuck_Campfire", (7, "Works well but less optimal than Ribeye or Sirloin."));
        comboRatings.Add("Chuck_Martini", (6, "Works but doesn’t highlight the cocktail’s citrus as well."));
        comboRatings.Add("Chuck_Sake", (8, "Pairs well with the umami depth, enhancing the Sake’s flavour."));
        comboRatings.Add("Chuck_OldFashioned", (7, "Works well with the whiskey’s complexity, though not as ideal as Ribeye or Sirloin."));
        comboRatings.Add("Sirloin_Campfire", (8, "Good match as the smoky notes complement the Sirloin's balance."));
        comboRatings.Add("Sirloin_Martini", (9, "Complements the Sirloin’s balanced flavour with its citrusy notes."));
        comboRatings.Add("Sirloin_Sake", (7, "Good match but not as impactful as with Chuck."));
        comboRatings.Add("Sirloin_OldFashioned", (8, "Complements the Sirloin nicely, adding a sophisticated touch to its balanced taste."));
        comboRatings.Add("Ribeye_Campfire", (10, "Pairs well with Ribeye due to its rich, smoky flavour enhancing the marbled texture."));
        comboRatings.Add("Ribeye_Martini", (7, "Decent pairing but less vibrant than with Sirloin."));
        comboRatings.Add("Ribeye_Sake", (6, "Less synergy with the Sake’s flavour profile."));
        comboRatings.Add("Ribeye_OldFashioned", (10, "The rich, fat-washed whiskey pairs exceptionally well with the marbled Ribeye, enhancing its flavours."));
        comboRatings.Add("Karubi_Campfire", (6, "Less ideal due to competing flavours with the smoky profile."));
        comboRatings.Add("Karubi_Martini", (5, "Less effective due to a mismatch with the cocktail's bright notes."));
        comboRatings.Add("Karubi_Sake", (7, "Complements the Sake, though slightly less effective than Chuck."));
        comboRatings.Add("Karubi_OldFashioned", (6, "Good pairing, but the fat-washed whiskey might not stand out as much."));
        comboRatings.Add("BeefTongue_Campfire", (5, "Not the best match as its milder flavour doesn't stand out."));
        comboRatings.Add("BeefTongue_Martini", (5, "Less effective due to a mismatch with the cocktail's bright notes."));
        comboRatings.Add("BeefTongue_Sake", (5, "Not a great fit as its subtle flavour doesn’t pair well with the Martini."));
        comboRatings.Add("BeefTongue_OldFashioned", (5, "Less suitable as its milder flavour doesn’t match the bold whiskey notes."));

        cheerEffect = GetComponent<ParticleSystem>();
    }

    //Check for drink
    private bool IsAnyDrinkActive()
    {
        foreach (string drink in drinkItems)
        {
            if (IsItemActive(drink))
            {
                return true;
            }
        }
        return false;
    }

    //Spawn drinks
    public void SpawnCampfire()
    {
        if (IsAnyDrinkActive())
        {
            ShowInvalidMessage();
        }
        else if (campfire != null && spawnPoint != null)
        {
            Instantiate(campfire, spawnPoint.position, spawnPoint.rotation);
        }
    }

    public void SpawnSake()
    {
        if (IsAnyDrinkActive())
        {
            ShowInvalidMessage();
        }
        else if (sake != null && spawnPoint != null)
        {
            Instantiate(sake, spawnPoint.position, spawnPoint.rotation);
        }
    }

    public void SpawnMartini()
    {
        if (IsAnyDrinkActive())
        {
            ShowInvalidMessage();
        }
        else if (martini != null && spawnPoint != null)
        {
            Instantiate(martini, spawnPoint.position, spawnPoint.rotation);
        }
    }

    public void SpawnOldFashioned()
    {
        if (IsAnyDrinkActive())
        {
            ShowInvalidMessage();
        }
        else if (oldFashioned != null && spawnPoint != null)
        {
            Instantiate(oldFashioned, spawnPoint.position, spawnPoint.rotation);
        }
    }

    //To prevent multiple drinks from spawning
    private void ShowInvalidMessage()
    {
        string invalidMessage = "Cannot spawn a new drink while another drink is present";
        ratingText.text = invalidMessage;
        invalidPanel.SetActive(true);
    }

    //Detect items via OnTrigger & Tags
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.activeInHierarchy)
        {
            string tag = other.tag;
            Debug.Log($"Tag detected: {tag}");
            detectedItems.Add(tag);
        }
    }

    //To remove items from being undetected
    private bool IsItemActive(string itemTag)
    {
        GameObject[] itemObjects = GameObject.FindGameObjectsWithTag(itemTag);
        foreach (GameObject itemObject in itemObjects)
        {
            if (itemObject.activeInHierarchy)
            {
                return true;
            }
        }
        return false;
    }

    //Check for combination via tags
    public void CheckForCombinations()
    {
        bool validPairFound = false;

        // Check for which beef
        foreach (string beef in beefItems)
        {
            // Check for which drink
            foreach (string drink in drinkItems)
            {
                if (IsItemActive(beef) && IsItemActive(drink))
                {
                    if (detectedItems.Contains(beef) && detectedItems.Contains(drink))
                    {
                        //Call function for rating items 
                        RatePairing(beef, drink);
                        validPairFound = true;

                        //Add count
                        ClickCounter();

                        //Exit the loop if a valid pair is found
                        break; 
                    }
                }
            }
            if (validPairFound)
            {
                //Exit the loop if a valid pair is found
                break; 
            }
        }

        // If no valid pair is found, set the rating to invalid
        if (!validPairFound)
        {
            string ratingString = "Rating invalid";
            ratingText.text = ratingString;
            ratingPanel.SetActive(true);
        }
    }

    //Remove items once rated
    public void DeactivateRatedItems()
    {
        List<string> itemsToDeactivate = new List<string>();

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

    //Rate pairings
    private void RatePairing(string beef, string drink)
    {
        //Convert to string
        string comboKey = $"{beef}_{drink}";

        //Getting rating
        if (comboRatings.TryGetValue(comboKey, out var ratingInfo))
        {
            string ratingString = $"Rating for pairing is {ratingInfo.rating}\n{ratingInfo.description}";
            ratingText.text = ratingString;
            ratingPanel.SetActive(true);
        }
        else
        {
            string ratingString = "Rating invalid";
            ratingText.text = ratingString;
            ratingPanel.SetActive(true);
        }
    }

    //Add count to give free dish
    public void ClickCounter()
    {
        clickCounter++;
        Debug.Log("Click count:" + clickCounter);

        if (clickCounter == 5)
        {
            endPagePanel.gameObject.SetActive(true);
            clickCounter = 0;
            cheerFX.gameObject.SetActive(true);
            cheerAudio.Play();
            //cheerEffect.Play();
        }
    }

    //Close Cheer FX on Button Click
    public void EndCheerFX()
    {
        cheerFX.gameObject.SetActive(false);
        cheerAudio.Stop();
        //cheerEffect.Stop();
    }

}

