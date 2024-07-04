using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Debugger : MonoBehaviour
{
    public TextMeshProUGUI debugText; // Assign in Inspector
    private Queue<string> debugMessages = new Queue<string>();
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
            debugMessages.Dequeue();
        }
        debugMessages.Enqueue(message);
        UpdateDebugText();
    }

    private void UpdateDebugText()
    {
        debugText.text = string.Join("\n", debugMessages.ToArray());
    }
}
