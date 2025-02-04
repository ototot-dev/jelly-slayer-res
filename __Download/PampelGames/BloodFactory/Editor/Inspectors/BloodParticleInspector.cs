// ----------------------------------------------------
// Blood Factory
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using PampelGames.Shared.Editor;
using UnityEditor;
using UnityEngine.UIElements;

namespace PampelGames.BloodFactory.Editor
{
    [CustomEditor(typeof(BloodParticle))]
    public class BloodParticleInspector : UnityEditor.Editor
    {
        private VisualElement container;
        private BloodParticle bloodParticle;

        /********************************************************************************************************************************/

        private VisualElement BloodFactory;

        /********************************************************************************************************************************/
        protected void OnEnable()
        {
            container = new VisualElement();
            bloodParticle = target as BloodParticle;

            FindElements(container);
            BindElements();
        }

        /********************************************************************************************************************************/

        private void FindElements(VisualElement root)
        {
            BloodFactory = new VisualElement();
            container.Add(BloodFactory);
        }

        private void BindElements()
        {
            PGEditorAutoSetup.CreateAndBindClassElements<BloodParticle>(serializedObject, BloodFactory);
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