using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using scrapper.Scrapper.Animation;

namespace scrapper.Scrapper.Entities.Mechanics.Enemies
{
    public abstract class Enemy : AnimatedSprite
    {
        private readonly float _maxHealth;
        private float _health;
        private readonly bool _hasPhases;
        private readonly float _phasingHealth;
        private bool _hasPhased = false;
        private bool _isDead = false;

        protected Enemy(Game game, byte spriteWidth, byte spriteHeight, byte animationStepCount, TimeSpan animationStepTime, EPrefab textureName, IEnemySettings settings, Vector2 position) :
            base(game, spriteWidth, spriteHeight, animationStepCount, animationStepTime, textureName, position)
        {
            this._maxHealth = settings.MaxHealth;
            _health = _maxHealth;
            _hasPhases = settings.HasPhases;
            _phasingHealth = settings.PhasingHealth;
        }

        public delegate void Phasing();
        public event Phasing Phase;

        public void ReceiveDamage(float damage)
        {
            _health -= damage;
            if (_health <= 0)
            {
                _isDead = true;
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
