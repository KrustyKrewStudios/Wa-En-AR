/*
 * Author: Curtis Low
 * Date: 06/08/2024
 * Description:   Captures log messages and stack traces and displays them on the UI for debugging purposes

 */
using UnityEngine;
using TMPro;

public class ConsoleToText : MonoBehaviour
{

    public TextMeshProUGUI debugText;
    string output = ""; // Stores the debug messages
    string stack = ""; // Stores the stack trace

    private void OnEnable()
    {
        // Subscribe to the log message received event
        Application.logMessageReceived += HandleLog;
        Debug.Log("Log enabled");
    }

    private void OnDisable()
    {
        // Unsubscribe from the log message received event
        Application.logMessageReceived -= HandleLog;
        // Clear the log when the script is disabled
        ClearLog();
    }

    // Handles incoming log messages and appends them to the output string
    void HandleLog(string logString, string stackTrace, LogType type)
    {
        output = logString+ "\n" + output;
        stack = stackTrace; // Store stack trace for debugging
    }

    private void OnGUI()
    {
        debugText.text = output;
        
    }

    // Clears the current log output
    public void ClearLog()
    {
        output = "";
    }




}
