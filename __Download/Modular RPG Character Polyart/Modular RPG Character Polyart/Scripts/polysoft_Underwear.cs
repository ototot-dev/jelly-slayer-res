using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class polysoft_Underwear : ScriptableObject {
    public Inventory.Gender gender;
    public enum underwearType {pants, brassiere}
    public string _name;
    public underwearType itemType;
    public SkinnedMeshRenderer ItemMeshRenderer;
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