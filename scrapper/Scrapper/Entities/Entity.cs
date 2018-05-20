using System;
using Microsoft.Xna.Framework;

namespace scrapper.Scrapper.Entities
{
    public class Entity : DrawableGameComponent
    {
        public delegate void BasicEntityEvent(Entity entity);

        private bool _dieded;
        protected bool _collided;

        public Entity(Game game, Vector2 position) : base(game)
        {
            Position = position;
        }

        public Entity(Game game, Vector2 position, float hitBoxRadius) : base(game)
        {
            Position = position;
            HitBoxRadius = hitBoxRadius;
        }

        public Vector2 Position { get; protected set; }
        public float HitBoxRadius { get; protected set; }

        public event BasicEntityEvent Dead;
        public event BasicEntityEvent Collision;

        public virtual void GetAttacked(float Damage)
        {

        }

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