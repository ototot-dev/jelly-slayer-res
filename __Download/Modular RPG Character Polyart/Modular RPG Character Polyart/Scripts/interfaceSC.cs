using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class interfaceSC : MonoBehaviour
{
    public static interfaceSC Instance = null;
    public Color[] Colors;
    public Image[] ColorsToggleImages;
    public int menuIndex;
    public int itemColorIndex;
    public GameObject[] itemButton;
    public GameObject[] hairButton;
    public GameObject[] beardButton;
    public GameObject[] mustacheButton;
    public GameObject[] pantsButton;
    public GameObject[] Panels;
    public GameObject[] maleOptions;//UI objects for male
    public GameObject[] femaleOptions;//UI objects for female
    public Slider breastSlider;
    public void Awake()
    {
        Instance = this;
        for (int i = 0; i < Colors.Length; i++)
        {
            Colors[i].a = 1f;
            ColorsToggleImages[i].color = Colors[i];
        }
    }
    private void Start()
    {
        SetMenuIndex(0);
        if(Inventory.Instance != null)
        {
            for (int i = 0; i < maleOptions.Length; i++)
            {
                maleOptions[i].SetActive(Inventory.Instance.gender == Inventory.Gender.male);
            }
            for (int i = 0; i < femaleOptions.Length; i++)
            {
                femaleOptions[i].SetActive(Inventory.Instance.gender == Inventory.Gender.female);
            }
        }
    }
    public void SetMenuIndex(int i)
    {
        menuIndex = i;
        if (i == 1)
            SceneScript.Instance.SetLookPosition(1);
        else
            SceneScript.Instance.SetLookPosition(0);

        for (int p = 0; p < Panels.Length; p++)
        {
            if (p != i)
                Panels[p].SetActive(false);
        }
        Panels[i].SetActive(true);
    }
    public void SetColor(int i)
    {
        if(menuIndex == 0)
        {
            SetSkinColor(i);
        }
        else if(menuIndex == 1)
        {
            SetHairColor(i);
        }
        else if(menuIndex == 2)
        {
            SetUnderwearColor(i);
        }
        else if (menuIndex == 3)
        {
            SetItemColor1(i);
        }
    }
    public void SetSkinColor(int i)
    {
        ItemData.Instance.inventory.skinMaterial.SetColor("_Color" + (itemColorIndex + 1), Colors[i]);
    }
    public void SetHairColor(int i)
    {
        ItemData.Instance.inventory.hairMaterial.SetColor("_Color" + (itemColorIndex + 1), Colors[i]);
    }
    public void SetUnderwearColor(int i)
    {
        ItemData.Instance.inventory.underwearMaterial.SetColor("_Color" + (itemColorIndex + 1), Colors[i]);
    }
    public void SetItemColorIndex(int i)
    {
        itemColorIndex = i;
    }
    public void SetItemColor1(int i)
    {
        if(ItemData.Instance.inventory.itemMaterial)
            ItemData.Instance.inventory.itemMaterial.SetColor("_Color" + (itemColorIndex+1), Colors[i]);
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            GetComponent<Canvas>().enabled = !GetComponent<Canvas>().enabled;
        }
    }
}
