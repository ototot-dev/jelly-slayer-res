using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneScript : MonoBehaviour
{
    public static SceneScript Instance = null;
    public Vector3 LookPosition;
    public Transform Character;
    private Vector2 MousePos;
    private Vector2 oldMousePos;
    private float Yrot = 180;
    private float zoomZ;
    public float rotateCharacter = 0;
    public GameObject canvas;
    private void Awake()
    {
        Instance = this;
        canvas.SetActive(true);
    }
    void Start()
    {
        if (!Character)
        {
            Character = Inventory.Instance.transform;
        }
        zoomZ = -2f;
        oldMousePos = MousePos;
        SetLookPosition(0);
    }
    public void AutoRotateCharacter()
    {
        if (rotateCharacter == 0)
            rotateCharacter = 2;
        else
            rotateCharacter = 0;
    }
    public void SetLookPosition(int i)
    {
        if (i == 0)
            LookPosition = new Vector3(0, 1.1f,0);//look center body
        else
            LookPosition = new Vector3(0, 1.4f, 0);//look head
    }
    void Update()
    {
        if (Input.GetMouseButton(0))
            MousePos =  Input.mousePosition;
        if (Input.GetMouseButtonDown(0))
            oldMousePos = MousePos;
        Yrot += rotateCharacter + (oldMousePos.x - MousePos.x) * 20f * Time.deltaTime;
        Character.eulerAngles = new Vector3(0,Mathf.LerpAngle(Character.eulerAngles.y , Yrot, 6f * Time.deltaTime),0);
       
        float ScrollWheel = Input.GetAxis("Mouse ScrollWheel");

        zoomZ += ScrollWheel * 200f * Time.deltaTime;
        zoomZ = Mathf.Clamp(zoomZ, -2.5f, -1.0f);
        transform.position = Vector3.Lerp(transform.position, LookPosition+Vector3.forward*zoomZ, 10f * Time.deltaTime);

        oldMousePos = MousePos;
    }
}
