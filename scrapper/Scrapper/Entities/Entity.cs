using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace scrapper.Scrapper.Entities
{
    public class Entity : DrawableGameComponent
    {
        public Vector2 Position { get; protected set; }

        public Entity(Game game) : base(game)
        {
        }
    }
}
