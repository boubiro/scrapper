using System;
using Microsoft.Xna.Framework;
using scrapper.Scrapper.Animation;
using scrapper.Scrapper.Helper;

namespace scrapper.Scrapper.Entities.Mechanics
{
    public class Scrap : AnimatedSprite
    {
        private static readonly Random Rand = new Random();

        public Scrap(Game game, Vector2 position) : base(game, 16, 16, 1, TimeSpan.Zero, TimeSpan.Zero, EPrefab.scrap,
            position, Color.White, 3)
        {
            SetAnimation((byte) Rand.Next(0, 2));
            var force = Rand.Next(50, 200) / 1f;
            var direction = Vector2.UnitX.Rotate(Rand.Next(0, 360) / 360f * 2 * (float) Math.PI);
            ApplyForce(direction * force);
        }
    }
}