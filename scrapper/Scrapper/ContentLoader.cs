using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace scrapper.Scrapper
{
    public static class ContentLoader
    {
        private static Game _game;
        private static readonly Dictionary<EPrefab, object> Resources = new Dictionary<EPrefab, object>();

        public static void SetGame(Game game)
        {
            _game = game;
        }

        public static T GetResource<T>(EPrefab prefab) // where T : GraphicsResource
        {
            if (!Resources.ContainsKey(prefab)) Resources.Add(prefab, _game.Content.Load<T>(prefab.ToString()));

            return (T) Resources[prefab];
        }
    }
}