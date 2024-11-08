using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class polysoft_Item : ScriptableObject {
    public enum ItemType { helmet, shoulders, torso, waist, legs}
    public enum ArmorType { cloth, leather, plate, outfit}//Other for hair and pants/brassiere
    public string _name;
    public ItemType itemType;
    public ArmorType armorType;
    public SkinnedMeshRenderer ItemMeshRenderer;
    public bool disableHair;
    public bool disableBeard;
    public bool disableMustache;
    [Serializable]
    public class _boneChainOptions
    {
        public BoneManager.BoneChain.boneChainType BoneUsed;
        public int boneLength;
    }
    public _boneChainOptions[] BoneChainOptions;
    [TextArea]
    public string description;
}