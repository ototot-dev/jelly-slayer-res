// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System;
using UnityEngine;

namespace PampelGames.Shared.Tools
{
    [Serializable]
    public class SerializableTreeInstance
    {
        public int prototypeIndex;
        public Vector3 position;
        public float widthScale;
        public float heightScale;
        public float rotation;
        public Color32 color;
        public Color32 lightmapColor;

        public SerializableTreeInstance(TreeInstance treeInstance)
        {
            prototypeIndex = treeInstance.prototypeIndex;
            position = treeInstance.position;
            widthScale = treeInstance.widthScale;
            heightScale = treeInstance.heightScale;
            rotation = treeInstance.rotation;
            color = treeInstance.color;
            lightmapColor = treeInstance.lightmapColor;
        }

        public TreeInstance ToTreeInstance()
        {
            var treeInstance = new TreeInstance
            {
                prototypeIndex = prototypeIndex,
                position = position,
                widthScale = widthScale,
                heightScale = heightScale,
                rotation = rotation,
                color = color,
                lightmapColor = lightmapColor
            };

            return treeInstance;
        }
    }
}