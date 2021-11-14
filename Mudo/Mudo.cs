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
        public static float ball_mass = 10;
        public static Vector2 air_k = new Vector2(-0.0015f, -0.0015f);
        public static float air_rot_k = -0.002f;

        ControlForm controlform;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        int defeat1 = 0, defeat2 = 0;
        Random r = new Random();

        // Driving values
        Vector2 ball_velocity = new Vector2(0, 0);


        public Mudo()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            this.IsFixedTimeStep = false;
            this.TargetElapsedTime = TimeSpan.FromSeconds(1d / 30d); //60);

            this.IsMouseVisible = true;
            this.Window.AllowUserResizing = false;

            graphics.PreferredBackBufferHeight = 400;
            graphics.PreferredBackBufferWidth = 600;

            controlform = new ControlForm(this);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            this.Window.Title = "Mudo 2.0";

            ball = new Ball(this, null, 40, 40) { position = new Vector2(250, 300) };
            wall = new Walls(this, 600, 400) { position = new Vector2(300, 200) };
            player = new Platform(this, new Player_An(Keys.A, Keys.D), ball) { position = new Vector2(100, 385) };
            player1 = new Platform(this, new Ai_An(ball), ball)/*(new Player_An(Keys.Left, Keys.Right))*/ { position = new Vector2(100, 15) };

            wall.coll.Add(player);
            wall.coll.Add(player1);
            wall.coll.Add(ball);

            Components.Add(ball);
            Components.Add(player);
            Components.Add(player1);

            do
            {
                ball_velocity = new Vector2(r.Next() % 10 - 5, r.Next() % 10 - 5);
            } while
           (ball_velocity.Length() == 0 || ball_velocity.Y == 0);

            ball.velocity = ball_velocity;
            ball.rotation_velocity = (float)(r.Next() % 20) / 10.0f - 1;

            //ball.velocity = new Vector2(3.5f, 3.5f);

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
            ball.texture = Content.Load<Texture2D>("acc");

            ((Platform)player).setTexture(GraphicsDevice);
            ((Platform)player1).setTexture(GraphicsDevice, 1);

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

        Ball ball;
        VObject wall;
        VObject player;
        VObject player1;

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
            if(location_prev != Window.Position)
            {
                location_prev = Window.Position;
                SnapControlWindow();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Q))
                Exit();
            if (Keyboard.GetState().IsKeyDown(Keys.F))
            {
                if (!button_lock)
                {
                    if (controlform.Visible)
                        controlform.Hide();
                    else
                    {
                        controlform.Show();
                        SnapControlWindow();
                    }
                }
            }
            button_lock = Keyboard.GetState().IsKeyDown(Keys.F);

            Rectangle screen = GraphicsDevice.PresentationParameters.Bounds;

            /*if (ball.position.Y - 20 < 0)
            {
                ball.position.Y = 20;
                ball.Collision(wall, true);
                defeat1++;
            }
            else if (ball.position.Y + 20 > screen.Height)
            {
                ball.Collision(wall, true);
                ball.position.Y = screen.Height - 20;
                defeat2++;
            }
            if (ball.position.X -20 < 0)
            {
                ball.position.X = 20;
                ball.Collision(wall, false);
            }
            else if (ball.position.X + 20 > screen.Width)
            {
                ball.position.X = screen.Width - 20;
                ball.Collision(wall, false);
            }*/

            /*if( ball.position.X > player.position.X - player.texture.Width / 2  && ball.position.X < player.position.X + player.texture.Width/2 && ball.position.Y + ball.height / 2 >= player.position.Y - player.texture.Height / 2)
            {
                ball.Collision(player, true, true);
            }

            if (ball.position.X > player1.position.X - player1.texture.Width / 2 && ball.position.X < player1.position.X + player1.texture.Width / 2 &&  ball.position.Y - ball.height / 2 <= player1.position.Y + player1.texture.Height/2)
            {
                ball.Collision(player1, true, true);
            }*/

                //ball.Update(gameTime);
                //player.Update(gameTime);
                //player1.Update(gameTime);

                wall.coll.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin();

            ball.Draw(spriteBatch);
            player.Draw(spriteBatch);
            player1.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
