// ---------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ---------------------------------------------------

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace PampelGames.Shared.Tools
{
    public static class PGShakeSharedClassDrawerCreation
    {
        public static VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var container = new VisualElement();
            if (property == null) return container;
            
            FloatField duration = new("Duration");
            MinMaxSlider fadeInOut = new("Fade In / Out");
            FloatField amplitude = new("Amplitude");
            FloatField frequency = new("Frequency");
            
            duration.BindProperty(property.FindPropertyRelative(nameof(PGShakeSharedClass.duration)));
            fadeInOut.BindProperty(property.FindPropertyRelative(nameof(PGShakeSharedClass.fadeInOut)));
            var amplitudeProperty = property.FindPropertyRelative(nameof(PGShakeSharedClass.amplitude));
            amplitude.BindProperty(amplitudeProperty);
            frequency.BindProperty(property.FindPropertyRelative(nameof(PGShakeSharedClass.frequency)));

            duration.tooltip = "Total duration in seconds.";
            fadeInOut.tooltip = "Fade in and fade out times relative to the duration.";
            fadeInOut.lowLimit = 0f;
            fadeInOut.highLimit = 1f;
            amplitude.tooltip = "Controls the magnitude of the shake effect. Larger values result in a greater shake distance.";
            frequency.tooltip = "Determines the speed or rate of the shake effect. Higher values result in a faster shake.";
            duration.PGClampValue();
            amplitude.PGClampValue();
            frequency.PGClampValue();

            /********************************************************************************************************************************/

            container.Add(duration);
            container.Add(fadeInOut);
            container.Add(amplitude);
            container.Add(frequency);

            return container;
        }
    }
}
#endif