using LetHimCooked.Screen;
using LetHimCooked.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Collections;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace LetHimCooked
{
    public class Game1 : Game
    {

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        public GameplayScreen GameplayScreen;
        public CandyScreen CandyScreen;
        public SeaScreen SeaScreen;

        public RestauarntScreen RestauarntScreen;
        public TitleScreen TitleScreen;
        public screen mCurrentScreen;
        public Vector2 _bgPosition;
        Player player;
        public static Vector2 move;
        public static OrthographicCamera _camera;
        public Vector2 _cameraPosition;

        ///****/
        public Texture2D book;
        Texture2D ui;
        public Texture2D uiHeart;
        Texture2D inventory;
        Texture2D popup;
        Texture2D FridgeUi;
        Texture2D bookUi;
        public Texture2D QuestUI;
        public Texture2D Quest;
        public RectangleF questboxRec;
        public RectangleF bagRec;
        public Texture2D bag;

        public static List<Food> BagList = new List<Food>();
        public static List<Food> foodList = new();
        public static List<Enemy> enemyList = new();
        public static List<Food> CraftList = new List<Food>();
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 450;
            _graphics.ApplyChanges();
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            
        }
        AnimatedTexture SpriteTexture;
        Vector2 playerPos;
        Game1 game;
        public RectangleF Bounds = new RectangleF(new Vector2(750, 440), new Vector2(40, 60));
        protected override void Initialize()
        {
           // player = new Player(SpriteTexture, playerPos, game, Bounds);
            //camera_ = new Camera_1();
            // TODO: Add your initialization logic here
            base.Initialize();
        }
        public static int currentHeart;

        protected override void LoadContent()
        {
            SpriteTexture = new AnimatedTexture(new Vector2(0, 0), 0, 2f, 1f);
            SpriteTexture.Load(Content, "PlayerIdel", 5, 4, 5);
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            ui = Content.Load<Texture2D>("ui");
            uiHeart = Content.Load<Texture2D>("uiHeart");
            book = Content.Load<Texture2D>("book");
            Quest = Content.Load<Texture2D>("Quest");
            bag = Content.Load<Texture2D>("bag");
            FridgeUi = Content.Load<Texture2D>("FridgeUI");
            bookUi = Content.Load<Texture2D>("BookUI");
            QuestUI = Content.Load<Texture2D>("QuestUI");
            inventory = Content.Load<Texture2D>("inventory");
            popup = Content.Load<Texture2D>("popup");

            ///***///
            TitleScreen = new TitleScreen(this, new EventHandler(GameplayScreenEvent));
            RestauarntScreen = new RestauarntScreen(this, new EventHandler(GameplayScreenEvent));
            CandyScreen = new CandyScreen(this, new EventHandler(GameplayScreenEvent));
            SeaScreen = new SeaScreen(this, new EventHandler(GameplayScreenEvent));
            GameplayScreen = new GameplayScreen(this, new EventHandler(GameplayScreenEvent));
            mCurrentScreen = GameplayScreen;
            currentHeart = CandyScreen.uiHeart.Width - 10;
        }
        public RectangleF bookRec;
        public RectangleF mouseRec;
        public RectangleF XboxQ;
        public RectangleF xBox;
        bool OnCursor1;
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
           Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
            UpDateUI();
            //player.Update(gameTime);
            mCurrentScreen.Update(gameTime);
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            mCurrentScreen.Draw(_spriteBatch);
            _spriteBatch.End();
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            DrawUiGameplay(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
        public void GameplayScreenEvent(object obj, EventArgs e)
        {
            mCurrentScreen = (screen)obj;
        }
        public void UpdateCamera(Vector2 move)
        {
            _camera.LookAt(_bgPosition + _cameraPosition);//******//
            _cameraPosition += move;
        }
        public float GetCameraPosX()
        {
            return _cameraPosition.X;
        }
        public float GetCameraPosY()
        {
            return _cameraPosition.Y;
        }
        bool OncursorXBOX = false;
        bool OncursorxboxQ = false;
        bool closeXBox = false;
        bool closeXBoxQuest = false;
        public bool ShowInventory = false;
        public static bool IsPopUp = false;
        public void UpDateUI()
        {
            MouseState ms = Mouse.GetState();
            mouseRec = new RectangleF(ms.X, ms.Y, 50, 50);
            bagRec = new Rectangle(750,25, 25, 20);
            bookRec = new RectangleF(755, 100, 25, 15);
            questboxRec = new Rectangle(750, 150, 25, 20);//680, 30
            xBox = new Rectangle(650, 75, 30, 30);
            XboxQ = new Rectangle(680, 30, 30, 30);
            if (mouseRec.Intersects(bookRec) && ms.LeftButton == ButtonState.Pressed)
            {
                closeXBoxQuest = false;
                closeXBox = false;
                openbookUI = true;
                openQuestUI = false;
                ShowInventory = false;
            }
            else { OnCursor1 = false; }
            if (mouseRec.Intersects(questboxRec) && ms.LeftButton == ButtonState.Pressed)
            {
                closeXBox = false;
                openQuestUI = true;
                ShowInventory = false;
                openbookUI = false;
            }
            else { OnCursor2 = false; }
            if (mouseRec.Intersects(bagRec))
            {
                OnCursor = true;
                if (ms.LeftButton == ButtonState.Pressed && mouseRec.Intersects(bagRec))
                {
                    closeXBox = false;
                    ShowInventory = true;
                    openQuestUI = false;
                    openbookUI = false;
                }
            }
            else { OnCursor = false; }
            if (mouseRec.Intersects(xBox) && ms.LeftButton == ButtonState.Pressed)
            {
                closeXBox = true;
                openbookUI = false;
                ShowInventory = false;
                openQuestUI = false;
            }
            if (mouseRec.Intersects(XboxQ) && ms.LeftButton == ButtonState.Pressed)
            {
                closeXBoxQuest = true;
                closeXBox = false;
                openbookUI = false;
                ShowInventory = false;
                openQuestUI = false;
            }
            if (mouseRec.Intersects(xBox)) { OnCursorXBOX = true; }
            else { OnCursorXBOX = false; }
            if (mouseRec.Intersects(XboxQ)) { OncursorxboxQ = true; }
            else { OncursorxboxQ = false; }
            if (mouseRec.Intersects(bookRec)) { OnCursor1 = true; }
            else { OnCursor1 = false;  }
            if (mouseRec.Intersects(questboxRec)) { OnCursor2 = true; }
            else { OnCursor2 = false; }
            if(ShowInventory) { OnCursor1 = false; OnCursor2 = false; }
            if (openbookUI) { OnCursor = false; OnCursor2 = false; }
            if (openQuestUI) { OnCursor = false; OnCursor1 = false; }
            if (currentHeart < 60) { color = Color.Red; }
            else { color = Color.White; }

        }
        
        Color color = Color.White;
        bool OnCursor = false;
        bool OnCursor2 = false;
        bool OnCursorXBOX = false;
        bool openbookUI = false;
        bool openQuestUI = false;
        bool openFridgeUI = false;
        int mouse_state = 1;
        public void DrawUiGameplay(SpriteBatch _spriteBatch)
        {
            foreach (Food food in BagList)
            {
                if (IsPopUp == true)
                {
                    for (int i = 0; i < BagList.Count; i++)
                    {
                        _spriteBatch.Draw(popup, new Vector2(635, 170), Color.White);
                        _spriteBatch.Draw(BagList[i].foodTexture, new Vector2(653, 180), Color.White);
                    }
                    CountTime(100);
                }
            }
            if (openQuestUI == true)
            {
                _spriteBatch.Draw(QuestUI, new Vector2(0, 0), new Rectangle(0, 0, 700, 400), Color.White);
                if (closeXBox == false)
                {
                    _spriteBatch.Draw(QuestUI, new Vector2(655, 35), new Rectangle(745 + 64, 81, 64, 40), Color.White);
                }
                if (OnCursorXBOX == true)
                {
                    _spriteBatch.Draw(QuestUI, new Vector2(655, 35), new Rectangle(745 , 81, 64, 40), Color.White);
                }
            }
            if (openbookUI == true)
            {
                _spriteBatch.Draw(bookUi, new Vector2(150, 0), new Rectangle(153, 0, 800, 500), Color.White);
                if (OncursorxboxQ == false)
                {
                    _spriteBatch.Draw(QuestUI, new Vector2(683, 20), new Rectangle(745 + 64, 81, 64, 40), Color.White);
                }
                if (OncursorxboxQ == true)
                {
                    _spriteBatch.Draw(QuestUI, new Vector2(683, 20), new Rectangle(745, 81, 64, 40), Color.White);
                }
            }
            if (ShowInventory == true)
            {
                _spriteBatch.Draw(inventory, new Vector2(123, 125), Color.White);
                for (int i = 0; i < BagList.Count; i++)
                {
                    _spriteBatch.Draw(BagList[i].foodTexture, new Vector2(153 + i * 53, 156), Color.White);
                }
                if (closeXBox == false)
                {
                    _spriteBatch.Draw(QuestUI, new Vector2(650, 60), new Rectangle(745 + 64, 81, 64, 40), Color.White);
                }
                if (OnCursorXBOX == true)
                {
                    _spriteBatch.Draw(QuestUI, new Vector2(650, 60), new Rectangle(745, 81, 64, 40), Color.White);
                }
            }
            _spriteBatch.Draw(bag, new Vector2(735, 15), new Rectangle(64, 0, 48, 44), Color.White);
            if (OnCursor == true || ShowInventory)
            {
                _spriteBatch.Draw(bag, new Vector2(735 - 2, 15), new Rectangle(1, 0, 63, 44), Color.White);
            }
            _spriteBatch.Draw(book, new Vector2(735, 65), new Rectangle(64, 0, 48, 44), Color.White);
            if (OnCursor1 == true || openbookUI)
            {
                _spriteBatch.Draw(book, new Vector2(735-2, 65), new Rectangle(1, 0, 63, 44), Color.White);
            }
            _spriteBatch.Draw(Quest, new Vector2(735, 115), new Rectangle(64, 0, 48, 44), Color.White);
            if (OnCursor2 == true || openQuestUI == true)
            {
                _spriteBatch.Draw(Quest, new Vector2(735-2, 115), new Rectangle(1, 0, 63, 44), Color.White);
            }
            _spriteBatch.Draw(uiHeart, new Vector2(110, 22),
new Rectangle(0, 0, currentHeart + 10, 18), color);
            _spriteBatch.Draw(ui, new Vector2(6, 6), Color.White);
            SpriteTexture.DrawFrame(_spriteBatch, new Vector2(23, 25), 1);

        }

        public int countPopUp;
        public void CountTime(int timePopup)
        {
            countPopUp += 1;
            {
                if (countPopUp > timePopup)
                {
                    countPopUp = 0;
                    IsPopUp = false;
                    //ShowInventory = false;

                }
            }
        }



    }
}
