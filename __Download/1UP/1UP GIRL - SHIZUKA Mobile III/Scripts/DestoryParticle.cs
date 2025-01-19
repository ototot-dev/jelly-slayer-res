using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace G1UP
{
    public class DestoryParticle : MonoBehaviour
    {
        private ParticleSystem system;

        void Update()
        {

            if (system == null)
            {
                system = GetComponent<ParticleSystem>();
            }

            if (system != null && !system.IsAlive(true))
            {
                Destroy(gameObject);
            }
        }
    }
}