using System;
using Microsoft.Xna.Framework;

namespace scrapper.Scrapper.Entities.Mechanics
{
    public struct ProjectileSettings : IMechanicSettings
    {
        public byte Width;
        public byte Height;
        public EPrefab Prefab;
        public Color Color;
        public TimeSpan LifeTime;
        public float Hitbox { get; set; }
    }
}