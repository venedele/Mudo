﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Mudo
{
    class VObject
    {
        public float mass = 0.0f;
        protected float rotation_mass = 0.0f;
        public Vector2 position = new Vector2(0.0f, 0.0f);
        protected Vector2 acceleration = new Vector2(0.0f, 0.0f);
        public Vector2 velocity = new Vector2(0.0f, 0.0f);
        public Texture2D texture = null;
        protected float rotation = 0.0f;
        public float rotation_velocity = 0.0f;
        protected float rotation_acceleration = 0.0f;
        protected Vector2 air_k = new Vector2(-0.0015f, -0.0015f);
        protected float air_rot_k = -0.002f;
        protected float rotation_coef = 0.1f;
        public float bounce = 1.0f;
        public CollisionEngine coll;


        public int width_int;
        public int width
        {
            get { return width_int == 0 ? texture.Width : width_int; }
            protected set { width_int = value; }
        }
        public int height_int;
        public int height
        {
            get { return height_int == 0 ? texture.Height : height_int; }
            protected set { height_int = value; }
        }


        public float min_y = 0;
        public float min_speed = 0;
        public bool container = false;


        public VObject(Texture2D texture, int width, int height, bool container = false)
        {
            this.width = width;
            this.height = height;
            this.texture = texture;
            coll = new CollisionEngine(this, !container);
            this.container = container;
        }

        public void Draw(SpriteBatch draw)
        {//TODO: Set width and height for 0
            /*if (width * height == 0)
                draw.Draw(texture, position, null, Color.White, rotation, new Vector2(texture.Width / 2, texture.Height / 2), 1f, SpriteEffects.None, 1);
            else*/
                draw.Draw(texture, new Rectangle((int)position.X, (int)position.Y, width, height), null, Color.White, rotation, new Vector2(texture.Width / 2, texture.Height / 2), SpriteEffects.None, 1);
        }

        public void Collision(VObject collisioned, bool collision_orientation, bool anti_clipping = false)
        {
            //TODO: Modify second body attributes

            Vector2 old = velocity;
            float new_val;
            float new_val2;
            if (collision_orientation)
            {
                //Full elastic collision: w1 = (m1 - m2)v1/M + 2m2v2/M (Conservation of moment and conservation of kinetic energy) //Non cartisean hits not fully supported
                new_val = ((this.mass - collisioned.mass) * this.velocity.Y + 2 * collisioned.mass * collisioned.velocity.Y) / (this.mass + collisioned.mass);
                new_val2 = this.velocity.Y + new_val - collisioned.velocity.Y;
                //float combined_hit = this.velocity.Y - collisioned.velocity.Y;
                new_val *= bounce;
                velocity.Y = new_val;
                
                //Rotation handling
                float direction = new_val > 0? 1f: -1f;
                float combined_speed = velocity.X - collisioned.velocity.X ;
                //TODO: Two bodies rotation not supported

                float avr = (combined_speed + direction * rotation_velocity*rotation_coef)/2;
                if (!Double.IsNaN(avr))
                {
                    rotation_velocity -= direction * avr / rotation_coef;
                    velocity.X -= avr;
                }

                if(anti_clipping)
                    position.Y = collisioned.position.Y + ((position.Y > collisioned.position.Y) ?1:-1)* ((collisioned.height + this.height) / 2 - (collisioned.container?this.height:0));

            }
            else
            {
                new_val = ((this.mass - collisioned.mass) * this.velocity.X + 2 * collisioned.mass * collisioned.velocity.X) / (this.mass + collisioned.mass);
                new_val2 = this.velocity.X + new_val - collisioned.velocity.X;
                //float combined_hit = this.velocity.X - collisioned.velocity.X;
                new_val *= bounce; //TODO: Get mass ration into equasion
                velocity.X = new_val;
                //velocity.X = -velocity.X;

                float direction = new_val > 0 ? -1f : 1f;
                float combined_speed = velocity.Y + direction * collisioned.velocity.Y;


                float avr = (combined_speed + direction * rotation_velocity*rotation_coef) / 2;
                if (!Double.IsNaN(avr))
                {
                    rotation_velocity -= direction * avr / rotation_coef;
                    velocity.Y -= avr;
                }

                if(anti_clipping)
                    position.X = collisioned.position.X + ((position.X > collisioned.position.X) ? 1 : -1) * ((collisioned.width + this.width) / 2 - (collisioned.container ? this.width : 0));
            }
            if (Math.Abs(velocity.Y) < min_y) velocity.Y = (velocity.Y > 0 ? 1 : -1) * min_y;
            //rotation_velocity -= direction*(float)Math.Sqrt((old.Length() - velocity.Length()) * 2 / rotation_inertia);

        }

        public virtual void Update(GameTime time)
        {
            if (velocity.Length() > min_speed)
            {
                Vector2 air_resis_accell = (velocity * air_k);
                if (velocity.Y > min_y) air_resis_accell.Y = 0;
                velocity += air_resis_accell;
            }

            float rot_retardation = rotation_velocity * air_rot_k;
            rotation_velocity += rot_retardation;

            rotation += (rotation_velocity += rotation_acceleration);
            position += (velocity += acceleration);

            coll.Update(time);
        }

        public float LeftX
        {
            get { return position.X - width / 2.0f; }
        }

        public float RightX
        {
            get { return position.X + width / 2.0f; }
        }

        public float TopY
        {
            get { return position.Y - height / 2.0f; }
        }

        public float BottomY
        {
            get { return position.Y + height / 2.0f; }
        }


    }
}
