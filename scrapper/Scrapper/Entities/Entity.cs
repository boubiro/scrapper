using System;
using Microsoft.Xna.Framework;

namespace scrapper.Scrapper.Entities
{
    public class Entity : DrawableGameComponent
    {
        public delegate void Death(Entity entity);

        private bool _dieded;
        protected bool _collided;

        public Entity(Game game, Vector2 position) : base(game)
        {
            Position = position;
        }

        public Vector2 Position { get; protected set; }
        public float HitBoxRadius { get; protected set; }

        public event Death Dead;
        public event Death Collision;

        protected void Die()
        {
            if (_dieded) return;
            _dieded = true;
            Dead?.Invoke(this);
        }

        public bool DidCollide => _collided;

        public bool WouldCollide(Entity other)
        {
            return (other.Position - Position).LengthSquared() -
                   Math.Pow(other.HitBoxRadius + HitBoxRadius, 2) < 0;
        }

        public bool Collide(Entity other)
        {
            if (this.GetType() == other.GetType()) return false;
            if (!WouldCollide(other)) return false;
            other.Collision?.Invoke(other);
            this.Collision?.Invoke(this);
            this._collided = true;
            other._collided = true;
            return true;
        }
    }
}