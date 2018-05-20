using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using scrapper.Scrapper.Entities;

namespace scrapper.Scrapper.Animation
{
    public abstract class AnimatedSprite : Entity
    {
        private readonly byte _animationStepCount;
        private readonly TimeSpan _animationStepTime;
        private readonly TimeSpan _attackAnimationStepTime;
        private readonly byte _spriteHeight;
        private readonly byte _spriteWidth;
        private readonly EPrefab _textureName;
        private byte _animationIndex;
        protected bool _inAttackAnimation = false;
        private Color _color;
        private byte _currentAnimationStepIndex;
        private TimeSpan _elapsedSinceLastAnimationChange = TimeSpan.Zero;
        private Texture2D _sprite;

        public AnimatedSprite(Game game, byte spriteWidth, byte spriteHeight, byte animationStepCount,
            TimeSpan animationStepTime, TimeSpan attackAnimationStepTime, EPrefab textureName, Vector2 position, Color color) : base(game, position)
        {
            _spriteWidth = spriteWidth;
            _spriteHeight = spriteHeight;
            _animationStepTime = animationStepTime;
            _attackAnimationStepTime = attackAnimationStepTime;
            _textureName = textureName;
            _animationStepCount = animationStepCount;
            _color = color;
        }

        public AnimatedSprite(Game game, byte spriteWidth, byte spriteHeight, byte animationStepCount,
            TimeSpan animationStepTime, TimeSpan attackAnimationStepTime, EPrefab textureName, Vector2 position, Color color, float hitBoxRadius) : base(game, position, hitBoxRadius)
        {
            _spriteWidth = spriteWidth;
            _spriteHeight = spriteHeight;
            _animationStepTime = animationStepTime;
            _attackAnimationStepTime = attackAnimationStepTime;
            _textureName = textureName;
            _animationStepCount = animationStepCount;
            _color = color;
        }

        public event BasicEntityEvent DealDamage;

        protected void ResetCurrentAnimationStepIndex()
        {
            _currentAnimationStepIndex = 0;
        }

        public void SetAnimation(byte index)
        {
            _animationIndex = index;
        }

        public void SetAnimation(EView view)
        {
            _animationIndex = (byte) view;
        }

        protected override void LoadContent()
        {
            _sprite = ContentLoader.GetResource<Texture2D>(_textureName);
        }

        public override void Draw(GameTime gameTime)
        {
            _elapsedSinceLastAnimationChange += gameTime.ElapsedGameTime;
            if (_elapsedSinceLastAnimationChange > (_inAttackAnimation ? _attackAnimationStepTime : _animationStepTime))
            {
                _elapsedSinceLastAnimationChange = TimeSpan.Zero;
                _currentAnimationStepIndex++;
                if (_currentAnimationStepIndex == _animationStepCount)
                {
                    _currentAnimationStepIndex = 0;
                    if (_inAttackAnimation) DealDamage?.Invoke(this);
                    _inAttackAnimation = false;
                }
            }

            var source = new Rectangle(_currentAnimationStepIndex * _spriteWidth, (_animationIndex + (_inAttackAnimation ? 4 : 0)) * _spriteHeight,
                _spriteWidth, _spriteHeight);

            ((Game1) Game).SpriteBatch.Draw(_sprite, Position - new Vector2(_spriteWidth / 2f, _spriteHeight / 2f), source, _color);
        }
    }
}