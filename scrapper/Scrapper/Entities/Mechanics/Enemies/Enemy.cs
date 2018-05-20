using System;
using Microsoft.Xna.Framework;
using scrapper.Scrapper.Animation;

namespace scrapper.Scrapper.Entities.Mechanics.Enemies
{
    public abstract class Enemy : AnimatedSprite
    {
        public delegate void Phasing();

        private readonly bool _hasPhases;
        private readonly float _maxHealth;
        private readonly float _phasingHealth;
        private bool _hasPhased;
        private float _health;
        private bool _isDead = false;

        protected Enemy(Game game, byte spriteWidth, byte spriteHeight, byte animationStepCount,
            TimeSpan animationStepTime, TimeSpan attackAnimationStepTime, EPrefab textureName, IEnemySettings settings,
            Vector2 position) :
            base(game, spriteWidth, spriteHeight, animationStepCount, animationStepTime, attackAnimationStepTime,
                textureName, position, Color.White, settings.Hitbox)
        {
            _maxHealth = settings.MaxHealth;
            _health = _maxHealth;
            _hasPhases = settings.HasPhases;
            _phasingHealth = settings.PhasingHealth;
        }

        public event Phasing Phase;

        public override void GetAttacked(float damage)
        {
            ReceiveDamage(damage);
        }

        public void ReceiveDamage(float damage)
        {
            _health -= damage;
            if (_health <= 0)
            {
                Die();
                return;
            }

            if (!_hasPhases && !_hasPhased) return;
            if (_health <= _phasingHealth)
            {
                _hasPhased = true;
                Phase?.Invoke();
            }
        }
    }
}