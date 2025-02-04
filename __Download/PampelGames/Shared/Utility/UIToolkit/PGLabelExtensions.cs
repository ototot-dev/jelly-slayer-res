// ---------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ---------------------------------------------------

using UnityEngine;
using UnityEngine.UIElements;

namespace PampelGames.Shared.Utility
{
    /// <summary>
    ///     See also <see cref="PGVisualElementExtensions"/> for header-wrapper styles.
    /// </summary>
    public static class PGLabelExtensions
    {

        /// <summary>
        ///     Small header for a section.
        /// </summary>
        public static void PGHeaderSmall(this Label label)
        {
            label.PGMargin(0,0,4,2);
            label.PGBoldText();
        }
        
        /// <summary>
        ///     Big header for a section.
        /// </summary>
        public static void PGHeaderBig(this Label label)
        {
            label.PGBoldText();
            label.style.fontSize = 14;
            label.PGMargin(0f, 0f, 12f, 2f);
        }

        /// <summary>
        ///     Sets up the label style similar to the FloatField label.
        /// </summary>
        public static void PGFloatFieldLabel(this Label label)
        {
            label.AddToClassList(PGConstantsUSS.BaseFieldLabel);
            label.AddToClassList(PGConstantsUSS.BaseTextFieldLabel);
            label.AddToClassList(PGConstantsUSS.FloatFieldLabel);
            label.AddToClassList(PGConstantsUSS.BaseFieldLabelWithDragger);
        }

        
    }
}