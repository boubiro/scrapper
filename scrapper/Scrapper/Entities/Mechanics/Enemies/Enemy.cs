using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using scrapper.Scrapper.Animation;

namespace scrapper.Scrapper.Entities.Mechanics.Enemies
{
    public abstract class Enemy : AnimatedSprite
    {
        protected Enemy(Game game, byte spriteWidth, byte spriteHeight, byte animationStepCount, TimeSpan animationStepTime, EPrefab textureName) :
            base(game, spriteWidth, spriteHeight, animationStepCount, animationStepTime, textureName)
        {
        }
    }
}
