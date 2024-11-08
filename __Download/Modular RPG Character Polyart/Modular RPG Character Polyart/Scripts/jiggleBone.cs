//
// One script enabled at the last character joint.  BoneManager enable and disable it automatically 
//

using UnityEngine;

public class jiggleBone : MonoBehaviour
{
    private Transform myT;
    public float speedPosInterpolate = 20;
    public float speedRotInterpolate = 20;
    public float maxUpdatePos = 0.05f;
    public float maxUpdateRot = 6;

    private Vector3 oldWorldPosBone;
    private Quaternion oldWorldRotBone;
    private Vector3 fromWorldPosBone;
    private Quaternion fromdWorldRotBone;
    private Vector3 toPosition;
    private Quaternion toRotation;

    void Awake()
    {
        myT = transform;
        oldWorldPosBone = myT.position;
        oldWorldRotBone = myT.rotation;
    }

    void LateUpdate()
    {
        //Update bones
        fromWorldPosBone = myT.position;
        fromdWorldRotBone = myT.rotation;
        toPosition = Vector3.Slerp(oldWorldPosBone, myT.position, Time.deltaTime * speedPosInterpolate);
        toRotation = Quaternion.Slerp(oldWorldRotBone, myT.rotation, Time.deltaTime * speedRotInterpolate);
        myT.position = Vector3.MoveTowards(fromWorldPosBone, toPosition, maxUpdatePos);
        myT.rotation = Quaternion.RotateTowards(fromdWorldRotBone, toRotation, maxUpdateRot);
        oldWorldPosBone = myT.position;
        oldWorldRotBone = myT.rotation;
    }
}