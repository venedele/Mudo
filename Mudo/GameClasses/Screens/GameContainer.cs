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

namespace Mudo.GameClasses.Screens
{
    internal class GameContainer : Screen
    {

        Ball ball;
        Walls wall;
        PhysObject player;
        PhysObject player1;

        public int Points2 { get { return ball.defeat1; } }
        public int Points1 { get { return ball.defeat2; } }
        Random r = new Random();

        public event EventHandler Player1PointChanged
        {
            add { ball.Player2DefeatChanged += value; }
            remove { ball.Player2DefeatChanged -= value;  }
        }
        public event EventHandler Player2PointChanged
        {
            add { ball.Player1DefeatChanged += value; }
            remove { ball.Player1DefeatChanged -= value; }
        }

        // Driving values
        Vector2 ball_init_velocity = new Vector2(0, 0);

        



        public GameContainer(Game context) : base(context)
        {
            ball = new Ball(null, 40, 40) { location = new Vector2(250, 300) };
            wall = new Walls(600, 400) { location = new Vector2(300, 200) };
            player = new Platform(new Player_Ctr(Keys.A, Keys.D), ball, 0) { location = new Vector2(100, 385) };
            player1 = new Platform(new Ai_Ctr(ball), ball, 1)/*(new Player_Ctr(Keys.Left, Keys.Right))*/ { location = new Vector2(100, 15) };
            ball.setScoring(wall, true);

            wall.coll.Add(player);
            wall.coll.Add(player1);
            wall.coll.Add(ball);


            do
            {
                ball_init_velocity = new Vector2(r.Next() % 10 - 5, r.Next() % 10 - 5);
            } while
            (ball_init_velocity.Length() == 0 || ball_init_velocity.Y == 0);

            ball.velocity = ball_init_velocity;
            ball.rotation_velocity = (float)(r.Next() % 20) / 10.0f - 1;

        }

        protected override void Resource_Load(ContentManager content_l = null)
        {
            ball.LoadContent(content);
            ((Platform)player).LoadContent(graphicsDevice);
            ((Platform)player1).LoadContent(graphicsDevice);
        }

        protected override void Current_Update(GameTime gameTime)
        {
            ball.Update(gameTime);
            player.Update(gameTime);
            player1.Update(gameTime);
            wall.Update(gameTime);
        }

        protected override void Current_Draw(SpriteBatch spriteBatch)
        {
            ball.Draw(spriteBatch, this.customroot);
            player.Draw(spriteBatch, this.customroot);
            player1.Draw(spriteBatch, this.customroot);
        }

        protected override void Dispose()
        {

        }
    }
}
