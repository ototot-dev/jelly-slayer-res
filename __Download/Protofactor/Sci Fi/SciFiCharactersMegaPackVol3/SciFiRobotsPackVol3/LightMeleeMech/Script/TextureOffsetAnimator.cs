using UnityEngine;
#pragma warning disable 0108

namespace Protofactor
{
    public class TextureOffsetAnimator : MonoBehaviour
{
    public Vector2 offsetSpeed = new Vector2(1.0f, 0.0f); // Speed of the texture offset

    private Renderer renderer;

    void Start()
    {
        renderer = GetComponent<Renderer>();
        if (renderer == null)
        {
            Debug.LogError("Renderer component not found.");
        }
    }

    void Update()
    {
        if (renderer == null) return;

        // Calculate new offset
        Vector2 offset = renderer.material.mainTextureOffset;
        offset += offsetSpeed * Time.deltaTime;

        // Apply new offset
        renderer.material.mainTextureOffset = offset;
    }
}
}