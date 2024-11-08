using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class breastControl : MonoBehaviour
{
    [Range(0.6f,1.5f)]
    public float BreastSize = 1.0f;
    public Transform[] BreastBones;
    public float minValue = 0.6f;
    public float maxValue = 1.5f;
    public void setupDefaultBreast()
    {
        interfaceSC.Instance.breastSlider.value = BreastSize;
        interfaceSC.Instance.breastSlider.minValue = minValue;
        interfaceSC.Instance.breastSlider.maxValue = maxValue;
    }
    public void SetBreastSize()
    {
        BreastSize = interfaceSC.Instance.breastSlider.value;
        BreastBones[0].localScale = Vector3.one * BreastSize;
        BreastBones[1].localScale = Vector3.one * BreastSize;
    }
    public void SetBreastSizeEditor(float newSize)
    {
        BreastSize = newSize;
        BreastBones[0].localScale = Vector3.one * BreastSize;
        BreastBones[1].localScale = Vector3.one * BreastSize;
    }
}
