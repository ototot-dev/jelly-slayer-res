using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class MoveCamera : MonoBehaviour
{
    public GameObject targetObject = null;
    public Toggle uiToggle;
    [Range(1.0f, 20.0f)]
    public float speed = 10.0f;
    [Header("Camera View Params(Far)")]
    public Vector3 rotationFar = new Vector3(30, 215, 0);
    public Vector3 offsetFar = new Vector3(0, 0, 0);
    public float distanceFar = 20.0f;
    [Header("Camera View Params(Near)")]
    public Vector3 rotationNear = new Vector3(30, 215, 0);
    public Vector3 offsetNear = new Vector3(0, 0, 0);
    public float distanceNear = 8.0f;

    Camera cam;
    float currDistance = 20.0f;

    bool qPressed = false;
    bool ePressed = false;
    bool camNear = false;
    void Start()
    {
        cam = this.GetComponent<Camera>();
        if (cam == null || targetObject == null) return;
        cam.transform.rotation = Quaternion.Euler(rotationFar);
        currDistance = distanceFar;
    }

    // Update is called once per frame
    void Update()
    {
        qPressed |= Input.GetKeyDown(KeyCode.Q);
        qPressed &= !Input.GetKeyUp(KeyCode.Q);
        ePressed |= Input.GetKeyDown(KeyCode.E);
        ePressed &= !Input.GetKeyUp(KeyCode.E);

        if (Input.GetKeyDown(KeyCode.C))
        {
            camNear = !camNear;
        }
    }

    private void FixedUpdate()
    {
        if (uiToggle != null) uiToggle.isOn = camNear;
        if (cam == null || targetObject == null) return;

        if (camNear)
        {
            currDistance -= Time.deltaTime * speed;
            currDistance = Mathf.Max(currDistance, distanceNear);
        }
        else
        {
            currDistance += Time.deltaTime * speed;
            currDistance = Mathf.Min(currDistance, distanceFar);
        }

        Quaternion quatFar = Quaternion.Euler(rotationFar);
        Quaternion quatNear = Quaternion.Euler(rotationNear);
        Vector3 rotAngle = Quaternion.AngleAxis(Time.deltaTime * 50, Vector3.up).eulerAngles;
        if (qPressed)
        {
            quatFar = Quaternion.Euler(quatFar.eulerAngles + rotAngle);
            quatNear = Quaternion.Euler(quatNear.eulerAngles + rotAngle);
        }

        if (ePressed)
        {
            quatFar = Quaternion.Euler(quatFar.eulerAngles - rotAngle);
            quatNear = Quaternion.Euler(quatNear.eulerAngles + rotAngle);
        }

        rotationFar = quatFar.eulerAngles;
        rotationNear = quatNear.eulerAngles;

        float lerpT =
            (currDistance - distanceNear) / (distanceFar - distanceNear);
        cam.transform.rotation =
            Quaternion.Lerp(quatNear, quatFar, lerpT);
        cam.transform.position =
            targetObject.transform.position -
            Vector3.Normalize(cam.transform.forward) * currDistance +
            Vector3.Lerp(offsetNear, offsetFar, lerpT);
    }
}
