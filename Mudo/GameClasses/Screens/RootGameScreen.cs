using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoControls.Containers.Additions.Animatables;
using MonoControls.Containers.Base;

namespace Mudo.GameClasses.Screens
{
    internal class RootGameScreen: Screen
    {

        Animatable pause_an;
        PulsingText score2;
        PulsingText score1;

        int screen_width;
        int screen_height;
         
        GameContainer game = null;

        public void Player2Pointnew(object sender, EventArgs a)
        {
            score2.str = game.Points2.ToString();
        }
        public void Player1Pointnew(object sender, EventArgs a)
        {
            score1.str = game.Points1.ToString();
        }

        public RootGameScreen(Game context) : base(context)
        {
            nested = game = new GameContainer(context);
            TogglePause();
            game.Player2PointChanged += Player2Pointnew;
            game.Player1PointChanged += Player1Pointnew;
        }

        private bool pause = false;
        public bool Pause { get { return pause; } set { if (pause != value) TogglePause(); } }

        public void TogglePause()
        {
            pause = !pause;
            game.paused = pause;
        }

        protected override void Resource_Load(ContentManager content_l = null)
        {
            screen_width = context.Window.ClientBounds.Width;
            screen_height = context.Window.ClientBounds.Height;
            pause_an = new Animatable(content.Load<Texture2D>("pause"), new Vector2(screen_width - 5 - 32, 5), new Point(32, 32), Color.White);
            Animatable pause_an_t = new Animatable(content.Load<SpriteFont>("File"), "Pause On", new Vector2(-13, 34), Color.Black);
            pause_an.AddFirst(pause_an_t);
            pause_an.alpha = 195f/255;
            score2 = new PulsingText(content.Load<SpriteFont>("File1"), new Vector2(screen_width/2f, screen_height/4f), 1, 0.1f, 0.7f, Color.Black, "0");
            score1 = new PulsingText(content.Load<SpriteFont>("File1"), new Vector2(screen_width / 2f, screen_height * 0.75f), 1, 0.1f, 0.7f, Color.Black, "0");
            score2.setCentralCoords(true);
            score1.setCentralCoords(true);
        }

        Point location_prev = new Point(0, 0);
        bool button_lock = false;

        protected override void Current_Update(GameTime gameTime)
        {

            if(location_prev != context.Window.Position)
            {
                location_prev = context.Window.Position;
            }

            KeyboardState keystate = Keyboard.GetState();
            if (!button_lock)
                if (keystate.IsKeyDown(Keys.P))
                {
                    TogglePause();
                }
            button_lock = keystate.IsKeyDown(Keys.P);

            score2.Update(gameTime);
            score1.Update(gameTime);

            //Replace with an Interlopator if needed
            float target = (pause ? 195f/255 : -120f/255);
            if (pause_an.alpha != target)
            {
                pause_an.alpha += pause_an.alpha > target ? -15f/255 : 15f/255;
            }
        }

        protected override void Current_Draw(SpriteBatch spriteBatch)
        {
            pause_an.Draw(spriteBatch, customroot);
            score2.Draw(spriteBatch, customroot);
            score1.Draw(spriteBatch, customroot);
        }

        protected override void Dispose()
        {
        }
    }
}
