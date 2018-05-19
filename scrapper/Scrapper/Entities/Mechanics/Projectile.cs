using System;
using Microsoft.Xna.Framework;
using scrapper.Scrapper.Animation;

namespace scrapper.Scrapper.Entities.Mechanics
{
    internal class Projectile : AnimatedSprite
    {
        private readonly ProjectileSettings _settings;
        private TimeSpan _lifeSpan = TimeSpan.Zero;
        private readonly Vector2 _direction;

        public Projectile(Game game, ProjectileSettings settings, Vector2 position, Vector2 direction) :
            base(game, settings.Width, settings.Height, 0, TimeSpan.Zero, settings.Prefab, position, settings.Color)
        {
            _settings = settings;
            _direction = direction;
        }

        public override void Update(GameTime gameTime)
        {
            _lifeSpan += gameTime.ElapsedGameTime;
            if (_lifeSpan > _settings.LifeTime)
            {
                Die();
                base.Update(gameTime);
                return;
            }

            Position += _direction * (float) gameTime.ElapsedGameTime.TotalSeconds;

            base.Update(gameTime);
        }
    }
}