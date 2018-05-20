using System;
using Microsoft.Xna.Framework;
using scrapper.Scrapper.Animation;

namespace scrapper.Scrapper.Entities.Mechanics
{
    internal class Projectile : AnimatedSprite
    {
        private readonly Vector2 _direction;
        private readonly ProjectileSettings _settings;
        private TimeSpan _lifeSpan = TimeSpan.Zero;

        public Projectile(Game game, ProjectileSettings settings, Vector2 position, Vector2 direction) :
            base(game, settings.Width, settings.Height, 1, TimeSpan.Zero, TimeSpan.Zero, settings.Prefab, position,
                settings.Color, settings.Hitbox)
        {
            _settings = settings;
            _direction = direction;
            Collision += entity => Die();
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