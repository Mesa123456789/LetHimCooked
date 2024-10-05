using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetHimCooked.Sprite
{
    public class Sprite_ : IEntity, ICollisionActor
    {
        public Vector2 position;
        public int state = 0;
        public Rectangle hitbox;
        public Texture2D myTexture;
        public Texture2D InventoryTex;
        public Rectangle drect;
        public float rotation, postrotation, scale, depth;
        public Vector2 origin;
        public Vector2 knockBackDirection;
        public IShapeF Bounds { get; }
        public int speed;
        public int count;

        public int frame;
        public int framePerSec;
        public float totalElapsed;
        public float timePerFream;
        public Sprite_()
        {

        }
        public virtual void Load(ContentManager content, string asset)
        {
            
        }
        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual void Draw(SpriteBatch _spriteBatch)
        {
            Rectangle sourcerect = new Rectangle();
            _spriteBatch.Draw(myTexture, position, sourcerect, Color.White, rotation, origin, scale, SpriteEffects.None, depth);
        }
        public void OnCollision(CollisionEventArgs collisionInfo)
        {
            if (collisionInfo.Other.ToString().Contains("PlatformEntity"))
            {
                Bounds.Position -= collisionInfo.PenetrationVector;
            }

        }
    }
}
