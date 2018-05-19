using System;
using Microsoft.Xna.Framework;
using scrapper.Scrapper.Entities.Mechanics;
using scrapper.Scrapper.Entities.Mechanics.Enemies;

namespace scrapper.Scrapper.Maps
{
    internal class Level1 : Map
    {
        public Level1(Game game) : base(game, new Rectangle(-200, -200, 400, 400))
        {
        }

        public override void Initialize()
        {
            var mapData = new MapData(1)
            {
                new MapData.DataPoint
                {
                    Position = new Vector2(50, 50),
                    Settings = new BenderSettings
                    {
                        ProjectileMode = BenderSettings.EBenderProjectileMode.Spiral,
                        ProjectileSettings = new ProjectileSettings
                        {
                            Color = Color.Red,
                            Height = 5,
                            Width = 5,
                            Prefab = EPrefab.pixel,
                            LifeTime = TimeSpan.FromMilliseconds(5000)
                        },
                        LaserEnabled = false,
                        BulletCount = 5,
                        LaserDelay = TimeSpan.FromMilliseconds(3000),
                        ProjectileVelocity = 150f,
                        MaxHealth = 200,
                        HasPhases = false,
                        ProjectilesCooldown = TimeSpan.FromMilliseconds(10000),
                        PhasingHealth = 50,
                        BulletTickAngle = 1f,
                        LaserCooldown = TimeSpan.FromMilliseconds(20000)
                    },
                    Type = EMechanicType.Bender
                }
            };


            SetMapData(mapData);
        }
    }
}