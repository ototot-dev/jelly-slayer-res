// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using PampelGames.Shared.Utility;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace PampelGames.Shared.Editor
{
    public static class PGEditorAutoSetup
    {
        /// <summary>
        ///     Method to dynamically create and bind UI elements for a specified class type.
        /// </summary>
        public static void CreateAndBindClassElements<T>(SerializedObject serializedObject, VisualElement parentElement)
        {
            CreateAndBindClassElementsInternal<T>(serializedObject, null, parentElement);
        }

        /// <summary>
        ///     Method to dynamically create and bind UI elements for a specified class type.
        /// </summary>
        public static void CreateAndBindClassElements<T>(SerializedProperty property, VisualElement parentElement)
        {
            CreateAndBindClassElementsInternal<T>(null, property, parentElement);
        }

        /********************************************************************************************************************************/

        private static void CreateAndBindClassElementsInternal<T>(SerializedObject serializedObject, SerializedProperty property,
            VisualElement parentElement)
        {
            var isSerializedObject = serializedObject != null;

            var insertAtClasses = new List<InsertAtClass>();

            foreach (var field in typeof(T).GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                /********************************************************************************************************************************/
                // Custom Attributes

                var headerAttr = field.GetCustomAttribute<PGHeader>();
                var headerUsed = false;
                if (headerAttr != null)
                {
                    var label = new Label(CreateLabelString(headerAttr.Header));
                    label.name = field.Name;
                    if (headerAttr.HeaderType == HeaderType.Small) label.PGHeaderSmall();
                    else label.PGHeaderBig();
                    parentElement.Add(label);
                    headerUsed = true;
                }

                if (field.GetCustomAttribute<PGHide>() != null) continue;

                var fieldLabel = field.Name;
                var nameAttr = field.GetCustomAttribute<PGLabel>();
                if (nameAttr != null) fieldLabel = nameAttr.Label;
                else fieldLabel = CreateLabelString(fieldLabel);

                var isTagField = field.GetCustomAttribute<PGTagField>() != null;
                if (isTagField && field.FieldType != typeof(string))
                {
                    Debug.LogWarning("The PGTagField attribute can only be used on strings!\n" +
                                     "Field: " + field.Name);
                    continue;
                }

                var pgSlider = field.GetCustomAttribute<PGSlider>();
                var isSlider = pgSlider != null;
                if (isSlider && field.FieldType != typeof(float))
                {
                    Debug.LogWarning("The PGSlider attribute can only be used on floats!\n" +
                                     "Field: " + field.Name);
                    continue;
                }

                var pgMinMaxSlider = field.GetCustomAttribute<PGMinMaxSlider>();
                var isMinMaxSlider = pgMinMaxSlider != null;
                if (isMinMaxSlider && field.FieldType != typeof(Vector2))
                {
                    Debug.LogWarning("The PGSlider attribute can only be used on Vector2!\n" +
                                     "Field: " + field.Name);
                    continue;
                }

                var pgClamp = field.GetCustomAttribute<PGClamp>();
                var isClamped = pgClamp != null;

                var pgInsertAt = field.GetCustomAttribute<PGInsertAt>();
                var pgMargin = field.GetCustomAttribute<PGMargin>();


                /********************************************************************************************************************************/
                // Binding Fields

                if (field.FieldType == typeof(LayerMask))
                {
                    var layerField = new LayerMaskField(fieldLabel);
                    layerField.name = field.Name;
                    BindElement(layerField, field.Name);
                    AddToParent(layerField, pgInsertAt, pgMargin);
                }
                else if (field.FieldType == typeof(string) && isTagField)
                {
                    var tagField = new TagField(fieldLabel);
                    tagField.name = field.Name;
                    BindElement(tagField, field.Name);
                    AddToParent(tagField, pgInsertAt, pgMargin);
                }
                else if (field.FieldType == typeof(string))
                {
                    var textField = new TextField(fieldLabel);
                    textField.name = field.Name;
                    BindElement(textField, field.Name);
                    AddToParent(textField, pgInsertAt, pgMargin);
                }
                else if (field.FieldType.IsSubclassOf(typeof(Object)))
                {
                    var objectField = new ObjectField(fieldLabel);
                    objectField.name = field.Name;
                    objectField.objectType = field.FieldType;
                    BindElement(objectField, field.Name);
                    AddToParent(objectField, pgInsertAt, pgMargin);
                }
                else if (field.FieldType == typeof(bool))
                {
                    var toggle = new Toggle(fieldLabel);
                    toggle.PGToggleStyleDefault();
                    toggle.name = field.Name;
                    BindElement(toggle, field.Name);
                    AddToParent(toggle, pgInsertAt, pgMargin);
                }
                else if (field.FieldType == typeof(int))
                {
                    var integerField = new IntegerField(fieldLabel);
                    integerField.name = field.Name;
                    if (isClamped) integerField.PGClampValue(Mathf.RoundToInt(pgClamp.MinValue), Mathf.RoundToInt(pgClamp.MaxValue));
                    BindElement(integerField, field.Name);
                    AddToParent(integerField, pgInsertAt, pgMargin);
                }
                else if (field.FieldType == typeof(float) && isSlider)
                {
                    var slider = new Slider(fieldLabel);
                    slider.name = field.Name;
                    slider.lowValue = pgSlider.LowValue;
                    slider.highValue = pgSlider.HighValue;
                    slider.showInputField = true;
                    BindElement(slider, field.Name);
                    AddToParent(slider, pgInsertAt, pgMargin);
                }
                else if (field.FieldType == typeof(float))
                {
                    var floatField = new FloatField(fieldLabel);
                    floatField.name = field.Name;
                    if (isClamped) floatField.PGClampValue(pgClamp.MinValue, pgClamp.MaxValue);
                    BindElement(floatField, field.Name);
                    AddToParent(floatField, pgInsertAt, pgMargin);
                }
                else if (field.FieldType == typeof(Vector2) && isMinMaxSlider)
                {
                    var minMaxSlider = new MinMaxSlider(fieldLabel);
                    minMaxSlider.name = field.Name;
                    minMaxSlider.lowLimit = pgMinMaxSlider.LowLimit;
                    minMaxSlider.highLimit = pgMinMaxSlider.HighLimit;
                    BindElement(minMaxSlider, field.Name);
                    AddToParent(minMaxSlider, pgInsertAt, pgMargin);
                }
                else if (field.FieldType == typeof(Vector2))
                {
                    var vector2Field = new Vector2Field(fieldLabel);
                    vector2Field.name = field.Name;
                    if (isClamped) vector2Field.PGClampValue(pgClamp.MinValue, pgClamp.MaxValue);
                    BindElement(vector2Field, field.Name);
                    AddToParent(vector2Field, pgInsertAt, pgMargin);
                }
                else if (field.FieldType == typeof(Vector3))
                {
                    var vector3Field = new Vector3Field(fieldLabel);
                    vector3Field.name = field.Name;
                    if (isClamped) vector3Field.PGClampValue(pgClamp.MinValue, pgClamp.MaxValue);
                    BindElement(vector3Field, field.Name);
                    AddToParent(vector3Field, pgInsertAt, pgMargin);
                }
                else if (field.FieldType == typeof(Vector2Int))
                {
                    var vector2IntField = new Vector2IntField(fieldLabel);
                    vector2IntField.name = field.Name;
                    if (isClamped) vector2IntField.PGClampValue(Mathf.RoundToInt(pgClamp.MinValue), Mathf.RoundToInt(pgClamp.MaxValue));
                    BindElement(vector2IntField, field.Name);
                    AddToParent(vector2IntField, pgInsertAt, pgMargin);
                }
                else if (field.FieldType == typeof(Vector3Int))
                {
                    var vector3IntField = new Vector3IntField(fieldLabel);
                    vector3IntField.name = field.Name;
                    if (isClamped) vector3IntField.PGClampValue(Mathf.RoundToInt(pgClamp.MinValue), Mathf.RoundToInt(pgClamp.MaxValue));
                    BindElement(vector3IntField, field.Name);
                    AddToParent(vector3IntField, pgInsertAt, pgMargin);
                }
                else if (field.FieldType.IsEnum)
                {
                    var enumField = new EnumField(fieldLabel);
                    enumField.name = field.Name;
                    BindElement(enumField, field.Name);
                    AddToParent(enumField, pgInsertAt, pgMargin);
                }
                else if (field.FieldType == typeof(AnimationCurve))
                {
                    var curveField = new CurveField(fieldLabel);
                    curveField.name = field.Name;
                    BindElement(curveField, field.Name);
                    AddToParent(curveField, pgInsertAt, pgMargin);
                }
                else if (field.FieldType.IsGenericType && field.FieldType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    var listProperty = serializedObject.FindProperty(field.Name);
                    var listView = new ListView();
                    listView.BindProperty(listProperty);
                    listView.PGObjectListViewStyle(headerUsed ? null : fieldLabel);
                    listView.reorderMode = ListViewReorderMode.Animated;
                    listView.reorderable = true;
                    AddToParent(listView, pgInsertAt, pgMargin);
                }
                else if (field.FieldType.IsClass)
                {
                    var propertyField = new PropertyField();
                    propertyField.name = field.Name;
                    BindElement(propertyField, field.Name);
                    AddToParent(propertyField, pgInsertAt, pgMargin);
                }
            }

            insertAtClasses = insertAtClasses.OrderBy(i => i.index).ToList();

            for (var i = 0; i < insertAtClasses.Count; i++)
            {
                var insertClass = insertAtClasses[i];
                parentElement.Insert(insertClass.index, insertClass.element);
            }

            return;

            /********************************************************************************************************************************/

            void BindElement(IBindable element, string fieldName)
            {
                if (isSerializedObject) element.PGSetupBindProperty(serializedObject, fieldName);
                else element.PGSetupBindPropertyRelative(property, fieldName);
            }

            void AddToParent(VisualElement element, PGInsertAt pgInsertAt, PGMargin pgMargin)
            {
                if (pgInsertAt == null) parentElement.Add(element);
                else insertAtClasses.Add(new InsertAtClass(element, pgInsertAt.Index));

                if (pgMargin != null)
                {
                    if(pgMargin.MarginLeft > -9999f) element.style.marginLeft = pgMargin.MarginLeft;
                    if (pgMargin.MarginRight > -9999f) element.style.marginRight = pgMargin.MarginRight;
                    if (pgMargin.MarginTop > -9999f) element.style.marginTop = pgMargin.MarginTop;
                    if (pgMargin.MarginBottom > -9999f) element.style.marginBottom = pgMargin.MarginBottom;
                }
            }
        }

        private static string CreateLabelString(string labelName)
        {
            if (string.IsNullOrEmpty(labelName)) return string.Empty;

            var sb = new StringBuilder();
            sb.Append(char.ToUpper(labelName[0]));

            for (var i = 1; i < labelName.Length; i++)
            {
                if (char.IsUpper(labelName[i])) sb.Append(' ');
                sb.Append(labelName[i]);
            }

            return sb.ToString();
        }

        private class InsertAtClass
        {
            public readonly VisualElement element;
            public readonly int index;

            public InsertAtClass(VisualElement element, int index)
            {
                this.element = element;
                this.index = index;
            }
        }
    }
}