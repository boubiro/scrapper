using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace scrapper.Scrapper.Maps
{
    public abstract class Map : DrawableGameComponent
    {
        private MapData _mapData;
        private List<DrawableGameComponent> _visibleComponents = new List<DrawableGameComponent>();

        public Map(Game game) : base(game)
        {
        }

        public Map(Game game, MapData mapData) : base(game)
        {
            _mapData = mapData;
        }

        public void SetMapData(MapData mapData)
        {
            _mapData = mapData;
            Initialize();
        }

        public override void Initialize()
        {
            foreach (var dataPoint in _mapData)
            {
                
            }

            base.Initialize();
        }
    }
}
