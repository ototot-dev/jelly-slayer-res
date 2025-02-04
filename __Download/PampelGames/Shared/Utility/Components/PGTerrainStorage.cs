// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UIElements;
#endif
using PampelGames.Shared.Tools;
using UnityEngine;
using UnityEngine.UIElements;

namespace PampelGames.Shared.Utility
{
    [AddComponentMenu("Pampel Games/Shared/Terrain Storage")]
    public class PGTerrainStorage : MonoBehaviour
    {
        public Terrain terrain;
        public bool heights = true;
        public bool textures = true;
        public bool details = true;
        public bool trees = true;

        public SO_TerrainStorage _TerrainStorage;

        public void SeralizeTerrain()
        {
            if (terrain == null) return;
            _TerrainStorage.serializedTerrain = new SerializableTerrain();
            _TerrainStorage.serializedTerrain.SerializeTerrain(terrain, heights, textures, details, trees);
#if UNITY_EDITOR
            EditorUtility.SetDirty(_TerrainStorage);
#endif
        }
        
        public void SetTerrainData()
        {
            if (terrain == null) return;
            if (_TerrainStorage.serializedTerrain == null) return;
            _TerrainStorage.serializedTerrain.SetTerrainData(terrain, heights, textures, details, trees);
        }
    }



#if UNITY_EDITOR
    [CustomEditor(typeof(PGTerrainStorage))]  
    public class PGTerrainStorageInspector : Editor  
    {  
        private PGTerrainStorage _pgTerrainStorage;
	    
        private VisualElement container;
        
        private ObjectField terrain;
        private ObjectField _TerrainStorage;
        
        private Toolbar ButtonToolbar;
        private VisualElement ToolbarLeft;
        private VisualElement ToolbarRight;
        private ToolbarToggle heights;
        private ToolbarToggle textures;
        private ToolbarToggle details;
        private ToolbarToggle trees;
        private ToolbarButton createStorage;

        private VisualElement ButtonParent;
        private Button serializeButton;
        private Button setDataButton;
        
        /********************************************************************************************************************************/
        
        protected void OnEnable()  
        {         
            _pgTerrainStorage = target as PGTerrainStorage;
            container = new VisualElement();

            BindElements();
            VisualizeElements();
            AddElements();
        }
        
        
        private void BindElements()
        {
            terrain = new ObjectField("Terrain");
            terrain.BindProperty(serializedObject.FindProperty(nameof(terrain)));
            
            _TerrainStorage = new ObjectField("Terrain Storage");
            _TerrainStorage.BindProperty(serializedObject.FindProperty(nameof(_TerrainStorage)));

            ButtonParent = new VisualElement();
            serializeButton = new Button();
            setDataButton = new Button();

            ButtonToolbar = new Toolbar();
            ToolbarLeft = new VisualElement();
            ToolbarRight = new VisualElement();
            heights = new ToolbarToggle();
            heights.text = "Heights";
            heights.BindProperty(serializedObject.FindProperty(nameof(heights)));
            textures = new ToolbarToggle();
            textures.text = "Textures";
            textures.BindProperty(serializedObject.FindProperty(nameof(textures)));
            details = new ToolbarToggle();
            details.text = "Details";
            details.BindProperty(serializedObject.FindProperty(nameof(details)));
            trees = new ToolbarToggle();
            trees.text = "Trees";
            trees.BindProperty(serializedObject.FindProperty(nameof(trees)));
            createStorage = new ToolbarButton();
            createStorage.text = "New Storage";
        }

        private void VisualizeElements()
        {
            terrain.objectType = typeof(Terrain);
            _TerrainStorage.objectType = typeof(SO_TerrainStorage);
            serializeButton.text = "Serialize Terrain";
            serializeButton.tooltip = "Serializes the current terrain data.";
            setDataButton.text = "Reset Terrain";
            setDataButton.tooltip = "Sets the last serialized data back to the terrain.";
            
            ButtonStyle(serializeButton);
            ButtonStyle(setDataButton);

            ButtonToolbar.PGBorderWidth(0);
            ButtonToolbar.PGMargin(0, 0, 10, 0);
            ButtonToolbar.style.backgroundColor = PGColors.InspectorBackground();
            ButtonToolbar.style.justifyContent = new StyleEnum<Justify>(Justify.SpaceBetween);
            ToolbarLeft.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);
            ToolbarRight.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);
            heights.PGBorderWidth(1);
            textures.PGBorderWidth(1);
            details.PGBorderWidth(1);
            trees.PGBorderWidth(1);
            heights.tooltip = "Serializes/sets heightmap data.";
            textures.tooltip = "Serializes/sets textures data.";
            details.tooltip = "Serializes/sets details data.";
            trees.tooltip = "Serializes/sets trees data.";
            createStorage.tooltip = "Create a new storage file to serialize terrain data in the project.";
            createStorage.PGBorderWidth(1);
            createStorage.style.unityTextAlign = new StyleEnum<TextAnchor>(TextAnchor.MiddleCenter);
        }
        
        private void ButtonStyle(Button button)
        {
            button.PGMargin(10,10,10,0);
            button.style.height = 40;
        }
        
        
        private void AddElements()
        {
            ButtonParent.Add(serializeButton);
            ButtonParent.Add(setDataButton);
            
            container.Add(terrain);
            container.Add(_TerrainStorage);
            
            
            ToolbarLeft.Add(heights);
            ToolbarLeft.Add(textures);
            ToolbarLeft.Add(details);
            ToolbarLeft.Add(trees);
            ToolbarRight.Add(createStorage);
            ButtonToolbar.Add(ToolbarLeft);
            ButtonToolbar.Add(ToolbarRight);
            container.Add(ButtonToolbar);
            container.Add(ButtonParent);
        }
        
        /********************************************************************************************************************************/
        
        public override VisualElement CreateInspectorGUI()
        {
            serializeButton.clicked += () =>
            {
                if (_pgTerrainStorage.terrain == null) return;
                _pgTerrainStorage.SeralizeTerrain();
                Debug.Log("Terrain data has been serialized on terrain: " + _pgTerrainStorage.terrain.name);
            };
            
            setDataButton.clicked += () =>
            {
                if (_pgTerrainStorage.terrain == null) return;

                Undo.RegisterCompleteObjectUndo(_pgTerrainStorage.terrain.terrainData, "TerrainUndo");
                Undo.RegisterCompleteObjectUndo(_pgTerrainStorage.terrain.terrainData.alphamapTextures, "TerrainUndo");
                
                _pgTerrainStorage.SetTerrainData();
            };

            createStorage.clicked += () =>
            {
                var newFile = CreateInstance<SO_TerrainStorage>();

                var defaultDirectory = "Assets/";

                var defaultFileName = "New Terrain Storage";
                var extension = "asset";

                var filePath = EditorUtility.SaveFilePanel("Create Terrain Storage File", defaultDirectory, defaultFileName, extension);
                if (string.IsNullOrEmpty(filePath)) return;

                var assetPath = "Assets" + filePath.Substring(Application.dataPath.Length);

                AssetDatabase.CreateAsset(newFile, assetPath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                _pgTerrainStorage._TerrainStorage = newFile;
                EditorUtility.SetDirty(_pgTerrainStorage);
            };
            
            
            return container;  
        }
        
    }  
#endif
}