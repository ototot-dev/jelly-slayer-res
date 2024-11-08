using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Rendering;
using UnityEditorInternal;
public class CharacterBuilder : EditorWindow
{
    string myName = "NewCharacter";
    bool groupEnabled;
    bool myBool = true;
    float myFloat = 1.23f;
    public GameObject selectedCharacter = null;
    private Inventory inventory;
    private ItemData items;
    private BoneManager bManager;

    private Color skin, eye, eyebrow;
    private Color hair, decor1, decor2;
    private Color underwearColor1, underwearColor2;
    private Color armor1, armor2, armor3, armor4, armor5, armor6;
    private Material[] Materials = new Material[4];

    private string[] itemT = { "helmet", "shoulders", "torso", "waist", "legs" };
    private int itemType = 0;
    private string[] armorT = { "cloth", "leather", "plate", "outfit"};
    private int armorType = 0;

    private string[] hairT = { "hair", "mustache", "beard" };
    private int hairType;
    private LayerMask mask;
    private DefaultAsset MaterialFolder;
    private int sel;
    private string femaleAssetPath = "_";
    private string maleAssetPath = "_";
    private bool updateArmorTypeSel;
    private string Fsave = "_";
    private string Msave = "_";
    private string[] menuT = { "Setup", "Skin", "Head", "Underwear", "Armor"};
    private int menuSel = 0;
    private bool male;
    private int reSelect = -1;
    private LayerMask layerCharacter;
    private LayerMask layerRig;
    private GameObject oldFocus;
    private Vector2 scrollPosSettings;
    /*private GameObject previewObj;
    private MeshFilter pmf;
    private MeshRenderer pmr;*/
    [MenuItem("POLYSOFT/Humans/Character Builder")]
    public static void ShowWindow()
    {
        GetWindow(typeof(CharacterBuilder));
    }
    private void OnEnable()
    {
        string[] finds = AssetDatabase.FindAssets("Modular Character");
        bool ok = false;
        foreach (string s in finds)
        {
            string path = AssetDatabase.GUIDToAssetPath(s);
            if (path.Contains("Modular ") && path.Contains("Character ") && path.Contains("Male"))
            {
                if (AssetDatabase.IsValidFolder(path + "/CharacterBuilder"))
                {
                    maleAssetPath = path;
                    ok = true;
                }
            }
            if (path.Contains("Modular ") && path.Contains("Character ") && path.Contains("Female"))
            {
                if (AssetDatabase.IsValidFolder(path + "/CharacterBuilder"))
                {
                    femaleAssetPath = path;
                    ok = true;
                }
            }
        }
        if(!ok)
        {
            Debug.LogWarning("Asset folders not found", null);
            return;
        }
        Select();
        if(femaleAssetPath != "_")
        {
            string save = femaleAssetPath + "/CharacterBuilder/prefs";
            if (!MaterialFolder)
            {
                if (File.Exists(save))
                {
                    StreamReader sr = File.OpenText(save);
                    string PathMaterialFolder = sr.ReadLine();
                    if (AssetDatabase.IsValidFolder(PathMaterialFolder))
                    {
                        MaterialFolder = (DefaultAsset)AssetDatabase.LoadAssetAtPath(PathMaterialFolder, typeof(DefaultAsset));
                    }
                    int outLChar = 0;
                    int.TryParse(sr.ReadLine(), out outLChar);
                    layerCharacter = outLChar;
                    outLChar = 0;
                    int.TryParse(sr.ReadLine(), out outLChar);
                    layerRig = outLChar;
                    sr.Close();
                }
                else
                {
                    StreamWriter sw = File.CreateText(save);
                    sw.Close();
                }
            }
            Fsave = save;
        }
        if (maleAssetPath != "_")
        {
            string save = maleAssetPath + "/CharacterBuilder/prefs";
            if (!MaterialFolder)
            {
                if (File.Exists(save))
                {
                    StreamReader sr = File.OpenText(save);
                    string PathMaterialFolder = sr.ReadLine();
                    if (AssetDatabase.IsValidFolder(PathMaterialFolder))
                    {
                        MaterialFolder = (DefaultAsset)AssetDatabase.LoadAssetAtPath(PathMaterialFolder, typeof(DefaultAsset));
                    }
                    int outLChar = 0;
                    int.TryParse(sr.ReadLine(), out outLChar);
                    layerCharacter = outLChar;
                    outLChar = 0;
                    int.TryParse(sr.ReadLine(), out outLChar);
                    layerRig = outLChar;
                    sr.Close();
                }
                else
                {
                    StreamWriter sw = File.CreateText(save);
                    sw.Close();
                }
            }
            Msave = save;
        }
    }
    private void OnSelectionChange()
    {
        Select();
    }
    void Select()
    {
        GameObject selectionRoot = null;
        if (Selection.activeGameObject && Selection.activeGameObject.scene.IsValid())
        {
            if (!Selection.activeGameObject.transform.Find("metarig"))
            {
                if (Selection.activeGameObject.transform.root.Find("metarig") && Selection.activeGameObject.transform.root.GetComponent<Inventory>())
                    selectionRoot = Selection.activeGameObject.transform.root.gameObject;
            }
            else
            {
                if (Selection.activeGameObject.transform.GetComponent<Inventory>())
                    selectionRoot = Selection.activeGameObject;
            }
        }
        if (selectionRoot == null && selectedCharacter)
            selectionRoot = selectedCharacter;
        if (selectionRoot)
        {
            if (selectedCharacter != selectionRoot)
            {
                reSelect = 1;
            }
            selectedCharacter = selectionRoot;
            updateArmorTypeSel = true;
            Transform[] bn = selectedCharacter.transform.GetComponentsInChildren<Transform>(true);
            male = false;
            foreach (Transform b in bn)
            {
                if (b.name == "beard_01")
                {
                    male = true;
                    break;
                }
            }
            if (male)
            {
                if(File.Exists(maleAssetPath + "/CharacterBuilder/ItemDataMale.prefab"))
                {
                    GameObject data = (GameObject)AssetDatabase.LoadAssetAtPath(maleAssetPath + "/CharacterBuilder/ItemDataMale.prefab", typeof(GameObject));
                    items = data.GetComponent<ItemData>();
                }
                else
                {
                    Debug.LogWarning("No ItemDataMale prefab found in the CharacterBuilder folder", AssetDatabase.LoadAssetAtPath(maleAssetPath + "/CharacterBuilder", typeof(Object)));
                }

            }
            else
            {
                if (File.Exists(femaleAssetPath + "/CharacterBuilder/ItemDataFemale.prefab"))
                {
                    GameObject data = (GameObject)AssetDatabase.LoadAssetAtPath(femaleAssetPath + "/CharacterBuilder/ItemDataFemale.prefab", typeof(GameObject));
                    items = data.GetComponent<ItemData>();
                }
                else
                {
                    Debug.LogWarning("No ItemDataFemale prefab found in the CharacterBuilder folder", AssetDatabase.LoadAssetAtPath(femaleAssetPath + "/CharacterBuilder", typeof(Object)));
                }
            }
        }
        if (!items)
        {
            return;
        }
        if (selectedCharacter)
        {
            inventory = selectedCharacter.GetComponent<Inventory>();
            Materials[0] = inventory.skinMaterial;
            Materials[1] = inventory.hairMaterial;
            Materials[2] = inventory.underwearMaterial;
            Materials[3] = inventory.itemMaterial;
        }
        if (selectedCharacter && inventory)
        {
            bManager = selectedCharacter.GetComponent<BoneManager>();
            bManager.inventory = inventory;
            bManager.Awake();
            inventory.boneManager = bManager;
            items.inventory = inventory;
            if (!male)
                inventory.breast_control = inventory.gameObject.GetComponent<breastControl>();
            if (inventory.hairUsed.Length < 3)
                inventory.hairUsed = new polysoft_Hair[3];
            if (inventory.equiped.Length < 5)
                inventory.equiped = new polysoft_Item[5];
            if (inventory.underwearUsed.Length < 2)
                inventory.underwearUsed = new polysoft_Underwear[2];
        }
        Repaint();
    }
    void updateMaterials()
    {
        for (int i = 0; i < inventory.BodyParts.Length; i++)
        {
            if (inventory.BodyParts[i] == null)
                continue;
            if (inventory.BodyParts[i].name.Contains("torso") || inventory.BodyParts[i].name.Contains("head") || inventory.BodyParts[i].name.Contains("legs") || inventory.BodyParts[i].name.Contains("shoulder") || inventory.BodyParts[i].name.Contains("waist"))
            {
                Material[] materials = inventory.BodyParts[i].sharedMaterials;
                if (inventory.BodyParts[i].GetComponent<SkinnedMeshRenderer>().sharedMesh && !inventory.BodyParts[i].GetComponent<SkinnedMeshRenderer>().sharedMesh.name.Contains("base"))
                {
                    materials[0] = Materials[3];
                    if (materials.Length > 1)
                        materials[1] = Materials[0];
                }
                else
                {
                    materials = new Material[1];
                    materials[0] = Materials[0];
                }
                inventory.BodyParts[i].sharedMaterials = materials;
            }
            else
            if (inventory.BodyParts[i].name.Contains("hair") || inventory.BodyParts[i].name.Contains("beard") || inventory.BodyParts[i].name.Contains("mustache"))
            {
                Material[] materials = inventory.BodyParts[i].sharedMaterials;
                materials[0] = Materials[1];
                inventory.BodyParts[i].sharedMaterials = materials;
            }
            else
            if (inventory.BodyParts[i].name.Contains("pants") || inventory.BodyParts[i].name.Contains("brassiere"))
            {
                Material[] materials = inventory.BodyParts[i].sharedMaterials;
                materials[0] = Materials[2];
                inventory.BodyParts[i].sharedMaterials = materials;
            }
        }
    }
    void SkinMenu()
    {
        GUILayout.Space(20);
        if (Materials[0])
        {
            skin = Materials[0].GetColor("_Color1");
            eye = Materials[0].GetColor("_Color2");
            eyebrow = Materials[0].GetColor("_Color3");
        }
        GUILayout.BeginHorizontal();
        GUILayout.Label("Skin color", EditorStyles.boldLabel, GUILayout.Width(120));
        GUILayout.Label("Eye color", EditorStyles.boldLabel, GUILayout.Width(120));
        GUILayout.Label("Eyebrow color", EditorStyles.boldLabel, GUILayout.Width(120));
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        skin = EditorGUILayout.ColorField(skin, GUILayout.Width(120));
        eye = EditorGUILayout.ColorField(eye, GUILayout.Width(120));
        eyebrow = EditorGUILayout.ColorField(eyebrow, GUILayout.Width(120));
        if (Materials[0])
        {
            Materials[0].SetColor("_Color1", skin);
            Materials[0].SetColor("_Color2", eye);
            Materials[0].SetColor("_Color3", eyebrow);
        }
        if (GUILayout.Button("Random colors", GUILayout.Width(120)))
        {
            RandomColors(1);
        }
        GUILayout.EndHorizontal();
        if (!male)
        {
            if (inventory.breast_control)
            {
                GUILayout.Space(20);
                GUILayout.BeginHorizontal();
                GUILayout.Label("Breast size", EditorStyles.boldLabel, GUILayout.Width(100));
                inventory.breast_control.SetBreastSizeEditor(EditorGUILayout.Slider(inventory.breast_control.BreastSize, inventory.breast_control.minValue, inventory.breast_control.maxValue, GUILayout.Width(150)));
                GUILayout.EndHorizontal();
            }
        }
    }
    void RandomCharacter(int r)
    {
        if (r == 0 && !male)
        {
            inventory.breast_control.BreastSize = Random.Range(inventory.breast_control.minValue, inventory.breast_control.maxValue);
            inventory.breast_control.SetBreastSizeEditor(inventory.breast_control.BreastSize);
        }
        if (r == 0 || r == 2)
        {
            polysoft_Hair _hairRandom = items.hair[Random.Range(0, items.hair.Length)];
            setupHair(_hairRandom,0);
            if (male)
            {
                polysoft_Hair _beardRandom = items.beard[Random.Range(0, items.beard.Length)];
                polysoft_Hair _mustacheRandom = items.mustache[Random.Range(0, items.mustache.Length)];
                setupHair(_beardRandom,2);
                setupHair(_mustacheRandom,1);
            }
        }
        if (r == 0 || r == 3)
        {
            polysoft_Underwear _pantsRandom = items.pants[Random.Range(0, items.pants.Length)];
            inventory.underwearUsed[(int)polysoft_Underwear.underwearType.pants] = _pantsRandom;
            inventory.BodyParts[8].sharedMesh = _pantsRandom.ItemMeshRenderer.sharedMesh;
            if (!male)
            {
                polysoft_Underwear _brassiereRandom = items.brassiere[Random.Range(0, items.brassiere.Length)];
                inventory.underwearUsed[(int)polysoft_Underwear.underwearType.brassiere] = _brassiereRandom;
                inventory.BodyParts[9].sharedMesh = _brassiereRandom.ItemMeshRenderer.sharedMesh;
            }
        }
        if (r == 0 || r == 4)
        {
            List<polysoft_Item>[] itms_ = new List<polysoft_Item>[5];
            for (int i = 0; i < 5; i++)
            {
                itms_[i] = new List<polysoft_Item>();
            }
            for (int it = 0; it < items.Items.Length; it++)
            {
                itms_[(int)items.Items[it].itemType].Add(items.Items[it]);
            }
            for (int i = 0; i < 5; i++)
            {
                items.EquipItem(itms_[i][Random.Range(0, itms_[i].Count)]);
            }
            for (int i = 0; i < 5; i++)
            {
                if(i < 4 && Random.Range(0,100) < 20 && inventory.equiped[i])
                    items.RemoveItem(inventory.equiped[i]);
            }
        }
        updateMaterials();
    }
    void setupHair(polysoft_Hair hair_, int Htype)
    {
        bManager.setHairBones(hair_);
        if (!inventory.hairUsed[(int)hair_.itemType].ItemMeshRenderer)
        {
            inventory.BodyParts[5 + Htype].gameObject.SetActive(false);
        }
        else
        {
            if (inventory.equiped[0] && ((inventory.equiped[0].disableHair && Htype == 0) || (inventory.equiped[0].disableBeard && Htype == 1) || (inventory.equiped[0].disableBeard && Htype == 2)))
            {
                if (Htype == 0)
                    inventory.BodyParts[5 + Htype].sharedMesh = inventory.hairUnderHelmet.sharedMesh;
                else
                {
                    inventory.hairUsed[(int)hair_.itemType] = hair_;
                    inventory.BodyParts[5 + Htype].sharedMesh = inventory.hairUsed[(int)hair_.itemType].ItemMeshRenderer.sharedMesh;
                }
            }
            else
            {

                inventory.BodyParts[5 + Htype].gameObject.SetActive(true);
                inventory.hairUsed[(int)hair_.itemType] = hair_;
                inventory.BodyParts[5 + Htype].sharedMesh = inventory.hairUsed[(int)hair_.itemType].ItemMeshRenderer.sharedMesh;
            }
        }
    }
    void HairMenu()
    {
        GUILayout.Space(20);
        if (Materials[1])
        {
            hair = Materials[1].GetColor("_Color1");
            decor1 = Materials[1].GetColor("_Color2");
            decor2 = Materials[1].GetColor("_Color3");
        }
        GUILayout.BeginHorizontal();
        GUILayout.Label("Hair color", EditorStyles.boldLabel, GUILayout.Width(120));
        GUILayout.Label("Decor color 1", EditorStyles.boldLabel, GUILayout.Width(120));
        GUILayout.Label("Decor color 1", EditorStyles.boldLabel, GUILayout.Width(120));
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        hair = EditorGUILayout.ColorField(hair, GUILayout.Width(120));
        decor1 = EditorGUILayout.ColorField(decor1, GUILayout.Width(120));
        decor2 = EditorGUILayout.ColorField(decor2, GUILayout.Width(120));
        if (Materials[1])
        {
            Materials[1].SetColor("_Color1", hair);
            Materials[1].SetColor("_Color2", decor1);
            Materials[1].SetColor("_Color3", decor2);
        }
        if (GUILayout.Button("Random colors", GUILayout.Width(120)))
        {
            RandomColors(2);
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(17);
        if (GUILayout.Button("Random hair", GUILayout.Width(120), GUILayout.Height(23)))
        {
            RandomCharacter(2);
        }

        GUILayout.Space(10);
        int oldHairType = hairType;
        hairType = GUILayout.SelectionGrid(hairType, hairT, hairT.Length, GUILayout.Width(450), GUILayout.Height(40));

        int itemIndex = 0;
        polysoft_Hair[] hair_ = new polysoft_Hair[0];
        if (hairType == 0)
        {
            hair_ = items.hair;
        }
        else if (hairType == 2)
        {
            hair_ = items.beard;
        }
        else if (hairType == 1)
        {
            hair_ = items.mustache;
        }
        Texture2D[] previews = new Texture2D[hair_.Length];
        for (int i = 0; i < hair_.Length; i++)
        {
            itemIndex++;
            Texture2D preview = null;
            if (!hair_[i].ItemMeshRenderer)
            {
                if (!inventory.BodyParts[5 + hairType].sharedMesh)
                {
                    sel = i;
                }
                preview = null;
            }
            else
            {
                if (inventory.BodyParts[5 + hairType].sharedMesh && inventory.BodyParts[5 + hairType].sharedMesh == hair_[i].ItemMeshRenderer.sharedMesh)
                {
                    sel = i;
                }
                preview = AssetPreview.GetAssetPreview(hair_[i].ItemMeshRenderer.sharedMesh);
            }
            previews[i] = preview;
        }
        int xCount = 4;
        int sizeIcon = 498 / 4;

        int w = xCount * sizeIcon;
        if (previews.Length < xCount)
            w = (previews.Length+1) * sizeIcon;
        int h = Mathf.CeilToInt(previews.Length / (float)xCount) * sizeIcon;
        int oldSel = sel;

        scrollPosSettings = GUILayout.BeginScrollView(scrollPosSettings, GUILayout.Width(517));
        sel = GUILayout.SelectionGrid(sel, previews, xCount, GUILayout.Width(w), GUILayout.Height(h));
        GUILayout.EndScrollView();

        if (sel != oldSel)
        {
            setupHair(hair_[sel], hairType);
            updateMaterials();
        }
    }
    void UnderwearMenu()
    {
        GUILayout.Space(20);
        if (Materials[2])
        {
            underwearColor1 = Materials[2].GetColor("_Color1");
            underwearColor2 = Materials[2].GetColor("_Color2");
        }
        GUILayout.BeginHorizontal();
        GUILayout.Label("Color 1", EditorStyles.boldLabel, GUILayout.Width(120));
        GUILayout.Label("Color 2", EditorStyles.boldLabel, GUILayout.Width(120));
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        underwearColor1 = EditorGUILayout.ColorField(underwearColor1, GUILayout.Width(120));
        underwearColor2 = EditorGUILayout.ColorField(underwearColor2, GUILayout.Width(120));
        if (Materials[2])
        {
            Materials[2].SetColor("_Color1", underwearColor1);
            Materials[2].SetColor("_Color2", underwearColor2);
        }
        if (GUILayout.Button("Random colors", GUILayout.Width(120)))
        {
            RandomColors(3);
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(17);
        if (GUILayout.Button("Random underwear", GUILayout.Width(120), GUILayout.Height(23)))
        {
            RandomCharacter(3);
        }
        List<Texture2D> previewsPants = new List<Texture2D>();
        List<Texture2D> previewsBrassiere = new List<Texture2D>();
        int selPants = 0;
        int selBrassiere = 0;
        for (int it = 0; it < 2; it++)
        {
            List<polysoft_Underwear> items_ = new List<polysoft_Underwear>();
            if (it == 0)
                items_.AddRange(items.pants);
            else
                items_.AddRange(items.brassiere);
            for (int i = 0; i < items_.Count; i++)
            {
                if (!items_[i].ItemMeshRenderer)
                {
                    if (!inventory.BodyParts[8+it].sharedMesh)
                    {
                        if (it == 0)
                            selPants = i;
                        else
                            selBrassiere = i;
                    }
                    if (it == 0)
                        previewsPants.Add(null);
                    else
                        previewsBrassiere.Add(null);
                }
                else
                {
                    if (inventory.BodyParts[8 + it].sharedMesh && inventory.BodyParts[8 + it].sharedMesh == items_[i].ItemMeshRenderer.sharedMesh)
                    {
                        if (it == 0)
                            selPants = i;
                        else
                            selBrassiere = i;
                    }
                    if (it == 0)
                        previewsPants.Add(AssetPreview.GetAssetPreview(items_[i].ItemMeshRenderer.sharedMesh));
                    else
                        previewsBrassiere.Add(AssetPreview.GetAssetPreview(items_[i].ItemMeshRenderer.sharedMesh));
                }
            }
        }
        GUILayout.Space(10);
        int sizeIcon = 120;
        int xCount = 4;

        int w = xCount * sizeIcon;
        if (previewsBrassiere.Count < xCount)
            w = (previewsBrassiere.Count + 1) * sizeIcon;
        int h = Mathf.CeilToInt(previewsBrassiere.Count / (float)xCount) * sizeIcon;
        int oldSelBrassiere = selBrassiere;
        selBrassiere = GUILayout.SelectionGrid(selBrassiere, previewsBrassiere.ToArray(), xCount, GUILayout.Width(w), GUILayout.Height(h));
        if (selBrassiere != oldSelBrassiere)
        {
            inventory.underwearUsed[(int)polysoft_Underwear.underwearType.brassiere] = items.brassiere[selBrassiere];
            inventory.BodyParts[9].sharedMesh = items.brassiere[selBrassiere].ItemMeshRenderer.sharedMesh;
            updateMaterials();
        }
        if (!male)
            GUILayout.Space(20);
        w = xCount * sizeIcon;
        if (previewsPants.Count < xCount)
            w = (previewsPants.Count + 1) * sizeIcon;
        h = Mathf.CeilToInt(previewsPants.Count / (float)xCount) * sizeIcon;
        int oldSelPants = selPants;
        selPants = GUILayout.SelectionGrid(selPants, previewsPants.ToArray(), xCount, GUILayout.Width(w), GUILayout.Height(h));
        if (selPants != oldSelPants)
        {
            inventory.underwearUsed[(int)polysoft_Underwear.underwearType.pants] = items.pants[selPants];
            inventory.BodyParts[8].sharedMesh = items.pants[selPants].ItemMeshRenderer.sharedMesh;
            updateMaterials();
        }
    }
    void ArmorMenu()
    {
        GUILayout.Space(20);
        if (Materials[3])
        {
            armor1 = Materials[3].GetColor("_Color1");
            armor2 = Materials[3].GetColor("_Color2");
            armor3 = Materials[3].GetColor("_Color3");

            armor4 = Materials[3].GetColor("_EmissionColor1");
            armor5 = Materials[3].GetColor("_EmissionColor2");
            armor6 = Materials[3].GetColor("_EmissionColor3");
        }
        GUILayout.BeginHorizontal();
        GUILayout.Label("Color 1", EditorStyles.boldLabel, GUILayout.Width(60));
        GUILayout.Label("Color 2", EditorStyles.boldLabel, GUILayout.Width(60));
        GUILayout.Label("Color 3", EditorStyles.boldLabel, GUILayout.Width(60));

        GUILayout.Label("Emiss 1", EditorStyles.boldLabel, GUILayout.Width(60));
        GUILayout.Label("Emiss 2", EditorStyles.boldLabel, GUILayout.Width(60));
        GUILayout.Label("Emiss 3", EditorStyles.boldLabel, GUILayout.Width(60));

        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        armor1 = EditorGUILayout.ColorField(armor1, GUILayout.Width(60));
        armor2 = EditorGUILayout.ColorField(armor2, GUILayout.Width(60));
        armor3 = EditorGUILayout.ColorField(armor3, GUILayout.Width(60));

        armor4 = EditorGUILayout.ColorField(armor4, GUILayout.Width(60));
        armor5 = EditorGUILayout.ColorField(armor5, GUILayout.Width(60));
        armor6 = EditorGUILayout.ColorField(armor6, GUILayout.Width(60));
        if (Materials[3])
        {
            Materials[3].SetColor("_Color1", armor1);
            Materials[3].SetColor("_Color2", armor2);
            Materials[3].SetColor("_Color3", armor3);

            Materials[3].SetColor("_EmissionColor1", armor4);
            Materials[3].SetColor("_EmissionColor2", armor5);
            Materials[3].SetColor("_EmissionColor3", armor6);
        }
        if (GUILayout.Button("Random colors", GUILayout.Width(120)))
        {
            RandomColors(4);
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(17);
        if (GUILayout.Button("Random armors", GUILayout.Width(120), GUILayout.Height(23)))
        {
            RandomCharacter(4);
        }
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        int oldItemType = itemType;
        itemType = GUILayout.SelectionGrid(itemType, itemT, itemT.Length, GUILayout.Width(450), GUILayout.Height(60));
        if (inventory.equiped[itemType] && GUILayout.Button("Remove", GUILayout.Width(60), GUILayout.Height(60)))
            items.RemoveItem(inventory.equiped[itemType]);
       // GUILayout.Space(10);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.BeginVertical(GUILayout.Width(300));

        if (itemType != oldItemType)
            updateArmorTypeSel = true;
        if (updateArmorTypeSel)
        {
            for (int i = 0; i < items.Items.Length; i++)
            {
                if (items.Items[i].ItemMeshRenderer)
                {
                    if (inventory.BodyParts[itemType].sharedMesh && inventory.BodyParts[itemType].sharedMesh == items.Items[i].ItemMeshRenderer.sharedMesh)
                    {
                        armorType = (int)items.Items[i].armorType;
                    }
                }
            }
            updateArmorTypeSel = false;
        }

        List<Texture2D> previews = new List<Texture2D>();
        List<polysoft_Item> items_ = new List<polysoft_Item>();
        for (int i = 0; i < items.Items.Length; i++)
        {
            if ((int)items.Items[i].itemType == itemType && (int)items.Items[i].armorType == armorType)
            {
                items_.Add(items.Items[i]);
            }
        }
        int itemIndex = items_.Count;
        bool selOK = false;
        for (int i = 0; i < items_.Count; i++)
        {
            if (!items_[i].ItemMeshRenderer)
            {
                if (!inventory.BodyParts[itemType].sharedMesh)
                {
                    sel = i;
                    selOK = true;
                }
                previews.Add(null);
            }
            else
            {
                if (inventory.BodyParts[itemType].sharedMesh && inventory.BodyParts[itemType].sharedMesh == items_[i].ItemMeshRenderer.sharedMesh)
                {
                    sel = i;
                    selOK = true;
                }
                previews.Add(AssetPreview.GetAssetPreview(items_[i].ItemMeshRenderer.sharedMesh));
            }
        }
        if (!selOK)
            sel = 100;

        int xCount = 3;
        int sizeIcon = 433 / 3;

        int w = xCount * sizeIcon;
        if (previews.Count < xCount)
            w = previews.Count * sizeIcon;
        int h = Mathf.CeilToInt(previews.Count / (float)xCount) * sizeIcon;
        int oldSel = sel;

        scrollPosSettings = GUILayout.BeginScrollView(scrollPosSettings, GUILayout.Width(453));
        sel = GUILayout.SelectionGrid(sel, previews.ToArray(), xCount, GUILayout.Width(w), GUILayout.Height(h));
        GUILayout.EndScrollView();

        if (sel != oldSel)
        {
            items.EquipItem(items_[sel]);
            updateMaterials();
        }
        GUILayout.EndVertical();
        int oldArmorType = armorType;
        armorType = GUILayout.SelectionGrid(armorType, armorT, 1, GUILayout.Width(60), GUILayout.Height(450));
        GUILayout.EndHorizontal();
    }
    void RandomColors(int r)
    {
        if (r == 0 || r == 2)
        {
            Materials[1].SetColor("_Color1", new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f)));
            Materials[1].SetColor("_Color2", new Color(Random.Range(0.20f, 1.0f), Random.Range(0.20f, 1.0f), Random.Range(0.20f, 1.0f)));
            Materials[1].SetColor("_Color3", new Color(Random.Range(0.20f, 1.0f), Random.Range(0.20f, 1.0f), Random.Range(0.20f, 1.0f)));
            Materials[0].SetColor("_Color3", Materials[1].GetColor("_Color1"));
        }
        if (r == 0 || r == 1)
        {
            Materials[0].SetColor("_Color1", new Color(0.77f, 0.56f, 0.47f) * Random.Range(0.30f, 1.20f));
            Materials[0].SetColor("_Color2", new Color(Random.Range(0.20f, 0.80f), Random.Range(0.20f, 0.80f), Random.Range(0.20f, 0.80f)));
            Materials[0].SetColor("_Color3", Materials[1].GetColor("_Color1"));
        }
        if (r == 0 || r == 3)
        {
            Materials[2].SetColor("_Color1", new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f)));
            Materials[2].SetColor("_Color2", new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f)));
            Materials[2].SetColor("_Color3", new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f)));
        }
        if (r == 0 || r == 4)
        {
            Materials[3].SetColor("_Color1", new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f)));
            Materials[3].SetColor("_Color2", new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f)));
            Materials[3].SetColor("_Color3", new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f)));

            Materials[3].SetColor("_EmissionColor1", new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f)));
            Materials[3].SetColor("_EmissionColor2", new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f)));
            Materials[3].SetColor("_EmissionColor3", new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f)));
        }
    }
    void newMaterials()
    {
        Material mat = null;
        if (GraphicsSettings.defaultRenderPipeline == null)
        {
            mat = new Material(Shader.Find("PolySoft/StandardSpecialMask"));
        }
        else
        {
            mat = new Material(Shader.Find("Shader Graphs/URPspecial"));
        }
        string[] finds = AssetDatabase.FindAssets("Modular RPG Character Polyart");
        foreach (string s in finds)
        {
            string path = AssetDatabase.GUIDToAssetPath(s);
            if (path.Contains("Modular ") && path.Contains("RPG ") && path.Contains("Character ") && path.Contains("Polyart"))
            {
                if (File.Exists(path+"/Textures/colorsMap.png"))
                {
                    mat.SetTexture("_MainTex", (Texture2D)AssetDatabase.LoadAssetAtPath(path+"/Textures/colorsMap.png", typeof(Texture2D)));
                }
                else
                {
                    Debug.LogWarning("No colorsMap.png found in the 'Modular RPG Character Polyart/Textures' folder", null);
                }
                break;
            }
        }
        string _myNum = myName;
        if (File.Exists(AssetDatabase.GetAssetPath(MaterialFolder) + "/" + myName + "_Skin.mat"))
        {
            for (int i = 0; i < 1000; i++)
            {
                if((i + 1) < 10)
                    _myNum = myName + "_" + 0 + (i + 1);
                else
                    _myNum = myName + "_" + (i + 1);
                if (!File.Exists(AssetDatabase.GetAssetPath(MaterialFolder) + "/" + _myNum + "_Skin.mat"))
                {
                    break;
                }
            }
        }
        Material matSkin = Instantiate(mat);
        Materials[0] = matSkin;
        AssetDatabase.CreateAsset(matSkin, AssetDatabase.GetAssetPath(MaterialFolder) + "/" + _myNum + "_Skin.mat");
        Material matHair = Instantiate(mat);
        Materials[1] = matHair;
        AssetDatabase.CreateAsset(matHair, AssetDatabase.GetAssetPath(MaterialFolder) + "/" + _myNum + "_Hair.mat");
        Material matUnderwear = Instantiate(mat);
        Materials[2] = matUnderwear;
        AssetDatabase.CreateAsset(matUnderwear, AssetDatabase.GetAssetPath(MaterialFolder) + "/" + _myNum + "_Underwear.mat");
        Material matArmor = Instantiate(mat);
        Materials[3] = matArmor;
        AssetDatabase.CreateAsset(matArmor, AssetDatabase.GetAssetPath(MaterialFolder) + "/" + _myNum + "_Armor.mat");
        inventory.skinMaterial = Materials[0];
        inventory.hairMaterial = Materials[1];
        inventory.underwearMaterial = Materials[2];
        inventory.itemMaterial = Materials[3];
        RandomColors(0);
        updateMaterials();
    }
    void SetLayers(GameObject g)
    {
        g.layer = layerCharacter;
        Collider[] colliders = g.transform.Find("metarig").GetComponentsInChildren<Collider>(true);
        foreach (Collider c in colliders)
        {
            c.gameObject.layer = layerRig;
        }
    }
    void SetupMenu()
    {
        GUILayout.Space(15);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Folder for materials", EditorStyles.boldLabel, GUILayout.Width(120), GUILayout.Height(30));
        DefaultAsset oldMaterialFolder = MaterialFolder;
        MaterialFolder = (DefaultAsset)EditorGUILayout.ObjectField(MaterialFolder, typeof(DefaultAsset), false, GUILayout.Width(114), GUILayout.Height(30));
        GUILayout.Label("(New materials will be saved here)", GUILayout.Width(220), GUILayout.Height(30));
        GUILayout.EndHorizontal();

        GUILayout.Space(15);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Character Layer", EditorStyles.boldLabel, GUILayout.Width(98));
        int oldLayerCh = layerCharacter;
        layerCharacter = EditorGUILayout.LayerField(layerCharacter, GUILayout.Width(114));
        GUILayout.Label("Rig Layer", EditorStyles.boldLabel, GUILayout.Width(59));
        int oldLayerRig = layerRig;
        layerRig = EditorGUILayout.LayerField(layerRig, GUILayout.Width(114));
        GUILayout.Space(5);
        if (selectedCharacter && GUILayout.Button("Set Layers", GUILayout.Width(80)))
        {
            SetLayers(selectedCharacter);
        }
        GUILayout.EndHorizontal();

        GUI.contentColor = Color.yellow;
        if (layerRig == layerCharacter)
            GUILayout.Label("(Choose different Layers to the Character and Rig)", GUILayout.Width(480));
        if (!Physics.GetIgnoreLayerCollision(layerCharacter, layerRig))
            GUILayout.Label("(Disable collision between Character and Rig layers in Physics settings)", GUILayout.Width(480));
        GUI.contentColor = Color.white;
        if (layerRig != layerCharacter && Physics.GetIgnoreLayerCollision(layerCharacter, layerRig))
            GUILayout.Label("(You can now use CharacterController with your characters)", GUILayout.Width(480));

        if (oldMaterialFolder != MaterialFolder || oldLayerCh != layerCharacter || oldLayerRig != layerRig)
        {
            if (File.Exists(Fsave))
            {
                StreamWriter sw = File.CreateText(Fsave);
                if(MaterialFolder)
                    sw.WriteLine(AssetDatabase.GetAssetPath(MaterialFolder));
                else
                    sw.WriteLine("null");
                sw.WriteLine("" + (int)layerCharacter);
                sw.WriteLine("" + (int)layerRig);
                sw.Close();
            }
            if (File.Exists(Msave))
            {
                StreamWriter sw = File.CreateText(Msave);
                if (MaterialFolder)
                    sw.WriteLine(AssetDatabase.GetAssetPath(MaterialFolder));
                else
                    sw.WriteLine("null");
                sw.WriteLine("" + (int)layerCharacter);
                sw.WriteLine("" + (int)layerRig);
                sw.Close();
            }
        }

        if (MaterialFolder != null)
        {
            GUILayout.Space(15);
            GUILayout.BeginHorizontal();
            GUILayout.Label("Character name ", EditorStyles.boldLabel, GUILayout.Width(120));
            myName = EditorGUILayout.TextField(myName, GUILayout.Width(130));
            GUILayout.Label("(Part of materials name)", GUILayout.Width(180));
            GUILayout.EndHorizontal();
            if (selectedCharacter)
            {
                GUILayout.Space(15);
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Create new materials", GUILayout.Width(240), GUILayout.Height(30)))
                {
                    newMaterials();
                }
                GUILayout.Label("(Replace materials of selected character)", GUILayout.Width(250), GUILayout.Height(30));
                GUILayout.EndHorizontal();
                GUILayout.Space(15);
                GUILayout.BeginHorizontal();
                GUILayout.Label("body", EditorStyles.boldLabel, GUILayout.Width(125));
                GUILayout.Label("hair", EditorStyles.boldLabel, GUILayout.Width(125));
                GUILayout.Label("underwear", EditorStyles.boldLabel, GUILayout.Width(125));
                GUILayout.Label("armor", EditorStyles.boldLabel, GUILayout.Width(125));
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                Materials[0] = (Material)EditorGUILayout.ObjectField(Materials[0], typeof(Material), false, GUILayout.Width(125));
                Materials[1] = (Material)EditorGUILayout.ObjectField(Materials[1], typeof(Material), false, GUILayout.Width(125));
                Materials[2] = (Material)EditorGUILayout.ObjectField(Materials[2], typeof(Material), false, GUILayout.Width(125));
                Materials[3] = (Material)EditorGUILayout.ObjectField(Materials[3], typeof(Material), false, GUILayout.Width(125));
                GUILayout.EndHorizontal();
            }
        }
        if (MaterialFolder)
        {
            GUILayout.Space(15);
            GUILayout.BeginHorizontal();
            if (AssetDatabase.IsValidFolder(maleAssetPath + "/CharacterBuilder") && GUILayout.Button("new Male", GUILayout.Width(120), GUILayout.Height(30)))
            {
                if (File.Exists(maleAssetPath + "/CharacterBuilder/DefaultMale.prefab"))
                {
                    GameObject gg = (GameObject)AssetDatabase.LoadAssetAtPath(maleAssetPath + "/CharacterBuilder/DefaultMale.prefab", typeof(GameObject));
                    selectedCharacter = Instantiate(gg);
                    selectedCharacter.transform.eulerAngles = new Vector3(0, 180, 0);
                    selectedCharacter.name = myName;
                    inventory = selectedCharacter.GetComponent<Inventory>();
                    newMaterials();
                    Selection.activeGameObject = selectedCharacter;
                    SetLayers(selectedCharacter);
                }
                else
                {
                    Debug.LogWarning("No DefaultMale.prefab found at the 'CharacterBuilder' folder", null);
                }
            }
            if (AssetDatabase.IsValidFolder(femaleAssetPath + "/CharacterBuilder") && GUILayout.Button("new Female", GUILayout.Width(120), GUILayout.Height(30)))
            {
                if (File.Exists(femaleAssetPath + "/CharacterBuilder/DefaultFemale.prefab"))
                {
                    GameObject gg = (GameObject)AssetDatabase.LoadAssetAtPath(femaleAssetPath + "/CharacterBuilder/DefaultFemale.prefab", typeof(GameObject));
                    selectedCharacter = Instantiate(gg);
                    selectedCharacter.transform.eulerAngles = new Vector3(0, 180, 0);
                    selectedCharacter.name = myName;
                    breastControl bc = selectedCharacter.GetComponent<breastControl>();
                    bc.BreastSize = Random.Range(bc.minValue, bc.maxValue);
                    bc.SetBreastSizeEditor(bc.BreastSize);
                    inventory = selectedCharacter.GetComponent<Inventory>();
                    newMaterials();
                    Selection.activeGameObject = selectedCharacter;
                    SetLayers(selectedCharacter);
                }
                else
                {
                    Debug.LogWarning("No DefaultFemale.prefab found at the 'CharacterBuilder' folder", null);
                }
            }
            GUILayout.Label("Create new character", EditorStyles.boldLabel, GUILayout.Width(240), GUILayout.Height(30));
            GUILayout.EndHorizontal();
        }
        if (selectedCharacter)
        {
            GUILayout.Space(25);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Randomize colors", GUILayout.Width(150), GUILayout.Height(25)))
            {
                RandomColors(0);
            }
            if (GUILayout.Button("Randomize character", GUILayout.Width(150), GUILayout.Height(25)))
            {
                RandomCharacter(0);
                updateMaterials();
            }
            GUILayout.EndHorizontal();
            if (PrefabUtility.IsAnyPrefabInstanceRoot(selectedCharacter) && PrefabUtility.HasPrefabInstanceAnyOverrides(selectedCharacter, false))
            {
                GUILayout.Space(25);
                if (GUILayout.Button("Apply prefab (apply all changes)", GUILayout.Width(350), GUILayout.Height(50)))
                {
                    PrefabUtility.ApplyPrefabInstance(selectedCharacter, InteractionMode.AutomatedAction);
                }
            }
        }
    }
    /*void CleanEditorPreview()
    {
        for (int i=0;i<previewButton.Length;i++)
        {
            DestroyImmediate(previewButton[i]);
        }
        previewButton = new Editor[0];
    }*/
    void FocusOn(GameObject g)
    {
        if (g && g.activeInHierarchy && oldFocus != g)
        {
            GameObject oldSelection = Selection.activeGameObject;
            Selection.activeGameObject = g;
            if (SceneView.lastActiveSceneView.FrameSelected())
                SceneView.lastActiveSceneView.FrameSelected(false, false);

            Selection.activeGameObject = oldSelection;
            oldFocus = g;
            reSelect = -1;
        }
    }
    void OnGUI()
    {
        EditorGUI.DrawRect(new Rect(2, 10, 515, 50), new Color(0.1f, 0.1f, 0.1f));
        GUILayout.Space(15);

        if (selectedCharacter)
            GUILayout.Label(selectedCharacter.name + " selected", EditorStyles.boldLabel);
        else
            GUILayout.Label(" Select a character or create a new one", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();

        int oldMenuSel = menuSel;
        menuSel = GUILayout.SelectionGrid(menuSel,menuT, menuT.Length, GUILayout.Width ( 510));

        if (menuSel != 0 && selectedCharacter)
        {
            EditorGUI.DrawRect(new Rect(2, 68, 515, 50), new Color(0.1f, 0.1f, 0.1f));
            EditorGUI.DrawRect(new Rect(2, 124, 515, 32), new Color(0.1f, 0.1f, 0.1f));
        }
        if(oldMenuSel != menuSel)
            reSelect = 1;
        if (selectedCharacter && Materials[0] && Materials[1] && Materials[2] && Materials[3])
        {
            if (oldMenuSel != menuSel || reSelect==0)
            {
                if (menuSel == 1)
                {
                    SkinnedMeshRenderer[] sm = selectedCharacter.transform.GetComponentsInChildren<SkinnedMeshRenderer>();
                    foreach (SkinnedMeshRenderer _sm in sm)
                    {
                        if (_sm.name.Contains("torso"))
                        {
                            FocusOn(_sm.gameObject);
                            break;
                        }
                    }
                }
                if (menuSel == 2)
                {
                    SkinnedMeshRenderer[] sm = selectedCharacter.transform.GetComponentsInChildren<SkinnedMeshRenderer>();
                    foreach (SkinnedMeshRenderer _sm in sm)
                    {
                        if (_sm.name.Contains("head"))
                        {
                            FocusOn(_sm.gameObject);
                            break;
                        }
                    }
                }
                if (menuSel == 3)
                {
                    SkinnedMeshRenderer[] sm = selectedCharacter.transform.GetComponentsInChildren<SkinnedMeshRenderer>();
                    foreach (SkinnedMeshRenderer _sm in sm)
                    {
                        if (_sm.name.Contains("torso"))
                        {
                            FocusOn(_sm.gameObject);
                            break;
                        }
                    }
                }
                if (menuSel == 4)
                {
                    updateArmorTypeSel = true;
                    FocusOn(selectedCharacter);
                }
            }
        }
        if (menuSel == 0 && reSelect == 0)
        {
            FocusOn(selectedCharacter);
        }
        GUILayout.EndHorizontal();

        if (menuSel == 0)
            SetupMenu();
        if (selectedCharacter)
        {
            if (menuSel == 1)
                SkinMenu();
            else if (menuSel == 2)
                HairMenu();
            else if (menuSel == 3)
                UnderwearMenu();
            else if (menuSel == 4)
                ArmorMenu();
            if (inventory)
                EditorUtility.SetDirty(inventory);
            if (bManager)
            {
                if (bManager.boneChainDictionary.Count == 0)
                    bManager.Awake();
                EditorUtility.SetDirty(bManager);
            }
            EditorUtility.SetDirty(selectedCharacter);
        }
        reSelect--;
    }
}