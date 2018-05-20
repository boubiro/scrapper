using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using scrapper.Scrapper.Animation;
using scrapper.Scrapper.Helper;

namespace scrapper.Scrapper.Entities
{
    public class Player : AnimatedSprite
    {
        private const float MoveSpeed = 200.5f;
        private const float DodgeDistance = 10f;
        private const bool Teleport = false;
        public const float SwordRange = 50f;
        public const float SwordSpread = 30f; // naming ftw, degree in both directions

        // ReSharper disable once InconsistentNaming
        private const float TOLERANCE = 0.001f;

        private const byte AnimationStepCount = 4;
        private const float DamageTimeScaling = 0.2f;

        private static readonly TimeSpan AttackAnimationTime =
            TimeSpan.FromMilliseconds(320 / (float) AnimationStepCount);

        private readonly TimeSpan _maxDodgeTimeSpan = TimeSpan.FromMilliseconds(100);
        private bool _attacked;
        private bool _dodged;

#if DEBUG
        private Color _debugColor = Color.White;
#endif

        private TimeSpan _dodgeTimeSpan = TimeSpan.Zero;

        public Player(Game game) : base(game, 32, 32, AnimationStepCount, TimeSpan.FromMilliseconds(100),
            AttackAnimationTime, EPrefab.player,
            Vector2.One * 300, Color.White)
        {
            HitBoxRadius = 16;
            DealDamage += entity => PlayerAttack?.Invoke(this);
        }

        public float SwordDamage { get; private set; } = 1f;
        public Vector2 LastDirection { get; private set; }

        public float AttackDamage
        {
            get
            {
                SwordDamage -= SwordDamage > 1 ? 1f : 0;
                AttackAnimationStepTime =
                    TimeSpan.FromMilliseconds(SwordDamage * DamageTimeScaling * AttackAnimationTime.TotalMilliseconds);
#if DEBUG
                _debugColor = Color.Black;
#endif
                return SwordDamage;
            }
        }

        public event BasicEntityEvent PlayerAttack;

        public void Sharpen(float amount)
        {
            SwordDamage += amount;
#if DEBUG
            _debugColor = Color.White;
#endif
        }

        public override void Update(GameTime gameTime)
        {
            var direction = Vector2.Zero;
            var dodging = false;
            var attacking = false;
            var pad = GamePad.GetState(PlayerIndex.One);
            if (pad.IsConnected)
            {
                direction = pad.ThumbSticks.Left;
                direction.Y = -direction.Y;
                if (pad.IsButtonDown(Buttons.X) && LastDirection.LengthSquared() > 0) dodging = true;
                if (pad.IsButtonDown(Buttons.A) && LastDirection.LengthSquared() > 0) attacking = true;
            }
            else
            {
                if (Keyboard.GetState().IsKeyDown(Keys.W)) direction.Y -= 1;
                if (Keyboard.GetState().IsKeyDown(Keys.S)) direction.Y += 1;
                if (Keyboard.GetState().IsKeyDown(Keys.A)) direction.X -= 1;
                if (Keyboard.GetState().IsKeyDown(Keys.D)) direction.X += 1;
                if (Keyboard.GetState().IsKeyDown(Keys.Space) && LastDirection.LengthSquared() > 0) dodging = true;
                if (Keyboard.GetState().IsKeyDown(Keys.Enter) && LastDirection.LengthSquared() > 0) attacking = true;
            }

            if (direction.LengthSquared() > 1) direction.Normalize();

            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (Teleport)
            {
                if (!_dodged)
                    if (dodging)
                    {
                        _dodged = true;
                        direction *= DodgeDistance;
                    }

                if (!dodging) _dodged = false;
            }
            // ReSharper disable once RedundantIfElseBlock
            else
            {
                if (dodging && !_dodged)
                {
                    direction = LastDirection;
                    direction.Normalize();
                    direction *= DodgeDistance;
                    _dodgeTimeSpan += gameTime.ElapsedGameTime;
                    if (_dodgeTimeSpan > _maxDodgeTimeSpan) _dodged = true;
                }
                else
                {
                    if (!dodging)
                    {
                        _dodged = false;
                        _dodgeTimeSpan = TimeSpan.Zero;
                    }

                    LastDirection = direction;
                }
            }

            if (!_attacked)
            {
                if (attacking)
                {
                    _attacked = true;
                    Attack();
                }
            }
            else
            {
                if (!attacking) _attacked = false;
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

            base.Update(gameTime);
        }

        public new bool Collide(Entity other)
        {
            if (_dodgeTimeSpan > _maxDodgeTimeSpan || _dodgeTimeSpan == TimeSpan.Zero) return base.Collide(other);
            return false;
        }

        private void Attack()
        {
            if (!_inAttackAnimation)
            {
                _inAttackAnimation = true;
                ResetCurrentAnimationStepIndex();
            }
        }

        private Vector2 ClampToMap(Vector2 vec)
        {
            var map = ((Game1) Game).Map;
            if (vec.X < map.X) vec.X = map.X;
            if (vec.X > map.X + map.Width) vec.X = map.X + map.Width;
            if (vec.Y < map.Y) vec.Y = map.Y;
            if (vec.Y > map.Y + map.Height) vec.Y = map.Y + map.Height;

            return vec;
        }

        public override void Draw(GameTime gameTime)
        {
            var sb = ((Game1) Game).SpriteBatch;
#if DEBUG
            var dir = new Vector2(LastDirection.X, LastDirection.Y);
            dir.Normalize();
            DrawingHelper.DrawLine(sb, ContentLoader.GetResource<Texture2D>(EPrefab.pixel), Position,
                Position + (dir * SwordRange).Rotate(SwordSpread * VectorHelper.DegreeToRadian), 1, _debugColor);
            DrawingHelper.DrawLine(sb, ContentLoader.GetResource<Texture2D>(EPrefab.pixel), Position,
                Position + (dir * SwordRange), 1, _debugColor);
            DrawingHelper.DrawLine(sb, ContentLoader.GetResource<Texture2D>(EPrefab.pixel), Position,
                Position + (dir * SwordRange).Rotate(-SwordSpread * VectorHelper.DegreeToRadian), 1, _debugColor);
            _debugColor = Color.Orange;
#endif

            base.Draw(gameTime);

            sb.DrawString(ContentLoader.GetResource<SpriteFont>(EPrefab.StandardFont),
                Position + " " + LastDirection, Vector2.One * 10f, Color.White);
        }
    }
}