// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using PampelGames.Shared.Tools;
using UnityEngine;

namespace PampelGames.Shared.Utility
{
    // [CreateAssetMenu(fileName = "TerrainStorage", menuName = "Pampel Games/Shared/Terrain Storage", order = 1)]
    public class SO_TerrainStorage : ScriptableObject
    {
        public SerializableTerrain serializedTerrain;
    }
}
