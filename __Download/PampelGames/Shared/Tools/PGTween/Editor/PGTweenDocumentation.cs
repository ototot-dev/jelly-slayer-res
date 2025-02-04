// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System.IO;
using UnityEngine;

namespace PampelGames.Shared.Tools
{
    public static class PGTweenDocumentation 
    {
        public static void OpenEasingTypes()
        {
            string[] files = Directory.GetFiles(Application.dataPath, "PGTweenEasingTypes.html", SearchOption.AllDirectories);
            if (files.Length > 0)
            {
                string filePath = files[0];
                Application.OpenURL("file://" + filePath);
            }
            else
            {
                Debug.LogError("HTML file not found in the project folder.");
            }
        }
		
    }
}