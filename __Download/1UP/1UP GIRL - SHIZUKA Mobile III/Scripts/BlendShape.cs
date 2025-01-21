using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace G1UP
{
    public class BlendShape : MonoBehaviour
    {
        public bool autoSwitch = false;
        [Range(0.1f, 10.0f)]
        public float duration = 3.0f;
        float lastDuration = 3.0f;

        int blendShapeCount;
        int currentID;
        int currentWeight = 0;
        SkinnedMeshRenderer skinnedMeshRenderer;
        Mesh skinnedMesh;
        float BlinkAmount = 0;
        bool isBlinking;
        bool isSwitchingBlendShape;
        int switchSpeed = 4;
        int blinkID = 12;
        ////BlendShapes ID Range(Input Mode)
        int startID = 4;
        int endID = 31;
        public float blinkRate = 3.0f;
        //BlendShapes ID Array(Play Mode)
        int[] arrayInt = { 3, 7, 8, 20, 7, 21, 22, 32, 33, 34, 41, 42, 44, 45 };

        void Awake()
        {
            skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
            skinnedMesh = GetComponent<SkinnedMeshRenderer>().sharedMesh;
        }

        void Start()
        {
            blendShapeCount = skinnedMesh.blendShapeCount;
            if (checkBlinkID(blinkID))
                InvokeRepeating("blink", 1.0f, blinkRate);
        }

        void Update()
        {
            if (isBlinking == true)
            {
                // string currentBlendShapeName = skinnedMesh.GetBlendShapeName(currentID);
                //  if (currentBlendShapeName.Contains("Joy"))
                //     {
                //          return;
                //     }

                skinnedMeshRenderer.SetBlendShapeWeight(blinkID, BlinkAmount);
                BlinkAmount = BlinkAmount + 25;

                if (BlinkAmount > 180)
                {
                    BlinkAmount = 0;
                    skinnedMeshRenderer.SetBlendShapeWeight(blinkID, BlinkAmount);
                    isBlinking = false;
                }

            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                skinnedMeshRenderer.SetBlendShapeWeight(currentID, 0);
                //currentID = (int)Random.Range(0, blendShapeCount);
                currentID = (int)Random.Range(startID, endID);

                if (!checkExtraBlendShape(currentID))
                    skinnedMeshRenderer.SetBlendShapeWeight(currentID, 100);
            }

            lastDuration -= Time.deltaTime;
            if (lastDuration <= 0.0f)
            {
                lastDuration = duration;
                if (autoSwitch) switchBlendShape();
            }
            if (!checkExtraBlendShape(currentID))
            {
                currentWeight = (int)(100.0 * (1.0 - (lastDuration / duration))) * switchSpeed;
                if (currentWeight <= 100)
                    skinnedMeshRenderer.SetBlendShapeWeight(currentID, currentWeight);
            }
        }

        bool checkExtraBlendShape(int currentID)
        {
            string currentBlendShapeName = skinnedMesh.GetBlendShapeName(currentID);
            if (currentBlendShapeName.Contains("Iris") || currentBlendShapeName.Contains("Extra") || currentBlendShapeName.Contains("MTH_Down"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        bool checkBlinkID(int blinkID)
        {
            string currentBlendShapeName = skinnedMesh.GetBlendShapeName(blinkID);
            if (currentBlendShapeName == "Fcl_EYE_Close")
            {
                return true;
            }
            else
            {
                Debug.Log("Need to check whether the BlinkID is set correctly.");
                return false;
            }
        }

        void switchBlendShape()
        {
            if (checkExtraBlendShape(currentID))
            {
                return;
            }
            else
            {
                currentWeight = 0;
                skinnedMeshRenderer.SetBlendShapeWeight(currentID, currentWeight);
                //currentID = (int)Random.Range(startID, endID);
                currentID = arrayInt[(int)Random.Range(0, arrayInt.Length - 1)];
                skinnedMeshRenderer.SetBlendShapeWeight(currentID, currentWeight);

            }

        }

        void blink()
        {
            isBlinking = true;

        }
    }
}