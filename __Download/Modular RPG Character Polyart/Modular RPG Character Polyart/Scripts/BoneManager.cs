using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneManager : MonoBehaviour
{
    [System.Serializable]
    public class BoneChain
    {
        public enum boneChainType { waist_forward, waist_back, waist_left, waist_right, waist_att1, waist_att2, back,head, left_shoulder, right_shoulder, tail1, tail2, tailL, tailR, moustacheL, moustacheR, beard}
        public boneChainType _type; //BoneChain type
        public GameObject[] bones; //All bones 
        public int[] ItemUseBoneCount; //Item Count used this bone
    }
    public BoneChain[] boneChain;
    public Dictionary<BoneChain.boneChainType, BoneChain> boneChainDictionary = new Dictionary<BoneChain.boneChainType, BoneChain>(); //Dictionary for fast  BoneChain reference
    Dictionary<GameObject, jiggleBone> boneJiggleScriptDictionary = new Dictionary<GameObject, jiggleBone>(); //Dictionary for fast  Bone Jiggle script reference
    [HideInInspector]public Inventory inventory;
    public void Awake()
    {
        foreach (BoneChain bc in boneChain)//Add all BoneChain in Dictionary
        {
            if (!boneChainDictionary.ContainsKey(bc._type))// If Dictionary dont contain this BoneChain
            {
                boneChainDictionary.Add(bc._type, bc);// so add him
                foreach (GameObject bone in bc.bones)
                {
                    boneJiggleScriptDictionary.Add(bone, bone.GetComponent<jiggleBone>());
                }
            }
            if(bc.ItemUseBoneCount.Length != bc.bones.Length)
             bc.ItemUseBoneCount = new int[bc.bones.Length];
        }
    }
    public void removeBones(polysoft_Item item)
    {
        if (inventory.equiped[(int)item.itemType] != null)//If item equiped of this item type "itemType"
        {
            for (int i = 0; i < inventory.equiped[(int)item.itemType].BoneChainOptions.Length; i++) //Look at all BoneChain used by Item
            {
                if (boneChainDictionary.ContainsKey(inventory.equiped[(int)item.itemType].BoneChainOptions[i].BoneUsed))// If Dictionary contain this BoneChain
                {
                    BoneChain bc = boneChainDictionary[inventory.equiped[(int)item.itemType].BoneChainOptions[i].BoneUsed];
                    for (int b = 0; b < inventory.equiped[(int)item.itemType].BoneChainOptions[i].boneLength; b++)
                    {
                        bc.ItemUseBoneCount[b]--; //Item no more use this bone -1 to the count
                        if (bc.ItemUseBoneCount[b] <= 0)//If no item use this bone
                        {
                            bc.ItemUseBoneCount[b] = 0;
                            bc.bones[b].SetActive(false);//Disable it
                        }

                    }
                }
            }
        }
        inventory.equiped[(int)item.itemType] = null;
    }
    public void setBones(polysoft_Item item)
    {
        if (inventory.equiped[(int)item.itemType] != null)//If item equiped of this item type "itemType"
        {
            for (int i = 0; i < inventory.equiped[(int)item.itemType].BoneChainOptions.Length; i++) //Look at all BoneChain used by Item
            {
                if (boneChainDictionary.ContainsKey(inventory.equiped[(int)item.itemType].BoneChainOptions[i].BoneUsed))// If Dictionary contain this BoneChain
                {
                    BoneChain bc = boneChainDictionary[inventory.equiped[(int)item.itemType].BoneChainOptions[i].BoneUsed];
                    for (int b = 0; b < inventory.equiped[(int)item.itemType].BoneChainOptions[i].boneLength; b++)
                    {
                        bc.ItemUseBoneCount[b]--; //Item no more use this bone -1 to the count
                        if (bc.ItemUseBoneCount[b] <= 0)//If no item use this bone
                        {
                            bc.ItemUseBoneCount[b] = 0;
                            bc.bones[b].SetActive(false);//Disable it
                        } 
                         
                    }
                }
            }
        }
        inventory.equiped[(int)item.itemType] = item;
        for (int i = 0; i < item.BoneChainOptions.Length; i++) //Look at all BoneChain used by Item
        {
            if (boneChainDictionary.ContainsKey(item.BoneChainOptions[i].BoneUsed))// If Dictionary contain this BoneChain
            {
                BoneChain bc = boneChainDictionary[item.BoneChainOptions[i].BoneUsed];
                for (int b = 0; b < item.BoneChainOptions[i].boneLength; b++)
                {
                    bc.ItemUseBoneCount[b]++; //Item use this bone +1 to the count
                    if (bc.ItemUseBoneCount[b] == 1)//If bone used by at least one item
                    {
                        bc.bones[b].SetActive(true);//Enable it
                        if (b == item.BoneChainOptions[i].boneLength - 1)//If it is last bone used by item
                            boneJiggleScriptDictionary[bc.bones[b]].enabled = true;//Enable Jiggle script
                        else
                            boneJiggleScriptDictionary[bc.bones[b]].enabled = false;//Disable no last Jiggle script
                    }
                }
            }
        }
    }
    public void setHairBones(polysoft_Hair hair)
    {
        if (inventory.hairUsed[(int)hair.itemType] != null)//If item equiped of this item type "itemType"
        {
            for (int i = 0; i < inventory.hairUsed[(int)hair.itemType].BoneChainOptions.Length; i++) //Look at all BoneChain used by Item
            {
                if (boneChainDictionary.ContainsKey(inventory.hairUsed[(int)hair.itemType].BoneChainOptions[i].BoneUsed))// If Dictionary contain this BoneChain
                {
                    BoneChain bc = boneChainDictionary[inventory.hairUsed[(int)hair.itemType].BoneChainOptions[i].BoneUsed];
                    for (int b = 0; b < inventory.hairUsed[(int)hair.itemType].BoneChainOptions[i].boneLength; b++)
                    {
                        bc.ItemUseBoneCount[b]--; //Item no more use this bone -1 to the count
                        if (bc.ItemUseBoneCount[b] <= 0)//If no item use this bone
                        {
                            bc.ItemUseBoneCount[b] = 0;
                            bc.bones[b].SetActive(false);//Disable it
                        }
                    }
                }
            }
        }
        inventory.hairUsed[(int)hair.itemType] = hair;
        for (int i = 0; i < hair.BoneChainOptions.Length; i++) //Look at all BoneChain used by Item
        {
            if (boneChainDictionary.ContainsKey(hair.BoneChainOptions[i].BoneUsed))// If Dictionary contain this BoneChain
            {
                BoneChain bc = boneChainDictionary[hair.BoneChainOptions[i].BoneUsed];
                for (int b = 0; b < hair.BoneChainOptions[i].boneLength; b++)
                {
                    bc.ItemUseBoneCount[b]++; //Item use this bone +1 to the count
                    if (bc.ItemUseBoneCount[b] == 1)//If bone used by at least one item and no head armor or armor no disable hair
                    {
                        if (!inventory.equiped[0] || (hair.itemType == polysoft_Hair.hairType.hair && !inventory.equiped[0].disableHair) || (hair.itemType == polysoft_Hair.hairType.beard && !inventory.equiped[0].disableBeard) || (hair.itemType == polysoft_Hair.hairType.mustache && !inventory.equiped[0].disableMustache))
                            bc.bones[b].SetActive(true);//Enable it
                        if (b == hair.BoneChainOptions[i].boneLength - 1)//If it is last bone used by hair
                            boneJiggleScriptDictionary[bc.bones[b]].enabled = true;//Enable Jiggle script
                        else
                            boneJiggleScriptDictionary[bc.bones[b]].enabled = false;//Disable no last Jiggle script
                    }
                }
            }
        }
    }
    public void UpdateHairBones(polysoft_Hair hair)
    {
        if (!hair)
            return;
        for (int i = 0; i < hair.BoneChainOptions.Length; i++) //Look at all BoneChain used by Item
        {
            if (boneChainDictionary.ContainsKey(hair.BoneChainOptions[i].BoneUsed))// If Dictionary contain this BoneChain
            {
                BoneChain bc = boneChainDictionary[hair.BoneChainOptions[i].BoneUsed];
                for (int b = 0; b < hair.BoneChainOptions[i].boneLength; b++)
                {
                    if (bc.ItemUseBoneCount[b] == 1 && (!inventory.equiped[0] || (hair.itemType == polysoft_Hair.hairType.hair && !inventory.equiped[0].disableHair) || (hair.itemType == polysoft_Hair.hairType.beard && !inventory.equiped[0].disableBeard) || (hair.itemType == polysoft_Hair.hairType.mustache && !inventory.equiped[0].disableMustache)))//If bone used by at least one item and no head armor or armor no disable hair
                        bc.bones[b].SetActive(true);//Enable it
                    else
                        bc.bones[b].SetActive(false);
                }
            }
        }
    }
}
