using LetHimCooked.Screen;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetHimCooked.Sprite

{
    public class Enemy : Food
    {
        bool isHit;
        public Texture2D enemytex;
        public Vector2 enemyPosition;
        private double hitCooldown = 2.0; // Cooldown period in seconds
        private double lastHitTime = 0;
        int countDamage;
        RectangleF enemyBox;

        public Enemy(Texture2D enemytex, Vector2 enemyPosition) : base(enemytex, enemyPosition) 
        {
            this.enemytex = enemytex;
            this.enemyPosition = enemyPosition;
            framePerSec = 7;
            timePerFream = (float)1 / framePerSec;
            frame = 5;
            enemyBox = new RectangleF((int)enemyPosition.X, (int)enemyPosition.Y, 50, 50);
        }

        public override void Update(GameTime gameTime)
        {
            MouseState ms = Mouse.GetState();
            if (enemyBox.Intersects(GameplayScreen.player.Bounds) && !isHit)//!OntableAble)
            {
                enemyPosition.X += 5;
                Hit();
                if (ms.LeftButton == ButtonState.Pressed)
                {
                    OnCollision();
                }
            }
            if (isHit == true)
            {
                countDamage += 1;
                {
                    if (countDamage > 100)
                    {
                        countDamage = 0;
                        isHit = false;
                    }
                }
            }
            enemyBox = new RectangleF((int)enemyPosition.X, (int)enemyPosition.Y, 50, 50);
            UpdateFream((float)gameTime.ElapsedGameTime.TotalSeconds);
        }
        public override void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(enemytex, enemyPosition, new Rectangle(32 * frame, 0, 32, 32), Color.White, 0.0f, new Vector2(16, 16), 2.0f, SpriteEffects.None, 0.0f);
        }

        public void Hit()
        {
            Game1.currentHeart -= 20;
            isHit = true;
        }
        public void OnCollision()
        {
           // OntableAble = true;
            Game1.BagList.Add(this);
            Game1.IsPopUp = true;
            foreach (Enemy enemy in Game1.enemyList)
            {
                Game1.enemyList.Remove(this);
                break;
            }
        }
        void UpdateFream(float elapsed)
        {
            totalElapsed += elapsed;
            if (totalElapsed > timePerFream)
            {
                frame = (frame + 1) % 5;
                totalElapsed -= timePerFream;
            }
        }

    }
}
