using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoControls.Containers.Base;
using MonoControls.Inputs.Mouse;

namespace MonoControls.Containers.Helpers.Animatables
{
    class MenuShaped : Menu
    {
        private Texture2D background;
        private Texture2D background_back;
        public MenuShaped(Type type, float x, float y, int width, int height, Color color, LinkedList<Pair<DuplexStateAnimatable, Func<Animatable, MouseKeys, short>>> parents, Texture2D background = null, bool spread = true, float rotation = 0, float startoffset = 0, float endoffset = 0, bool generate = true) : base(type, x, y, width, height, color, parents, spread, rotation, startoffset, endoffset, generate)
        {
            this.background = background;
            background_back = background;
        }
        public MenuShaped(Menu old, bool generate = true) : base(old.type, old.location, old.size, old.color, old, old.Mouse_events, old.spread, old.Rotaion, old.startoffset, old.endoffset, generate)
        {
            this.CreateState(old.texture_alt, old.changable_color ? old.color_alt.ToVector4() : new Vector4(-1, 0, 0, 0), old.alpha_alt);
            this.background = old.texture;
            background_back = background;
        }
        public MenuShaped(Type type, Vector2 location, Point size, Color color, LinkedList<Pair<DuplexStateAnimatable, Func<Animatable, MouseKeys, short>>> parents, Texture2D background, bool spread = true, float rotation = 0, float startoffset = 0, float endoffset = 0, bool generate = true) :base(type, location, size, color, parents, spread, rotation, startoffset, endoffset, generate)
        {
            this.background = background;
            background_back = background;
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 cord_root)
        {
            spriteBatch.Draw(background, new Rectangle((cord_root+location).ToPoint(), size), null, color * alpha, Rotaion, new Vector2(background.Width / 2, background.Height / 2), SpriteEffects.None, 1f);
            base.Draw(spriteBatch, cord_root);
        }
        public override void Draw(SpriteBatch spriteBatch, Vector2 cord_root, float alpha = -1)
        {
            spriteBatch.Draw(background, new Rectangle((cord_root + location).ToPoint() + new Point(size.X/2, size.Y/2), size), null, color * this.alpha*(alpha>-1?alpha:1), Rotaion, new Vector2(background.Width / 2, background.Height / 2), SpriteEffects.None, 1f);
            base.Draw(spriteBatch, cord_root, alpha);
        }

        Texture2D texturewas = null;

        public override void SwapStates(bool recursive)
        {
            texture = texturewas;
            base.SwapStates(recursive);
            texturewas = texture;
           background = texture ?? background_back;
            texture = null;
        }

    }
}
