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
        private MapData _mapData;
        private readonly List<Entity> _visibleComponents = new List<Entity>();
        private readonly List<Entity> _cmponentsToRemove = new List<Entity>();
        private Rectangle _dimensions;
        public byte WallWidth { get; set; } = 3;

        public Rectangle Dimensions
        {
            get => _dimensions;
            set
            {
                _dimensions = value;
                ((Game1) this.Game).Camera.Map = value;
            }
        }

        public Map(Game game, Rectangle dimensions) : base(game)
        {
            Dimensions = dimensions;
        }

        public Map(Game game, Rectangle dimensions, MapData mapData) : base(game)
        {
            _mapData = mapData;
            Dimensions = dimensions;
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
                        var bender = new Bender(this.Game, (BenderSettings) dataPoint.Settings, dataPoint.Position);
                        bender.Initialize();
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

        private void RemoveNextUpdate(Entity entity)
        {
            _cmponentsToRemove.Add(entity);
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var drawableGameComponent in _visibleComponents)
            {
                drawableGameComponent.Update(gameTime);
            }

            foreach (var entity in _cmponentsToRemove)
            {
                _visibleComponents.Remove(entity);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            DrawingHelper.DrawRectangle(((Game1) this.Game).SpriteBatch, ContentLoader.GetResource<Texture2D>(EPrefab.pixel), Dimensions, WallWidth, Color.Black);

            foreach (var drawableGameComponent in _visibleComponents)
            {
                drawableGameComponent.Draw(gameTime);
            }

            base.Draw(gameTime);
        }
    }
}
