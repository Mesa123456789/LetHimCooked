using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using System;
using System.Diagnostics;
using System.Net.Mime;
using MonoGame.Extended.ECS;
using System.Reflection.Metadata;
using System.Xml.Linq;


namespace LetHimCooked.Sprite
{
    public class Player : IEntity, ICollisionActor
    {
        Enemy enemy;
        public Vector2 CharPosition;
        AnimatedTexture SpriteTexture;
        AnimatedTexture SpriteTextureIdel;
        MouseState mouseSt;
        Vector2 mousepos;
        Vector2 distance = new();
        Vector2 posMouse = new();
        Vector2 knockbackdirection;
        public Camera camera = new Camera();
        Texture2D sword, effect;

        public Rectangle mouseCheck;
        public enum Direction { Left, Right, Up, Down }
        public Direction direction { get; set; }

        private readonly Game1 _game;
        public int Velocity = 4;
        public Vector2 move;
        public IShapeF Bounds { get; }
        private KeyboardState ks;
        private KeyboardState _oldKey;
        Texture2D myTexture;
        float rotation;


        public Player(AnimatedTexture SpriteTexture, Vector2 CharPosition, Game1 game, IShapeF circleF)
        {

            this.SpriteTexture = SpriteTexture;
            this.CharPosition = CharPosition;
            direction = Direction.Right;
            _game = game;
            Bounds = circleF;
            SpriteTextureIdel = new AnimatedTexture(new Vector2(16,16), 0, 2f, 1f);
            
        }

        public void Load(ContentManager content, string asset)
        {
            sword = content.Load<Texture2D>("Sword");
            effect = content.Load<Texture2D>("Effect");
            SpriteTextureIdel.Load(content, "PlayerIdel", 5, 4, 4);
            //myTexture = content.Load<Texture2D>("Player-Sheet");

        }

        Vector2 dr;
        bool IsMove = false;
        public void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            SpriteTextureIdel.Play();
            SpriteTexture.Pause();
            KeyboardState ks = Keyboard.GetState();
            ks = Keyboard.GetState();


            if (ks.IsKeyDown(Keys.D) && Bounds.Position.X < 1600 - ((RectangleF)Bounds).Width)
            {
                move = new Vector2(Velocity, 0) * gameTime.GetElapsedSeconds() * 42;
                move.Normalize();
                if (Bounds.Position.X - _game.GetCameraPosX() >= 400 && _game.GetCameraPosX() < 700)
                {
                    _game.UpdateCamera(move);
                }
                Bounds.Position += move;
                SpriteTexture.Play();
                direction = Direction.Right;
                 IsMove = true;
            }
            else
            {
                IsMove = false;
            }
            if (ks.IsKeyDown(Keys.A) && Bounds.Position.X > 0 - ((RectangleF)Bounds).Width)
            {
                move = new Vector2(-Velocity, 0) * gameTime.GetElapsedSeconds() * 42;
                move.Normalize();
                if (Bounds.Position.X - _game.GetCameraPosX() <= 400 && _game.GetCameraPosX() > 0)
                {
                    _game.UpdateCamera(move);
                }
 
                Bounds.Position += move;
                SpriteTexture.Play();
                direction = Direction.Left;
               IsMove = true;
            }
            //else
            //{
            //    IsMove = false;
            //}
            if (ks.IsKeyDown(Keys.S) && Bounds.Position.Y < 900 - ((RectangleF)Bounds).Height)
            {
                move = new Vector2(0, Velocity) * gameTime.GetElapsedSeconds() * 42;
                move.Normalize();
                if (Bounds.Position.Y - _game.GetCameraPosY() >= 225 && _game.GetCameraPosY() < 450)
                {
                    _game.UpdateCamera(move);
                }
                Bounds.Position += move;
                SpriteTexture.Play();
                direction = Direction.Right;
                IsMove = true;

            }
            //else
            //{
            //    IsMove = false;
            //}
            if (ks.IsKeyDown(Keys.W) && Bounds.Position.Y > 0 - ((RectangleF)Bounds).Height)
            {
                move = new Vector2(0, -Velocity) * gameTime.GetElapsedSeconds() * 42;
                move.Normalize();
                if (Bounds.Position.Y - _game.GetCameraPosY() <= 225 && _game.GetCameraPosY() > 0)
                {
                    _game.UpdateCamera(move);
                }
                Bounds.Position += move;
                SpriteTexture.Play();
                direction = Direction.Down;
                IsMove = true;
            }
            //else
            //{
            //    IsMove = false;
            //}

            if (move != Vector2.Zero)
                {
                    move.Normalize();
                }
           if(IsMove == false)
            {
                SpriteTextureIdel.Play();
            }
            SpriteTexture.UpdateFrame(elapsed);
            SpriteTextureIdel.UpdateFrame(elapsed);


                /////*****
                camera.WorldPos(camera.cameraPos.X, camera.cameraPos.Y);

                mouseSt = Mouse.GetState();
                mousepos = Mouse.GetState().Position.ToVector2();
                posMouse = new Vector2(mousepos.X + (camera.cameraPos.X), mousepos.Y + (camera.cameraPos.Y));
                mouseCheck = new Rectangle((int)posMouse.X, (int)posMouse.Y, 24, 24);

                distance = new Vector2(posMouse.X - Bounds.Position.X, posMouse.Y - Bounds.Position.Y);

                rotation = (float)Math.Atan2(distance.Y, distance.X);

                Attack(enemy);

            }
        


        public void Draw(SpriteBatch _spriteBatch)
        {
            if (IsMove == true)
            {
                SpriteTexture.DrawFrame(_spriteBatch, new Vector2((int)((RectangleF)Bounds).X + 10, (int)((RectangleF)Bounds).Y +  32), (int)direction + 1);
            }
            if (IsMove == false)
            {
                SpriteTextureIdel.DrawFrame(_spriteBatch, new Vector2((int)((RectangleF)Bounds).X + 10 , (int)((RectangleF)Bounds).Y + 32), (int)direction + 1);
            }

            if (state == 1 && isAttack == true)
            {
                _spriteBatch.Draw(effect, new Vector2(Bounds.Position.X , Bounds.Position.Y + 15), new Rectangle(0, 0, 48, 48), Color.White, rotation, new Vector2(24, 24), 2.0f, SpriteEffects.FlipVertically, 0.0f);
                _spriteBatch.Draw(sword, new Vector2(Bounds.Position.X, Bounds.Position.Y + 15), new Rectangle(0, 0, 48, 48), Color.White, rotation, new Vector2(24, 24), 2.0f, SpriteEffects.None, 0.0f);
            }

            //Attack 2
            if (state == 2 && isAttack == true)
            {
                postrotation = rotation;
                _spriteBatch.Draw(effect, new Vector2(Bounds.Position.X, Bounds.Position.Y + 15), new Rectangle(0, 0, 48, 48), Color.White, rotation, new Vector2(24, 24), 2.0f, SpriteEffects.None, 0.0f);
                _spriteBatch.Draw(sword, new Vector2(Bounds.Position.X, Bounds.Position.Y + 15), new Rectangle(0, 0, 48, 48), Color.White, rotation + 135.0f, new Vector2(24, 24), 2.0f, SpriteEffects.FlipHorizontally, 0.0f);
            }

            //Attack 3
            if (state == 3 && isAttack == true)
            {
                postrotation = rotation;
                _spriteBatch.Draw(effect, new Vector2(Bounds.Position.X, Bounds.Position.Y + 15), new Rectangle(0, 0, 48, 48), Color.White, rotation, new Vector2(24, 24), 2.0f, SpriteEffects.None, 0.0f);
                _spriteBatch.Draw(sword, new Vector2(Bounds.Position.X, Bounds.Position.Y + 15), new Rectangle(0, 0, 48, 48), Color.White, rotation - 190.0f, new Vector2(24, 24), 2.0f, SpriteEffects.FlipHorizontally, 0.0f);
            }

        }
       

        public void OnCollision(CollisionEventArgs collisionInfo)
        {
            if (collisionInfo.Other.ToString().Contains("PlatformEntity"))
            {
                Bounds.Position -= collisionInfo.PenetrationVector;
            }

        }

        public float postrotation;
        bool isClick;
        bool isAttack;
        int state;
        int count;
        public void Attack(Enemy enemy)
        {

            if (mouseSt.LeftButton == ButtonState.Pressed)
            {
                if (isClick == false)
                {
                    if (state <= 3)
                    {
                        state = state + 1;
                        count = 0;
                        isClick = true;
                        isAttack = true;

                        //------------------- ตีแล้วพุ่งไปด้านหน้า/-------------------
                        //knockbackdirection = posmouse - position;
                        //if (knockbackdirection != vector2.zero)
                        // {
                        //     knockbackdirection.normalize();
                        // }

                        //if(isattack == true)
                        //{
                        //     position += knockbackdirection * 15;
                        //     camera.camerapos.x += knockbackdirection.x * 15;
                        //     camera.camerapos.y += knockbackdirection.y * 15;
                        //
                    }
                }
            }

            if (mouseSt.LeftButton == ButtonState.Released)
            {
                postrotation = rotation;
                isClick = false;
                isAttack = false;
            }

            if (state > 3 || count > 20)
            {
                state = 0;
            }
            if (state < 4)
            {
                count++;
            }

            if (count < 15)
            {
                isAttack = true;

            }
            else
            {
                isAttack = false;
                //isCheck = false;
            }

        //    Rectangle attck_top_rignt = new Rectangle((int)position.X + 16, (int)position.Y - 32, 48, 48);
        //    Rectangle attck_top_left = new Rectangle((int)position.X - 32, (int)position.Y - 32, 48, 48);
        //    Rectangle attck_bot_rignt = new Rectangle((int)position.X + 16, (int)position.Y + 16, 48, 48);
        //    Rectangle attck_bot_left = new Rectangle((int)position.X - 32, (int)position.Y + 16, 48, 48);

        //    // attck_top_rignt
        //    if ((mousepos.X >= renderPlayer.X && mousepos.Y <= renderPlayer.Y) && attck_top_rignt.Intersects(enemy.hitbox) && isAttack)
        //    {
        //        isCheck = true;
        //        knockBackDirection = enemy.position - position;
        //        if (knockBackDirection != Vector2.Zero)
        //        {
        //            knockBackDirection.Normalize();
        //        }
        //        enemy.position += knockBackDirection * 20;
        //    }
        //    //attck_top_left
        //    if ((mousepos.X <= renderPlayer.X && mousepos.Y <= renderPlayer.Y) && attck_top_left.Intersects(enemy.hitbox) && isAttack)
        //    {
        //        isCheck = true;
        //        knockBackDirection = enemy.position - position;
        //        if (knockBackDirection != Vector2.Zero)
        //        {
        //            knockBackDirection.Normalize();
        //        }
        //        enemy.position += knockBackDirection * 20;
        //    }
        //    //attck_bot_rignt
        //    if ((mousepos.X >= renderPlayer.X && mousepos.Y >= renderPlayer.Y) && attck_bot_rignt.Intersects(enemy.hitbox) && isAttack)
        //    {
        //        isCheck = true;
        //        knockBackDirection = enemy.position - position;
        //        if (knockBackDirection != Vector2.Zero)
        //        {
        //            knockBackDirection.Normalize();
        //        }
        //        enemy.position += knockBackDirection * 20;
        //    }
        //    //attck_bot_left
        //    if ((mousepos.X <= renderPlayer.X && mousepos.Y >= renderPlayer.Y) && attck_bot_left.Intersects(enemy.hitbox) && isAttack)
        //    {
        //        isCheck = true;
        //        knockBackDirection = enemy.position - position;
        //        if (knockBackDirection != Vector2.Zero)
        //        {
        //            knockBackDirection.Normalize();
        //        }
        //        enemy.position += knockBackDirection * 20;
        //    }



        //    Debug.WriteLine("count : " + enemy.position);
        }
    }
}
