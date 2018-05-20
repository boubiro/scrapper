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
    ///     This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        private readonly List<Entity> _entitiesToRemove = new List<Entity>();
        private readonly Player _player;
        private readonly GraphicsDeviceManager graphics;
        private readonly Map _map;

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

            Camera = new Camera
            {
                ViewportWidth = graphics.PreferredBackBufferWidth,
                ViewportHeight = graphics.PreferredBackBufferHeight
            };

            _player = new Player(this);
            _map = new Level1(this);
            _player.PlayerAttack += _map.AttackMove;
        }

        public SpriteBatch SpriteBatch { get; }

        public Camera Camera { get; }

        public Rectangle Map => _map.Dimensions;
        public byte WallWidth => _map.WallWidth;

        private void RemoveNextUpdate(Entity entity)
        {
            _entitiesToRemove.Add(entity);
        }

        /// <summary>
        ///     Allows the game to perform any initialization it needs to before starting to run.
        ///     This is where it can query for any required services and load any non-graphic
        ///     related content.  Calling base.Initialize will enumerate through any components
        ///     and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _map.Initialize();
            _player.Initialize();
            base.Initialize();
        }

        /// <summary>
        ///     LoadContent will be called once per game and is the place to load
        ///     all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        ///     UnloadContent will be called once per game and is the place to unload
        ///     game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        public void AddEntities(List<Entity> entities)
        {
            _map.AddEntities(entities);
        }

        /// <summary>
        ///     Allows the game to run logic such as updating the world,
        ///     checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _map.Update(gameTime, _player);

            // TODO: Add your update logic here
            _player.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        ///     This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            Camera.CenterOn(_player);
            SpriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: Camera.TranslationMatrix);
            // TODO: Add your drawing code here
            _map.Draw(gameTime);
            _player.Draw(gameTime);
            SpriteBatch.End();
            base.Draw(gameTime);
        }
    }
}