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
    public class RestauarntScreen : screen
    {
        Texture2D popup;
        Texture2D interact;
        Texture2D craft;
        Texture2D inventory;
        Texture2D FridgeUi;
        Texture2D QuestUI;
        Texture2D uni;
        AnimatedTexture SpriteTexture;
        public static Player player;
        TiledMap _tiledMap;
        TiledMapRenderer _tiledMapRenderer;
        TiledMapObjectLayer _platformTiledObj;
        private readonly List<IEntity> _entities = new List<IEntity>();
        public readonly CollisionComponent _collisionComponent;
        Game1 game;
        Vector2 playerPos;// = new Vector2((int)player.Bounds.Position.X,(int) player.Bounds.Position.Y);
        RectangleF Bounds = new RectangleF(new Vector2(180,330), new Vector2(40, 60));


        public RestauarntScreen(Game1 game, EventHandler theScreenEvent) : base(theScreenEvent)
        {
            game._bgPosition = new Vector2(400, 225);//******/
            SpriteTexture = new AnimatedTexture(new Vector2(16,16), 0, 2f, 1f);
            SpriteTexture.Load(game.Content, "Player-Sheet", 5, 4,10);
            player = new Player(SpriteTexture, playerPos, game, Bounds);
            popup = game.Content.Load<Texture2D>("popup");
            interact = game.Content.Load<Texture2D>("interact");
            craft = game.Content.Load<Texture2D>("craft");
            inventory = game.Content.Load<Texture2D>("inventory");
            FridgeUi = game.Content.Load<Texture2D>("FridgeUI");
            QuestUI = game.Content.Load<Texture2D>("QuestUI");
            player.Load(game.Content, "Sword");
            player.Load(game.Content, "Effect");
            uni = game.Content.Load<Texture2D>("Uni");

            //Load the background texture for the screen

            _collisionComponent = new CollisionComponent(new RectangleF(0, 0, 800, 450));


            _tiledMap = game.Content.Load<TiledMap>("Tile_Inrestaurant");

            _tiledMapRenderer = new TiledMapRenderer(game.GraphicsDevice, _tiledMap);
            //Get object layers
            foreach (TiledMapObjectLayer layer in _tiledMap.ObjectLayers)
            {
                if (layer.Name == "Tile_Wall_Inretaurant")
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

        RectangleF mouseBox;
       public static RectangleF doorRec = new RectangleF(120,40, 200, 20);
        bool IssendMenuInterect = false;
        bool openFridgeUI = false;
        bool IsInterect = false;
        public static bool Ontable = false;
        bool GotMenu = false;    
        bool Crafting = false;
        public override void Update(GameTime theTime)
        {
            if (player.Bounds.Intersects(doorRec) && !GameplayScreen.EnterDoor)
            {
                ScreenEvent.Invoke(game.GameplayScreen, new EventArgs());
                GameplayScreen.player.Bounds.Position = new Vector2(600, 300);
                GameplayScreen.EnterDoor = true;
                return;
            }
            if (!player.Bounds.Intersects(doorRec))
            {
                GameplayScreen.EnterDoor = false;
            }
            MouseState ms = Mouse.GetState();
            mouseBox = new RectangleF(ms.X, ms.Y, 50, 50);
            RectangleF FrigeRec = new RectangleF(348, 120, 40, 80);
            RectangleF tableBox = new RectangleF(450, 150, 130, 20);
            RectangleF sendMenu = new RectangleF(600,240,40,30);
            RectangleF equal = new RectangleF(345, 140, 120, 50);
            if (player.Bounds.Intersects(FrigeRec))
            {
                IsFrigeInterect = true;
                if (mouseBox.Intersects(FrigeRec) && ms.LeftButton == ButtonState.Pressed)
                {
                    openFridgeUI = true;
                }
            }
            else { IsFrigeInterect = false; openFridgeUI = false; }
            if (player.Bounds.Intersects(tableBox))
            {
                IsInterect = true;
                if (ms.LeftButton == ButtonState.Pressed && mouseBox.Intersects(tableBox))
                {
                    Ontable = true;
                }
            }
            else
            {
                IsInterect = false;
                Ontable = false;
            }
            if (player.Bounds.Intersects(equal) && Ontable)
            {
                Crafting = true;
            }
            else
            {
                Crafting = false;
            }
            for (int i = 0; i < Game1.BagList.Count; i++)
            {
                Game1.BagList[i].Update(theTime);
                if (mouseBox.Intersects(Game1.BagList[i].foodBox) && ms.LeftButton == ButtonState.Pressed && Ontable)
                {
                    Game1.CraftList.Add(Game1.BagList[i]);
                    Game1.BagList.RemoveAt(i);
                    break;
                }
            }
            if (player.Bounds.Intersects(sendMenu))
            {
                IssendMenuInterect = true;
                //if (mouseBox.Intersects(sendMenu) && ms.LeftButton == ButtonState.Pressed && GotMenu == true)
                //{
                //    sendingMenu = false;
                //}
            }
            else
            {
                IssendMenuInterect = false;
            }
            foreach (IEntity entity in _entities)
            {
                entity.Update(theTime);
            }
            _collisionComponent.Update(theTime);
            _tiledMapRenderer.Update(theTime);
            player.Update(theTime);
            base.Update(theTime);
        }

        int MenuPopup;
        bool IsFrigeInterect = false;
        bool sendingMenu = false;
        ///ย้ายไปเกม1 เมาสืจะไม่เพี้ยนมั้ง
        public override void Draw(SpriteBatch _spriteBatch)
        {

            _tiledMapRenderer.Draw();//******//
            _spriteBatch.End();
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);//******//
            foreach (IEntity entity in _entities)
            {
                entity.Draw(_spriteBatch);
            }
            //_spriteBatch.Draw(popup, new Rectangle((int)doorRec.X, (int)doorRec.Y, (int)doorRec.Width, (int)doorRec.Height), Color.White);
            if (IsInterect == true)
            {
                _spriteBatch.Draw(interact, new Rectangle(443, 148, 140, 44), Color.White);
            }
            if (IsFrigeInterect == true)
            {
                _spriteBatch.Draw(interact, new Rectangle(346, 116, 40, 80), Color.White);
            }
            if (IssendMenuInterect == true)
            {
                _spriteBatch.Draw(interact, new Rectangle(600, 240, 45, 33), Color.White);
            }
            
            player.Draw(_spriteBatch);
            //menu
            //if (sendingMenu == true && GotMenu == true && food.getFood == 2)
            //{
            //    _spriteBatch.Draw(uni, new Rectangle((int)player.CharPosition.X, (int)player.CharPosition.Y + 13, 32, 32), Color.White);
            //}
            if (openFridgeUI == true)
            {
                _spriteBatch.Draw(FridgeUi, new Vector2(0, 0), Color.White);
            }
            if (Ontable == true)
            {
                _spriteBatch.Draw(craft, new Vector2(215, 60), Color.White);
                _spriteBatch.Draw(inventory, new Vector2(129, 220), Color.White);
                if (!GotMenu)
                {
                    for (int i = 0; i < Game1.CraftList.Count; i++)
                    {
                        _spriteBatch.Draw(Game1.CraftList[i].foodTexture, new Vector2(240 + i * 68, 100), Color.White);
                    }
                }
                for (int i = 0; i < Game1.BagList.Count; i++)
                {
                    _spriteBatch.Draw(Game1.BagList[i].foodTexture, new Vector2(160 + i * 52, 250), Color.White);
                }
            }

            if (Crafting == true && Ontable)// && food.getFood == 2)
            {
                _spriteBatch.Draw(QuestUI, new Vector2(720, 320) , Color.White);
                //_spriteBatch.Draw(menuBG, new Rectangle(650, 230, 300, 300), Color.White);
                _spriteBatch.Draw(uni, new Rectangle(733, 330, 128, 128), Color.White);
                GotMenu = true;
            }
            //_spriteBatch.Draw(QuestUI, new Vector2(720, 320), new Rectangle(725, 133, 146, 190), Color.White,rotationMenuBG, Vector2.Zero, 1f, 0, 1);
            //if (MenuPopup == 1 && !FinsihCooking)
            //{
            //    if (Crafting == true && food.getFood == 2)
            //    {
            //        _spriteBatch.Draw(QuestUI, new Vector2(720, 320), new Rectangle(725, 133, 146, 190), Color.White,
            //            rotationMenuBG, Vector2.Zero, 1f, 0, 1);
            //        _spriteBatch.Draw(menuBG, new Rectangle(650, 230, 300, 300), Color.White);
            //        _spriteBatch.Draw(uni, new Rectangle(733, 330, 128, 128), Color.White);
            //        GotMenu = true;
            //    }

            //    CountTime(200);
            //}

        }

        public int countPopUp;
        public void CountTime(int timePopup)
        {
            countPopUp += 1;
            {
                if (countPopUp > timePopup)
                {
                    countPopUp = 0;
                    //GA.IsPopUp = false;
                    Ontable = false;
                    MenuPopup = 0;
                }
            }
        }

    }
}

