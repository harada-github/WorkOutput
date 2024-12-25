using System;
using System.Collections.Generic;
using UnityEngine;

namespace MySystem.ExcelToAsset
{
    [Serializable]
    public sealed class TestAssetDatas : ScriptableObject
    {
        [SerializeField] public List<TestAssetData> data;
    }
}