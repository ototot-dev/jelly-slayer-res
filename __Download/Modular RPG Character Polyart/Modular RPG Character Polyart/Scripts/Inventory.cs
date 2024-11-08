using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance = null;
    public enum Gender {male, female};
    public Gender gender;
    public BoneManager boneManager;
    public polysoft_Hair[] hairUsed;
    public polysoft_Underwear[] underwearUsed;
    public polysoft_Item[] equiped;
    public Material skinMaterial;
    public Material hairMaterial;
    public Material underwearMaterial;
    public Material itemMaterial;
    public SkinnedMeshRenderer[] BodyParts;//0-head,1-shoulders,2-torso,3-waist,4-legs,5-hair,6-beard,7-mustache,8-pants,9-brassiere
    public breastControl breast_control;
    public SkinnedMeshRenderer hairUnderHelmet; // hair appear only under head armors only if character have hair!
    public Animator anim;

    private void Awake()
    {
        Instance = this;
        boneManager.inventory = this;
        if (equiped.Length < 5)
            equiped = new polysoft_Item[5];
        if (hairUsed.Length < 3)
            hairUsed = new polysoft_Hair[3];
        if (underwearUsed.Length < 2)
            underwearUsed = new polysoft_Underwear[2];
    }
}
