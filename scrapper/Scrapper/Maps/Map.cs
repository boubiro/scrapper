using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using scrapper.Scrapper.Entities;
using scrapper.Scrapper.Entities.Mechanics;
using scrapper.Scrapper.Entities.Mechanics.Enemies;
using scrapper.Scrapper.Helper;

namespace scrapper.Scrapper.Maps
{
    public abstract class Map : DrawableGameComponent
    {
        private readonly List<Entity> _componentsToAdd = new List<Entity>();
        private readonly List<Entity> _componentsToRemove = new List<Entity>();
        private readonly List<Entity> _dynamicEntities = new List<Entity>();
        private readonly List<Entity> _visibleComponents = new List<Entity>();
        private Rectangle _dimensions;
        private MapData _mapData;

        public Map(Game game, Rectangle dimensions) : base(game)
        {
            Dimensions = dimensions;
        }

        public Map(Game game, Rectangle dimensions, MapData mapData) : base(game)
        {
            _mapData = mapData;
            Dimensions = dimensions;
        }

        public byte WallWidth { get; set; } = 3;

        public Rectangle Dimensions
        {
            get => _dimensions;
            set
            {
                _dimensions = value;
                ((Game1) Game).Camera.Map = value;
            }
        }

        public void AddEntities(List<Entity> entities)
        {
            foreach (var entity in entities) entity.Dead += RemoveNextUpdate;
            _componentsToAdd.AddRange(entities);
        }

        public void AttackMove(Entity entity)
        {
            var player = (Player) entity;
            foreach (var component in _visibleComponents)
                if ((player.Position - component.Position).LengthSquared() - Math.Pow(component.HitBoxRadius, 2) <
                    Math.Pow(Player.SwordRange, 2))
                    if (player.LastDirection.internalAngle(component.Position - player.Position) <
                        Player.SwordSpread * VectorHelper.DegreeToRadian)
                        component.GetAttacked(player.AttackDamage);
        }

        public void SetMapData(MapData mapData)
        {
            _mapData = mapData;
            LoadContent();
        }

        protected override void LoadContent()
        {
            foreach (var data in _mapData)
            {
                var dataPoint = (MapData.DataPoint) data;
                switch (dataPoint.Type)
                {
                    case EMechanicType.Bender:
                        var bender = new Bender(Game, (BenderSettings) dataPoint.Settings, dataPoint.Position);
                        bender.Initialize();
                        bender.Dead += SpawnScrap;
                        _visibleComponents.Add(bender);
                        break;
                }
            }

            foreach (var entity in _visibleComponents)
            {
                entity.Dead += RemoveNextUpdate;
            }

            base.LoadContent();
        }

        private void SpawnScrap(Entity enemy)
        {
            var count = 0;
            if (enemy.GetType() == typeof(Bender)) count = 10;

            for (var i = 0; i < count; i++)
            {
                var scrap = new Scrap(Game, enemy.Position);
                scrap.Initialize();
                _componentsToAdd.Add(scrap);
            }
        }

        private void RemoveNextUpdate(Entity entity)
        {
            _componentsToRemove.Add(entity);
        }

        public void Update(GameTime gameTime, Player player)
        {
            foreach (var entity in _componentsToAdd)
            {
                entity.Dead += RemoveNextUpdate;
                _dynamicEntities.Add(entity);
            }

            _componentsToAdd.Clear();

            foreach (var drawableGameComponent in _visibleComponents) drawableGameComponent.Update(gameTime);

            foreach (var dynamicEntity in _dynamicEntities)
            {
                dynamicEntity.Update(gameTime);
                foreach (var entity in _dynamicEntities)
                {
                    if (dynamicEntity.DidCollide) break;
                    dynamicEntity.Collide(entity);
                }

                foreach (var entity in _visibleComponents)
                {
                    if (entity.GetType().IsSubclassOf(typeof(Enemy))) break;
                    if (dynamicEntity.DidCollide) break;
                    dynamicEntity.Collide(entity);
                }

                player.Collide(dynamicEntity);
            }

            foreach (var entity in _componentsToRemove)
            {
                _visibleComponents.Remove(entity);
                _dynamicEntities.Remove(entity);
            }
            _componentsToRemove.Clear();

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (var entity in _dynamicEntities) entity.Draw(gameTime);

            DrawingHelper.DrawRectangle(((Game1) Game).SpriteBatch, ContentLoader.GetResource<Texture2D>(EPrefab.pixel),
                Dimensions, WallWidth, Color.Black);

            foreach (var drawableGameComponent in _visibleComponents) drawableGameComponent.Draw(gameTime);

            base.Draw(gameTime);
        }
    }
}