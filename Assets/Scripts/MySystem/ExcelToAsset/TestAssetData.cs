using System;
using UnityEngine;

namespace MySystem.ExcelToAsset
{
    [Serializable]
    public sealed class TestAssetData
    {
        [SerializeField] private string _key = "";
        [SerializeField] private string _name = "";
        [SerializeField] private int _hp = 0;
        [SerializeField] private int _attack = 0;
        [SerializeField] private int _defense = 0;
        [SerializeField] private float _speed = 0;

        public string Key => _key;
        public string Name => _name;
        public int Hp => _hp;
        public int Attack => _attack;
        public int Defense => _defense;
        public float Speed => _speed;


        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="key"></param>
        /// <param name="name"></param>
        /// <param name="hp"></param>
        /// <param name="attack"></param>
        /// <param name="deense"></param>
        /// <param name="speed"></param>
        public TestAssetData(string key, string name, int hp, int attack, int defense, float speed)
        {
            _key = key;
            _name = name;
            _hp = hp;
            _attack = attack;
            _defense = defense;
            _speed = speed;
        }
    }
}
