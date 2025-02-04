// ----------------------------------------------------
// Blood Factory
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using PampelGames.Shared.Editor;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace PampelGames.BloodFactory.Editor
{
    [CustomEditor(typeof(BloodFactory))]
    public class BloodFactoryInspector : UnityEditor.Editor
    {
        public VisualTreeAsset visualTreeAsset;
        private VisualElement container;
        private BloodFactory bloodFactory;

        /********************************************************************************************************************************/

        private ToolbarButton documentation;
        private ToolbarButton execute;

        private VisualElement BloodFactory;
        
        /********************************************************************************************************************************/
        protected void OnEnable()
        {
            container = new VisualElement();
            visualTreeAsset.CloneTree(container);
            bloodFactory = target as BloodFactory;

            FindElements(container);
            BindElements();
            VisualizeElements();
        }

        /********************************************************************************************************************************/

        private void FindElements(VisualElement root)
        {
            documentation = root.Q<ToolbarButton>(nameof(documentation));
            execute = root.Q<ToolbarButton>(nameof(execute));
            BloodFactory = root.Q<VisualElement>(nameof(BloodFactory));
        }
        
        private void BindElements()
        {
            PGEditorAutoSetup.CreateAndBindClassElements<BloodFactory>(serializedObject, BloodFactory);

            var getBloodChildren = new Button();
            getBloodChildren.text = "Get Blood Children";
            getBloodChildren.clicked += () =>
            {
                for (int i = bloodFactory.bloodParticles.Count - 1; i >= 0; i--)
                {
                    if(bloodFactory.bloodParticles[i] != null) continue;
                    bloodFactory.bloodParticles.RemoveAt(i);
                }
                foreach (Transform child in bloodFactory.gameObject.transform)
                {
                    if(!child.gameObject.TryGetComponent<BloodParticle>(out var bloodParticle)) continue;
                    bool exists = false;
                    for (int i = 0; i < bloodFactory.bloodParticles.Count; i++)
                    {
                        if (bloodFactory.bloodParticles[i] == bloodParticle)
                        {
                            exists = true;
                            break;
                        }
                    }
                    if(exists) continue;
                    bloodFactory.bloodParticles.Add(bloodParticle);
                }
                EditorUtility.SetDirty(bloodFactory);
            };
            container.Add(getBloodChildren);
        }

        private void VisualizeElements()
        {
            documentation.tooltip = "Open the documentation page.";
            documentation.clicked += () => { Application.OpenURL(Constants.DocumentationURL); };
            execute.tooltip = "Execute (runtime only)";
            execute.clicked += () =>
            {
                if(Application.isPlaying) bloodFactory.Execute();
            };
        }
        
        
        /********************************************************************************************************************************/
        /********************************************************************************************************************************/

        public override VisualElement CreateInspectorGUI()
        {

            return container;
        }


        /********************************************************************************************************************************/
    }
}