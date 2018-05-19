using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scrapper.Scrapper.Entities.Mechanics.Enemies
{
    public struct BenderSettings : IEnemySettings
    {
        public bool LaserEnabled;
        public TimeSpan LaserDelay;
        public TimeSpan LaserCooldown;
        public TimeSpan ProjectilesCooldown;
        public EBenderProjectileMode ProjectileMode;
        public uint BulletCount;
        public float BulletAngle;
        
        public enum EBenderProjectileMode
        {
            None,
            Spiral,
            Pulse
        }

        public float MaxHealth { get; set; }
        public bool HasPhases { get; set; }
        public float PhasingHealth { get; set; }
    }
}
