using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoControls.Containers.Additions.Animatables;
using MonoControls.Containers.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mudo.GameClasses.Screens
{
    internal class ScoresScreen : Screen
    {

        PulsingText score2;
        PulsingText score1;
        Animatable strip_S;

        GameContainer game;

        public void Player2Pointnew(object sender, EventArgs a)
        {
            score2.str = game.Points2.ToString();
        }
        public void Player1Pointnew(object sender, EventArgs a)
        {
            score1.str = game.Points1.ToString();
        }

        Point size = Point.Zero; 
        public void setSize(Point size)
        {
            this.size = size;
            if(score1 != null)
            {
                score1.size = size;
            }
            if(score2 != null)
            {
                score2.size = size;
            }
        }

        public ScoresScreen (Game context, GameContainer tofollow): base(context)
        {
            this.game = tofollow;
            game.Player2PointChanged += Player2Pointnew;
            game.Player1PointChanged += Player1Pointnew;
        }

        protected override void Resource_Load(ContentManager content_l = null)
        {
            Texture2D stripe = new Texture2D(graphicsDevice, size.X, 6);

            Color[] data = new Color[size.X * 6];
            for(int y = 0; y < 6; y++)
            {
                for (int x = 0; x < size.X; x++)
                    if ((x+8) / 13 % 2 == 1)
                    {
                        if (y == 0 || y == 5 || x == 0 || x == size.X - 1)
                            data[x + y * size.X] = 0.25f*Color.Black;
                        else 
                            data[x + y * size.X] = 0.1f*Color.Black;
                    }
                    else data[x + y * size.X] = 0.1f*Color.White;
            }

            stripe.SetData(data);
            strip_S = new Animatable(stripe, new Vector2(0, size.Y / 2f), new Point(size.X, 6), Color.White);
            //strip_S.setCentralCoords(true);
            //strip_S.alpha = 0.1f;
            score2 = new PulsingText(content.Load<SpriteFont>("File1"), new Vector2(7, size.Y *0.28f), 1, 0.1f, 0.7f, Color.Black, "0");
            score1 = new PulsingText(content.Load<SpriteFont>("File1"), new Vector2(7, size.Y * 0.72f), 1, 0.1f, 0.7f, Color.Black, "0");
            score2.Y -= score2.size.Y/2;
            score1.Y -= score1.size.Y/2;
            //score2.setCentralCoords(true);
            //score1.setCentralCoords(true);
        }

        protected override void Current_Update(GameTime gameTime)
        {
            score2.Update(gameTime);
            score1.Update(gameTime);
        }

        protected override void Current_Draw(SpriteBatch spriteBatch)
        {
            score2.Draw(spriteBatch, customroot);
            score1.Draw(spriteBatch, customroot);
            strip_S.Draw(spriteBatch, customroot);
        }

        protected override void Dispose()
        {
            
        }
    }
}
