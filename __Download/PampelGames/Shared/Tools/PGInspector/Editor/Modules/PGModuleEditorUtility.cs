// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

#if UNITY_EDITOR
using System;
using PampelGames.Shared.Utility;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace PampelGames.Shared.Tools.PGInspector
{
    public static class PGModuleEditorUtility
    {
        public const string Toolbar = "toolbar";
        public const string LeftSide = "leftSide";
        public const string RightSide = "rightSide";
        public const string ItemLabel = "itemLabel";
        public const string ItemToggle = "itemToggle";
        public const string ItemMenu = "itemMenu";
        public const string ActiveButton = "activeButton";
        public const string ActiveButtonImage = "activeButtonImage";
        public const string ItemPropertyParent = "itemPropertyParent";
        public const string ChanceSlider = "chanceSlider";

        public static VisualElement CreateItemParent(PGIModule module, int i)
        {
            return CreateItemParentInternal(module, i, false, false);
        }
        public static VisualElement CreateItemParent(PGIModule module, int i, bool activeButton, bool chanceSlider)
        {
            return CreateItemParentInternal(module, i, activeButton, chanceSlider);
        }
        
        public static VisualElement CreateItemParentWithToggle(PGIModule module, int i)
        {
            var itemParent = CreateItemParentInternal(module, i, false, false);
            var leftSide = itemParent.Q<VisualElement>(LeftSide + i);
            leftSide.style.flexGrow = 1f;
            var itemLabel = leftSide.Q<Label>(ItemLabel + i);
            leftSide.Remove(itemLabel);
            var itemToggle = new ToolbarToggle();
            itemToggle.PGBorderWidth(1f, 0f, 0f, 0f);
            itemToggle.name = ItemToggle + i;
            itemToggle.style.flexGrow = 1f;
            itemToggle.PGMargin(0f);
            itemToggle.PGPadding(3, 3, 0, 0);
            leftSide.Add(itemToggle);
            return itemParent;
        }

        private static VisualElement CreateItemParentInternal(PGIModule module, int i, bool activeButton, bool chanceSlider)
        {
            var itemParent = new VisualElement();
            itemParent.style.marginBottom = 0;
            itemParent.PGPadding(5, 5, 1, 1);
            itemParent.style.paddingBottom = 0;

            var itemWrapper = new VisualElement();
            itemWrapper.style.marginTop = 4;
            itemWrapper.PGBorderWidth(1);
            itemWrapper.PGBorderColor(PGColors.CustomBorder());

            var toolbar = new Toolbar();
            toolbar.name = Toolbar + i;
            toolbar.PGBorderWidth(0);
            toolbar.style.justifyContent = new StyleEnum<Justify>(Justify.SpaceBetween);
            toolbar.style.borderBottomWidth = 1;

            var leftSide = new VisualElement();
            leftSide.name = LeftSide + i;
            leftSide.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);
            var rightSide = new VisualElement();
            rightSide.name = RightSide + i;
            rightSide.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);

            var itemLabel = new Label();
            itemLabel.name = ItemLabel + i;
            itemLabel.style.marginLeft = 3;
            itemLabel.style.unityTextAlign = new StyleEnum<TextAnchor>(TextAnchor.MiddleCenter);
            itemLabel.PGBoldText();

            var itemMenu = new ToolbarMenu();
            itemMenu.name = ItemMenu + i;
            itemMenu.PGBorderWidth(1, 0, 0, 0);

            var itemPropertyParent = new GroupBox();
            itemPropertyParent.name = ItemPropertyParent + i;
            itemPropertyParent.style.marginTop = 2;
            itemPropertyParent.style.marginBottom = 2;
            itemPropertyParent.PGPadding(0);

            itemParent.tooltip = module.ModuleInfo();
            itemLabel.text = module.ModuleName();

            rightSide.Add(itemMenu);

            if (activeButton)
            {
                ToolbarButton moduleActive = new ToolbarButton();
                moduleActive.name = ActiveButton + i;
                moduleActive.text = "";
                moduleActive.tooltip = "Set this item active/inactive.";
                moduleActive.style.alignItems = new StyleEnum<Align>(Align.Center);
                moduleActive.style.justifyContent = new StyleEnum<Justify>(Justify.Center);
                moduleActive.PGMargin(0, 0, 0, 0);
                moduleActive.PGPadding(0);
                moduleActive.PGBorderWidth(1,0, 0, 0);

                VisualElement moduleActiveImage = new VisualElement();
                moduleActiveImage.name = ActiveButtonImage + i;
                moduleActiveImage.style.width = 16;
                moduleActiveImage.style.height = 16;
                moduleActiveImage.PGMargin(3, 2, 2, 2);
                moduleActive.Add(moduleActiveImage);
                
                rightSide.Add(moduleActive);
            }
            
            if(chanceSlider)
            {
                var chance = new Slider();
                chance.name = ChanceSlider + i;
                chance.lowValue = 0f;
                chance.highValue = 1f;
                chance.showInputField = true;
                chance.style.width = 100;
                chance.style.marginRight = 3;
                chance.tooltip = "Chance of this item being applied.";
                var textField = chance.Q<TextField>();
                textField.style.width = 33;
                rightSide.Insert(0, chance);
            }

            leftSide.Add(itemLabel);
            toolbar.Add(leftSide);
            toolbar.Add(rightSide);
            itemWrapper.Add(toolbar);
            itemWrapper.Add(itemPropertyParent);
            itemParent.Add(itemWrapper);
            return itemParent;
        }
    }
}
#endif