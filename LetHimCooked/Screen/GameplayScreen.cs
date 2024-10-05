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
using System.Diagnostics;
using System.Reflection.Metadata;


namespace LetHimCooked.Screen
{
    public class GameplayScreen : screen
    {
        RestauarntScreen res;
        Texture2D texture;
        AnimatedTexture SpriteTexture;
        public static Player player;
        
        TiledMap _tiledMap;
        TiledMapRenderer _tiledMapRenderer;
        TiledMapObjectLayer _platformTiledObj;
        private readonly List<IEntity> _entities = new List<IEntity>();
        public readonly CollisionComponent _collisionComponent;
        Game1 game;
        public RectangleF Bounds = new RectangleF(new Vector2(750,440), new Vector2(32, 32));
        Texture2D foodTexture;
        Texture2D foodTex2;
        Texture2D foodTex3;
        Texture2D foodTex4;
        Texture2D foodTex5;
        Texture2D foodTex6;
        Texture2D foodTex7;
        Texture2D foodTex8;
        Texture2D foodTex9;
        Texture2D foodTex10;
        Texture2D foodTex11;

        Vector2 playerPos;// = new Vector2(player.Bounds.Position.X, player.Bounds.Position.Y);
        public GameplayScreen(Game1 game, EventHandler theScreenEvent ) : base(theScreenEvent)
        {
            foodTexture = game.Content.Load<Texture2D>("chicken");
            //foodTex2 = game.Content.Load<Texture2D>("enemy");
            //foodTex3 = game.Content.Load<Texture2D>("enemy2");
            //foodTex4 = game.Content.Load<Texture2D>("flower");
            //foodTex5 = game.Content.Load<Texture2D>("fruit");
            //foodTex6 = game.Content.Load<Texture2D>("mushroom1");
            //foodTex7 = game.Content.Load<Texture2D>("mushroom2");
            //foodTex8 = game.Content.Load<Texture2D>("pumkin");
            //foodTex9 = game.Content.Load<Texture2D>("pumkin2");
            //foodTex10 = game.Content.Load<Texture2D>("snail");
            //foodTex11 = game.Content.Load<Texture2D>("snail2");
            Game1.enemyList.Add(new Enemy(foodTexture, new Vector2(550, 250)));
            //Game1.enemyList.Add(new Enemy(foodTex2, new Vector2(500, 400)));
            //Game1.foodList.Add(new Food(foodTex3, new Vector2(300 + 100, 300)));
            //Game1.foodList.Add(new Food(foodTex4, new Vector2(150 + 100, 150)));
            //Game1.foodList.Add(new Food(foodTex5, new Vector2(300 + 100, 200)));
            //Game1.foodList.Add(new Food(foodTex6, new Vector2(380 + 100, 330)));
            //Game1.foodList.Add(new Food(foodTex7, new Vector2(230 + 100, 260)));
            //Game1.foodList.Add(new Food(foodTex8, new Vector2(300, 200)));
            //Game1.foodList.Add(new Food(foodTex9, new Vector2(100, 200)));
            //Game1.enemyList.Add(new Enemy(foodTex10, new Vector2(100, 250)));
            //Game1.enemyList.Add(new Enemy(foodTex11, new Vector2(150, 280)));
            game._cameraPosition = new Vector2(400, 200);
            var viewportadapter = new BoxingViewportAdapter(game.Window, game.GraphicsDevice, 800, 450);
            Game1._camera = new OrthographicCamera(viewportadapter);//******//
            game._bgPosition = new Vector2(400, 225);//******//

            SpriteTexture = new AnimatedTexture(new Vector2(16, 16), 0, 2f,1f);
            SpriteTexture.Load(game.Content, "Player-Sheet", 5,4,10);
            player = new Player(SpriteTexture, playerPos, game, Bounds);
            player.Load(game.Content,"Sword");
            player.Load(game.Content, "Effect");
            //Load the background texture for the screen
            _collisionComponent = new CollisionComponent(new RectangleF(0, 0, 1600, 900));
            _tiledMap = game.Content.Load<TiledMap>("Tile_FrontRestaurant");
            _tiledMapRenderer = new TiledMapRenderer(game.GraphicsDevice, _tiledMap);
            //Get object layers
            foreach (TiledMapObjectLayer layer in _tiledMap.ObjectLayers)
            {
                if (layer.Name == "Tile_Wall_Frontres")
                {
                    _platformTiledObj = layer;
                }
            }
            foreach (TiledMapObject obj in _platformTiledObj.Objects)
            {
                Vector2 position = new Vector2(obj.Position.X, obj.Position.Y);
                _entities.Add(new PlatformEntity(game, new RectangleF(position, obj.Size)));
            }

            _entities.Add(player);

            foreach (IEntity entity in _entities)
            {
                _collisionComponent.Insert(entity);


            }
            this.game = game;
        }
        RectangleF doorRec = new RectangleF(750, 400, 100, 20);
        RectangleF SeaMapRec = new RectangleF(700, 880, 100, 10);
        RectangleF CandyMapRec = new RectangleF(0, 400, 50, 100);
        RectangleF mouseRec;
        public static bool EnterDoor = false;
        public override void Update(GameTime theTime)
        {
            MouseState ms = Mouse.GetState();
            mouseRec = new RectangleF(ms.X, ms.Y, 50, 50);
            if (!player.Bounds.Intersects(doorRec) && !player.Bounds.Intersects(CandyMapRec) && !player.Bounds.Intersects(SeaMapRec))
            {
                GameplayScreen.EnterDoor = false;
            }
            if (player.Bounds.Intersects(doorRec) && !EnterDoor)
            {
                EnterDoor = true;
                ScreenEvent.Invoke(game.RestauarntScreen, new EventArgs());
                return;
            }
            if (player.Bounds.Intersects(CandyMapRec) && !EnterDoor)
            {
                EnterDoor = true;
                ScreenEvent.Invoke(game.CandyScreen, new EventArgs());
                game._cameraPosition = new Vector2(800, 200);
                return;
            }
            if (player.Bounds.Intersects(SeaMapRec) && !EnterDoor)
            {
                EnterDoor = true;
                ScreenEvent.Invoke(game.SeaScreen, new EventArgs());
                // player.Bounds.Position = new Vector2(780, 64);
                game._cameraPosition = new Vector2(440, 0);
                return;
            }
            for (int i = 0; i < Game1.BagList.Count; i++)
            {
                Game1.BagList[i].Update(theTime);
            }
           
            for (int i = Game1.foodList.Count - 1; i >= 0; i--)
            {
                Game1.foodList[i].Update(theTime);
            }
            for (int i = Game1.enemyList.Count - 1; i >= 0; i--)
            {
                Game1.enemyList[i].Update(theTime);
            }

            foreach (IEntity entity in _entities)
            {
                entity.Update(theTime);
            }
            _collisionComponent.Update(theTime);
            _tiledMapRenderer.Update(theTime);
            
            Game1._camera.LookAt(game._bgPosition + game._cameraPosition);//******//
            player.Update(theTime);
            base.Update(theTime);
        }

        public override void Draw(SpriteBatch _spriteBatch)
        {

            var transformMatrix = Game1._camera.GetViewMatrix();//******//
            _tiledMapRenderer.Draw(transformMatrix);//******//
            _spriteBatch.End();
            _spriteBatch.Begin(transformMatrix: transformMatrix, samplerState: SamplerState.PointClamp);//******//
            foreach (Food food in Game1.foodList)
            {
                for (int i = 0; i < Game1.foodList.Count; i++)
                {
                    Game1.foodList[i].Draw(_spriteBatch);
                }
            }
            foreach (Enemy enemy in Game1.enemyList)
            {
                for (int i = 0; i < Game1.enemyList.Count; i++)
                {
                    Game1.enemyList[i].Draw(_spriteBatch);
                }
            }
            player.Draw(_spriteBatch);





        }


    }
}

