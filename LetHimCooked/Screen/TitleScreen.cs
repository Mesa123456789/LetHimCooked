using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
namespace LetHimCooked.Screen
{
    public class TitleScreen : screen
    {
        Texture2D menuTexture;
        Game1 game; public TitleScreen(Game1 game, EventHandler theScreenEvent) : base(theScreenEvent)
        {
            //Load the background texture for the screen
             this.game = game;
        }
        public override void Update(GameTime theTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.B) == true)
            {
                ScreenEvent.Invoke(game.GameplayScreen, new EventArgs());
                return;
            }
            base.Update(theTime);
        }
        public override void Draw(SpriteBatch theBatch)
        {
            base.Draw(theBatch);
        }
    }
}