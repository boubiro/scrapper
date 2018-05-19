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

        public Player(Game game) : base(game, 32, 32, 4, TimeSpan.FromMilliseconds(100), EPrefab.placeholder, Vector2.One * 300, Color.White)
        {
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
                    SetAnimation(direction.Y < 0 ? EView.Back : EView.Front);
                else
                    SetAnimation(EView.Idle);
            }
            else
            {
                SetAnimation(direction.X > 0 ? EView.Right : EView.Left);
            }

            Position += direction * (float) gameTime.ElapsedGameTime.TotalSeconds * MoveSpeed;
            Position = ClampToMap(Position);
        }

        private Vector2 ClampToMap(Vector2 vec)
        {
            var map = ((Game1) this.Game).Map;
            if (vec.X < map.X) vec.X = map.X;
            if (vec.X > map.X + map.Width) vec.X = map.X + map.Width;
            if (vec.Y < map.Y) vec.Y = map.Y;
            if (vec.Y > map.Y + map.Height) vec.Y = map.Y + map.Height;

            return vec;
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            ((Game1) this.Game).SpriteBatch.DrawString(ContentLoader.GetResource<SpriteFont>(EPrefab.StandardFont), Position.ToString(), Vector2.One * 10f, Color.White);
        }
    }
}