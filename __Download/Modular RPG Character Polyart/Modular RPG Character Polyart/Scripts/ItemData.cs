using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ItemData : MonoBehaviour
{
    private interfaceSC InterfaceSC;
    public Inventory inventory;
    public static ItemData Instance = null;
    public polysoft_Item[] Items;
    private int armorTypeSelected;
    private int itemTypeSelected;
    public List<polysoft_Item> sortedItems = new List<polysoft_Item>();
    public polysoft_Hair[] hair;
    private List<polysoft_Hair> sortedHair = new List<polysoft_Hair>();
    public polysoft_Hair[] beard; // only male
    public polysoft_Hair[] mustache; // only male
    public polysoft_Underwear[] pants;
    private List<polysoft_Underwear> sortedPants = new List<polysoft_Underwear>();
    public polysoft_Underwear[] brassiere; // only for female
    public SkinnedMeshRenderer[] defaultMaleMeshes;//head torso legs
    public SkinnedMeshRenderer[] defaultFemaleMeshes;//head torso legs
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        inventory = Inventory.Instance;
        InterfaceSC = interfaceSC.Instance;
        armorTypeSelected = 0;
        itemTypeSelected = 0;
        SortItems();
        SortHair();
        SortPants();
        if(inventory.gender == Inventory.Gender.female)
        {
            inventory.breast_control.setupDefaultBreast();
            SetBrassiere(0);
        }
        else
        {
            UpdateBeardButtons();
            UpdateMustacheButtons();
        }
    }
    public void SetBreastSize()
    {
        if(inventory.breast_control)
            inventory.breast_control.SetBreastSize();
    }
    public void SortItems()//Sort items by armor type and item type
    {
        sortedItems.Clear();
        foreach (polysoft_Item item in Items)
        {
            if(((int)item.armorType == armorTypeSelected || armorTypeSelected == -1) && ((int)item.itemType == itemTypeSelected || itemTypeSelected == -1))
            {
                sortedItems.Add(item);
            }
        }
        UpdateButtons();
    }
    public void SortHair()//Get all hair compatible with character
    {
        sortedHair.Clear();
        foreach (polysoft_Hair h in hair)
        {
            if (h.gender == inventory.gender)
            {
                sortedHair.Add(h);
            }
        }
        UpdateHairButtons();//Update hair toggle visibility
    }
    public void SortPants()//Get all pants compatible with character
    {
        sortedPants.Clear();
        foreach (polysoft_Underwear p in pants)
        {
            if (p.gender == inventory.gender)
            {
                sortedPants.Add(p);
            }
        }
        UpdatePantsButtons();//Update pants toggle visibility
    }
    public void UpdateHair()
    {
        if (!inventory.hairUsed[(int)polysoft_Hair.hairType.hair])
            return;
        inventory.boneManager.UpdateHairBones(inventory.hairUsed[(int)polysoft_Hair.hairType.hair]);
        if (!inventory.hairUsed[(int)polysoft_Hair.hairType.hair].ItemMeshRenderer)//if no mesh disable hair game object
        {
            inventory.BodyParts[5].gameObject.SetActive(false);
        }
        else
        {
            if (inventory.equiped[0] && inventory.equiped[(int)polysoft_Item.ItemType.helmet].disableHair)
                inventory.BodyParts[5].sharedMesh = inventory.hairUnderHelmet.sharedMesh;
            else
                inventory.BodyParts[5].sharedMesh = inventory.hairUsed[(int)polysoft_Hair.hairType.hair].ItemMeshRenderer.sharedMesh;
            inventory.BodyParts[5].gameObject.SetActive(true);
        }
    }
    public void SetHair(int i)
    {
        inventory.boneManager.setHairBones(sortedHair[i]);//update bones for hair
        if (!sortedHair[i].ItemMeshRenderer)//if no mesh disable hair game object
        {
            inventory.BodyParts[5].gameObject.SetActive(false);
        }
        else
        {
            if (inventory.equiped[0] && inventory.equiped[0].disableHair)
                inventory.BodyParts[5].sharedMesh = inventory.hairUnderHelmet.sharedMesh;
            else
                inventory.BodyParts[5].sharedMesh = inventory.hairUsed[(int)polysoft_Hair.hairType.hair].ItemMeshRenderer.sharedMesh;
            inventory.BodyParts[5].gameObject.SetActive(true);
        }
    }
    public void SetBeard(int i)
    {
        inventory.boneManager.setHairBones(beard[i]);//update bones for beard
        if (!inventory.hairUsed[(int)polysoft_Hair.hairType.beard].ItemMeshRenderer)
        {
            inventory.BodyParts[7].gameObject.SetActive(false);
        }
        else
        {
            inventory.BodyParts[7].sharedMesh = inventory.hairUsed[(int)polysoft_Hair.hairType.beard].ItemMeshRenderer.sharedMesh;
            if (!inventory.equiped[0] || !inventory.equiped[0].disableBeard)
                inventory.BodyParts[7].gameObject.SetActive(true);
            else
                inventory.BodyParts[7].gameObject.SetActive(false);
        }
    }
    public void SetMustache(int i)
    {
        inventory.boneManager.setHairBones(mustache[i]);//update bones for mustache
        if (!inventory.hairUsed[(int)polysoft_Hair.hairType.mustache].ItemMeshRenderer)
        {
            inventory.BodyParts[6].gameObject.SetActive(false);
        }
        else
        {
            inventory.BodyParts[6].sharedMesh = inventory.hairUsed[(int)polysoft_Hair.hairType.mustache].ItemMeshRenderer.sharedMesh;
            if (!inventory.equiped[0] || !inventory.equiped[0].disableMustache)
                inventory.BodyParts[6].gameObject.SetActive(true);
            else
                inventory.BodyParts[6].gameObject.SetActive(false);
        }
    }
    public void SetPants(int i)
    {
        inventory.underwearUsed[(int)polysoft_Underwear.underwearType.pants] = sortedPants[i];
        inventory.BodyParts[8].sharedMesh = sortedPants[i].ItemMeshRenderer.sharedMesh;
    }
    public void SetBrassiere(int i)
    {
        inventory.underwearUsed[(int)polysoft_Underwear.underwearType.brassiere] = brassiere[i];
        inventory.BodyParts[9].sharedMesh = brassiere[i].ItemMeshRenderer.sharedMesh;
    }
    public void UpdateBeardButtons()
    {
        for (int b = 0; b < InterfaceSC.beardButton.Length; b++)//Update buttons
        {
            if (b < beard.Length)
                InterfaceSC.beardButton[b].SetActive(true);
            else
                InterfaceSC.beardButton[b].SetActive(false);
        }
    }
    public void UpdateMustacheButtons()
    {
        for (int b = 0; b < InterfaceSC.mustacheButton.Length; b++)//Update buttons
        {
            if (b < mustache.Length)
                InterfaceSC.mustacheButton[b].SetActive(true);
            else
                InterfaceSC.mustacheButton[b].SetActive(false);
        }
    }
    public void UpdateHairButtons()
    {
        for (int b = 0; b < InterfaceSC.hairButton.Length; b++)//Update buttons
        {
            if (b < sortedHair.Count)
                InterfaceSC.hairButton[b].SetActive(true);
            else
                InterfaceSC.hairButton[b].SetActive(false);
        }
    }
    public void UpdatePantsButtons()
    {
        for (int b = 0; b < InterfaceSC.pantsButton.Length; b++)//Update buttons
        {
            if (b < sortedPants.Count)
                InterfaceSC.pantsButton[b].SetActive(true);
            else
                InterfaceSC.pantsButton[b].SetActive(false);
        }
    }
    public void UpdateButtons()
    {
        for (int b = 0; b < InterfaceSC.itemButton.Length; b++)//Update buttons
        {
            if (b < sortedItems.Count)
            {
                if (itemTypeSelected >= 0 && inventory.equiped[itemTypeSelected] == sortedItems[b])
                {
                    InterfaceSC.itemButton[b].GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
                }
                else
                {
                    InterfaceSC.itemButton[b].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f);
                }
                InterfaceSC.itemButton[b].SetActive(true);
            }
            else
                InterfaceSC.itemButton[b].SetActive(false);
        }
    }
    public void SortByArmorType(int i)
    {
        armorTypeSelected = i;
        SortItems();
    }
    public void SortByItemType(int i)
    {
        itemTypeSelected = i;
        SortItems();
    }
    public void SetItem(int i)
    {
        if (inventory.equiped[(int)sortedItems[i].itemType] == sortedItems[i])//if this item already equiped Return
            return;
        EquipItem(sortedItems[i]);
        UpdateButtons();
    }
    public void RemoveItem(polysoft_Item item)
    {
        inventory.boneManager.removeBones(item);
        if (item.itemType == polysoft_Item.ItemType.shoulders || item.itemType == polysoft_Item.ItemType.waist)
            inventory.BodyParts[(int)item.itemType].gameObject.SetActive(false);
        else
        {
            int bodyIndex = 0;
            if(item.itemType == polysoft_Item.ItemType.torso)
                bodyIndex = 1;
            else if (item.itemType == polysoft_Item.ItemType.legs)
                bodyIndex = 2;
            if(inventory.gender == Inventory.Gender.male)
            {
                inventory.BodyParts[(int)item.itemType].sharedMesh = defaultMaleMeshes[bodyIndex].sharedMesh;
                Material[] mats = new Material[1];
                mats[0] = inventory.skinMaterial;
                inventory.BodyParts[(int)item.itemType].sharedMaterials = mats;
            }
            else
            {
                inventory.BodyParts[(int)item.itemType].sharedMesh = defaultFemaleMeshes[bodyIndex].sharedMesh;
                Material[] mats = new Material[1];
                mats[0] = inventory.skinMaterial;
                inventory.BodyParts[(int)item.itemType].sharedMaterials = mats;
            }
        }
        if (item.itemType == polysoft_Item.ItemType.helmet)
        {
            UpdateHair();
            if (inventory.BodyParts[6])
            {
                if (inventory.hairUsed[(int)polysoft_Hair.hairType.mustache])
                {
                    inventory.boneManager.UpdateHairBones(inventory.hairUsed[(int)polysoft_Hair.hairType.mustache]);
                    inventory.BodyParts[6].gameObject.SetActive(true);
                }
            }
            if (inventory.BodyParts[7])
            {
                if (inventory.hairUsed[(int)polysoft_Hair.hairType.beard])
                {
                    inventory.boneManager.UpdateHairBones(inventory.hairUsed[(int)polysoft_Hair.hairType.beard]);
                    inventory.BodyParts[7].gameObject.SetActive(true);
                }
            }

        }
        if (item.itemType == polysoft_Item.ItemType.legs)
        {
            if (inventory.BodyParts[8])
                inventory.BodyParts[8].gameObject.SetActive(true);
        }
        if (item.itemType == polysoft_Item.ItemType.torso)
        {
            if (inventory.BodyParts[9])
                inventory.BodyParts[9].gameObject.SetActive(true);
        }
    }
    public void EquipItem(polysoft_Item item)
    {
        inventory.boneManager.setBones(item);
        inventory.BodyParts[(int)item.itemType].gameObject.SetActive(true);
        inventory.BodyParts[(int)item.itemType].sharedMesh = item.ItemMeshRenderer.sharedMesh;//Set Mesh from item prefab
        inventory.BodyParts[(int)item.itemType].materials = item.ItemMeshRenderer.sharedMaterials;//Set materials from item prefab
        if (item.itemType == polysoft_Item.ItemType.helmet)
        {
            UpdateHair();
            if (inventory.BodyParts[6])
            {
                if (!inventory.hairUsed[(int)polysoft_Hair.hairType.mustache] || !inventory.hairUsed[(int)polysoft_Hair.hairType.mustache].ItemMeshRenderer)
                {
                    inventory.BodyParts[6].gameObject.SetActive(false);
                }
                else
                {
                    inventory.boneManager.UpdateHairBones(inventory.hairUsed[(int)polysoft_Hair.hairType.mustache]);
                    if (inventory.equiped[0] && inventory.equiped[(int)polysoft_Item.ItemType.helmet].disableMustache)
                    {
                        inventory.BodyParts[6].gameObject.SetActive(false);
                    }
                    else
                    {
                        inventory.BodyParts[6].gameObject.SetActive(true);
                    }
 
                }
            }
            if (inventory.BodyParts[7])
            {
                if (!inventory.hairUsed[(int)polysoft_Hair.hairType.beard] || !inventory.hairUsed[(int)polysoft_Hair.hairType.beard].ItemMeshRenderer)
                {
                    inventory.BodyParts[7].gameObject.SetActive(false);
                }
                else
                {
                    inventory.boneManager.UpdateHairBones(inventory.hairUsed[(int)polysoft_Hair.hairType.beard]);
                    if (inventory.equiped[0] && inventory.equiped[(int)polysoft_Item.ItemType.helmet].disableBeard)
                    {
                        inventory.BodyParts[7].gameObject.SetActive(false);
                    }
                    else
                    {
                        inventory.BodyParts[7].gameObject.SetActive(true);
                    }

                }

            }

        }
        if (item.itemType == polysoft_Item.ItemType.legs)
        {
            if (inventory.BodyParts[8])
                inventory.BodyParts[8].gameObject.SetActive(false);//Disable pants
        }
        if (item.itemType == polysoft_Item.ItemType.torso)
        {
            if(inventory.BodyParts[9])
                inventory.BodyParts[9].gameObject.SetActive(false);//Disable brassiere
        }
        /*foreach (Material m in item.ItemMeshRenderer.sharedMaterials)
        {
            if (m.name != "body")
            {
                inventory.itemMaterial = m;
                return;
            }
        }*/
    }
    public void SetAnimation(int i)
    {
        if (i == 0)
            inventory.anim.SetTrigger("idle");
        else
        if (i == 1)
            inventory.anim.SetTrigger("walk");
        else
        if (i == 2)
            inventory.anim.SetTrigger("fight");
    }
}
