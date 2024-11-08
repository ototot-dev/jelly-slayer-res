using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDel : MonoBehaviour {
    public float lifeTime = 5.0f;
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }
}
