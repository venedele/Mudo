﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using MonoControls.Containers.Base;

namespace Mudo
{
    class PhysObject : Animatable
    {
        public float mass = 0.0f;
        protected float rotation_mass = 0.0f;
        protected Vector2 acceleration = new Vector2(0.0f, 0.0f);
        public Vector2 velocity = new Vector2(0.0f, 0.0f);
        //public Texture2D texture = null;
        //protected float rotation = 0.0f;
        public float rotation_velocity = 0.0f;
        protected float rotation_acceleration = 0.0f;
        protected Vector2 air_k = new Vector2(-0.0015f, -0.0015f);
        protected float air_rot_k = -0.002f;
        protected float rotation_coef = 0.1f;
        public float bounce = 1.0f;
        public CollisionEngine coll;


        public float min_y = 0;
        public float min_speed = 0;
        public bool container = false;


        public PhysObject(Texture2D texture, int width, int height, bool container = false): base(texture, 0, 0, width, height, Color.White)
        {
            coll = new CollisionEngine(this, !container);
            this.container = container;
            this.setCentralCoords(true);
        }

        private SpriteBatch spr = null;
        /*public override void Draw(GameTime time)
        {
            if (texture != null)
            {
                if (spr == null)
                    spr = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
                spr.Draw(texture, new Rectangle((int)position.X, (int)position.Y, width, height), null, Color.White, rotation, new Vector2(texture.Width / 2, texture.Height / 2), SpriteEffects.None, 1);
            }
        }*/

        public virtual void Collision(PhysObject collisioned, bool collision_orientation, bool anti_clipping = false)
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
                    this.Y = collisioned.location.Y + ((this.Y > collisioned.location.Y) ?1:-1)* ((collisioned.height + this.height) / 2 - (collisioned.container?this.height:0));

            }
            else
            {
                new_val = ((this.mass - collisioned.mass) * this.velocity.X + 2 * collisioned.mass * collisioned.velocity.X) / (this.mass + collisioned.mass);
                new_val2 = this.velocity.X + new_val - collisioned.velocity.X;
                new_val *= bounce; //TODO: Get mass ration into equasion
                velocity.X = new_val;

                float direction = new_val > 0 ? -1f : 1f;
                float combined_speed = velocity.Y + direction * collisioned.velocity.Y;


                float avr = (combined_speed + direction * rotation_velocity*rotation_coef) / 2;
                if (!Double.IsNaN(avr))
                {
                    rotation_velocity -= direction * avr / rotation_coef;
                    velocity.Y -= avr;
                }

                if(anti_clipping)
                    this.X = collisioned.location.X + ((this.X > collisioned.location.X) ? 1 : -1) * ((collisioned.width + this.width) / 2 - (collisioned.container ? this.width : 0));
            }
            if (Math.Abs(velocity.Y) < min_y) velocity.Y = (velocity.Y > 0 ? 1 : -1) * min_y;

        }

        public bool Enabled = true;
        public virtual void Update(GameTime time)
        {
            if (Enabled)
            {
                if (velocity.Length() > min_speed)
                {
                    Vector2 air_resis_accell = (velocity * air_k);
                    if (velocity.Y > min_y) air_resis_accell.Y = 0;
                    velocity += air_resis_accell;
                }

                float rot_retardation = rotation_velocity * air_rot_k;
                rotation_velocity += rot_retardation;

                this.Rotation += (rotation_velocity += rotation_acceleration);
                location += (velocity += acceleration);

                coll.Update(time);
                //base.Update(time);
            }
        }

    }
}