using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using scrapper.Scrapper;
using scrapper.Scrapper.Entities;
using scrapper.Scrapper.Maps;

namespace scrapper
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        readonly GraphicsDeviceManager graphics;
        public SpriteBatch SpriteBatch { get; private set; }
        private readonly Camera _camera;
        private readonly Player _player;
        private readonly List<Entity> _dynamicEntities = new List<Entity>();
        private readonly List<Entity> _entitiesToRemove = new List<Entity>();
        private Map _map;

        public Camera Camera => _camera;
        public Rectangle Map => _map.Dimensions;
        public byte WallWidth => _map.WallWidth;
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
#if !DEBUG
            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            graphics.IsFullScreen = true;
#endif
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
            ContentLoader.SetGame(this);

            SpriteBatch = new SpriteBatch(GraphicsDevice);

            _camera = new Camera
            {
                ViewportWidth = graphics.PreferredBackBufferWidth,
                ViewportHeight = graphics.PreferredBackBufferHeight
            };

            _player = new Player(this);
            _map = new Level1(this);
        }

        public void AddEntities(List<Entity> entities)
        {
            foreach (var entity in entities)
            {
                entity.Dead += RemoveNextUpdate;
            }
            _dynamicEntities.AddRange(entities);
        }

        private void RemoveNextUpdate(Entity entity)
        {
            _entitiesToRemove.Add(entity);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _map.Initialize();
            _player.Initialize();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _map.Update(gameTime);

            // TODO: Add your update logic here
            _player.Update(gameTime);

            foreach (var dynamicEntity in _dynamicEntities)
            {
                dynamicEntity.Update(gameTime);
                foreach (var entity in _dynamicEntities)
                {
                    if (dynamicEntity.DidCollide) break;
                    dynamicEntity.Collide(entity);
                }

                dynamicEntity.Collide(_player);
            }

            foreach (var entity in _entitiesToRemove)
            {
                _dynamicEntities.Remove(entity);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _camera.CenterOn(_player);
            SpriteBatch.Begin(transformMatrix:_camera.TranslationMatrix);
            // TODO: Add your drawing code here
            _map.Draw(gameTime);
            _player.Draw(gameTime);
            foreach (var entity in _dynamicEntities)
            {
                entity.Draw(gameTime);
            }
            SpriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
