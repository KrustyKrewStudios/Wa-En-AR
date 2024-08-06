using UnityEngine;

public class OutlineSelection : MonoBehaviour
{
    public static OutlineSelection Instance { get; private set; }

    public Material outlineMaterial; // Drag your outline material here in the inspector

    private Renderer previousSelectionRenderer;
    private Material[] previousMaterials;

    private void Awake()
    {
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

    private void RestoreMaterials(Renderer renderer)
    {
        // Restore the original materials
        if (previousMaterials != null)
        {
            renderer.materials = previousMaterials;
        }
    }
}
