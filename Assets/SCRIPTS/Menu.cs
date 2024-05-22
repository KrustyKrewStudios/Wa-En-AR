using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GameObject[  ] menuItems;
    private int currentItemIndex = 0;

    void Start()
    {
        SetActiveMenuItem(currentItemIndex);
    }
        
    void SetActiveMenuItem(int index)
    {
        foreach (GameObject menuItem in menuItems)
        {
            menuItem.SetActive(false);
        }
        menuItems[index].SetActive(true);
    }

    public void NextItem()
    {
        currentItemIndex = (currentItemIndex + 1) % menuItems.Length;
        SetActiveMenuItem(currentItemIndex);
    }

    // Function to cycle to the previous menu item
    public void PreviousItem()
    {
        // Decrement the current index
        currentItemIndex--;
        // If the index is less than 0, wrap around to the end of the array
        if (currentItemIndex < 0)
        {
            currentItemIndex = menuItems.Length - 1;
        }
        // Set the active menu item based on the new index
        SetActiveMenuItem(currentItemIndex);
    }


}
