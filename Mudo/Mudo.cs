using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Mudo.GameClasses.Screens;
using System;

namespace Mudo
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Mudo : Game
    {

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        RootGameScreen root;

        public Mudo()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            root = new RootGameScreen(this); 
        }



        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            this.Window.Title = "Mudo 2.1";
            this.IsFixedTimeStep = false;
            this.TargetElapsedTime = TimeSpan.FromSeconds(1d / 30d); //60);

            this.IsMouseVisible = true;
            this.Window.AllowUserResizing = false;

            graphics.PreferredBackBufferHeight = 400;
            graphics.PreferredBackBufferWidth = 600;
            graphics.ApplyChanges();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Services.AddService(typeof(SpriteBatch), spriteBatch);

            root.Load();

            base.LoadContent();
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {

            // TODO: Unload any non ContentManager content here
            root.Initialised = false;
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState keystate = Keyboard.GetState();

            if (keystate.IsKeyDown(Keys.Q))
                Exit();

            root.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        Color background = new Color(0xf5f5f5);
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(background/*Color.White*/);

            spriteBatch.Begin();

            //TODO: Fix cordinate system for the Animatable
            //Possibly add a "Use centric coordinated field"
            root.Draw();

            base.Draw(gameTime);
            
            spriteBatch.End();
        }
    }
}
