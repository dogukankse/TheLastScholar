using System;
using Base;
using UnityEngine;

namespace Characters
{
    public class Player : MonoSingleton<Player>
    {
        public Action<int> TakeDamage;
        public Vector2 Position { get; set; }
        public string LifeTime { get; set; }
        public int KilledEnemyCount { get; set; }
        public float Wisdom { get; set; } = 5;

        
    }
}