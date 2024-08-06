/*
 * Author: Curtis Low
 * Date: 06/08/2024
 * Description:
 * This class provides functionality for outlining selected objects in the scene.
 * It maintains a singleton instance and manages the application and removal of 
 * outline materials to highlight selected objects. It handles restoring original
 * materials when selection changes.
 */
using UnityEngine;

public class OutlineSelection : MonoBehaviour
{
    // Singleton instance of OutlineSelection
    public static OutlineSelection Instance { get; private set; }

    // Material used for outlining selected objects
    public Material outlineMaterial;

    // Reference to the renderer of the previously selected object  
    private Renderer previousSelectionRenderer;
    // Array of materials of the previously selected object
    private Material[] previousMaterials;

    private void Awake()
    {
        // Implement singleton pattern to ensure only one instance of OutlineSelection exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Apply outline to the specified beef object
    public void OutlineBeef(GameObject beef)
    {
        // Remove outline from the previous selection, if any
        if (previousSelectionRenderer != null)
        {
            RestoreMaterials(previousSelectionRenderer);
        }

        // Apply the outline material to the new selection
        var renderer = beef.GetComponent<Renderer>();
        if (renderer != null)
        {
            // Save current materials and renderer
            previousSelectionRenderer = renderer;
            previousMaterials = renderer.materials;

            // Create a new array with one more slot for the outline material
            var materials = new Material[renderer.materials.Length + 1];
            for (int i = 0; i < renderer.materials.Length; i++)
            {
                materials[i] = renderer.materials[i];
            }
            materials[materials.Length - 1] = outlineMaterial;

            // Set the new materials array to the renderer
            renderer.materials = materials;
        }
    }

    // Remove outline from the currently selected object
    public void Unselect()
    {
        // Remove outline from the previously selected object
        if (previousSelectionRenderer != null)
        {
            RestoreMaterials(previousSelectionRenderer);
            previousSelectionRenderer = null;
            previousMaterials = null;
        }
    }

    // Restore the original materials of the specified renderer
    private void RestoreMaterials(Renderer renderer)
    {
        // Restore the original materials
        if (previousMaterials != null)
        {
            renderer.materials = previousMaterials;
        }
    }
}
