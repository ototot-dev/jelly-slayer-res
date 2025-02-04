// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System;
using System.Linq;
using UnityEngine;

namespace PampelGames.Shared.Tools
{
    /// <summary>
    ///     Represents a serializable terrain.
    /// </summary>
    [Serializable]
    public class SerializableTerrain
    {
        public float[] flatHeights = Array.Empty<float>();
        public int heightmapResolution;

        public SerializableTextures[] flatAlphaArrays = Array.Empty<SerializableTextures>();
        public int alphamapResolution;

        public SerializableDetails[] flatDetailsArrays = Array.Empty<SerializableDetails>();
        public int detailsResolution;

        public SerializableTreeInstance[] treeInstances = Array.Empty<SerializableTreeInstance>();

        /// <summary>
        ///     Serializes a Terrain into a SerializableTerrain object.
        /// </summary>
        public void SerializeTerrain(Terrain terrain, 
            bool setHeights = true, bool setTextures = true, bool setDetails = true, bool setTrees = true)
        {
            var terrainData = terrain.terrainData;

            heightmapResolution = terrainData.heightmapResolution;
            alphamapResolution = terrainData.alphamapResolution;
            detailsResolution = terrainData.detailResolution;

            if(setHeights)
            {
                var heights = terrainData.GetHeights(0, 0, terrainData.heightmapResolution, terrainData.heightmapResolution);
                flatHeights = FlattenArray(heights);
            }


            if(setTextures)
            {
                var alphaMap = terrainData.GetAlphamaps(0, 0, terrainData.alphamapWidth, terrainData.alphamapHeight);
                var textureLayerCount = alphaMap.GetLength(2);
                flatAlphaArrays = new SerializableTextures[textureLayerCount];

                for (var layer = 0; layer < textureLayerCount; layer++)
                {
                    var alphas2D = new float[terrainData.alphamapWidth, terrainData.alphamapHeight];
                    for (var x = 0; x < terrainData.alphamapWidth; x++)
                    for (var y = 0; y < terrainData.alphamapHeight; y++)
                        alphas2D[x, y] = alphaMap[x, y, layer];

                    flatAlphaArrays[layer] = new SerializableTextures
                    {
                        flatArray = FlattenArray(alphas2D),
                        layer = layer
                    };
                }
            }

            if(setDetails)
            {
                flatDetailsArrays = new SerializableDetails[terrainData.detailPrototypes.Length];
                for (var layer = 0; layer < terrainData.detailPrototypes.Length; layer++)
                {
                    var details = terrainData.GetDetailLayer(0, 0, terrainData.detailWidth, terrainData.detailHeight, layer);
                    flatDetailsArrays[layer] = new SerializableDetails
                    {
                        flatArray = FlattenArray(details),
                        layer = layer
                    };
                }
            }

            if(setTrees)
            {
                treeInstances = terrainData.treeInstances.Select(ti => new SerializableTreeInstance(ti)).ToArray();
            }
        }

        /********************************************************************************************************************************/

        /// <summary>
        /// Sets serilalized data into a terrain.
        /// </summary>
        public void SetTerrainData(Terrain terrain,
            bool setHeights = true, bool setTextures = true, bool setDetails = true, bool setTrees = true)
        {
            var terrainData = terrain.terrainData;
            
            if(setHeights)
            {
                float[,] heights = UnflattenArray(flatHeights, heightmapResolution, heightmapResolution);
                terrainData.SetHeights(0, 0, heights);
            }

            if(setTextures)
            {
                float[,,] alphaMap = new float[alphamapResolution, alphamapResolution, flatAlphaArrays.Length];
                
                for (int layer = 0; layer < flatAlphaArrays.Length; layer++)
                {
                    var alphas2D = UnflattenArray(flatAlphaArrays[layer].flatArray, alphamapResolution, alphamapResolution);

                    for (int x = 0; x < alphamapResolution; x++)
                    for (int y = 0; y < alphamapResolution; y++)
                        alphaMap[x, y, layer] = alphas2D[x, y];
                }

                terrainData.SetAlphamaps(0, 0, alphaMap);
            }

            if(setDetails)
            {
                for (int layer = 0; layer < flatDetailsArrays.Length; layer++)
                {
                    int[,] details = UnflattenArray(flatDetailsArrays[layer].flatArray, detailsResolution, detailsResolution);
                    terrainData.SetDetailLayer(0, 0, layer, details);
                }
            }

            if (setTrees)
            {
                TreeInstance[] deserializedTrees = treeInstances.Select(sti => sti.ToTreeInstance()).ToArray();
                terrainData.treeInstances = deserializedTrees;
            }
        }


        /********************************************************************************************************************************/

        /// <summary>
        ///     Flattens a 2D array into a 1D array.
        /// </summary>
        public static float[] FlattenArray(float[,] original)
        {
            var totalHeight = original.GetLength(0);
            var totalWidth = original.GetLength(1);
            var flat = new float[totalHeight * totalWidth];

            for (var i = 0; i < totalHeight; i++)
            for (var j = 0; j < totalWidth; j++)
                flat[i * totalWidth + j] = original[i, j];

            return flat;
        }

        public static int[] FlattenArray(int[,] original)
        {
            var totalHeight = original.GetLength(0);
            var totalWidth = original.GetLength(1);
            var flat = new int[totalHeight * totalWidth];

            for (var i = 0; i < totalHeight; i++)
            for (var j = 0; j < totalWidth; j++)
                flat[i * totalWidth + j] = original[i, j];

            return flat;
        }
        
        /// <summary>
        ///     Unflattens a 1D array into a 2D array. 
        /// </summary>
        public static float[,] UnflattenArray(float[] flatArray, int targetHeight, int targetWidth)
        {
            var original = new float[targetHeight, targetWidth];
            for (var i = 0; i < targetHeight; i++)
            for (var j = 0; j < targetWidth; j++)
                original[i, j] = flatArray[i * targetWidth + j];

            return original;
        }

        public static int[,] UnflattenArray(int[] flatArray, int targetHeight, int targetWidth)
        {
            var original = new int[targetHeight, targetWidth];
            for (var i = 0; i < targetHeight; i++)
            for (var j = 0; j < targetWidth; j++)
                original[i, j] = flatArray[i * targetWidth + j];

            return original;
        }
    }
    

    /********************************************************************************************************************************/
    /********************************************************************************************************************************/
    
    [Serializable]
    public class SerializableTextures
    {
        public float[] flatArray;
        public int layer;
    }

    [Serializable]
    public class SerializableDetails
    {
        public int[] flatArray;
        public int layer;
    }
    
}