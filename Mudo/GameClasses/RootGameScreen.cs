using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoControls.Containers.Base;

namespace Mudo.GameClasses
{
    internal class RootGameScreen: Screen
    {
        Texture2D pause_t;
        SpriteFont text_f;
        int pause_alpha = 195;
        int screen_width;
        int screen_height;
         
        //ControlForm controlform;
        GameContainer game = null;

        public RootGameScreen(Game context) : base(context)
        {
            nested = game = new GameContainer(context);
            //controlform = new ControlForm(this);
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
            pause_t = content.Load<Texture2D>("pause");
            text_f = content.Load<SpriteFont>("File");
        }

        Point location_prev = new Point(0, 0);
        bool button_lock = false;

        /*void SnapControlWindow()
        {
            controlform.supressmove = true;
            controlform.Top = Window.Position.Y;
            controlform.Left = Window.Position.X + Window.ClientBounds.Width;
            controlform.supressmove = false;
        }*/

        protected override void Current_Update(GameTime gameTime)
        {

            /*if(location_prev != Window.Position)
            {
                location_prev = Window.Position;
                SnapControlWindow();
            }*/

            KeyboardState keystate = Keyboard.GetState();
            if (!button_lock)
                /*if (keystate.IsKeyDown(Keys.F))
                {

                    if (controlform.Visible)
                        controlform.Hide();
                    else
                    {
                        controlform.Show();
                        SnapControlWindow();
                    }

                } else */
                if (keystate.IsKeyDown(Keys.P))
                {
                    TogglePause();
                }
            button_lock = /*keystate.IsKeyDown(Keys.F) ||*/ keystate.IsKeyDown(Keys.P);

            int target = (pause ? 195 : -120);
            if (pause_alpha != target)
            {
                pause_alpha += pause_alpha > target ? -15 : 15;
            }
        }

        protected override void Current_Draw(SpriteBatch spriteBatch)
        {
            if (pause_alpha > 0)
            {
                spriteBatch.Draw(pause_t, new Rectangle(screen_width - 5 - 33, 5, 33, 33), new Color(Color.White, pause_alpha));
                spriteBatch.DrawString(text_f, "Pause On", new Vector2(screen_width - 50, 39), new Color(Color.Black, pause_alpha));
            }
        }

        protected override void Dispose()
        {
            //controlform.Close();
        }
    }
}
