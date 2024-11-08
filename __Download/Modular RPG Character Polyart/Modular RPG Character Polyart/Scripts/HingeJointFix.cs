using UnityEngine;

public class HingeJointFix : MonoBehaviour
{
    private Quaternion StartLocalRotation;
    private Vector3 StartLocalPosition;

    void Awake()
    {
        StartLocalRotation = transform.localRotation;
        StartLocalPosition = transform.localPosition;
    }
    void OnDisable()
    {
        transform.localRotation = StartLocalRotation;
        transform.localPosition = StartLocalPosition;
    }
}