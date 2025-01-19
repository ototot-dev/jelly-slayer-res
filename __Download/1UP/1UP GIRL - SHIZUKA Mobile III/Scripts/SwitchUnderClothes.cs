using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace G1UP
{
    public class SwitchUnderClothes : MonoBehaviour
    {
        //AutoSwitch Function
        public bool autoSwitch = false;
        public float switchFrequency = 6.0f;

        //SwitchEffect
        public GameObject effectPrefab;
        public Transform[] effectTargets;

        //UnderClothes
        public Material[] underClothes;
        int currentUnderMatID;
        int underBodyMatID = 1;
        public GameObject[] underGirlBody;

        // //OuterClothes
        // public Material[] outerClothes;
        // int currentOuterMatID;
        // int OuterBodyMatID = 3;
        // public GameObject[] outerGirlBody;

        // //Eyes
        // public Material[] _eyeMats;
        // int currentEyeMatID;
        // int eyeMatID = 2;
        // public GameObject[] eyes;

        //Items
        public GameObject[] items;
        private int currentItemIndex;


        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(AutoSwitchClothes());
        }

        IEnumerator AutoSwitchClothes()
        {
            while (true)//Timing execution
            {
                yield return new WaitForSeconds(switchFrequency);
                if (autoSwitch)
                {
                    SwitchModelClothes();
                    NextItem();
                }
            }
        }

        void SwitchModelClothes()
        {
            foreach (var target in effectTargets)
            {
                GameObject cloneEffect = Instantiate(effectPrefab, target.position, Quaternion.identity);
                cloneEffect.transform.SetParent(target.transform);
            }

            //UnderClothes

            foreach (var body in underGirlBody)
            {
                Material currentUnderMat = underClothes[currentUnderMatID];
                //Debug.Log("currentUnderMatID" + currentUnderMatID);
                var mat = body.GetComponent<Renderer>().materials[underBodyMatID];
                if (mat.name.Contains("BodyBase"))
                {
                    mat.CopyPropertiesFromMaterial(currentUnderMat);
                }
                else
                {
                    Debug.Log("Need to select the correct Body Material");
                }
                currentUnderMatID++;
                if (currentUnderMatID >= underClothes.Length)
                {
                    currentUnderMatID = 0;
                }
            }

            if (currentUnderMatID > 0)
                currentUnderMatID--;

            // //OuterClothes
            // Material currentOuterMat = outerClothes[currentOuterMatID++];
            // foreach (var body in outerGirlBody)
            // {

            //     var mat = body.GetComponent<Renderer>().materials[OuterBodyMatID];
            //     if (mat.name.Contains("Cloth"))
            //     {
            //         mat.CopyPropertiesFromMaterial(currentOuterMat);
            //     }
            //     else
            //     {
            //         Debug.Log("Need to select the correct Cloth Material");
            //     }
            //     if (currentOuterMatID >= outerClothes.Length)
            //     {
            //         currentOuterMatID = 0;
            //     }
            // }

            // //Eyes
            // Material currentEyeMat = _eyeMats[currentEyeMatID++];
            // foreach (var eye in eyes)
            // {

            //     var mat = eye.GetComponent<Renderer>().materials[eyeMatID];
            //     if (mat.name.Contains("Eyes"))
            //     {
            //         mat.CopyPropertiesFromMaterial(currentEyeMat);
            //     }
            //     else
            //     {
            //         Debug.Log("Need to select the correct Eyes Material");
            //     }
            //     if (currentEyeMatID >= _eyeMats.Length)
            //     {
            //         currentEyeMatID = 0;
            //     }
            // }
        }

        public void NextItem()
        {
            foreach (var item in items)
            {
                item.SetActive(false);
            }
            items[currentItemIndex].SetActive(true);
            currentItemIndex++;
            items[currentItemIndex].SetActive(true);
            if (currentItemIndex >= items.Length - 1)
            {
                currentItemIndex = 0;
            }

        }


        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SwitchModelClothes();
                NextItem();
            }
        }
    }
}