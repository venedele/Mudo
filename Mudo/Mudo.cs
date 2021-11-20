using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Mudo
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Mudo : Game
    {
        ControlForm controlform;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Ball ball;
        Walls wall;
        VObject player;
        VObject player1;

        public int defeat1 { get {return ball.defeat1; } }
        public int defeat2 { get {return ball.defeat2; } }
        Random r = new Random();

        // Driving values
        Vector2 ball_velocity = new Vector2(0, 0);

        private bool pause = false;
        public bool Pause { get { return pause; } set { if (pause != value) TogglePause(); } }
        public void TogglePause()
        {
            ball.Enabled = pause;
            wall.Enabled = pause;
            player.Enabled = pause;
            player1.Enabled = pause;
            pause = !pause;
        }

        public Mudo()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            controlform = new ControlForm(this);

            ball = new Ball(this, null, 40, 40) { position = new Vector2(250, 300) };
            wall = new Walls(this, 600, 400) { position = new Vector2(300, 200) };
            player = new Platform(this, new Player_An(Keys.A, Keys.D), ball, 0) { position = new Vector2(100, 385) };
            player1 = new Platform(this, new Ai_An(ball), ball, 1)/*(new Player_An(Keys.Left, Keys.Right))*/ { position = new Vector2(100, 15) };
            ball.setScoring(wall, true);

            wall.coll.Add(player);
            wall.coll.Add(player1);
            wall.coll.Add(ball);

            TogglePause();

            Components.Add(ball);
            Components.Add(player);
            Components.Add(player1);
            Components.Add(wall);

        }


        Texture2D pause_t;
        SpriteFont text_f;
        int pause_alpha = 195;
        int screen_width;
        int screen_height;


        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            this.Window.Title = "Mudo 2.0";
            this.IsFixedTimeStep = false;
            this.TargetElapsedTime = TimeSpan.FromSeconds(1d / 30d); //60);

            this.IsMouseVisible = true;
            this.Window.AllowUserResizing = false;

            graphics.PreferredBackBufferHeight = 400;
            graphics.PreferredBackBufferWidth = 600;
            graphics.ApplyChanges();

            screen_width = Window.ClientBounds.Width;
            screen_height = Window.ClientBounds.Height;

            do
            {
                ball_velocity = new Vector2(r.Next() % 10 - 5, r.Next() % 10 - 5);
            } while
           (ball_velocity.Length() == 0 || ball_velocity.Y == 0);

            ball.velocity = ball_velocity;
            ball.rotation_velocity = (float)(r.Next() % 20) / 10.0f - 1;
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

            pause_t = Content.Load<Texture2D>("pause");
            text_f = Content.Load<SpriteFont>("File");

            base.LoadContent();
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            controlform.Close();
            // TODO: Unload any non ContentManager content here
        }

        Point location_prev = new Point(0, 0);
        bool button_lock = false;

        void SnapControlWindow()
        {
            controlform.supressmove = true;
            controlform.Top = Window.Position.Y;
            controlform.Left = Window.Position.X + Window.ClientBounds.Width;
            controlform.supressmove = false;
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState keystate = Keyboard.GetState();
            if(location_prev != Window.Position)
            {
                location_prev = Window.Position;
                SnapControlWindow();
            }

            if (keystate.IsKeyDown(Keys.Q))
                Exit();
            if (!button_lock)
                if (keystate.IsKeyDown(Keys.F))
                {

                    if (controlform.Visible)
                        controlform.Hide();
                    else
                    {
                        controlform.Show();
                        SnapControlWindow();
                    }

                } else if(keystate.IsKeyDown(Keys.P))
                {
                    TogglePause();
                }
            button_lock = keystate.IsKeyDown(Keys.F) || keystate.IsKeyDown(Keys.P);

            int target = (pause ? 195 : -120);
            if (pause_alpha != target)
            {
                pause_alpha += pause_alpha > target ? -15 : 15;
            }

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

            base.Draw(gameTime);
            if (pause_alpha > 0)
            {
                spriteBatch.Draw(pause_t, new Rectangle(screen_width - 5 - 33, 5, 33, 33), new Color(Color.White, pause_alpha));
                spriteBatch.DrawString(text_f, "Pause On", new Vector2(screen_width-50, 39), new Color(Color.Black, pause_alpha));
            }
            spriteBatch.End();
        }
    }
}
