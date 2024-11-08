using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class polysoft_Hair : ScriptableObject {
    public Inventory.Gender gender;
    public enum hairType {hair, beard, mustache}
    public string _name;
    public hairType itemType;
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