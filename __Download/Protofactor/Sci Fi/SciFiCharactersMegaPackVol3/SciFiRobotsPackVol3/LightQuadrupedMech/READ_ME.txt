First and foremost: Thanks for purchasing this asset! We do hope you'll enjoy using it in your project.

We developed the Light Quadruped Mech in such a way that it doesn't have any death animation. Instead we decided to provide a split into pieces version of the mech in its idle active pose.

Developer simply needs to create a small script in order to add and activate an explosion VFX as well as adding force to the separate pieces in order to mimic a nice looking explosion.

As an example, you can use a script like this (Make sure to attach the Script to the root Game Object: "LightQuadrupedMech_SplitIntoPieces" prefab):

////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Protofactor
{
public class RobotExplosionHandler : MonoBehaviour
{
    [Tooltip("The force magnitude applied to the robot pieces.")]
    public float explosionForce = 10.0f;

    [Tooltip("The radius within which the explosion force affects.")]
    public float explosionRadius = 5.0f;

    [Tooltip("The position where the explosion force originates from.")]
    public Transform explosionSource;

    private void OnEnable()
    {
        if (explosionSource == null)
        {
            explosionSource = transform; // default to the position of the robot itself.
        }
        
        //Explode(); //uncomment if you want the explosion to happen on Enable()
    }

    private void Start()
    {
        if (explosionSource == null)
        {
            explosionSource = transform; // default to the position of the robot itself.
        }
        
        //Explode(); //uncomment if you want the explosion to happen on Start()
    }

    // Call this method to explode the robot. You can manually call this public method or inside a unity inspector event as well
    public void Explode()
    {
        Debug.Log("Explosion of Body Parts Called");
        foreach (Transform child in transform)
        {
            Rigidbody rb = child.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Debug.Log("Applying explosion to " + child.name);
                rb.AddExplosionForce(explosionForce, explosionSource.position, explosionRadius);
            }
            else
            {
                Debug.Log("No Rigidbody found on " + child.name);
            }
        }
    }
}
}
////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////