using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoControls.Containers.Additions;
using MonoControls.Containers.Additions.Animatables;
using MonoControls.Containers.Base;

namespace Mudo.GameClasses.Screens
{
    internal class RootGameScreen: Screen
    {

        Animatable pause_an;

        int screen_width;
        int screen_height;
         
        GameContainer game = null;
        ScoresScreen score = null;
        InterpolAnimatable multiplayer_alert; 

        public RootGameScreen(Game context) : base(context)
        {
            nested = game = new GameContainer(context);
            game.nested = score = new ScoresScreen(context, game);
            TogglePause();
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

            multiplayer_alert = new InterpolAnimatable(content.Load<Texture2D>("back"), new Vector2(screen_width -170, 5), new Point(80, 30), Color.White*0);
            multiplayer_alert.Add(new Animatable(content.Load<SpriteFont>("File"), "Multiplayer off", new Vector2(6, 9), Color.White));
            multiplayer_alert.First.Value.Scale = new Vector2(1.05f, 1.05f);
            multiplayer_alert.setAutoReset(true);
            //multiplayer_alert.setCentralCoords(true);

            multiplayer_alert.setInterpolators(new Interpolator(delegate (float x)
            {
                if (x < 10) return x * x / 100;
                else if (x < 50) return 1;
                else if (x < 61) return -(x - 50) * (x - 50)/100 + 1;
                return Interpolator.DONE;
            }, 0, 5, 1, 1), null, null);
            

            score.setSize(new Point(150, screen_height));
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
            {
                if (keystate.IsKeyDown(Keys.P))
                {
                    TogglePause();
                }
                if(keystate.IsKeyDown(Keys.M))
                {
                    game.ToggleMultiplayer();
                    if (game.multiplayer) multiplayer_alert.First.Value.str = "Multiplayer on";
                    else multiplayer_alert.First.Value.str = "Multiplayer off";
                    multiplayer_alert.StartAnimation();
                }
            }
            button_lock = keystate.IsKeyDown(Keys.P) || keystate.IsKeyDown(Keys.M);

            multiplayer_alert.Update(gameTime);

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
            multiplayer_alert.Draw(spriteBatch, customroot);
        }

        protected override void Dispose()
        {
        }
    }
}
