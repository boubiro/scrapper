using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using scrapper.Scrapper.Helper;

namespace scrapper.Scrapper.Entities.Mechanics.Enemies
{
    class Bender : Enemy
    {
        private BenderSettings _settings;
        private TimeSpan _timeSinceLastLaser = TimeSpan.Zero;
        private bool _laserFired = true;
        private TimeSpan _timeSinceLastBulletSpawn = TimeSpan.Zero;
        private int _tickindex = 0;

        public Bender(Game game, BenderSettings settings, Vector2 position) : base(game, 32, 32, 4, TimeSpan.FromMilliseconds(500), TimeSpan.FromMilliseconds(500), EPrefab.placeholder, settings, position)
        {
            _settings = settings;
            Phase += Randomize;
        }

        public override void Update(GameTime gameTime)
        {
            if (_settings.LaserEnabled)
            {
                _timeSinceLastLaser += gameTime.ElapsedGameTime;
                if (_laserFired && _timeSinceLastLaser > _settings.LaserCooldown)
                {
                    _timeSinceLastLaser = TimeSpan.Zero;
                    _laserFired = false;
                    // Indicator
                }
                else if (!_laserFired && _timeSinceLastLaser > _settings.LaserDelay)
                {
                    _laserFired = true;
                    // Fire!
                }
            }

            if (_settings.ProjectileMode != BenderSettings.EBenderProjectileMode.None)
            {
                _timeSinceLastBulletSpawn += gameTime.ElapsedGameTime;
                if (_timeSinceLastBulletSpawn > _settings.ProjectilesCooldown)
                {
                    _timeSinceLastBulletSpawn = TimeSpan.Zero;
                    FireBullets();
                }
            }

            base.Update(gameTime);
        }

        private void Randomize()
        {
            var rand = new Random();
            var laser = rand.Next(0, 1);
            _settings.LaserEnabled = laser == 0;
            _settings.ProjectileMode = (BenderSettings.EBenderProjectileMode) rand.Next(laser, 2);
        }

        private void FireBullets()
        {
            if (_settings.ProjectileMode == BenderSettings.EBenderProjectileMode.Spiral)
            {
                var bullets = new List<Entity>();
                var anglePerBullet = (360f / _settings.BulletCount) * VectorHelper.DegreeToRadian;
                var maxTick = 1 / (_settings.BulletTickAngle / 360f);
                for (var i = 0; i < _settings.BulletCount; i++)
                {
                    var direction = (Vector2.UnitY * _settings.ProjectileVelocity).Rotate(anglePerBullet * i + _settings.BulletTickAngle * VectorHelper.DegreeToRadian * _tickindex);
                    var projectile = new Projectile(this.Game, _settings.ProjectileSettings, this.Position, direction);
                    projectile.Initialize();
                    bullets.Add(projectile);
                }

                if (_tickindex > maxTick)
                    _tickindex = 1;
                _tickindex++;

                ((Game1) this.Game).AddEntities(bullets);
            }
        }
    }
}
