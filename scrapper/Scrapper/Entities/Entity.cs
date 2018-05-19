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
        private bool _dieded = false;

        public delegate void Death(Entity entity);

        public event Death Dead;

        public Entity(Game game, Vector2 position) : base(game)
        {
            Position = position;
        }

        protected void Die()
        {
            if (_dieded) return;
            _dieded = true;
            Dead?.Invoke(this);
        }
    }
}
