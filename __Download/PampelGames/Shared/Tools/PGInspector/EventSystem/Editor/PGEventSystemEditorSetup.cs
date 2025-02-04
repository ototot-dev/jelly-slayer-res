// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

#if UNITY_EDITOR
using PampelGames.Shared.Utility;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace PampelGames.Shared.Tools.PGInspector.Editor
{
    public static class PGEventSystemEditorSetup
    {
        public const string EventProperties = "EventProperties";
        
        public static void DrawEventClass(string eventClassName, PGEventClass eventClass, SerializedObject serializedObject,
            ToolbarMenu toolbarMenu, VisualElement ParentElement, bool chanceSlider = false)
        {
            var labelEventName = char.ToUpper(eventClassName[0]) + eventClassName.Substring(1);
            labelEventName = labelEventName.Replace("Class", "");

            toolbarMenu.menu.AppendAction(labelEventName, action =>
            {
                eventClass.activated = true;
                EditorUtility.SetDirty(serializedObject.targetObject);
                SetUnityEventDisplay(eventClassName, eventClass.activated, ParentElement);
            });

            CreateUnityEventWrapper(eventClassName, eventClass, serializedObject, ParentElement, chanceSlider);
            SetUnityEventDisplay(eventClassName, eventClass.activated, ParentElement);
        }

        private static void SetUnityEventDisplay(string eventClassName, bool display, VisualElement ParentElement)
        {
            var UnityEventWrapper = ParentElement.Q<VisualElement>("UnityEvent_" + eventClassName);
            UnityEventWrapper?.PGDisplayStyleFlex(display);
        }

        private static void CreateUnityEventWrapper(string eventClassName, PGEventClass eventClass, SerializedObject serializedObject,
            VisualElement ParentElement, bool chanceSlider)
        {
            var UnityEventWrapper = ParentElement.Q<VisualElement>("UnityEvent_" + eventClassName);
            if (UnityEventWrapper != null) return;

            UnityEventWrapper = new VisualElement();
            UnityEventWrapper.name = "UnityEvent_" + eventClassName;
            
            var UnityEventWrapperTop = new VisualElement();
            UnityEventWrapperTop.name = "UnityEventTop_" + eventClassName;
            
            var UnityEventWrapperProperties = new VisualElement();
            UnityEventWrapperProperties.name = nameof(EventProperties) + eventClassName;
            
            PropertyField eventProperty = new();
            var labelEventName = char.ToUpper(eventClassName[0]) + eventClassName.Substring(1);
            labelEventName = labelEventName.Replace("Class", "");
            eventProperty.label = labelEventName;

            eventProperty.style.flexGrow = 1f;
            VisualElement RemoveButtonWrapper = new();
            ToolbarMenu removeMenu = new();
            removeMenu.PGBorderWidth(1);
            removeMenu.style.height = 24;
            UnityEventWrapperTop.style.flexDirection = FlexDirection.Row;

            var eventClassProperty = serializedObject.FindProperty(eventClassName);
            var eventSerializedProperty = eventClassProperty.FindPropertyRelative(nameof(PGEventClass.unityEvent));
            eventProperty.BindProperty(eventSerializedProperty);

            removeMenu.menu.AppendAction("Remove", action =>
            {
                UnityEventWrapperTop.style.display = DisplayStyle.None;
                eventClass.activated = false;
                EditorUtility.SetDirty(serializedObject.targetObject);
                SetUnityEventDisplay(eventClassName, eventClass.activated, ParentElement);
            });

            UnityEventWrapperTop.Add(eventProperty);
            RemoveButtonWrapper.Add(removeMenu);
            UnityEventWrapperTop.Add(RemoveButtonWrapper);
            
            if (chanceSlider)
            {
                var chance = new Slider();
                chance.tooltip = "Chance of this event being invoked.";
                chance.style.width = 38;
                chance.lowValue = 0;
                chance.highValue = 1;
                chance.showInputField = true;
                chance.direction = SliderDirection.Vertical;
                UnityEventWrapperTop.Insert(1, chance);
                var chanceProperty = eventClassProperty.FindPropertyRelative(nameof(PGEventClass.chance));
                chance.BindProperty(chanceProperty);
            }
            
            UnityEventWrapper.Add(UnityEventWrapperProperties);
            UnityEventWrapper.Add(UnityEventWrapperTop);
            ParentElement.Add(UnityEventWrapper);
        }
    }
}
#endif