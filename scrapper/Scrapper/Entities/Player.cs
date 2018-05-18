﻿using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using scrapper.Scrapper.Animation;

namespace scrapper.Scrapper.Entities
{
    internal class Player : AnimatedSprite
    {
        private const float MoveSpeed = 200.5f;

        // ReSharper disable once InconsistentNaming
        private const float TOLERANCE = 0.001f;
        private Vector2 _position;

        public Player(Game game) : base(game, 32, 32, 4, TimeSpan.FromMilliseconds(100), "placeholder")
        {
            _position = Vector2.Zero;
        }

        public override void Update(GameTime gameTime)
        {
            var direction = Vector2.Zero;
            if (GamePad.GetState(PlayerIndex.One).IsConnected)
            {
                direction = GamePad.GetState(PlayerIndex.One).ThumbSticks.Left;
            }
            else
            {
                if (Keyboard.GetState().IsKeyDown(Keys.W)) direction.Y -= 1;
                if (Keyboard.GetState().IsKeyDown(Keys.S)) direction.Y += 1;
                if (Keyboard.GetState().IsKeyDown(Keys.A)) direction.X -= 1;
                if (Keyboard.GetState().IsKeyDown(Keys.D)) direction.X += 1;
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