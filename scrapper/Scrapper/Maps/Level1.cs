﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using scrapper.Scrapper.Entities.Mechanics;
using scrapper.Scrapper.Entities.Mechanics.Enemies;

namespace scrapper.Scrapper.Maps
{
    internal class Level1 : Map
    {
        public Level1(Game game) : base(game, new Rectangle(-200, -200, 1280, 1056))
        {
            Background = ContentLoader.GetResource<Texture2D>(EPrefab.map1);
        }

        public override void Initialize()
        {
            var mapData = new MapData(2)
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
                            Height = 10,
                            Width = 10,
                            Prefab = EPrefab.pixel,
                            LifeTime = TimeSpan.FromMilliseconds(500),
                            Hitbox = 5
                        },
                        LaserEnabled = false,
                        BulletCount = 15,
                        LaserDelay = TimeSpan.FromMilliseconds(500),
                        ProjectileVelocity = 150f,
                        MaxHealth = 2,
                        HasPhases = false,
                        ProjectilesCooldown = TimeSpan.FromMilliseconds(1500),
                        PhasingHealth = 50,
                        BulletTickAngle = 8f,
                        LaserCooldown = TimeSpan.FromMilliseconds(20000),
                        Hitbox = 16
                    },
                    Type = EMechanicType.Bender
                },
                new MapData.DataPoint
                {
                    Position = new Vector2(250, 400),
                    Settings = new BenderSettings
                    {
                        ProjectileMode = BenderSettings.EBenderProjectileMode.Spiral,
                        ProjectileSettings = new ProjectileSettings
                        {
                            Color = Color.Red,
                            Height = 20,
                            Width = 10,
                            Prefab = EPrefab.pixel,
                            LifeTime = TimeSpan.FromMilliseconds(2000),
                            Hitbox = 5
                        },
                        LaserEnabled = false,
                        BulletCount = 5,
                        LaserDelay = TimeSpan.FromMilliseconds(500),
                        ProjectileVelocity = 250f,
                        MaxHealth = 200,
                        HasPhases = false,
                        ProjectilesCooldown = TimeSpan.FromMilliseconds(800),
                        PhasingHealth = 50,
                        BulletTickAngle = 12f,
                        LaserCooldown = TimeSpan.FromMilliseconds(20000),
                        Hitbox = 16
                    },
                    Type = EMechanicType.Bender
                }
            };


            SetMapData(mapData);
        }
    }
}