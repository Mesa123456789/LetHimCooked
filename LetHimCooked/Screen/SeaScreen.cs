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
    public class SeaScreen : screen
    {

        Texture2D texture;
        AnimatedTexture SpriteTexture;
        Player player;
        Vector2 playerPos = Vector2.Zero;
        TiledMap _tiledMap;
        TiledMapRenderer _tiledMapRenderer;
        TiledMapObjectLayer _platformTiledObj;
        private readonly List<IEntity> _entities = new List<IEntity>();
        public readonly CollisionComponent _collisionComponent;
        //Camera _camera;
        Game1 game;
        RectangleF Bounds = new RectangleF(new Vector2(780, 64), new Vector2(40, 60));


        //Tile_FrontRestaurant Tile_Wall_Frontres
        public SeaScreen(Game1 game, EventHandler theScreenEvent) : base(theScreenEvent)
        {
            var viewportadapter = new BoxingViewportAdapter(game.Window, game.GraphicsDevice, 800, 450);
            Game1._camera = new OrthographicCamera(viewportadapter);//******//
            game._bgPosition = new Vector2(400, 225);//******//
            game._cameraPosition = new Vector2(440, 0);


            SpriteTexture = new AnimatedTexture(new Vector2(16, 16), 0, 2f, 1f);
            SpriteTexture.Load(game.Content, "Player-Sheet", 5, 4,10);
            player = new Player(SpriteTexture, playerPos, game, Bounds);
            player.Load(game.Content, "Sword");
            player.Load(game.Content, "Effect");

            //Load the background texture for the screen

            _collisionComponent = new CollisionComponent(new RectangleF(0, 0, 1600, 900));


            _tiledMap = game.Content.Load<TiledMap>("Tile_Sea");

            _tiledMapRenderer = new TiledMapRenderer(game.GraphicsDevice, _tiledMap);
            //Get object layers
            foreach (TiledMapObjectLayer layer in _tiledMap.ObjectLayers)
            {
                if (layer.Name == "Tile_Wall_Sea")
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

        RectangleF mouseRec;
        RectangleF FrontRec;
        public override void Update(GameTime theTime)
        {
            FrontRec = new RectangleF(740, 0, 100, 10);
            if (player.Bounds.Intersects(FrontRec) && !GameplayScreen.EnterDoor)
            {
                ScreenEvent.Invoke(game.GameplayScreen, new EventArgs());
                game._cameraPosition = new Vector2(400, 478);
                GameplayScreen.player.Bounds.Position = new Vector2(765, 880);
                GameplayScreen.EnterDoor = true;
                return;
            }
            if (!player.Bounds.Intersects(FrontRec))
            {
                GameplayScreen.EnterDoor = false;
            }
            MouseState ms = Mouse.GetState();

            mouseRec = new RectangleF(ms.X, ms.Y, 50, 50);
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
            //foreach (IEntity entity in _entities)
            //{
            //    entity.Draw(_spriteBatch);
            //}
            player.Draw(_spriteBatch);
  
            


        }

    }
}

