using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Threading;

// using GeonBit UI elements
using GeonBit.UI.Entities;
using GeonBit.UI.Entities.TextValidators;
using GeonBit.UI.DataTypes;
using GeonBit.UI.Utils.Forms;
using GeonBit.UI;
using Bindings;

namespace PVPGameClient
{
    public class GameHandler : Game
    {
        public static GameHandler I;

        private GraphicsDeviceManager Graphics;
        public static SpriteBatch SpriteBatch;

        // Events
        public delegate void DrawEvent();
        public delegate void UpdateEvent();
        public static event DrawEvent OnDraw;
        public static event UpdateEvent OnBeforeUpdate;
        public static event UpdateEvent OnUpdate;
        public static event UpdateEvent OnLateUpdate;

        // System Info
        int Width, Height;

        // Important System
        private FPSCounter FPS;

        // UI
        public ConnexionPanel ConnexionPanel;
        public CharacterSelectionPanel CharacterSelectionPanel;

        // Connexion state
        public Timer ConnexionTimer;
        public bool AwaitConnexion = false;
        public static int CurrentPlayerIndex;

        // Start
        public static Player[] Players = new Player[Constants.MAX_PLAYERS];
        public static SpriteFont _font;
        public static Texture2D _texture;
        public Texture2D PlayerBody;

        public static ClientTCP ClienTCP;
        public static ClientDataHandler DataHandler;

        public GameHandler()
        {
            I = this;
            Graphics = new GraphicsDeviceManager(this)
            {
                SynchronizeWithVerticalRetrace = false
            };
            Graphics.ApplyChanges();
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
        }

        protected override void Initialize()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            // create and init the UI manager
            UserInterface.Initialize(Content, BuiltinThemes.hd);
            UserInterface.Active.UseRenderTarget = true;

            // draw cursor outside the render target
            UserInterface.Active.IncludeCursorInRenderTarget = false;

            // Create a new SpriteBatch, which can be used to draw textures.
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            // make the window fullscreen (but still with border and top control bar)
            //Width = 1600;
            //Height = 900;
            Width = 1600 / 3 * 2;
            Height = 900 / 3 * 2;

            Graphics.PreferredBackBufferWidth = Width;
            Graphics.PreferredBackBufferHeight = Height;
            Graphics.IsFullScreen = false;
            Graphics.ApplyChanges();

            // Server part
            DataHandler = new ClientDataHandler();
            ClienTCP = new ClientTCP(false);

            base.Initialize();
        }
        protected override void LoadContent()
        {
            PlayerBody = Content.Load<Texture2D>("Sprites/Characters/body");
            _font = Content.Load<SpriteFont>("Fonts/PixelFont");
            _texture = Content.Load<Texture2D>("Sprites/Characters/player_1");

            // Initialize important systems
            new TickSystem();
            new InputSystem();
            FPS = new FPSCounter();

            InitializeUI();
            
            base.LoadContent();
        }
        protected override void Update(GameTime gameTime)
        {
            if (!IsActive) return;

            // Set Time
            Globals.GameTime = gameTime;
            BeforeUpdate();

            // Events
            OnUpdate?.Invoke();

            if (InputSystem.GetKeyUp(Keys.F3))
            {
                FPS.HideShow();
            }

            // Update UI
            UserInterface.Active.Update(gameTime);

            LateUpdate();

            base.Update(gameTime);
        }
        protected void BeforeUpdate()
        {
            OnBeforeUpdate?.Invoke();
        }
        protected void LateUpdate()
        {
            OnLateUpdate?.Invoke();
        }
        protected override void Draw(GameTime gameTime)
        {
            // draw ui
            UserInterface.Active.Draw(SpriteBatch);

            GraphicsDevice.Clear(Color.CornflowerBlue);

            SamplerState state = new SamplerState { Filter = TextureFilter.Point };
            SpriteBatch.Begin(samplerState: state);
            OnDraw?.Invoke();
            SpriteBatch.End();

            // finalize ui rendering
            UserInterface.Active.DrawMainRenderTarget(SpriteBatch);

            base.Draw(gameTime);
        }
        private void InitializeUI()
        {
            // Connexion panel
            ConnexionPanel = new ConnexionPanel(new Vector2(480, -1));
            UserInterface.Active.AddEntity(ConnexionPanel);

            //CharacterSelectionPanel = new CharacterSelectionPanel(new Vector2(480, -1));
            //UserInterface.Active.AddEntity(CharacterSelectionPanel);
        }
    }
}