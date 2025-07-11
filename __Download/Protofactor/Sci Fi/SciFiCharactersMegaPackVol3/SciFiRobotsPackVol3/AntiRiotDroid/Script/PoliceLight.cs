using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceLight : MonoBehaviour
{
    [SerializeField] private Material targetMaterial; // Material to apply the scroll effect
    [SerializeField] private float scrollSpeedX = 0.5f; // Speed of the texture scroll on X-axis
    [SerializeField] private float scrollSpeedY = 0.5f; // Speed of the texture scroll on Y-axis
    [SerializeField] private bool scrollX = true; // Enable scrolling on X-axis
    [SerializeField] private bool scrollY = false; // Enable scrolling on Y-axis

    private Vector2 offset;
    private readonly string[] textureProperties =
    {
        "_MainTex",      // Base color/albedo
        "_BumpMap",      // Normal map
        "_EmissionMap",  // Emissive map
        "_MetallicGlossMap", // Metallic map
        "_OcclusionMap", // Ambient occlusion
        "_DetailAlbedoMap", // Detail albedo
        "_DetailNormalMap" // Detail normal
    };

    void Start()
    {
        if (targetMaterial == null)
        {
            Renderer renderer = GetComponent<Renderer>();
            if (renderer != null)
            {
                targetMaterial = renderer.material;
            }
        }

        // Initialize offset from _MainTex (if available) to keep textures in sync
        if (targetMaterial.HasProperty("_MainTex"))
        {
            offset = targetMaterial.GetTextureOffset("_MainTex");
        }
    }

    void Update()
    {
        // Calculate scroll increments for each axis
        float scrollValueX = scrollX ? scrollSpeedX * Time.deltaTime : 0f;
        float scrollValueY = scrollY ? scrollSpeedY * Time.deltaTime : 0f;

        // Update offset based on enabled axes
        offset.x += scrollValueX;
        offset.y += scrollValueY;

        // Apply offset to all texture properties that exist in the material
        foreach (string texProperty in textureProperties)
        {
            if (targetMaterial.HasProperty(texProperty))
            {
                targetMaterial.SetTextureOffset(texProperty, offset);
            }
        }
    }
}