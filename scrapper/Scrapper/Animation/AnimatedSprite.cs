using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace scrapper.Scrapper.Animation
{
    public class AnimatedSprite : DrawableGameComponent
    {
        private readonly byte _animationStepCount;
        private readonly TimeSpan _animationStepTime;
        private readonly Game _game;
        private readonly byte _spriteHeight;
        private readonly byte _spriteWidth;
        private readonly string _textureName;
        private byte _animationIndex;
        private byte _currentAnimationIndex;
        private TimeSpan _elapsedSinceLastAnimationChange = TimeSpan.Zero;
        private Texture2D _sprite;
        private readonly SpriteBatch _spriteBatch;

        public AnimatedSprite(Game game, byte spriteWidth, byte spriteHeight, byte animationStepCount,
            TimeSpan animationStepTime, string textureName, SpriteBatch spriteBatch) : base(game)
        {
            _spriteWidth = spriteWidth;
            _spriteHeight = spriteHeight;
            _animationStepTime = animationStepTime;
            _textureName = textureName;
            _spriteBatch = spriteBatch;
            _game = game;
            _animationStepCount = animationStepCount;
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
            _sprite = _game.Content.Load<Texture2D>(_textureName);
        }

        public void Draw(GameTime gameTime, Vector2 position)
        {
            _elapsedSinceLastAnimationChange += gameTime.ElapsedGameTime;
            if (_elapsedSinceLastAnimationChange > _animationStepTime)
            {
                _elapsedSinceLastAnimationChange = TimeSpan.Zero;
                _currentAnimationIndex++;
                if (_currentAnimationIndex >= _animationStepCount) _currentAnimationIndex = 0;
            }

            var source = new Rectangle(_currentAnimationIndex * _spriteWidth, _animationIndex * _spriteHeight,
                _spriteWidth, _spriteHeight);
            
            _spriteBatch.Draw(_sprite, position, source, Color.White);
        }
    }
}