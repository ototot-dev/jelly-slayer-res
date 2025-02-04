// ---------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ---------------------------------------------------

using System;
using UnityEngine.UIElements;
#if UNITY_EDITOR
using UnityEditor.UIElements;
#endif

namespace PampelGames.Shared.Utility
{
    public static class PGFloatFieldExtensions
    {
#if UNITY_EDITOR
        /// <summary>
        ///     Rounds the value of a FloatField to a specified number of decimal points.
        /// </summary>
        public static void PGRoundValues(this FloatField floatField, int decimalPoints)
        {
            floatField.RegisterValueChangedCallback(evt =>
            {
                var roundedValue = (float) Math.Round(floatField.value, decimalPoints);
                floatField.value = roundedValue;
            });
        }
#endif
    }
}