/*
 * Author: Curtis Low
 * Date: 06/08/2024
 * Description: manages displaying debug messages in UI component.
 * It maintains a queue of recent debug messages, ensuring only a specified
 * maximum number of messages are displayed at any time.
 * It provides methods to add new messages and update the UI text accordingly.
 */
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Debugger : MonoBehaviour
{
    public TextMeshProUGUI debugText; 
    // Queue to store recent debug messages
    private Queue<string> debugMessages = new Queue<string>();
    // Maximum number of messages to display
    public int maxMessages = 10;

    private void Start()
    {
        if (debugText == null)
        {
            Debug.LogError("DebugText is not assigned.");
        }
    }

    public void AddDebugMessage(string message)
    {
        if (debugMessages.Count >= maxMessages)
        {
            // Remove the oldest message if the queue is full
            debugMessages.Dequeue();
        }
        // Add the new message to the queue
        debugMessages.Enqueue(message);
        // Update the UI with the latest messages
        UpdateDebugText();
    }

    private void UpdateDebugText()
    {
        // Display all messages in the queue
        debugText.text = string.Join("\n", debugMessages.ToArray());
    }
}
