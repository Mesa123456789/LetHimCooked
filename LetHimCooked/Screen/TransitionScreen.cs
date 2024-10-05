using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

using MonoGame.Extended.Collisions;
using MonoGame.Extended.Tiled.Renderers;
using MonoGame.Extended.Tiled;
using MonoGame.Extended;
using MonoGame.Extended.Timers;
using MonoGame.Extended.ViewportAdapters;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;
using LetHimCooked.Sprite;
namespace LetHimCooked.Screen
{
    public class TransitionScreen : screen
    {
        Texture2D bg;
        Game1 game; 
        public static Player player;
        AnimatedTexture SpriteTexture;
        Vector2 playerPos;// = new Vector2(player.Bounds.Position.X, player.Bounds.Position.Y);
        RectangleF Bounds = new RectangleF(new Vector2(700, 400), new Vector2(40, 60));
        public TransitionScreen(Game1 game, EventHandler theScreenEvent) : base(theScreenEvent)
        {
            //Load the background texture for the screen
            game._bgPosition = new Vector2(400, 225);
            player = new Player(SpriteTexture, playerPos, game, Bounds);
            bg = game.Content.Load<Texture2D>("map");
            this.game = game;
        }
        RectangleF doorRec = new RectangleF(100, 30, 100, 20);
        public override void Update(GameTime theTime)
        {

            if (player.Bounds.Intersects(doorRec))
            {
                CountTime(100);
                ScreenEvent.Invoke(game.GameplayScreen, new EventArgs());
                game._cameraPosition = new Vector2(400, 200);
                return;
            }
            base.Update(theTime);
        }
        public override void Draw(SpriteBatch theBatch)
        {

            theBatch.Draw(bg, Vector2.Zero, Color.Black);
            theBatch.End();

            theBatch.Begin();

            base.Draw(theBatch);
        }
        public int countPopUp;
        public void CountTime(int timePopup)
        {
            countPopUp += 1;
            {
                if (countPopUp > timePopup)
                {
                    countPopUp = 0;

                }
            }
        }
    }
}
