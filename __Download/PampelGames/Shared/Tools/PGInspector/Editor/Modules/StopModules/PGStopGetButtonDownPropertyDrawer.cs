﻿// ---------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ---------------------------------------------------

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace PampelGames.Shared.Tools.PGInspector.Editor
{
    [CustomPropertyDrawer(typeof(PGStopGetButtonDown))]
    public class PGStopGetButtonDownPropertyDrawer : PropertyDrawer
    {
        private SerializedProperty buttonNameProperty;
        private readonly TextField buttonName = new("Button");


        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var container = new VisualElement();

            FindAndBindProperties(property);
            DrawStop(property);

            container.Add(buttonName);
            return container;
        }

        private void FindAndBindProperties(SerializedProperty property)
        {
            buttonNameProperty = property.FindPropertyRelative(nameof(PGStopGetButtonDown.buttonName));
            buttonName.BindProperty(buttonNameProperty);
        }


        /********************************************************************************************************************************/

        private void DrawStop(SerializedProperty property)
        {
        }
    }
}
#endif