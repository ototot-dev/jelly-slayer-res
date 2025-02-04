// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using UnityEngine;

namespace PampelGames.Shared.Utility
{
    internal class PGAutoSetupAttributes
    {
    }

    public class PGHide : PropertyAttribute
    {
    }
    
    public class PGInsertAt : PropertyAttribute
    {
        public int Index { get; }

        public PGInsertAt(int index)
        {
            Index = index;
        }
    }
    
    public class PGClamp : PropertyAttribute
    {
        public float MinValue { get; }
        public float MaxValue { get; }

        public PGClamp(float minValue = 0f, float maxValue = float.MaxValue)
        {
            MinValue = minValue;
            MaxValue = maxValue;
        }
    }

    
    public class PGTagField : PropertyAttribute
    {
    }

    public class PGSlider : PropertyAttribute
    {
        public float LowValue { get; }
        public float HighValue { get; }

        public PGSlider(float lowValue, float highValue)
        {
            LowValue = lowValue;
            HighValue = highValue;
        }
    }

    public class PGMinMaxSlider : PropertyAttribute
    {
        public float LowLimit { get; }
        public float HighLimit { get; }

        public PGMinMaxSlider()
        {
            LowLimit = 0f;
            HighLimit = 1f;
        }
        public PGMinMaxSlider(float lowLimit, float highLimit)
        {
            LowLimit = lowLimit;
            HighLimit = highLimit;
        }
    }

    public class PGLabel : PropertyAttribute
    {
        public string Label { get; }

        public PGLabel(string label)
        {
            Label = label;
        }
    }

    public class PGHeader : PropertyAttribute
    {
        public string Header { get; }
        public HeaderType HeaderType { get; }

        public PGHeader(string header, HeaderType headerType)
        {
            Header = header;
            HeaderType = headerType;
        }
    }

    public enum HeaderType
    {
        Small,
        Big
    }
    
    /// <summary>
    ///     To ignore a value, set it to float.MinValue.
    /// </summary>
    public class PGMargin : PropertyAttribute
    {
        public float MarginLeft { get; }
        public float MarginRight { get; }
        public float MarginTop { get; }
        public float MarginBottom { get; }
        
        public PGMargin(float marginTop, float marginBottom)
        {
            MarginLeft = float.MinValue;
            MarginRight = float.MinValue;
            MarginTop = marginTop;
            MarginBottom = marginBottom;
        }
        
        public PGMargin(float marginLeft, float marginRight, float marginTop, float marginBottom)
        {
            MarginLeft = marginLeft;
            MarginRight = marginRight;
            MarginTop = marginTop;
            MarginBottom = marginBottom;
        }
    }
}