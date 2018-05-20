using System;

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
        public float BulletTickAngle;
        public ProjectileSettings ProjectileSettings;
        public float ProjectileVelocity;

        public enum EBenderProjectileMode
        {
            None,
            Spiral
        }

        public float MaxHealth { get; set; }
        public bool HasPhases { get; set; }
        public float PhasingHealth { get; set; }
        public float Hitbox { get; set; }
    }
}