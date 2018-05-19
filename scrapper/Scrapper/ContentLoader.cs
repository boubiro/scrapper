using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace scrapper.Scrapper
{
    public static class ContentLoader
    {
        private static Game _game;
        private static readonly Dictionary<EPrefab, GraphicsResource> Resources = new Dictionary<EPrefab, GraphicsResource>();

        public static void SetGame(Game game)
        {
            _game = game;
        }

        public static T GetResource<T>(EPrefab prefab) where T : GraphicsResource
        {
            if (!Resources.ContainsKey(prefab))
            {
                Resources.Add(prefab, _game.Content.Load<T>(prefab.ToString()));
            }

            return (T) Resources[prefab];
        }
    }
}
