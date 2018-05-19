using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
