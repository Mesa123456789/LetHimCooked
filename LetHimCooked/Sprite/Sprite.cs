using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetHimCooked.Sprite
{
    public class Sprite
    {
        public Rectangle drect;
        public Sprite()
        {

        }
        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual void Draw(SpriteBatch _spriteBatch)
        {

        }
        public virtual void PlayerDraw(SpriteBatch _spriteBatch, Vector2 offset)
        {
            Rectangle dest = new(
                drect.X + (int)offset.X,
                drect.Y + (int)offset.Y,
                drect.Width,
                drect.Height

                );
        }
    }
}
