using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using scrapper.Scrapper.Animation;

namespace scrapper.Scrapper.Entities
{
    public class Player : AnimatedSprite
    {
        private const float MoveSpeed = 200.5f;
        private const float DodgeDistance = 30f;

        // ReSharper disable once InconsistentNaming
        private const float TOLERANCE = 0.001f;
        private bool _dodged;
        private Vector2 _position;

        public Vector2 Position => _position;

        public Player(Game game, SpriteBatch spriteBatch) : base(game, 32, 32, 4, TimeSpan.FromMilliseconds(100), "placeholder", spriteBatch)
        {
            _position = Vector2.Zero;
        }

        public override void Update(GameTime gameTime)
        {
            var direction = Vector2.Zero;
            var dodging = false;
            var pad = GamePad.GetState(PlayerIndex.One);
            if (pad.IsConnected)
            {
                direction = pad.ThumbSticks.Left;
                direction.Y = -direction.Y;
                if (pad.IsButtonDown(Buttons.X)) dodging = true;
            }
            else
            {
                if (Keyboard.GetState().IsKeyDown(Keys.W)) direction.Y -= 1;
                if (Keyboard.GetState().IsKeyDown(Keys.S)) direction.Y += 1;
                if (Keyboard.GetState().IsKeyDown(Keys.A)) direction.X -= 1;
                if (Keyboard.GetState().IsKeyDown(Keys.D)) direction.X += 1;
                if (Keyboard.GetState().IsKeyDown(Keys.Space)) dodging = true;
            }

            if (direction.LengthSquared() > 1)
            {
                direction.Normalize();
            }

            if (!_dodged)
            {
                if (dodging)
                {
                    _dodged = true;
                    direction *= DodgeDistance;
                }
            }
            else
            {
                if (!dodging) _dodged = false;
            }

            if (Math.Abs(direction.X) < TOLERANCE)
            {
                if (Math.Abs(direction.Y) > TOLERANCE)
                    SetAnimation(direction.Y < 0 ? EView.back : EView.front);
                else
                    SetAnimation(EView.idle);
            }
            else
            {
                SetAnimation(direction.X > 0 ? EView.right : EView.left);
            }

            _position += direction * (float) gameTime.ElapsedGameTime.TotalSeconds * MoveSpeed;
        }

        public override void Draw(GameTime gameTime)
        {
            Draw(gameTime, _position);
        }
    }
}