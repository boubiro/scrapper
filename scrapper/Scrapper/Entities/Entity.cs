using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using scrapper.Scrapper.Helper;

namespace scrapper.Scrapper.Entities
{
    public class Entity : DrawableGameComponent
    {
        public delegate void BasicEntityEvent(Entity entity);

        private const float Drag = 1f;

        private bool _dieded;
        private Vector2 _force;

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

        public bool DidCollide { get; private set; }

        public void ApplyForce(Vector2 vec)
        {
            _force += vec;
        }

        public event BasicEntityEvent Dead;
        public event BasicEntityEvent Collision;

        public virtual void GetAttacked(float Damage)
        {
        }

        public override void Update(GameTime gameTime)
        {
            Position += _force * (float) gameTime.ElapsedGameTime.TotalSeconds;
            var appliedDrag = Drag * (float) gameTime.ElapsedGameTime.TotalSeconds;
            _force /= 1 + appliedDrag;

            base.Update(gameTime);
        }

        protected void Die()
        {
            if (_dieded) return;
            _dieded = true;
            Dead?.Invoke(this);
        }

        public bool WouldCollide(Entity other)
        {
            return (other.Position - Position).LengthSquared() -
                   Math.Pow(other.HitBoxRadius + HitBoxRadius, 2) < 0;
        }

        public bool Collide(Entity other)
        {
            if (GetType() == other.GetType()) return false;
            if (!WouldCollide(other)) return false;
            other.Collision?.Invoke(other);
            Collision?.Invoke(this);
            DidCollide = true;
            other.DidCollide = true;
            return true;
        }

        public override void Draw(GameTime gameTime)
        {
#if DEBUG
            var max = 100f;
            for (var i = 0; i < max; i++)
            {
                DrawingHelper.DrawLine(((Game1) this.Game).SpriteBatch, ContentLoader.GetResource<Texture2D>(EPrefab.pixel),
                    Position + (Vector2.UnitX * HitBoxRadius).Rotate(i * (360f / max) * VectorHelper.DegreeToRadian),
                    Position + (Vector2.UnitX * HitBoxRadius).Rotate((i - 1) * (360f / max) * VectorHelper.DegreeToRadian), 1, Color.Orange);
            }
#endif
            base.Draw(gameTime);
        }
    }
}